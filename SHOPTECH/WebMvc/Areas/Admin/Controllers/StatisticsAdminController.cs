using Application.Dtos.CategoryDtos;
using Application.Models;
using System.Net.Http;
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

        [HttpGet("ThongKeTongQuan")]
        public IActionResult ThongKeTongQuan()
        {
            return View();
        }

        [HttpGet("ThongKeNhanVien")]
        public IActionResult ThongKeNhanVien()
        {
            return View();
        }

        [HttpGet("ThongKeSanPham")]
        public async Task<IActionResult> ThongKeSanPham()
        {
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.CATEGORY());
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadFromJsonAsync<ErrorResponse>()
                        ?? throw new Exception("error while get errorMessage");
                    throw new Exception(errorMessage.Message);
                }

                var categories = await response.Content.ReadFromJsonAsync<IEnumerable<IndexCategoryDto>>()
                        ?? throw new Exception("error while get categories");
                ViewData["Categories"] = categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ViewData["Categories"] = new List<IndexCategoryDto>();
                TempData["Error Message"] = "Có lỗi xảy ra khi tải danh sách danh mục. Vui lòng thử lại sau.";
            }

            return View();
        }

    }
}
