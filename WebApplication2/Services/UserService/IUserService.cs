using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.User;
using WebApplication2.Models;

namespace WebApplication2.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<GetUserDto>>> GetUsersByAsync(GetAllDto getAllDto);
        Task<ServiceResponse<GetUserDto>> GetUserById(int id);
        Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateuser);
        Task<ServiceResponse<GetUserDto>> DeleteUser(int id);
    }
}
