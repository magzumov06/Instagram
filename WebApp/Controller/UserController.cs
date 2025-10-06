using Domain.DTOs.UserDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] UpdateUserDto updateUserDto)
    {
        var res = await service.UpdateUser(updateUserDto);
        return Ok(res);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var res = await service.DeleteUser(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res = await service.GetUsers(filter);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var res = await service.GetUser(id);
        return Ok(res);
    }
}