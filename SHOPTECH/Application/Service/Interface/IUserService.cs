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
    internal interface IUserService
    {
        Task<User?> AddAsync(AddUserDto categoryDto);
        Task<User?> UpdateAsync(UpdateUserDto categoryDto);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>?> GetAllAsync();
        Task<PageResultDto<IndexUserDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<User?> ChangeActiveAsync(int id, bool isActive);
    }
}
