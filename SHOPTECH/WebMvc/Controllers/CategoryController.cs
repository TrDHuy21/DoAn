using Application.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PartialCategoryMenu()
        {
            IEnumerable<IndexCategoryDto> categoryDtos = null;
            return PartialView("_PartialCategoryMenu", categoryDtos);
        }
    }
}
