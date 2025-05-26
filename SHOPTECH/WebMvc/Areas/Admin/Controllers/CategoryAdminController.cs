using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Helper;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Route("Admin/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]

    public class CategoryAdminController : Controller
    {
        private readonly IApiService _apiService;

        public CategoryAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //request to API server
                var response = await _apiService.GetAsync(AdminApiString.CATEGORY_ADMIN());
                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                // check content
                var categories = await response.Content.ReadFromJsonAsync<List<IndexCategoryDto>>() ?? throw new Exception("error");
                ViewData["categories"] = categories;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.CATEGORY_ADMIN_PAGE(pageIndex, pageSize));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var pageResult = await response.Content.ReadFromJsonAsync<PageResultDto<IndexCategoryDto>>() ?? throw new Exception("error");

                return View(pageResult);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi";
            }
            return View(null);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.CATEGORY_ADMIN(id));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var category = await response.Content.ReadFromJsonAsync<DetailCategoryDto>() ?? throw new Exception("error");

                return View(category);
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
                    Console.WriteLine($"Sending request to: {AdminApiString.CATEGORY_ADMIN()}");
                    Console.WriteLine($"Content type: {multiContent.Headers.ContentType}");
                    var response = await _apiService.PostAsync(AdminApiString.CATEGORY_ADMIN(), multiContent);

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
                var response = await _apiService.GetAsync(AdminApiString.CATEGORY_ADMIN(id));
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
                    var response = await _apiService.PutAsync(AdminApiString.CATEGORY_ADMIN(), multiContent);

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
                var response = await _apiService.DeleteAsync(AdminApiString.CATEGORY_ADMIN(id), null);
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
                var response = await _apiService.PutAsync(AdminApiString.CATEGORY_ADMIN_CHANGE_ACTIVE(id, isActive), null);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                TempData["SuccessMessage"] = "Thay đổi trạng thái danh mục thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước
        }

    }
}
