using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.UserDtos;
using Application.Service.Interface;
using Domain.Enity;

namespace Application.Service.Implementation
{
    public class UserService : IUserService
    {
        public Task<User?> AddAsync(AddUserDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<User?> ChangeActiveAsync(int id, bool isActive)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PageResultDto<IndexUserDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateAsync(UpdateUserDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
