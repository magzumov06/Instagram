using Domain.DTOs.LikeDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ILikeService
{
    Task<Responce<string>> CreateLike(CreateLikeDto dto);
    Task<Responce<string>> DeleteLike(int id);
    Task<Responce<GetLikeDto>> GetLike(int id);
}