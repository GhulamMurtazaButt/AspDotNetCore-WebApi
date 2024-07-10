using AutoMapper;
using DataLibrary.Models;
using DataLibrary.Repositry;
using DataLibrary.Dtos.GetAll;
using WebApplication2.Dtos.User;
using WebApplication2.Models;
using WebApplication2.Utilities;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Services.UserService.Impl
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositry<SystemUsers> _genericRepositry;

        public UserService(IMapper mapper, IGenericRepositry<SystemUsers> genericRepositry)
        {

            _mapper = mapper;
            _genericRepositry = genericRepositry;
        }
        public async Task<ServiceResponse<List<GetUserDto>>> GetUsersByAsync(GetAllDto getAllDto)
        {
            ServiceResponse<List<GetUserDto>> newserviceResponse = new ServiceResponse<List<GetUserDto>>();
            try
            {
                List<SystemUsers> systemUsers = _genericRepositry.GetAllWithInclude(getAllDto).Include(x => x.user).ToList();
                if (systemUsers != null)
                {
                    newserviceResponse.success = true;
                    newserviceResponse.data = systemUsers.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
                }
                else
                {
                    newserviceResponse.success = true;
                    newserviceResponse.message = MessageStrings.UserNotFound;
                }
            }catch(Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }
        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            ServiceResponse<GetUserDto> newserviceResponse = new ServiceResponse<GetUserDto>();
            try
            {
                SystemUsers systemUser = _genericRepositry.GetByIdWithInclude().Include(x => x.user).First(x => x.Id == id);   

                if (systemUser != null)
                {
                    newserviceResponse.success = true;
                    newserviceResponse.data = _mapper.Map<GetUserDto>(systemUser);
                }
                else
                {
                    newserviceResponse.success = true;
                    newserviceResponse.message = MessageStrings.UserNotFound;
                }
            }
            catch(Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }

            return newserviceResponse;
        }
        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateuser)
        {
            ServiceResponse<GetUserDto> newserviceResponse = new ServiceResponse<GetUserDto>();
            try
            {
                SystemUsers user = await _genericRepositry.GetByIdAsync(updateuser.Id);
                if(user != null)
                {
                    user.Name = updateuser.Name;
                    user.Address = updateuser.Address;
                    user.CNIC = updateuser.CNIC;
                    user.City = updateuser.City;
                    _genericRepositry.SaveChanges();
                    newserviceResponse.success = true;
                    newserviceResponse.message = MessageStrings.UserUpdatedSuccess;
                }else{
                    throw new Exception(MessageStrings.UserNotFound);
                }

            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }
        public async Task<ServiceResponse<GetUserDto>> DeleteUser(int id)
        {
            ServiceResponse<GetUserDto> newserviceResponse = new ServiceResponse<GetUserDto>();
            try
            {
                SystemUsers user = await _genericRepositry.GetByIdAsync(id);
                if (user != null)
                {
                    await _genericRepositry.DeleteAsync(user);
                    _genericRepositry.SaveChanges();
                    GetUserDto getUser = _mapper.Map<GetUserDto>(user);
                    newserviceResponse.data = getUser;
                    newserviceResponse.message = MessageStrings.UserDeletedSuccess;
                }
                else
                {
                    throw new Exception(MessageStrings.UserNotFound);
                }
            }
            catch (Exception exp)
            {
                newserviceResponse.success = false;
                newserviceResponse.message = exp.Message;
            }
            return newserviceResponse;
        }

      
    }
}
