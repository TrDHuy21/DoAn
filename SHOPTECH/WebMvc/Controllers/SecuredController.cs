using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuredController : ControllerBase
    {
        [HttpGet("authenticated")]
        [Authorize]
        public IActionResult Authenticated()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "Bạn đã được xác thực thành công!",
                UserId = userId,
                UserName = userName,
                Role = userRole
            });
        }

        // Action yêu cầu vai trò Admin
        [HttpGet("admin-only")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult AdminOnly()
        {
            return Ok(new { Message = "Bạn đã truy cập trang dành cho Admin!" });
        }

        // Action yêu cầu vai trò Saler
        [HttpGet("saler-only")]
        [Authorize(Policy = "RequireSalerRole")]
        public IActionResult SalerOnly()
        {
            return Ok(new { Message = "Bạn đã truy cập trang dành cho Saler!" });
        }

        // Action yêu cầu vai trò Customer
        [HttpGet("customer-only")]
        [Authorize(Policy = "RequireUserRole")]
        public IActionResult CustomerOnly()
        {
            return Ok(new { Message = "Bạn đã truy cập trang dành cho Customer!" });
        }
    }
}
