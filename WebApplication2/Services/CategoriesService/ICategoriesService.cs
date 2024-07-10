using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.Category;
using WebApplication2.Models;

namespace WebApplication2.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto addCategory);
        Task<ServiceResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto updateCategory);
        Task<ServiceResponse<GetCategoryDto>> DeleteCategory(int id, bool isAdmin);
        Task<ServiceResponse<List<GetCategoryDto>>> GetCategoriesAsync(GetAllDto getAllDto);
    }
}
