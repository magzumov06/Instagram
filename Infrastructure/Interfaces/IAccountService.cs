using Domain.DTOs.Account;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IAccountService
{
    Task<Responce<string>> Register(Register register);
    Task<Responce<string>> Login(Login login);
}