using Application.Dtos.ProductDetailDtos;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "ProductDetailList")]
    public class ProductDetailListViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;
        public ProductDetailListViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IViewComponentResult> InvokeAsync(string categoryName, Dictionary<string, string> queryParams)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.PRODUCT_DETAIL_FILTER(categoryName, queryParams));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to load product details.");
                }
                var productDetails = await response.Content.ReadFromJsonAsync<IEnumerable<IndexProductDetailDto>>()
                    ?? throw new Exception("Failed to load product details.");
                ViewData["ProductDetails"] = productDetails;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                Console.WriteLine(ex.Message);
                ViewData["ProductDetails"] = new List<IndexProductDetailDto>();
            }
            return View();
        }
    }
}
