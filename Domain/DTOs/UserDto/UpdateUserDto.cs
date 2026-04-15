using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public int Age { get; set; }
    [Phone(ErrorMessage = "phone number is not valid")]
    public string PhoneNumber { get; set; }
    
    public IFormFile? AvatarUrl { get; set; }
}