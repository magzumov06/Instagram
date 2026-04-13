using System.Security.Claims;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Put([FromForm] UpdateUserDto updateUserDto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.UpdateUser(updateUserDto, userId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.DeleteUser(userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res = await service.GetUsers(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var res = await service.GetUserById(id);
        return StatusCode(res.StatusCode, res);
    }
    
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var  userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.GetUser(userId);
        return StatusCode(res.StatusCode, res);
    }
}