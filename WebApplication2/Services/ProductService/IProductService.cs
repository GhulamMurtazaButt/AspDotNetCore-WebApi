using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.Product;
using WebApplication2.Models;

namespace WebApplication2.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<GetProductsDto>> GetProductById(int id);
        Task<ServiceResponse<GetProductsDto>> AddProduct(AddProductDto addProduct);
        Task<ServiceResponse<GetProductsDto>> UpdateProduct(UpdateProductDto updateProduct);
        Task<ServiceResponse<GetProductsDto>> DeleteProduct(int id, bool isAdmin);
        Task<ServiceResponse<List<GetProductsDto>>> GetProductsAsync(GetAllDto getAllDto);
    }
}
