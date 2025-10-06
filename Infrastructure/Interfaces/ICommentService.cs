using Domain.DTOs.CommentDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ICommentService
{
    Task<Responce<string>> CreateComment (CreateCommentDto dto);
    Task<Responce<string>> DeleteComment (int id);
    Task<Responce<string>> UpdateComment (UpdateCommentDto dto);
    Task<Responce<GetCommentDto>> GetComment(int id);
}