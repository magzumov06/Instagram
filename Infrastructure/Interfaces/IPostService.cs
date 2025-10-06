using Domain.DTOs.PostDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IPostService
{
    Task<Responce<string>> CreatePostAsync(CreatePost post);
    Task<Responce<string>> UpdatePost(UpdatePostDto post);
    Task<Responce<string>> DeletePost(int id);
    Task<Responce<GetPostDto>> GetPost(int id);
    Task<Responce<List<GetPostDto>>> GetPosts();
}