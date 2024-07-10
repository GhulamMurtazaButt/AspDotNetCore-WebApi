using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.User
{
    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string username { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
     
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        public string password { get; set; }
    }
}
