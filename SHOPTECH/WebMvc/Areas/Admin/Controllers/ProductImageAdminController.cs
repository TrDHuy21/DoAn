using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using Application.Dtos.ProductImage;
using WebMvc.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class ProductImageAdminController : Controller
    {
        private readonly IApiService _apiService;

        public ProductImageAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/ProductImageAdmin/GetAll
        public async Task<IActionResult> GetAll()
        {
            var url = AdminApiString.PRODUCT_IMAGE_ADMIN_GETALL();
            var response = await _apiService.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IEnumerable<ProductImageDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(result);
        }

        // GET: /Admin/ProductImageAdmin/GetByProductId/5
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var url = AdminApiString.PRODUCT_IMAGE_ADMIN_GETBYPRODUCTID(productId);
            var response = await _apiService.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IEnumerable<ProductImageDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(result);
        }

        // POST: /Admin/ProductImageAdmin/Add
        [HttpPost]
        public async Task<IActionResult> Add(List<IFormFile> files, int productId)
        {
            var url = AdminApiString.PRODUCT_IMAGE_ADMIN_ADD(productId);

            using var form = new MultipartFormDataContent();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    // Key phải là "files" để backend nhận đúng List<IFormFile> files
                    form.Add(streamContent, "files", file.FileName);
                }
            }

            var response = await _apiService.PostAsync(url, form);
            if (response.IsSuccessStatusCode)
                return Json(new { success = true });
            return Json(new { success = false });
        }

        // PUT: /Admin/ProductImageAdmin/Update
        [HttpPost]
        public async Task<IActionResult> Update(ProductImageDto dto)
        {
            var url = AdminApiString.PRODUCT_IMAGE_ADMIN_UPDATE();
            var response = await _apiService.PutAsync(url, dto);
            if (response.IsSuccessStatusCode)
                return Json(new { success = true });
            return Json(new { success = false });
        }

        // DELETE: /Admin/ProductImageAdmin/Delete
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]ProductImageDto dto)
        {
            var url = AdminApiString.PRODUCT_IMAGE_ADMIN_DELETE();
            var response = await _apiService.DeleteAsync(url, dto);
            if (response.IsSuccessStatusCode)
                return Json(new { success = true });
            return Json(new { success = false });
        }
    }
}
