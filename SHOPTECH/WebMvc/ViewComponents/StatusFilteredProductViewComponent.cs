using Application.Dtos.ProductDetailDtos;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.ViewComponents.Home
{
    [ViewComponent(Name = "StatusFilteredProduct")]
    public class StatusFilteredProductViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public StatusFilteredProductViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName = null, string filter = null, string name = null)
        {
            TempData["Name"] = name;
            try
            {
                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                if (filter != null)
                {

                    queryParams.Add(filter, "true");
                }
                var response = await _httpClient.GetAsync(CustomerApiString.PRODUCT_DETAIL_FILTER(categoryName, queryParams));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Loi trong qua trinh lay san pham tu server");
                }

                var products = await response.Content.ReadFromJsonAsync<IEnumerable<IndexProductDetailDto>>()
                    ?? throw new Exception("Loi trong qua trinh lay san pham " + categoryName);
                return View(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

        }
    }
}
