using Domain.DTOs.CommentDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ICommentService
{
    Task<Responce<string>> CreateComment (CreateCommentDto dto, int userId);
    Task<Responce<string>> DeleteComment (int id, int userId);
    Task<Responce<string>> UpdateComment (UpdateCommentDto dto, int userId);
    Task<Responce<GetCommentDto>> GetComment(int id);
    Task<Responce<List<GetCommentDto>>> GetCommentByPostId(int postId);
}