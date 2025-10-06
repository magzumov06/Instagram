using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account;

public class Login
{
    [Required]
    public required string Username { get; set; }
    public required string Password { get; set; }
}