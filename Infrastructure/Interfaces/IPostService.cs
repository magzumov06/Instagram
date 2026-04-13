using Domain.DTOs.PostDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IPostService
{
    Task<Responce<string>> CreatePostAsync(CreatePost post, int userId);
    Task<Responce<string>> UpdatePost(UpdatePostDto post, int userId);
    Task<Responce<string>> DeletePost(int id, int userId);
    Task<Responce<GetPostDto>> GetPost(int id);
    Task<Responce<List<GetPostDto>>> GetPostsByUserId(int userId);
    Task<Responce<List<GetPostDto>>> GetPosts();
}