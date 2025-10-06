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
    public async Task<Responce<string>> CreateComment(CreateCommentDto dto)
    {
        try
        {
            Log.Information("CreateComment");
            var post = await context.Posts.FirstOrDefaultAsync(x=>x.Id == dto.PostId);
            if (post == null) return new Responce<string>(HttpStatusCode.NotFound,"Post not found");
            var comment = new Comment
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            post.CommentCount += 1;
            await context.Comments.AddAsync(comment);
            var res= await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Comment created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Comment not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateComment");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteComment(int id)
    {
        try
        {
            Log.Information("DeleteComment");
            var comment = await context.Comments
                .Include(x=>x.Post)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return new Responce<string>(HttpStatusCode.NotFound,"Comment not found");
            context.Comments.Remove(comment);
            comment.Post.CommentCount -= 1;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Comment deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Comment not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteComment");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateComment(UpdateCommentDto dto)
    {
        try
        {
            Log.Information("UpdateComment");
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == dto.Id);
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
            Console.WriteLine(e);
            throw;
        }
    }
}