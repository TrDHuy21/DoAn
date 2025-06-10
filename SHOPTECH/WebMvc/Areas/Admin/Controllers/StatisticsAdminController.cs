using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class StatisticsAdminController : Controller
    {
        private readonly IApiService _apiService;

        public StatisticsAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ThongKeDoanhSo")]
        public IActionResult ThongKeDoanhSo()
        {
            return View();
        }

        public async Task<IActionResult> DoanhThuThang(int month, int year)
        {
            try
            {
                var result = await _apiService.GetAsync(AdminApiString.STATISTICS_ADMIN_DOANHTHUTHANG());
                if (!result.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Failed to retrieve data." });
                }
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
