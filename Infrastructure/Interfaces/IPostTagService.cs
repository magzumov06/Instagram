using Domain.DTOs.PostTag;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IPostTagService
{
    Task<Responce<string>> CreatePostTag(CreatePostTag dto);
    Task<Responce<string>> DeletePostTag(int id);
    Task<Responce<GetPostTagDto>> GetPostTag(int id);
}