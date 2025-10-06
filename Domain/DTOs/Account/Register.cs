using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.Account;

public class Register
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Username { get; set; }
    [Range(16,100)]
    public int Age { get; set; }
    [Phone]
    [StringLength(12, MinimumLength = 9)]
    public required string PhoneNumber { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public string? Address { get; set; }
    public IFormFile? AvatarUrl { get; set; }
}