using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Dtos;
using Application.Dtos.BrandDtos;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using Application.Helper;
using Application.Models;
using Domain.Enity;
using Azure;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]
    public class BrandAdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public BrandAdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                //request to API server
                var response = await _httpClient.GetAsync(CommonUrl.BRAND_ADMIN_PAGE(pageIndex, pageSize));

                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                // check content
                var pageResult = await response.Content.ReadFromJsonAsync<PageResultDto<IndexBrandDto>>() ?? throw new Exception("error");

                return View(pageResult);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(CommonUrl.BRAND_ADMIN(id));

                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                // check content
                var brand  = await response.Content.ReadFromJsonAsync<DetailBrandDto>() ?? throw new Exception("Error");

                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddBrandDto addBrandDto)
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
                    multiContent.Create(addBrandDto);

                    // Gửi request
                    var response = await _httpClient.PostAsync(CommonUrl.BRAND_ADMIN(), multiContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorContent?.Message);
                    }

                    var brandId = await response.Content.ReadFromJsonAsync<int>();
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction("Detail", new { id = brandId });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(addBrandDto);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                Console.WriteLine(CommonUrl.BRAND_ADMIN(id));
                var brand = await _httpClient.GetFromJsonAsync<UpdateBrandDto>(CommonUrl.BRAND_ADMIN(id));

                if (brand == null)
                {
                    throw new Exception("Brand not found");
                }

                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View();
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateBrandDto brandDto)
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
                    multiContent.Create(brandDto);
                    // Gửi request
                    var response = await _httpClient.PutAsync(CommonUrl.BRAND_ADMIN(), multiContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorContent?.Message);
                    }
                    //success
                    TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công!";
                    var brandId = await response.Content.ReadFromJsonAsync<int>();
                    return RedirectToAction("Detail", new { id = brandId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(brandDto);
            }

        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(CommonUrl.BRAND_ADMIN(id));
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
                Console.WriteLine(CommonUrl.BRAND_ADMIN_CHANGE_ACTIVE(id, isActive));
                var response = await _httpClient.PutAsync(CommonUrl.BRAND_ADMIN_CHANGE_ACTIVE(id, isActive), null);

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
