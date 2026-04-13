using Domain.DTOs.FollowDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IFollowService
{
    Task<Responce<string>> CreateFollow(CreateFollowDto follow, int userId);
    Task<Responce<string>> DeleteFollow(int id, int userId);
    Task<Responce<GetFollowDto>> GetFollow(int id);
    Task<Responce<List<GetFollowDto>>> GetFollowers(int userId);
    Task<Responce<List<GetFollowDto>>> GetFollowing(int userId);
    Task<Responce<List<GetFollowDto>>> GetFollows();
    
}