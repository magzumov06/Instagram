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
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdatePostDto dto)
    {
        var res = await service.UpdatePost(dto);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePost(int id)
    {
        var res = await service.DeletePost(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var res = await service.GetPost(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var res = await service.GetPosts();
        return StatusCode(res.StatusCode, res);
    }
}