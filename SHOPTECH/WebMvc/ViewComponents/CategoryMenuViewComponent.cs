using Microsoft.AspNetCore.Mvc;

namespace WebMvc.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
