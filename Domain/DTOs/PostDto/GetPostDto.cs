namespace Domain.DTOs.PostDto;

public class GetPostDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string? ImagePath { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}