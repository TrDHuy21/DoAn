using Application.Dtos.BrandDtos;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

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
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.BRAND());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to load brands.");
                }
                var brands = await response.Content.ReadFromJsonAsync<IEnumerable<IndexBrandDto>>()
                    ?? throw new Exception("Failed to load brands.");
                return View(brands);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                Console.WriteLine(ex.Message);
                return View();
            }
        }
    }
}
