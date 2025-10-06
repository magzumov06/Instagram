namespace Domain.DTOs.CommentDto;

public class CreateCommentDto
{
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}