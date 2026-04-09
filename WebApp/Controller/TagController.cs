using Domain.DTOs.TagDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class TagController(ITagService service) :  ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTag(CreateTagDto dto)
    {
        var res = await service.CreateTagAsync(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateTag(UpdateTagDto dto)
    {
        var res = await service.UpdateTagAsync(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var res = await service.DeleteTagAsync(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTag(int id)
    {
        var res = await service.GetTagAsync(id);
        return StatusCode(res.StatusCode, res);
    }
}