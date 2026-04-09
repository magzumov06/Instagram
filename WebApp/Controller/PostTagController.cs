using Domain.DTOs.PostTag;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class PostTagController(IPostTagService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreatePostTag postTag)
    {
        var res = await service.CreatePostTag(postTag);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeletePostTag(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetPostTag(id);
        return StatusCode(res.StatusCode, res);
    }

}