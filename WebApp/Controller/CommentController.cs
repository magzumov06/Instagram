using Domain.DTOs.CommentDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
 [ApiController]
 [Route("api/[controller]")]
public class CommentController(ICommentService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommentAsync(CreateCommentDto dto)
    {
        var res = await service.CreateComment(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCommentAsync(UpdateCommentDto dto)
    {
        var res = await service.UpdateComment(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCommentAsync(int id)
    {
        var res = await service.DeleteComment(id);
        return Ok(res);
    }
    
    [HttpGet("id")]
    public async Task<IActionResult> GetCommentAsync(int id)
    {
        var res = await service.GetComment(id);
        return Ok(res);
    }
    

}