using System.Security.Claims;
using Domain.DTOs.PostDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromForm] CreatePost dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.CreatePostAsync(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdatePostDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var  userId = int.Parse(userClaim);
        
        var res = await service.UpdatePost(dto, userId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePost(int id)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.DeletePost(id, userId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPost(int id)
    {
        var res = await service.GetPost(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPosts()
    {
        var res = await service.GetPosts();
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetPostByUserId()
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.GetPost(userId);
        return StatusCode(res.StatusCode, res);
    }
}