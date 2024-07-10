using AutoMapper;
using DataLibrary.Dtos.GetAll;
using DataLibrary.Models;
using DataLibrary.Repositry;
using WebApplication2.Dtos.Product;
using WebApplication2.Models;
using WebApplication2.Services.FileService;
using WebApplication2.Utilities;

namespace WebApplication2.Services.ProductService.Impl
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepositry<Products> _genericRepositry;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public ProductService(IGenericRepositry<Products> genericRepositry, IMapper mapper, IConfiguration configuration, IFileService fileService)
        {
            _genericRepositry = genericRepositry;
            _mapper = mapper;
            _configuration = configuration;
            _fileService = fileService;
        }
        public async Task<ServiceResponse<GetProductsDto>> GetProductById(int id)
        {
            ServiceResponse<GetProductsDto> newserviceResponse = new ServiceResponse<GetProductsDto>();
            try
            {
                Products getProduct = await _genericRepositry.GetByIdAsync(id);
                newserviceResponse.success = true;
                newserviceResponse.data = _mapper.Map<GetProductsDto>(getProduct);
                if (newserviceResponse.data == null)
                {
                    throw new Exception(MessageStrings.ProductNotFound);
                }
            }catch(Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }
        public async Task<ServiceResponse<GetProductsDto>> AddProduct(AddProductDto addProduct)
        {
            ServiceResponse<GetProductsDto> newserviceResponse = new ServiceResponse<GetProductsDto>();
            try
            {
                Products newProduct = await _genericRepositry.AddAsync(_mapper.Map<Products>(addProduct));
                if (addProduct.image != null)
                {
                    newProduct.ImgUrl = _fileService.AddFile(addProduct.image); 
                }
                else
                {
                    newProduct.ImgUrl = Constants.imgAttachmsg;
                }

                if (newProduct!= null)
                {
                     _genericRepositry.SaveChanges();
                    newserviceResponse.success = true;
                    newserviceResponse.data = _mapper.Map<GetProductsDto>(newProduct);
                    newserviceResponse.message = MessageStrings.ProductAddedSuccess;

                }else{
                    throw new Exception(MessageStrings.ProductAddedFail);
                }
            }
            catch (Exception ex)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = ex.Message;
            }
            return newserviceResponse;
        }
        public async Task<ServiceResponse<GetProductsDto>> UpdateProduct(UpdateProductDto updateProduct)
        {
            ServiceResponse<GetProductsDto> newserviceResponse = new ServiceResponse<GetProductsDto>();
            try
            {
                Products checkProduct = await _genericRepositry.GetByIdAsync(updateProduct.Id);
                if (checkProduct != null)
                {
                    checkProduct.Name = updateProduct.Name;
                    checkProduct.Description = updateProduct.Description;
                    checkProduct.Price = updateProduct.Price;
                    checkProduct.CategoryId = updateProduct.CategoryId;
                    if (updateProduct.newImageUrl != null)
                    {
                        string fileName = Path.GetFileName(checkProduct.ImgUrl.Replace("\\", "/"));
                        string oldFileName = fileName.Split('/').Last();
                        string newImageUrl = _fileService.UpdateFile(oldFileName, updateProduct.newImageUrl);
                        checkProduct.ImgUrl = newImageUrl;
                    }

                    _genericRepositry.SaveChanges();
                    newserviceResponse.success = true;
                    newserviceResponse.data = _mapper.Map<GetProductsDto>(checkProduct);
                    newserviceResponse.message = MessageStrings.ProductUpdatedSuccess;
                }
                else
                {
                    throw new Exception(MessageStrings.ProductNotFound);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

        public async Task<ServiceResponse<GetProductsDto>> DeleteProduct(int id, bool isAdmin)
        {
            ServiceResponse<GetProductsDto> newserviceResponse = new ServiceResponse<GetProductsDto>();
            try
            {
                Products product = await _genericRepositry.GetByIdAsync(id);
                if (product != null)
                {
                    if (isAdmin)
                    {
                        product.IsDeleted = true;
                        _genericRepositry.SaveChanges();
                        newserviceResponse.success = true;
                        newserviceResponse.message = MessageStrings.SoftDeletedSuccess;
                    }
                    else
                    {
                        await _genericRepositry.DeleteAsync(product);
                        string fileName = Path.GetFileName(product.ImgUrl.Replace("\\", "/"));
                        string oldFileName = fileName.Split('/').Last();
                        _fileService.DeleteFile(oldFileName);
                        _genericRepositry.SaveChanges();
                        newserviceResponse.success = true;
                        newserviceResponse.data = _mapper.Map<GetProductsDto>(product);
                        newserviceResponse.message = MessageStrings.ProductDeletedSuccess;
                    }
                }
                else
                {
                    throw new Exception(MessageStrings.ProductNotFound);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }
        public async Task<ServiceResponse<List<GetProductsDto>>> GetProductsAsync(GetAllDto getAllDto)
        {
            ServiceResponse<List<GetProductsDto>> newserviceResponse = new ServiceResponse<List<GetProductsDto>>();
            try
            {
                List<Products> products = await _genericRepositry.GetAllAsync(getAllDto);
                newserviceResponse.success = true;
                newserviceResponse.data = products.Select(c => _mapper.Map<GetProductsDto>(c)).ToList();
            }
            catch(Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }

            return newserviceResponse;
          

        }
    }
}
