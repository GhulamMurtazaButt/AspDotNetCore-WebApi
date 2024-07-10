using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    public class Users: IdentityUser
    {
        [ForeignKey("SystemUserId")]
        public int SystemUserId { get; set; }
        public SystemUsers SystemUser { get; set; }
       
        
    }
}
