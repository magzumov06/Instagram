using Domain.DTOs.LikeDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class LikeController(ILikeService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLike(CreateLikeDto dto)
    {
        var res = await service.CreateLike(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLike(int id)
    {
        var res = await service.DeleteLike(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLike(int id)
    {
        var res = await service.GetLike(id);
        return StatusCode(res.StatusCode, res);
    }
    [HttpGet]
    public async Task<IActionResult> GetLikes()
    {
        var res = await service.GetLikes();
        return StatusCode(res.StatusCode, res);
    }
}