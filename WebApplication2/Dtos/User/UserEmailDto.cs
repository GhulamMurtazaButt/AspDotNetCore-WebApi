using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.User
{
    public class UserEmailDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
    }
}
