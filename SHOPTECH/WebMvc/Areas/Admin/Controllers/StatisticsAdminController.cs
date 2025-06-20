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

    }
}
