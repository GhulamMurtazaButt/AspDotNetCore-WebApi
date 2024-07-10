using DataLibrary.Models;

namespace WebApplication2.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> generateToken(Users user);
    }
}
