namespace DataLibrary.Models
{
    public class SystemUsers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string City { get; set; }  
        public Users user { get; set; }


    }
}
