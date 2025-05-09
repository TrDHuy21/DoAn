using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    public class ProductCategory : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
