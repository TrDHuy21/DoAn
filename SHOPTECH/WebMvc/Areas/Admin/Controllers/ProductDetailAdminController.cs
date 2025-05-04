using Application.Dtos.ProductDetailDtos;
using Domain.Enity;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]
    public class ProductDetailAdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailAdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                var response = await _httpClient.GetAsync(CommonUrl.PRODUCT_DETAIL_ADMIN(id));
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
        public async Task<IActionResult> Create(int productId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddProductDetailDto productDetail)
        {
            if (ModelState.IsValid)
            {
                // Call the service to create the product detail
                return RedirectToAction("Index");
            }
            return View(productDetail);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(id);
        }
        [HttpPost]
        public IActionResult Edit(int id, UpdateProductDetailDto productDetail)
        {
            if (ModelState.IsValid)
            {
                // Call the service to update the product detail
                return RedirectToAction("Index");
            }
            return View(productDetail);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Call the service to delete the product detail
            return RedirectToAction("Index");
        }

    }
}
