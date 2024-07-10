using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.User
{
    public class UserResetPasswordDto
    {
        public string Id { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }
}
