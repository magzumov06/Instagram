using System.Net;
using Domain.DTOs.FollowDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class FollowService(DataContext context) :  IFollowService
{
    public async Task<Responce<string>> CreateFollow(CreateFollowDto follow)
    {
        try
        {
            Log.Information("Creating Follow");
            var exiting = await context.Follows.Include(x=>x.Following).FirstOrDefaultAsync(x=>x.FollowingId == follow.FollowingId && x.FollowerId == follow.FollowerId);
            if(exiting  !=  null) return new Responce<string>(HttpStatusCode.BadRequest,"You allready follow this user");
            var following = new Follow
            {
                FollowingId = follow.FollowingId,
                FollowerId = follow.FollowerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            exiting.Following.FollowingCount += 1;
            await context.Follows.AddAsync(following);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Follow added")
                : new Responce<string>(HttpStatusCode.NotFound, "Follow not found");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateFollow");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteFollow(int id)
    {
        try
        {
            Log.Information("DeleteFollow");
            var follow = await context.Follows.FirstOrDefaultAsync(x => x.Id == id);
            if (follow == null) return new Responce<string>(HttpStatusCode.NotFound, "Follow not found");
            context.Follows.Remove(follow);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Follow deleted")
                : new Responce<string>(HttpStatusCode.NotFound, "Follow not found");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteFollow");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetFollowDto>> GetFollow(int id)
    {
        try
        {
            Log.Information("Getting follow by id");
            var follow = await context.Follows.FirstOrDefaultAsync(x => x.Id == id);
            if (follow == null) return new Responce<GetFollowDto>(HttpStatusCode.NotFound, "Follow not found");
            var dto = new GetFollowDto
            {
                Id = follow.Id,
                FollowingId = follow.FollowingId,
                FollowerId = follow.FollowerId,
                CreatedAt = follow.CreatedAt,
                UpdatedAt = follow.UpdatedAt,
            };
            return new Responce<GetFollowDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetFollowById");
            return new Responce<GetFollowDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetFollowDto>>> GetFollows()
    {
        try
        {
            Log.Information("Getting follows");
            var follows = await context.Follows.ToListAsync();
            if(follows.Count == 0) return new Responce<List<GetFollowDto>>(HttpStatusCode.NotFound, "Follows not found");
            var dtos = follows.Select(x=> new GetFollowDto()
            {
                Id = x.Id,
                FollowingId = x.FollowingId,
                FollowerId = x.FollowerId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetFollowDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetFollows");
            return new Responce<List<GetFollowDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}