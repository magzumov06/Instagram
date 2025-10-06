using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.PostDto;

public class CreatePost
{
    public string Content { get; set; }
    public IFormFile? ImagePath { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}