namespace WebApplication2.Dtos.GetAll
{
    public class GetAllDto
    {
        public string searchTerm { get; set; } = "";
        public string sortBy { get; set; } = "";
        public int page { get; set; } = 1;
        public int limit { get; set; } = 10;
    }
}
