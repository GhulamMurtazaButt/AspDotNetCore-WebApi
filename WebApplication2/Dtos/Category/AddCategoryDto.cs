namespace WebApplication2.Dtos.Category
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string imageUrl { get; set; } = "";
        public IFormFile? image { get; set; }
    }
}
