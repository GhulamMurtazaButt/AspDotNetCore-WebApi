using AutoMapper;
using DataLibrary.Models;
using WebApplication2.Dtos.Category;
using WebApplication2.Dtos.Product;
using WebApplication2.Dtos.User;

namespace WebApplication2.Dtos.AutoMapperProfile
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Users, GetUserDto>();
            CreateMap<UpdateUserDto, SystemUsers>();
            CreateMap<UserRegisterDto, SystemUsers>();
            CreateMap<SystemUsers, Users>();
            CreateMap<SystemUsers, GetUserDto>()
            .ForMember(c => c.username, opt => opt.MapFrom(s => s.user.UserName))
            .ForMember(c => c.email, opt => opt.MapFrom(s => s.user.Email));
            CreateMap<AddProductDto, Products>();
            CreateMap<UpdateProductDto, Products>();
            CreateMap<Products, GetProductsDto>();
            CreateMap<UpdateProductDto, GetProductsDto>();
            CreateMap<UserRegisterDto, Users>();
            CreateMap<UpdateUserDto, Users>();
            CreateMap<Categories, GetCategoryDto>();
            CreateMap<AddCategoryDto, Categories>();

        }
    }
}
