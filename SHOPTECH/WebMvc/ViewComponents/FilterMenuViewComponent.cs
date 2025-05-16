using Application.Dtos.ProductAttributeDtos;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "FilterMenu")]
    public class FilterMenuViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public FilterMenuViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName, Dictionary<string, string> queryParams)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.Filtermenu(categoryName));
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadFromJsonAsync<ErrorResponse>()
                        ?? throw new Exception("error while get errorMessage");
                    throw new Exception(errorMessage.Message);
                }
                var filterMenus = await response.Content.ReadFromJsonAsync<IEnumerable<FilterMenuDto>>()
                        ?? throw new Exception("error while get filterMenus");

                var response2 = await _httpClient.GetAsync(CustomerApiString.CurrentFilter(categoryName, queryParams));
                if (!response2.IsSuccessStatusCode)
                {
                    var errorMessage = await response2.Content.ReadFromJsonAsync<ErrorResponse>()
                        ?? throw new Exception("error while get errorMessage");
                    throw new Exception(errorMessage.Message);
                }
                var queryParamsMenu = await response2.Content.ReadFromJsonAsync<IEnumerable<FilterMenuDto>>()
                        ?? throw new Exception("error while get currentFilter");

                ViewData["ProductAttributes"] = filterMenus;
                ViewData["QueryParams"] = queryParamsMenu;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                TempData["Error Message"] = "Có lỗi xảy ra khi tải danh sách bộ lọc. Vui lòng thử lại sau.";
            }
            ViewData["CategoryName"] = categoryName;
            return View();
        }
    }
}
