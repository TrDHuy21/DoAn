using Application.Dtos.LoginDtos;
using Application.Models;
using Application.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _authService.GetToken(loginDto);
                return Ok(new TokenResponse { 
                    Token = token,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { 
                    Message = ex.Message,
                });
            }
            
        }
    }
}
