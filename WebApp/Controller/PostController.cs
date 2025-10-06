using Domain.DTOs.PostDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromForm] CreatePost dto)
    {
        var res = await service.CreatePostAsync(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdatePostDto dto)
    {
        var res = await service.UpdatePost(dto);
        return Ok(res);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePost(int id)
    {
        var res = await service.DeletePost(id);
        return Ok(res);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var res = await service.GetPost(id);
        return Ok(res);
    }
}