using Domain.DTOs.PostTag;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class PostTagController(IPostTagService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostTag postTag)
    {
        var res = await service.CreatePostTag(postTag);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeletePostTag(id);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetPostTag(id);
        return Ok(res);
    }

}