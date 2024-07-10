using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataLibrary.Models;
using WebApplication2.Services.TokenService;
using WebApplication2.Utilities;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Services.TokenService.Impl
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Users> _userManager;

        public TokenService(IConfiguration configuration, UserManager<Users> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> generateToken(Users user)
        {
            try
            {
                IList<string> userRole = await _userManager.GetRolesAsync(user);
                List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, userRole.FirstOrDefault()),

            };
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection(Constants.TokenSection).Value)
                    );

                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = signingCredentials
                };
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception exp)
            {
                return exp.Message;
            }

        }

    }
}
