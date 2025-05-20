using System.Net.Http;
using Application.Models;
using Domain.Enity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]

    public class ProductAttributeAdminController : Controller
    {
        private readonly IApiService _apiService;

        public ProductAttributeAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("ChangeActive/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeActive(int id, [FromBody] bool isActive)
        {
            try
            {
                var response = await _apiService.PutAsync(AdminApiString.PRODUCT_ATTRIBUTE_ADMIN_CHANGE_ACTIVE(id, isActive), null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                TempData["SuccessMessage"] = "Thay đổi trạng thái thương hiệu thành công!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }

        // PUT: api/ProductAttributeAdminApi/ChangeDisplay/5
        [HttpPost("ChangeDisplay/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeDisplay(int id, [FromBody] bool isDisplay)
        {
            try
            {
                var response = await _apiService.PutAsync(AdminApiString.PRODUCT_ATTRIBUTE_ADMIN_CHANGE_DISPLAY(id, isDisplay), null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                TempData["SuccessMessage"] = "Thay đổi trạng thái thương hiệu thành công!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }

        // PUT: api/ProductAttributeAdminApi/ChangeFilter/5
        [HttpPost("ChangeFilter/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeFilter(int id, [FromBody] bool isFilter)
        {
            try
            {
                var response = await _apiService.PutAsync(AdminApiString.PRODUCT_ATTRIBUTE_ADMIN_CHANGE_FILTER(id, isFilter), null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                TempData["SuccessMessage"] = "Thay đổi trạng thái thương hiệu thành công!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }
    }
}
