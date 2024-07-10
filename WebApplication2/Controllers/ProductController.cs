using DataLibrary.Dtos.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos.Product;
using WebApplication2.Services.ProductService;
using WebApplication2.Utilities;

namespace WebApplication2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

       [Authorize(Roles = $"{Constants.Manager},{Constants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllDto getAllDto)
        {
            try
            {
                return Ok(await _productService.GetProductsAsync(getAllDto));
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
                return Ok(await _productService.GetProductById(id));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

       [Authorize(Roles = Constants.Manager)]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto addProduct)
        {
            try
            {
                return Ok(await _productService.AddProduct(addProduct));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Roles = Constants.Manager)]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProduct)
        {
            try
            {
                return Ok(await _productService.UpdateProduct(updateProduct));
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
                return Ok(await _productService.DeleteProduct(id, isAdmin));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
