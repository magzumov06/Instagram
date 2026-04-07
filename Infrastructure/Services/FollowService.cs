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
    await using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        Log.Information("Creating Follow");
        var userToFollow = await context.Users.FirstOrDefaultAsync(u => u.Id == follow.FollowingId);
        if (userToFollow == null)
            return new Responce<string>(HttpStatusCode.NotFound, "User to follow not found");

        var existingFollow = await context.Follows
            .FirstOrDefaultAsync(f => f.FollowingId == follow.FollowingId && f.FollowerId == follow.FollowerId);
        if (existingFollow != null)
            return new Responce<string>(HttpStatusCode.BadRequest, "You already follow this user");

        var newFollow = new Follow
        {
            FollowingId = follow.FollowingId,
            FollowerId = follow.FollowerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        await context.Follows.AddAsync(newFollow);

        await context.Database.ExecuteSqlRawAsync(
            "UPDATE Users SET FollowingCount = FollowingCount + 1 WHERE Id = {0}", userToFollow.Id);

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new Responce<string>(HttpStatusCode.OK, "Follow added");
    }
    catch (Exception e)
    {
        await transaction.RollbackAsync();
        Log.Error("Error in CreateFollow: {Message}", e.Message);
        return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
    }
}

public async Task<Responce<string>> DeleteFollow(int id)
{
    await using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        Log.Information("Deleting Follow");

        var follow = await context.Follows.FirstOrDefaultAsync(f => f.Id == id);
        if (follow == null)
            return new Responce<string>(HttpStatusCode.NotFound, "Follow not found");

        context.Follows.Remove(follow);

        await context.Database.ExecuteSqlRawAsync(
            "UPDATE Users SET FollowingCount = FollowingCount - 1 WHERE Id = {0}", follow.FollowingId);

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new Responce<string>(HttpStatusCode.OK, "Follow deleted");
    }
    catch (Exception e)
    {
        await transaction.RollbackAsync();
        Log.Error("Error in DeleteFollow: {Message}", e.Message);
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