namespace Domain.Entities;

public class Tag : BaseEntities
{
    public string Name { get; set; }
    public List<PostTag>? PostTags { get; set; }
}