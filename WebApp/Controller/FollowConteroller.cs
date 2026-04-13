using System.Security.Claims;
using Domain.DTOs.FollowDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class FollowConteroller(IFollowService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateFollow(CreateFollowDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
       
        var res = await service.CreateFollow(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteFollow(int id)
    {
        var  userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.DeleteFollow(id, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetFollow(int id)
    {
        var res = await service.GetFollow(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("users/{id}/followers")]
    [Authorize]
    public async Task<IActionResult> GetFollowers(int id)
    {
        var res = await service.GetFollowers(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("users/{id}/following")]
    [Authorize]
    public async Task<IActionResult> GetFollowing(int id)
    {
        var res = await service.GetFollowing(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFollows()
    {
        var res = await service.GetFollows();
        return StatusCode(res.StatusCode, res);
    }
    
}