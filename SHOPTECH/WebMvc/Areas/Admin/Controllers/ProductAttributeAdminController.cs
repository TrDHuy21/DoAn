using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Areas.Admin.Controllers
{
    public class ProductAttributeAdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
