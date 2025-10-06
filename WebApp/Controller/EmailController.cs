using Domain.DTOs.EmailDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class SendEmailController(IEmailService service): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendEmail(SendEmail dto)
    {
        await service.SendEmail(dto);
        return Ok();
    }
}