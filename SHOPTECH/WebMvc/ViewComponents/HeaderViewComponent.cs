﻿using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "Header")]
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy ClaimsPrincipal từ HttpContext
                var user = _httpContextAccessor.HttpContext.User;

            // Kiểm tra người dùng đã đăng nhập chưa
            if (user.Identity.IsAuthenticated)
            {
                // Tạo model chứa thông tin người dùng
                var userInfo = new UserViewModel
                {
                    Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Name =user.FindFirst(ClaimTypes.Name)?.Value,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value,
                    Role = user.FindFirst(ClaimTypes.Role)?.Value
                };

                // Truyền thông tin người dùng qua ViewData
                ViewData["User"] = userInfo;
            }
            else
            {
                // Nếu chưa đăng nhập, đặt User = null hoặc giá trị khác để View biết
                ViewData["User"] = null;
            }

            return View();
        }
    }
}
