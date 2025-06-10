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

        [HttpGet]
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

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                Console.WriteLine(AdminApiString.ORDER_ADMIN(id));
                var response = await _apiService.GetAsync(AdminApiString.ORDER_ADMIN(id));
                 if(response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<DetailOrderDto>()
                        ?? throw new Exception();
                    ViewData["Order"] = order;
                    return View(order);
                } 
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        [HttpPost("updatestatus")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderDto updateOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }
                var response = await _apiService.PutAsync(CustomerApiString.ORDER_STATUS(), updateOrderDto);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        success = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Đã xảy ra lỗi"
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
      
}
