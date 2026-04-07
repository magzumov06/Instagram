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
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFollow(int id)
    {
        var res = await service.DeleteFollow(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFollow(int id)
    {
        var res = await service.GetFollow(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFollows()
    {
        var res = await service.GetFollows();
        return StatusCode(res.StatusCode, res);
    }
    
}