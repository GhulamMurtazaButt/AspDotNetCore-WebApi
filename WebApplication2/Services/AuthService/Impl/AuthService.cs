using DataLibrary.Models;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Models;
using WebApplication2.Services.EmailService;
using WebApplication2.Services.TokenService;
using WebApplication2.Utilities.Base64Methods;
using WebApplication2.Utilities;
using WebApplication2.Dtos.User;
using AutoMapper;
using DataLibrary.Repositry;
using System.Data;

namespace WebApplication2.Services.AuthService.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<Users> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IBase64 _base64;
        private readonly IMapper _mapper;
        private readonly IGenericRepositry<SystemUsers> _genericRepositry;

        public AuthService(IEmailService emailService, UserManager<Users> userManager, ITokenService tokenService, IBase64 base64, IMapper mapper, IGenericRepositry<SystemUsers> genericRepositry)
        {
            _emailService = emailService;
            _userManager = userManager;
            _tokenService = tokenService;
            _base64 = base64;
            _mapper = mapper;
            _genericRepositry = genericRepositry;
        }
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                Users user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception(MessageStrings.UserNotFound);
                }
                else if (user.EmailConfirmed == true)
                {
                    if (!await _userManager.CheckPasswordAsync(user, password))
                    {
                        throw new Exception(MessageStrings.IncorrectPassword);
                    }
                    else
                    {
                        response.success = true;
                        response.data = await _tokenService.generateToken(user);
                    }
                }
                else
                {
                    throw new Exception(MessageStrings.EmailConfirmationMsg_LoginBefore);
                }
            }
            catch (Exception exp)
            {
                response.success = false;
                response.message = exp.Message;
            }

            return response;
        }
        public async Task<ServiceResponse<string>> Register(UserRegisterDto userRegister)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                using (IDbTransaction transaction = _genericRepositry.BeginTransaction())
                {
                    Users finduser = await _userManager.FindByEmailAsync(userRegister.email);
                    Users findusername = await _userManager.FindByNameAsync(userRegister.username);
                    if (finduser != null || findusername != null)
                    {
                        throw new Exception(MessageStrings.UserAlreadyExist);
                    }
                    SystemUsers systemuser = await _genericRepositry.AddAsync(_mapper.Map<SystemUsers>(userRegister));
                    _genericRepositry.SaveChanges();

                    Users user = _mapper.Map<Users>(userRegister);
                    user.SystemUserId = systemuser.Id;
                    IdentityResult result = await _userManager.CreateAsync(_mapper.Map<Users>(user), userRegister.password);

                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        throw new Exception(result.ToString());
                    }
                    else
                    {   
                        await _userManager.AddToRoleAsync(user, "User");
                        string token = await _userManager.GenerateEmailConfirmationTokenAsync(_mapper.Map<Users>(user));
                        string tokenEncode = _base64.Base64Encode(token);
                        string confirmationLink = $"https://localhost:7124/api/Auth/confirm-email?id={_mapper.Map<Users>(user).Id}&token={tokenEncode}";
                        await _emailService.SendEmailAsync(_mapper.Map<Users>(user).Email, "Confirm Your Email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.", true);
                    }

                    response.message = MessageStrings.EmailConfirmation;
                    response.data = _mapper.Map<Users>(userRegister).Id;
                    transaction.Commit();
                }
            }
            catch (Exception exp)
            {
                response.success = false;
                response.message = exp.Message;
            }
            
            return response;

        }
        public async Task<ServiceResponse<string>> ConfirmEmail(string id, string token)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try { 
            Users user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception(MessageStrings.UserNotFound);
            }
                string tokenDecode = _base64.Base64Decode(token);
                IdentityResult tokenValid = await _userManager.ConfirmEmailAsync(user, tokenDecode);
                if (tokenValid.Succeeded)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    response.success = true;
                    response.message = MessageStrings.EmailSuccess;
                }
                else
                {
                    throw new Exception(MessageStrings.InvalidToken);
                }
            }
            catch (Exception exp)
            {
                response.success = false;
                response.message = exp.Message;
            }

            return response;
        }
        public async Task<ServiceResponse<string>> ForgotPassword(UserEmailDto userEmail)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                Users user = await _userManager.FindByEmailAsync(userEmail.email);
                if (user != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string tokenEncode = _base64.Base64Encode(token);
                    string confirmationLink = $"https://localhost:7124/api/Auth/forgot-password?id={user.Id}&token={tokenEncode}";
                    await _emailService.SendEmailAsync(user.Email, "Reset Your Password", $"To Reset Your Password <a href='{confirmationLink}'>clicking here</a>;.", true);
                }
                else
                {
                    throw new Exception(MessageStrings.UserNotFound);
                }
                response.message = MessageStrings.ForgotPasswordLink;
                response.data = user.Id;
            }
            catch (Exception exp)
            {
                response.success = false;
                response.message = exp.Message;
            }
            return response;

        }
        public async Task<ServiceResponse<string>> ResetPassword(UserResetPasswordDto userResetPassword)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                Users user = await _userManager.FindByEmailAsync(userResetPassword.email);
                Users userId = await _userManager.FindByIdAsync(userResetPassword.Id);
                if (user != null && userId != null)
                {
                    string tokenDecode = _base64.Base64Decode(userResetPassword.token);
                    IdentityResult result = await _userManager.ResetPasswordAsync(user, tokenDecode, userResetPassword.password);
                    if (result.Succeeded)
                    {
                        response.success = true;
                        response.message = MessageStrings.PasswordChangesSuccessfuly;
                    }
                    else
                    {
                        throw new Exception(result.ToString());
                    }
                }
                else
                {
                    throw new Exception(MessageStrings.UserNotFound);
                }
            }
            catch (Exception exp) {
                response.success = false;
                response.message = exp.Message;
            }

            return response;
        } 
        
        
    }

}
