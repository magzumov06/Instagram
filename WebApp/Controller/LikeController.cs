using System.Security.Claims;
using Domain.DTOs.LikeDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class LikeController(ILikeService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateLike(CreateLikeDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaim == null)
            return Unauthorized("User not authorized");
        var userId = int.Parse(userClaim);
        var res = await service.CreateLike(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteLike(int id)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaim == null)
            return Unauthorized("User not authorized");
        var userId = int.Parse(userClaim);
        var res = await service.DeleteLike(id, userId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetLike(int id)
    {
        var res = await service.GetLike(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("posts/{postId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPostLike(int postId)
    {
        var res = await service.GetLikeByPostId(postId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetLikes([FromQuery]LikeFilter filter)
    {
        var res = await service.GetLikes(filter);
        return StatusCode(res.StatusCode, res);
    }
}