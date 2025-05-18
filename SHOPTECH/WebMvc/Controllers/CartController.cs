using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    public class CartController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
