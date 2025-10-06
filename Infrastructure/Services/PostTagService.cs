using System.Net;
using Domain.DTOs.PostTag;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class PostTagService(DataContext context) : IPostTagService
{
    public async Task<Responce<string>> CreatePostTag(CreatePostTag dto)
    {
        try
        {
            Log.Information("CreatePostTag");
            var posTag = new PostTag
            {
                PostId = dto.PostId,
                TagId = dto.TagId,
                CreatedAt = DateTime.UtcNow
            };
            await context.PostTags.AddAsync(posTag);
            var res = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.Created,"Post Tag created successfully!")
                : new Responce<string>(HttpStatusCode.BadRequest,"Post Tag could not be created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreatePostTag");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeletePostTag(int id)
    {
        try
        {
            Log.Information("DeletePostTag");
            var postTag = await context.PostTags.FirstOrDefaultAsync(x=>x.Id == id);
            if (postTag == null) return new Responce<string>(HttpStatusCode.NotFound,"Post Tag not found");
            context.PostTags.Remove(postTag);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Post Tag deleted successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Post Tag could not be deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeletePostTag");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetPostTagDto>> GetPostTag(int id)
    {
        try
        {
            Log.Information("GetPostTag");
            var  postTag = await context.PostTags.FirstOrDefaultAsync(x => x.Id == id);
            if(postTag == null)  return new Responce<GetPostTagDto>(HttpStatusCode.NotFound,"Post Tag not found");
            var posTag = new GetPostTagDto
            {
                Id = postTag.Id,
                PostId = postTag.PostId,
                TagId = postTag.TagId,
                CreatedAt = postTag.CreatedAt
            };
            return new Responce<GetPostTagDto>(posTag);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetPostTag");
            return new Responce<GetPostTagDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}