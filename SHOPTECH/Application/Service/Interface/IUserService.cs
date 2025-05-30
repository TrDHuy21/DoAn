using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CategoryDtos;
using Application.Dtos;
using Domain.Enity;
using Application.Dtos.UserDtos;

namespace Application.Service.Interface
{
    public interface IUserService
    {
        Task<User?> AddAsync(AddUserDto userDto);
        Task<User?> UpdateAsync(UpdateUserDto UserDto);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>?> GetAllAsync();
        Task<PageResultDto<IndexUserDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<User?> ChangeActiveAsync(int id, bool isActive);
        Task<User> ChangeActive(int id, bool isActive);
        Task<User> GetMyProfile();
    }
}
