using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Responce<string>> UpdateUser(UpdateUserDto update, int userId);
    Task<Responce<string>> DeleteUser(int userId);
    Task<Responce<GetUserDto>> GetUser(int userId);
    Task<Responce<GetUserDto>> GetUserById(int id); 
   Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}