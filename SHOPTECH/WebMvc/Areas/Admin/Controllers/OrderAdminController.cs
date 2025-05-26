using Application.Dtos.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]

    [Authorize(Policy = "RequireAdminRole")]

    public class OrderAdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;

        public OrderAdminController(HttpClient httpClient, IApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.ORDER_ADMIN());
                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<IEnumerable<IndexOrderDto>>();
                    ViewData["Orders"] = orders;
                }
                else
                {
                    throw new Exception("Lỗi khi lấy dữ liệu đơn hàng");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }
    }
}
