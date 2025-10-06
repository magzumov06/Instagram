using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.PostDto;

public class UpdatePostDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public IFormFile? ImagePath { get; set; }

}