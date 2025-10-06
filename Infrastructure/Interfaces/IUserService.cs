using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Responce<string>> UpdateUser(UpdateUserDto update);
    Task<Responce<string>> DeleteUser(int id);
    public Task<Responce<GetUserDto>> GetUser(int id);
    public Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}