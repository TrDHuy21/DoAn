using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.LoginDtos;
using Application.Service.Interface;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Service.Implementation
{
    public class AuthService : IAuthService
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _passwordService = passwordService;
        }

        public string? GenerateToken(User User)
        {
            // Tạo claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                new Claim(ClaimTypes.Name, User.Name ?? ""),  
                new Claim(ClaimTypes.Role, User?.Role?.Name ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Tạo signing key
            var keyString = _configuration["Jwt:Key"];
            Console.WriteLine($"JWT Key: {keyString}"); // Kiểm tra key

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString ?? throw new Exception("Miss jwt key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated Token: {tokenString}"); // In token để kiểm tra
            return tokenString;
        }

        public async  Task<string?> GetToken(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                var user = await _unitOfWork.Users.GetByUserName(loginDto.Username);
                if (user == null)
                {
                    throw new InvalidOperationException("Username not exist");
                }
                if (!user.IsActive) 
                {
                    throw new InvalidOperationException("This Account is not active");
                }

                if (!_passwordService.VerifyPassword(user.HashedPassword, loginDto.Password))
                {
                    throw new InvalidOperationException("Password is wrong");
                }

                if (user.Password != loginDto.Password)
                {
                    throw new InvalidOperationException("Password is wrong");
                }
                if(user.RoleId != null)
                {
                    await _unitOfWork.Roles.GetByIdAsync(user.RoleId.Value);
                }

                string? token = GenerateToken(user);
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                throw new Exception(ex.Message);
            }
        }
    }
}
