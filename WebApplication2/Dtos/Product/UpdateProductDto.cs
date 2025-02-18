﻿namespace WebApplication2.Dtos.Product
{
    public class UpdateProductDto
    {
        public int  Id { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }  
        public IFormFile? newImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
