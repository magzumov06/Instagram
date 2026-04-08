using System.Security.Claims;
using Domain.DTOs.CommentDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
 [ApiController]
 [Route("api/[controller]")]
public class CommentController(ICommentService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCommentAsync(CreateCommentDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        var res = await service.CreateComment(dto,  userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateCommentAsync(UpdateCommentDto dto)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        var res = await service.UpdateComment(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteCommentAsync(int id)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaim == null)
            return Unauthorized("User not authorized");
        
        var userId = int.Parse(userClaim);
        
        var res = await service.DeleteComment(id,userId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("id")]
    [Authorize]
    public async Task<IActionResult> GetCommentAsync(int id)
    {
        var res = await service.GetComment(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("postId")]
    [Authorize]
    public async Task<IActionResult> GetCommentPostAsync(int postId)
    {
        var res = await service.GetCommentByPostId(postId);
        return StatusCode(res.StatusCode, res);
    }
    
}