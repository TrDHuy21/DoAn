using Application.Dtos.CategoryDtos;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "CategoryMenu")]
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public CategoryMenuViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.CATEGORY());
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadFromJsonAsync<ErrorResponse>()
                        ?? throw new Exception("error while get errorMessage");
                    throw new Exception(errorMessage.Message);
                }

                var categories = await response.Content.ReadFromJsonAsync<IEnumerable<IndexCategoryDto>>()
                        ?? throw new Exception("error while get categories");
                ViewData["Categories"] = categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ViewData["Categories"] = new List<IndexCategoryDto>();
                TempData["Error Message"] = "Có lỗi xảy ra khi tải danh sách danh mục. Vui lòng thử lại sau.";
            }

            return View();
        }
    }
}
