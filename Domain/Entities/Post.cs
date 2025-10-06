namespace Domain.Entities;

public class Post : BaseEntities
{
    public string Content { get; set; }
    public string? ImagePath { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public User? User { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }
    public List<PostTag>? PostTags { get; set; }
    
}