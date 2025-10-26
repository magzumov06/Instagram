using System.Net;
using Domain.DTOs.TagDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class TagService(DataContext context) : ITagService
{
    public async Task<Responce<string>> CreateTagAsync(CreateTagDto dto)
    {
        try
        {
            Log.Information("Creating Tag");
            var tag = new Tag()
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await context.Tags.AddAsync(tag);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Tag created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Tag not created");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateTagAsync");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateTagAsync(UpdateTagDto dto)
    {
        try
        {
            Log.Information("Updating Tag");
            var tag = await context.Tags.FirstOrDefaultAsync(x=> x.Id == dto.Id);
            if (tag == null) return new Responce<string>(HttpStatusCode.NotFound,"Tag not found");
            tag.Name = dto.Name;
            tag.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Tag updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Tag not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateTagAsync");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteTagAsync(int id)
    {
        try
        {
            Log.Information("Deleting Tag");
            var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null) return new Responce<string>(HttpStatusCode.NotFound,"Tag not found");
            context.Tags.Remove(tag);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Tag deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Tag not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteTagAsync");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    
    public async Task<Responce<GetTagDto>> GetTagAsync(int id)
    {
        try
        {
            Log.Information("Getting Tag");
            var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null) return new Responce<GetTagDto>(HttpStatusCode.NotFound,"Tag not found");
            var dto = new GetTagDto()
            {
                Id = tag.Id,
                Name = tag.Name,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
            return new Responce<GetTagDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetTagAsync");
            return new Responce<GetTagDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}