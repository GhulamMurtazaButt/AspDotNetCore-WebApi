using AutoMapper;
using DataLibrary.Dtos.GetAll;
using DataLibrary.Models;
using DataLibrary.Repositry;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebApplication2.Dtos.Category;
using WebApplication2.Models;
using WebApplication2.Utilities;

namespace WebApplication2.Services.CategoriesService.Impl
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IGenericRepositry<Categories> _genericRepositry;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CategoriesService(IGenericRepositry<Categories> genericRepositry, IMapper mapper, IConfiguration configuration)
        {
            _genericRepositry = genericRepositry;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id)
        {
            ServiceResponse<GetCategoryDto> newserviceResponse = new ServiceResponse<GetCategoryDto>();
            try
            {
                Categories getCategoryById = await _genericRepositry.GetByIdAsync(id);
                if (getCategoryById != null)
                {
                    newserviceResponse.success = true;
                    newserviceResponse.data = _mapper.Map<GetCategoryDto>(getCategoryById);
                }
                else
                {
                    throw new Exception(MessageStrings.ProductNotFound);
                }
            }catch (Exception exp) {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto addCategory)
        {
            ServiceResponse<GetCategoryDto> newserviceResponse = new ServiceResponse<GetCategoryDto>();
            try
            {
                Categories addnewCategory = await _genericRepositry.AddAsync(_mapper.Map<Categories>(addCategory));
                if (addCategory.image != null)
                {
                    string folder_name = _configuration.GetSection(Constants.imagePath).Value;
                    string filePath = Path.Combine(folder_name, addCategory.image.FileName);
                    addCategory.image.CopyTo(new FileStream(filePath, FileMode.Create));
                    string imageUrl = _configuration.GetSection(Constants.imageUrl).Value;
                    string relativePath = Path.Combine(imageUrl, addCategory.image.FileName);


                    addnewCategory.ImgUrl = relativePath;
                }
                else
                {
                    addnewCategory.ImgUrl = Constants.imgAttachmsg;
                }
                if (addnewCategory != null) {
                    _genericRepositry.SaveChanges();
                    newserviceResponse.success = true;
                    newserviceResponse.data = _mapper.Map<GetCategoryDto>(addnewCategory);
                    newserviceResponse.message = MessageStrings.CategoryAddedSuccess;

                }
                else {
                    throw new Exception(MessageStrings.CategoryAddedFail);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto updateCategory)
        {
            ServiceResponse<GetCategoryDto> newserviceResponse = new ServiceResponse<GetCategoryDto>();
            try
            {
                Categories checkCategory = await _genericRepositry.GetByIdAsync(updateCategory.Id);
                if (checkCategory != null)
                {
                    checkCategory.Name = updateCategory.Name;
                    _genericRepositry.SaveChanges();
                    newserviceResponse.data = _mapper.Map<GetCategoryDto>(checkCategory);
                    newserviceResponse.message = MessageStrings.CategoryUpdatedSuccess;
                }else{
                    throw new Exception(MessageStrings.CategoryNotFound);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryDto>> DeleteCategory(int id, bool isAdmin)
        {
            ServiceResponse<GetCategoryDto> newserviceResponse = new ServiceResponse<GetCategoryDto>();
            try
            {
                Categories category = await _genericRepositry.GetByIdAsync(id);
                if (category != null)
                {
                    if (isAdmin)
                    {
                        category.IsDeleted = true;
                        _genericRepositry.SaveChanges();
                        newserviceResponse.success = true;
                        newserviceResponse.message = MessageStrings.SoftDeletedSuccess;
                    }
                    else
                    {
                        await _genericRepositry.DeleteAsync(category);
                        _genericRepositry.SaveChanges();
                        newserviceResponse.data = _mapper.Map<GetCategoryDto>(category);
                        newserviceResponse.message = MessageStrings.CategoryDeletedSuccess;
                    }
                }
                else
                {
                    throw new Exception(MessageStrings.CategoryNotFound);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

        public async Task<ServiceResponse<List<GetCategoryDto>>> GetCategoriesAsync(GetAllDto getAllDto)
        {
            ServiceResponse<List<GetCategoryDto>> newserviceResponse = new ServiceResponse<List<GetCategoryDto>>();
            try
            {
                List<Categories> categories = await _genericRepositry.GetAllAsync(getAllDto);
                newserviceResponse.success = true;
                newserviceResponse.data = categories.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();
                
            }catch(Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

       
    }
}
