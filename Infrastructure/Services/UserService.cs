using System.Net;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class UserService(
    DataContext context,
    IFileStorage file) : IUserService
{
    public async Task<Responce<string>> UpdateUser(UpdateUserDto update)
    {
        try
        {
            Log.Information("Updating User");
            var updateUser = await context.Users.FirstOrDefaultAsync(x => x.Id == update.Id);
            if (updateUser == null) return new Responce<string>(HttpStatusCode.BadRequest,"User not found");
            if (update.AvatarUrl != null)
            {
                if (!string.IsNullOrEmpty(updateUser.AvatarUrl))
                {
                    await file.DeleteFile(updateUser.AvatarUrl);
                }
                await file.SaveFile(update.AvatarUrl,"UserAvatar");
            }
            updateUser.FirstName = update.FirstName;
            updateUser.LastName = update.LastName;
            updateUser.UserName = update.Username;
            updateUser.Age = update.Age;
            updateUser.PhoneNumber = update.PhoneNumber;
            updateUser.UpdateAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"User successfully updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"User not found");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteUser(int id)
    {
        try
        {
            Log.Information("Deleting User");
            var deleteUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteUser == null) return new Responce<string>(HttpStatusCode.BadRequest,"User not found"); 
            deleteUser.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"User successfully deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"User not found");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetUserDto>> GetUser(int id)
    {
        try
        {
            Log.Information("Getting User");
            var getUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (getUser == null) return new Responce<GetUserDto>(HttpStatusCode.BadRequest,"User not found");
            var dto = new GetUserDto
            {
                Id = getUser.Id,
                FirstName = getUser.FirstName,
                LastName = getUser.LastName,
                Username = getUser.UserName,
                Age = getUser.Age,
                PhoneNumber = getUser.PhoneNumber,
                AvatarUrl = getUser.AvatarUrl,
                FollowingCount = getUser.FollowingCount,
                CreatedAt = getUser.CreatedAt,
                UpdateAt = getUser.UpdateAt
            };
            return new Responce<GetUserDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetUser");
            return new Responce<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            Log.Information("Getting users");
            var query = context.Users.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id.Value);
            }

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));
            }

            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(x => x.LastName.Contains(filter.LastName));
            }

            if (!string.IsNullOrEmpty(filter.Username))
            {
                query = query.Where(x => x.UserName.Contains(filter.Username));
            }

            if (filter.Age.HasValue)
            {
                query = query.Where(x=> x.Age == filter.Age.Value);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
            }
            query = query.Where(x=>x.IsDeleted==false);
            var totalCount = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var user = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(user.Count == 0 ) return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.NotFound,"User not found");
            var dtos = user.Select(x => new GetUserDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.UserName,
                Age = x.Age,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                FollowingCount = x.FollowingCount,
                AvatarUrl = x.AvatarUrl,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdateAt
            }).ToList();
            return new PaginationResponce<List<GetUserDto>>(dtos, totalCount, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetUsers");
            return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}