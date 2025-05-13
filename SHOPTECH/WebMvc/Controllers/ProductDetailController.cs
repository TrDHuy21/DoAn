using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{category}")]
        public async Task<IActionResult> GetListProductDetailWithCategory(string category, Dictionary<string, string> queryParams = null)
        {
            ViewData["Category"] = category;
            ViewData["QueryParams"] = queryParams;
            return View();
        }
       
    }
}
