using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Dtos.ProductAttributeDtos;
using Application.Dtos.ProductDetailDtos;
using Application.Dtos.ProductDtos;
using Application.Helper;
using Application.Models;
using Domain.Enity;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Route("Admin/[controller]")]
    public class ProductAdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductAdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //index
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                //request to API server
                var response = await _httpClient.GetAsync(CommonUrl.PRODUCT_ADMIN_PAGE(pageIndex, pageSize));
                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                // check content
                var pageResult = await response.Content.ReadFromJsonAsync<PageResultDto<IndexProductDto>>() 
                    ?? throw new Exception("error");
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
                //get product
                var response = await _httpClient.GetAsync(CommonUrl.PRODUCT_ADMIN(id));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var product = await response.Content.ReadFromJsonAsync<DetailProductDto>() ?? throw new Exception("error");

                //get product detail of this product
                response = await _httpClient.GetAsync(CommonUrl.PRODUCT_DETAIL_ADMIN_PRODUCT(id));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var productDetails = await response.Content.ReadFromJsonAsync<List<IndexProductDetailDto>>() ?? throw new Exception("error");
                ViewData["ProductDetails"] = productDetails;

                //get product detail of this product
                response = await _httpClient.GetAsync(CommonUrl.PRODUCT_ATTRIBUTE_ADMIN_CATEGORY(product.CategoryId));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var productAttributes = await response.Content.ReadFromJsonAsync<List<IndexProductAttributeDto>>() ?? throw new Exception("error");
                ViewData["ProductAttributes"] = productAttributes;

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet("add")]
        public async Task<IActionResult >Add()
        {
            //get category
            var response = await _httpClient.GetAsync(CommonUrl.CATEGORY_ADMIN());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Lỗi trong quá trình lấy danh sách danh mục");
            }
            var categories = await response.Content.ReadFromJsonAsync<List<IndexCategoryDto>>() ?? throw new Exception("Lỗi trong quá trình lấy danh sách danh mục");
            ViewData["Categories"] = categories;

            //get brand
            response = await _httpClient.GetAsync(CommonUrl.BRAND_ADMIN());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Lỗi trong quá trình lấy danh sách thương hiệu");
            }
            var brands = await response.Content.ReadFromJsonAsync<List<IndexBrandDto>>() ?? throw new Exception("Lỗi trong quá trình lấy danh sách thương hiệu");
            ViewData["Brands"] = brands;

            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddProductDto addProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Model is not valid");
                }

                using(var multiContent = new MultipartFormDataContent())
                {
                    multiContent.Create(addProductDto);
                    var response = await _httpClient.PostAsync(CommonUrl.PRODUCT_ADMIN(), multiContent);
                    //check response
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorResponse?.Message);
                    }

                    // check content

                    var productId = await response.Content.ReadFromJsonAsync<int?>() ?? throw new Exception("error");
                    TempData["SuccessMessage"] = "Thêm sản phẩm mới thành công!";
                    return RedirectToAction("Detail", new { id = productId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                //get category
                var response = await _httpClient.GetAsync(CommonUrl.CATEGORY_ADMIN());
                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception("Lỗi trong quá trình lấy danh sách danh mục");
                }
                var categories = await response.Content.ReadFromJsonAsync<List<IndexCategoryDto>>() ?? throw new Exception("Lỗi trong quá trình lấy danh sách danh mục");
                ViewData["Categories"] = categories;

                //get brand
                response = await _httpClient.GetAsync(CommonUrl.BRAND_ADMIN());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Lỗi trong quá trình lấy danh sách thương hiệu");
                }
                var brands = await response.Content.ReadFromJsonAsync<List<IndexBrandDto>>() ?? throw new Exception("Lỗi trong quá trình lấy danh sách thương hiệu");
                ViewData["Brands"] = brands;

                //get product
                response = await _httpClient.GetAsync(CommonUrl.PRODUCT_ADMIN(id));
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var product = await response.Content.ReadFromJsonAsync<DetailProductDto>() ?? throw new Exception("error");
                var updateProduct = new UpdateProductDto() { 
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    MainImage = product.MainImage,
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                    FormFile = null // Chỉ định null cho FormFile
                };
                return View(updateProduct);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Model is not valid");
                }
                using(var multiContent = new MultipartFormDataContent())
                {
                    multiContent.Create(updateProductDto);
                    var response = await _httpClient.PutAsync(CommonUrl.PRODUCT_ADMIN(), multiContent);
                    //check response
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorResponse?.Message);
                    }
                    var productId = await response.Content.ReadFromJsonAsync<int?>() ?? throw new Exception("error");
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Detail", new { id = productId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(updateProductDto);
            }
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(CommonUrl.PRODUCT_ADMIN(id));
                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                // check content
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước

        }

        [HttpGet("Active/{id}")]
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(CommonUrl.PRODUCT_ADMIN_CHANGE_ACTIVE(id, isActive), id);
                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                // check content
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước

        }
    }
}
