using Domain.DTOs.LikeDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ILikeService
{
    Task<Responce<string>> CreateLike(CreateLikeDto dto, int userId);
    Task<Responce<string>> DeleteLike(int id,  int userId);
    Task<Responce<GetLikeDto>> GetLike(int id);
    Task<Responce<List<GetLikeDto>>> GetLikes(LikeFilter filter);
    Task<Responce<List<GetLikeDto>>> GetLikeByPostId(int  postId);
    Task<Responce<List<GetLikeDto>>> GetLikesByUserId(int userId);
}