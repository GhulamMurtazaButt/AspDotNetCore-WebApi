using DataLibrary.Models;
using WebApplication2.Dtos.User;
using WebApplication2.Models;

namespace WebApplication2.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(UserRegisterDto userRegister);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<string>> ConfirmEmail(string id, string token);
        Task<ServiceResponse<string>> ForgotPassword(UserEmailDto userEmail);
        Task<ServiceResponse<string>> ResetPassword(UserResetPasswordDto userResetPassword);
    }
}
