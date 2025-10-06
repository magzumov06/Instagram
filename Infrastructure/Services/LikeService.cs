using System.Net;
using Domain.DTOs.LikeDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class LikeService(DataContext context) : ILikeService
{
    public async Task<Responce<string>> CreateLike(CreateLikeDto dto)
    {
        try
        {
            Log.Information("Creating Like");
            var exiting = await context.Likes.FirstOrDefaultAsync(x => x.PostId == dto.PostId && x.UserId == dto.UserId);
            if (exiting != null)
            {
                return new Responce<string>(HttpStatusCode.BadRequest,"You are already liked this post");
            }
            var post = await context.Posts.FirstOrDefaultAsync(x=>x.Id == dto.PostId);
            if (post == null) return new Responce<string>(HttpStatusCode.NotFound,"Post not found");
            var like = new Like()
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            post.LikeCount += 1;
            await context.Likes.AddAsync(like);
            var  res = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.OK, "Like created")
                : new Responce<string>(HttpStatusCode.NotFound, "Error");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateLike");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteLike(int id)
    {
        try
        {
            Log.Information("Deleting Like");
            var like = await context.Likes
                .Include(x=> x.Post)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (like == null) return new Responce<string>(HttpStatusCode.NotFound, "Like not found");
            context.Likes.Remove(like);
            like.Post.LikeCount -= 1;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Like deleted")
                : new Responce<string>(HttpStatusCode.NotFound, "Error");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteLike");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetLikeDto>> GetLike(int id)
    {
        try
        {
            Log.Information("Getting Like");
            var like = await context.Likes.FirstOrDefaultAsync(x => x.Id == id);
            if(like == null) return new Responce<GetLikeDto>(HttpStatusCode.NotFound, "Like not found");
            var dto = new GetLikeDto()
            {
                Id = like.Id,
                PostId = like.PostId,
                UserId = like.UserId,
                CreatedAt = like.CreatedAt,
                UpdatedAt = like.UpdatedAt,
            };
            return new Responce<GetLikeDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetLike");
            return new Responce<GetLikeDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetLikeDto>>> GetLikes()
    {
        try
        {
            Log.Information("Getting Likes");
            var likes = await context.Likes.ToListAsync();
            if(likes.Count == 0) return new Responce<List<GetLikeDto>>(HttpStatusCode.NotFound,"Like not found");
            var dtos = likes.Select(x => new GetLikeDto()
            {
                Id = x.Id,
                PostId = x.PostId,
                UserId = x.UserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetLikeDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetLikes");
            return new Responce<List<GetLikeDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}