using Microsoft.AspNetCore.Mvc;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "HotProduct")]
    public class HotProductViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
