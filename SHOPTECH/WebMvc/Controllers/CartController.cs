using Application.Dtos.CartDtos;
using Application.Dtos.ProductDetailDtos;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Controllers
{
    public class CartController : Controller
    {
        private readonly IApiService _apiService;

        public CartController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [Authorize]
        [HttpGet("cart")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.CART());
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                var cartItem = await response.Content.ReadFromJsonAsync<IEnumerable<CartDto>>() ?? throw new Exception("error");
                ViewData["cartItems"] = cartItem;
            }
            catch { 
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình lấy dữ liệu giỏ hàng.";
            }
            return View();
        }

        [Authorize]
        [HttpPost("cart/Add")]
        public async Task<IActionResult> Add(int productDetailId)
        {
            try
            {
                var response = await _apiService.PostAsync(CustomerApiString.CART_ADD(productDetailId), productDetailId);
                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình lấy dữ liệu giỏ hàng.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        [HttpPost("cart/ChangeQuantity/{productDetailId}")]
        public async Task<IActionResult> ChangeQuantity(int productDetailId,[FromQuery] int quantity)
        {
            try
            {
                var response = await _apiService.PutAsync(CustomerApiString.CART_CHANGE_QUANTITY(productDetailId, quantity), null);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình lấy dữ liệu giỏ hàng.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        [HttpPost("cart/remove/{productDetailId}")]
        public async Task<IActionResult> Remove(int productDetailId)
        {
            try
            {
                var response = await _apiService.DeleteAsync(CustomerApiString.CART_REMOVE(productDetailId), null);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình lấy dữ liệu giỏ hàng.";
                return BadRequest();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("cart/checkout")]
        public async Task<IActionResult> Checkout([FromBody]IEnumerable<CheckoutItem> checkoutItems)
        {
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.PRODUCT_DETAIL_CHECKOUT(checkoutItems.Select(x => x.productDetailId)));

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message);
                }

                var checkoutItems1 = await response.Content.ReadFromJsonAsync<IEnumerable<IndexProductDetailDto>>() ?? throw new Exception("error");

                foreach (var item in checkoutItems)
                {
                    item.ProductDetail = checkoutItems1.FirstOrDefault(x => x.Id == item.productDetailId);
                }
                ViewData["checkoutItems"] = checkoutItems;

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi";
            }
            return View();

        }
    }
}
