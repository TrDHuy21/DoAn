using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.LoginDtos;
using Domain.Enity;

namespace Application.Service.Interface
{
    public interface IAuthService
    {
        public string? GenerateToken(User User);
        Task<string?> GetToken(LoginDto loginDto);

    }
}
