namespace Domain.DTOs.UserDto;

public class GetUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int  FollowingCount { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
}