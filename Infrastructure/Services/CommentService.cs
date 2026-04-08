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
            Log.Information("Creating a new comment");
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
                "UPDATE \"Posts\" SET \"CommentCount\" = \"CommentCount\" + 1 WHERE \"Id\" = {0}", post.Id);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Responce<string>(HttpStatusCode.OK, "Comment created");
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured creating a new comment");
            await transaction.RollbackAsync();
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteComment(int id, int userId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            Log.Information("Deleting comment");
            var comment = await context.Comments
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (comment == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Comment not found");

            context.Comments.Remove(comment);

            await context.Database.ExecuteSqlRawAsync(
                "UPDATE \"Posts\" SET \"CommentCount\" = \"CommentCount\" - 1 WHERE \"Id\" = {0}", comment.PostId);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Responce<string>(HttpStatusCode.OK, "Comment deleted");
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured deleting comment");
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
            Log.Error(e, "An error occured updating comment");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetCommentDto>> GetComment(int id)
    {
        try
        {
            Log.Information("GetComment");
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task<Responce<List<GetCommentDto>>> GetCommentByPostId(int postId)
    {
        try
        {
            Log.Information("GetCommentByPostId");
            var comment =await context.Comments
                .Where(x => x.PostId == postId)
                .OrderByDescending(x=>x.CreatedAt)
                .ToListAsync();
            if (comment.Count == 0)
                return new Responce<List<GetCommentDto>>(HttpStatusCode.NotFound, "Comment not found");
            var dtos = comment.Select(x=> new GetCommentDto()
            {
                Id = x.Id,
                PostId = x.PostId,
                UserId = x.UserId,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetCommentDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error in GetCommentByPostId");
            return new  Responce<List<GetCommentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}