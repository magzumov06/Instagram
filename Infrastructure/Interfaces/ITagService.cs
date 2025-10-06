using Domain.DTOs.TagDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ITagService
{
    Task<Responce<string>> CreateTagAsync(CreateTagDto dto);
    Task<Responce<string>> UpdateTagAsync(UpdateTagDto dto);
    Task<Responce<string>> DeleteTagAsync(int id);
    Task<Responce<GetTagDto>> GetTagAsync(int id);
}