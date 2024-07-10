namespace DataLibrary.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Products> Products { get; set; } 
    }
}
