using System.Net;
using Domain.DTOs.PostDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class PostService(
    DataContext context,
    IFileStorage file) : IPostService
{
    public async Task<Responce<string>> CreatePostAsync(CreatePost post)
    {
        try
        {
            Log.Information("Creating Post");
            var newPost = new Post()
            {
                UserId = post.UserId,
                Content = post.Content,
                LikeCount = 0,
                CommentCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            if (post.ImagePath != null)
            {
                newPost.ImagePath = await file.SaveFile(post.ImagePath, "PostImage");
            }
            await context.Posts.AddAsync(newPost);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Post Created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Post not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreatePost");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdatePost(UpdatePostDto post)
    {
        try
        {
            Log.Information("Updating Post");
            var update = await context.Posts.FirstOrDefaultAsync(x=>x.Id == post.Id);
            if (update == null) return new Responce<string>(HttpStatusCode.NotFound,"Post not found");
            if (post.ImagePath != null)
            {
                if (!string.IsNullOrEmpty(update.ImagePath))
                {
                    await file.DeleteFile(update.ImagePath);
                }
                await file.SaveFile(post.ImagePath, "PostImage");
            }
            update.Content = post.Content;
            update.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.OK,"Post updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Post not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdatePost");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeletePost(int id)
    {
        try
        {
            Log.Information("Deleting Post");
            var delete = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (delete == null) return new Responce<string>(HttpStatusCode.NotFound,"Post not found");
            context.Posts.Remove(delete);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Post deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Post not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeletePost");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetPostDto>> GetPost(int id)
    {
        try
        {
            Log.Information("Getting Post");
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if(post == null) return new Responce<GetPostDto>(HttpStatusCode.NotFound,"Post not found");
            var dto = new GetPostDto()
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                LikeCount = post.LikeCount,
                CommentCount = post.CommentCount,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            };
            return new Responce<GetPostDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetPost");
            return new Responce<GetPostDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetPostDto>>> GetPosts()
    {
        try
        {
            Log.Information("Getting Posts");
            var posts = await context.Posts.ToListAsync();
            if(posts.Count == 0) return new Responce<List<GetPostDto>>(HttpStatusCode.NotFound,"Post not found");
            var dtos = posts.Select(x => new GetPostDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Content = x.Content,
                LikeCount = x.LikeCount,
                CommentCount = x.CommentCount,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetPostDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetPosts");
            return new Responce<List<GetPostDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}