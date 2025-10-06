using System.Net;
using Domain.DTOs.Account;
using Domain.DTOs.EmailDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.FileStorage;
using Infrastructure.Helper;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Infrastructure.Services;

public class AccountService(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    IEmailService emailService,
    IFileStorage file
    ) : IAccountService
{
    public async Task<Responce<string>> Register(Register register)
    {
        try
        {
            Log.Information("Registering new user");
            var existingUser = await userManager.FindByNameAsync(register.Username);
            if (existingUser != null)
                return new Responce<string>(HttpStatusCode.BadRequest,"Username already exists");
            

            var existingEmail = await userManager.FindByEmailAsync(register.Email);
            if (existingEmail != null)
                return new Responce<string>(HttpStatusCode.BadRequest,"Email already exists");
            
            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Username,
                PhoneNumber = register.PhoneNumber,
                Age = register.Age,
                Address = register.Address,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            if (register.AvatarUrl != null)
            {
                user.AvatarUrl = await file.SaveFile(register.AvatarUrl,"UserAvatar");
            }
            var password = PasswordGenerate.GeneratePassword();
            var result = await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
                return new Responce<string>(HttpStatusCode.BadRequest,"Something went wrong");
            await emailService.SendEmail(new SendEmail
            {
                To = user.Email,
                Subject = "Welcome to Instagram",
                Body =
                    $"<p>Салом {user.FirstName}!</p><br>Логини шумо {user.UserName}<br>Пароли шумо:{password}</p>"
            });
            return new Responce<string>(HttpStatusCode.OK,"User created and email sent");
        }
        catch (Exception e)
        {
            Log.Error("Error in Register");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> Login(Login login)
    {
        try
        {
            Log.Information("Logining new user");
            var user = await userManager.FindByNameAsync(login.Username);
            if(user == null)
                return new Responce<string>(HttpStatusCode.Unauthorized,"UserName or Password is incorrect");
            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, login.Password);
            if(!isPasswordCorrect)
                return new Responce<string>(HttpStatusCode.Unauthorized,"UserName or Password is incorrect");
            var token = await GenerateJwtTokenHelper.GenerateJwtToken(user, userManager, configuration);
            return new Responce<string>(token) {Message = "Welcome to Instagram"};
        }
        catch (Exception e)
        {
            Log.Error("Error in Login");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}