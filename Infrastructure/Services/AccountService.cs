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
            if (await userManager.IsLockedOutAsync(user))
            {
                var lokEnd = await userManager.GetLockoutEndDateAsync(user);
                var remaining = lokEnd.Value.UtcDateTime -  DateTime.UtcNow;
                return new Responce<string>(
                    HttpStatusCode.Unauthorized, 
                    $"Account locked.Try again in {remaining.Minutes} minutes and {remaining.Seconds} seconds");
            }
            
            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, login.Password);
            if (!isPasswordCorrect)
            {
                await userManager.AccessFailedAsync(user);
                int attemptsLeft = 5 - await userManager.GetAccessFailedCountAsync(user);
                string message = attemptsLeft > 0
                    ? $"Incorrect password. {attemptsLeft} attempts left."
                    : "Account locked due to too many failed attempts.";
                return new Responce<string>(HttpStatusCode.Unauthorized, message);
            }
            
            await userManager.ResetAccessFailedCountAsync(user);
            
            var token = await GenerateJwtTokenHelper.GenerateJwtToken(user, userManager, configuration);
            return new Responce<string>(token) {Message = "Welcome to Instagram"};
        }
        catch (Exception e)
        {
            Log.Error("Error in Login");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    
    public async Task<Responce<string>> ChangePassword(ChangePassword changePassword, int userId)
    {
        try
        {
            if (changePassword.NewPassword != changePassword.ConfirmNewPassword)
            {
                return new Responce<string>(HttpStatusCode.BadRequest, "Passwords do not match");
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return new Responce<string>(HttpStatusCode.Unauthorized, "User not found");

            var result = await userManager.ChangePasswordAsync(
                user,
                changePassword.OldPassword,
                changePassword.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Responce<string>(HttpStatusCode.BadRequest, errors);
            }

            return new Responce<string>(HttpStatusCode.OK, "User password changed successfully");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}