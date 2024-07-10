using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services.CategoriesService;
using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.Category;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Utilities;

namespace WebApplication2.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = $"{Constants.Manager},{Constants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllDto getAllDto)
        {
            try
            {
                return Ok(await _categoryService.GetCategoriesAsync(getAllDto));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Roles = Constants.Manager)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _categoryService.GetCategoryById(id));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Roles = Constants.Manager)]
        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryDto category)
        {
            try
            {
                return Ok(await _categoryService.AddCategory(category));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Roles = Constants.Manager)]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategory)
        {
            try
            {
                return Ok(await _categoryService.UpdateCategory(updateCategory));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Roles = $"{Constants.Manager},{Constants.Admin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isAdmin = User.IsInRole(Constants.Admin);
                return Ok(await _categoryService.DeleteCategory(id, isAdmin));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
