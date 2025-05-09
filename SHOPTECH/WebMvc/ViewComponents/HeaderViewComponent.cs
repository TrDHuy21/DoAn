using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "Header")]
    public class HeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
