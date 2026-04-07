using System.Net;
using Domain.DTOs.CommentDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class CommentService(DataContext context) : ICommentService
{
    public async Task<Responce<string>> CreateComment(CreateCommentDto dto, int userId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == dto.PostId);
            if (post == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Post not found");

            var comment = new Comment
            {
                PostId = dto.PostId,
                UserId = userId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await context.Comments.AddAsync(comment);

            await context.Database.ExecuteSqlRawAsync(
                "UPDATE Posts SET CommentCount = CommentCount + 1 WHERE Id = {0}", post.Id);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Responce<string>(HttpStatusCode.OK, "Comment created");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteComment(int id, int userId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var comment = await context.Comments
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (comment == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Comment not found");

            context.Comments.Remove(comment);

            await context.Database.ExecuteSqlRawAsync(
                "UPDATE Posts SET CommentCount = CommentCount - 1 WHERE Id = {0}", comment.PostId);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Responce<string>(HttpStatusCode.OK, "Comment deleted");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateComment(UpdateCommentDto dto, int userId)
    {
        try
        {
            Log.Information("UpdateComment");
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == dto.Id && x.UserId == userId);
            if (comment == null) return new Responce<string>(HttpStatusCode.NotFound,"Comment not found");
            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Comment updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Comment not updated");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetCommentDto>> GetComment(int id, int  userId)
    {
        try
        {
            Log.Information("GetComment");
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (comment == null) return new Responce<GetCommentDto>(HttpStatusCode.NotFound,"Comment not found");
            var dto = new GetCommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
            };
            return new Responce<GetCommentDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetComment");
            return new Responce<GetCommentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}