namespace Domain.Entities;

public class Comment : BaseEntities
{
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public User? User { get; set; }
    public Post? Post { get; set; }
}