using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User :  IdentityUser<int>
{
    [Required]
    [StringLength(100,  MinimumLength = 3)]
    public required string FirstName { get; set; }
    public string? LastName { get; set; } 
    [Range(16,100)]
    public int Age { get; set; }
    [Phone]
    [StringLength(12, MinimumLength = 9)]
    public override required string PhoneNumber { get; set; }
    [EmailAddress]
    public override required string Email { get; set; }
    public string? Address { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsDeleted { get; set; }
    public int FollowingCount { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdateAt { get; set; }
    public List<Post>? Posts { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Follow>? Followers { get; set; }
    public List<Follow>? Followings { get; set; }
    public List<Like>? Likes { get; set; }
}