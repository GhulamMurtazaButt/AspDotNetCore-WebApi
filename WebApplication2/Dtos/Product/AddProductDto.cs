using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApplication2.Dtos.Product
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string imageUrl { get; set; } = "";
        public IFormFile? image { get; set; }    
        public int CategoryId { get; set; }
    }
}
