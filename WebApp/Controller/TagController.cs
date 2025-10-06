using Domain.DTOs.TagDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class TagController(ITagService service) :  ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTag(CreateTagDto dto)
    {
        var res = await service.CreateTagAsync(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTag(UpdateTagDto dto)
    {
        var res = await service.UpdateTagAsync(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var res = await service.DeleteTagAsync(id);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        var res = await service.GetTagAsync(id);
        return Ok(res);
    }
}