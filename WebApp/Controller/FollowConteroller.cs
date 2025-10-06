using Domain.DTOs.FollowDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class FollowConteroller(IFollowService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFollow(CreateFollowDto dto)
    {
        var res = await service.CreateFollow(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFollow(int id)
    {
        var res = await service.DeleteFollow(id);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFollow(int id)
    {
        var res = await service.GetFollow(id);
        return Ok(res);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFollows()
    {
        var res = await service.GetFollows();
        return Ok(res);
    }
    
}