using Microsoft.AspNetCore.Mvc;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "BrandMenu")]
    public class BrandMenuViewComponent : ViewComponent 
    {
        private readonly HttpClient _httpClient;

        public BrandMenuViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
}
