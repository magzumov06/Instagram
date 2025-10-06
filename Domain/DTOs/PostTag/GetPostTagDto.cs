namespace Domain.DTOs.PostTag;

public class GetPostTagDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int TagId { get; set; }
    public DateTime CreatedAt { get; set; }
}