using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos.User;
using WebApplication2.Models;
using WebApplication2.Services.AuthService;
using WebApplication2.Utilities;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            try
            {
                return Ok(await _authService.Register(userRegister));
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                return Ok(await _authService.Login(userLogin.email, userLogin.password));
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message); 
            }
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string id, [FromQuery] string token)
        {
            try
            {
                ServiceResponse<string> response = await _authService.ConfirmEmail(id, token);
                if (response.success)
                {
                    return Ok(MessageStrings.Thanks);
                }
                else
                {
                    throw new Exception($"{MessageStrings.EmailConfirmationFailed + response.message}");
                }
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
           
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(UserEmailDto userEmail)
        {
            try
            {
                return Ok(await _authService.ForgotPassword(userEmail));
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
          
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordDto userResetPassword)
        {
            try{
                return Ok(await _authService.ResetPassword(userResetPassword));
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

    }
}
