using Microsoft.AspNetCore.Mvc;
using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.User;
using WebApplication2.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Utilities;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = Constants.Manager)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;


        public UserController(IUserService userService)
        {
            _userservice = userService;

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllDto getAllDto)
        {
            try
            {
                return Ok(await _userservice.GetUsersByAsync(getAllDto));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _userservice.GetUserById(id));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateuser)
        {
            try
            {
                return Ok(await _userservice.UpdateUser(updateuser));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _userservice.DeleteUser(id));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
