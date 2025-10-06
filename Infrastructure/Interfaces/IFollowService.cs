using Domain.DTOs.FollowDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IFollowService
{
    Task<Responce<string>> CreateFollow(CreateFollowDto follow);
    Task<Responce<string>> DeleteFollow(int id);
    Task<Responce<GetFollowDto>> GetFollow(int id);
    Task<Responce<List<GetFollowDto>>> GetFollows();
}