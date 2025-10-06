using Domain.DTOs.Account;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] Register register)
    {
        var res = await service.Register(register);
        return Ok(res);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(Login login)
    {
        var res =  await service.Login(login);
        return Ok(res);
    }
}