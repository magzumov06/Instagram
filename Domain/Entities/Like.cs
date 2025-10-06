namespace Domain.Entities;

public class Like : BaseEntities
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public User? User { get; set; }
    public Post? Post { get; set; }
}  