using System.Security.Claims;
using Domain.DTOs.Account;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] Register register)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var res = await service.Register(register);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(Login login)
    {
        var res =  await service.Login(login);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userIdClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userIdClaim);
        
        var res = await service.ChangePassword(changePassword, userId);
        return StatusCode(res.StatusCode, res);
    }
}