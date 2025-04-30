using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Helper;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Route("Admin/[controller]")]
    public class CategoryAdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryAdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync(CommonUrl.BRAND_ADMIN_PAGE(pageIndex, pageSize));
                var pageResult = await _httpClient.GetFromJsonAsync<PageResultDto<IndexCategoryDto>>(CommonUrl.CATEGORY_ADMIN_PAGE(pageIndex, pageSize));
                return View(pageResult);

            }
            catch (Exception ex)
            {
                return View(null);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var category = await _httpClient.GetFromJsonAsync<DetailCategoryDto>(CommonUrl.CATEGORY_ADMIN(id));
                if (category != null)
                {
                    return View(category);
                }
                else
                {
                    return NotFound("Category not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddCategoryDto addCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Model is not valid");
                }

                //  sử dụng MultipartFormDataContent để gửi cả data và file
                using (var multiContent = new MultipartFormDataContent())
                {
                    multiContent.Create(addCategoryDto);

                    // Gửi request
                    Console.WriteLine($"Sending request to: {CommonUrl.CATEGORY_ADMIN()}");
                    Console.WriteLine($"Content type: {multiContent.Headers.ContentType}");
                    var response = await _httpClient.PostAsync(CommonUrl.CATEGORY_ADMIN(), multiContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorContent?.Message);
                    }

                    //success
                    var categoryId = await response.Content.ReadFromJsonAsync<int>();
                    TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                    return RedirectToAction("Detail", new { id = categoryId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(addCategoryDto);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(CommonUrl.CATEGORY_ADMIN(id));
                //check response error
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                // check content
                var category = await response.Content.ReadFromJsonAsync<UpdateCategoryDto>() ?? throw new Exception("Error");
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Error: Model is not valid");
                }

                //  sử dụng MultipartFormDataContent để gửi cả data và file
                using (var multiContent = new MultipartFormDataContent())
                {
                    multiContent.Create(categoryDto);
                    // Gửi request
                    var response = await _httpClient.PutAsync(CommonUrl.CATEGORY_ADMIN(), multiContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorContent?.Message);
                    }

                    //success
                    TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công!";
                    var categoryId = await response.Content.ReadFromJsonAsync<int>();
                    return RedirectToAction("Detail", new { id = categoryId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(categoryDto);
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(CommonUrl.CATEGORY_ADMIN(id));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                TempData["SuccessMessage"] = "Xóa thương hiệu thành công!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";

            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }

        [HttpGet("Active/{id}")]
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            try
            {
                var response = await _httpClient.PutAsync(CommonUrl.CATEGORY_ADMIN_CHANGE_ACTIVE(id, isActive), null);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                TempData["SuccessMessage"] = "Thay đổi trạng thái thương hiệu thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }

    }
}
