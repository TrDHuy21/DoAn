using Application.Dtos.ProductDetailDtos;
using Application.Models;
using Domain.Enity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]

    public class ProductDetailAdminController : Controller
    {
        private readonly IApiService _apiService;

        public ProductDetailAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.PRODUCT_DETAIL_ADMIN(id));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Product detail not found.");

                }

                var productDetail = await response.Content.ReadFromJsonAsync<DetailProductDetailDto>()
                    ?? throw new Exception("Error while get product detail");
                return View(productDetail);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet("{productId}")]
        public IActionResult Create(int productId)
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(AddProductDetailDto productDetail)
        {
            if (ModelState.IsValid)
            {
                // Call the service to create the product detail
                return RedirectToAction("Index");
            }
            return View(productDetail);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            return View(id);
        }
        [HttpPost("edit")]
        public IActionResult Edit(int id, UpdateProductDetailDto productDetail)
        {
            if (ModelState.IsValid)
            {
                // Call the service to update the product detail
                return RedirectToAction("Index");
            }
            return View(productDetail);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            // Call the service to delete the product detail
            return RedirectToAction("Index");
        }

        [HttpGet("Active/{id}")]
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            try
            {
                var response = await _apiService.PutAsync(AdminApiString.PRODUCT_DETAIL_ADMIN_CHANGE_ACTIVE(id, isActive), id);
                //check response
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                // check content
                TempData["SuccessMessage"] = "Thay đổi trạng thái sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return Redirect(Request.Headers["Referer"].ToString()); // Quay lại trang trước

        }
    }
}
