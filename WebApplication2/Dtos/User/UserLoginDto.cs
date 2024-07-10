using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.User
{
    public class UserLoginDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        public string password { get; set; }
    }
}
