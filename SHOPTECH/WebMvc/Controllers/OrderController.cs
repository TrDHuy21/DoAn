using Application.Dtos.OrderDtos;
using Domain.Enity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Controllers
{
    [Route("Order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;

        public OrderController(HttpClient httpClient, IApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            //call api to get order by id
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.ORDER(id));
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<DetailOrderDto>();
                    ViewData["Order"] = order;
                    return View(order);
                }
                else
                {
                    throw new Exception("Lỗi khi lấy dữ liệu đơn hàng");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi trong quá trình lấy dữ liệu";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        [HttpPost("CreateOnlineOrder")]
        public async Task<IActionResult> CreateOnlineOrder(CreateOnlineOrderDto createOnlineOrderDto)
        {
            try
            {
                Console.WriteLine(CustomerApiString.ORDER_CREATE_ONLINE());
                var response = await _apiService.PostAsync(CustomerApiString.ORDER_CREATE_ONLINE(), createOnlineOrderDto);
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<DetailOrderDto>();
                    return RedirectToAction("Detail", "Order", new { id = order.Id });
                }
                else
                {
                    throw new Exception("Lỗi khi tạo đơn hàng");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Redirect(Request.Headers["Referer"].ToString());

        }

        [HttpPost("CreateOfflineOrder")]
        public async Task<IActionResult> CreateOfflineOrder(CreateOfflineOrderDto createOfflineOrderDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new Exception("Lỗi khi tạo đơn hàng");
                }
                Console.WriteLine(CustomerApiString.ORDER_CREATE_OFFLINE());
                var response = await _apiService.PostAsync(CustomerApiString.ORDER_CREATE_OFFLINE(), createOfflineOrderDto);
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<DetailOrderDto>();
                    return RedirectToAction("Detail", "Order", new { id = order.Id });
                }
                else
                {
                    throw new Exception("Lỗi khi tạo đơn hàng");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Redirect(Request.Headers["Referer"].ToString());

        }

        // get my orders
        [HttpGet("MyOrders")]
        public async Task<IActionResult> MyOrders(int? status)
        {
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.ORDER_MY_ORDERS());
                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<IEnumerable<IndexOrderDto>>();
                    ViewData["Orders"] = orders;
                    return View(orders);
                }
                else
                {
                    throw new Exception("Lỗi khi lấy danh sách đơn hàng");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(UpdateOrderDto updateOrderDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new Exception();
                }
                updateOrderDto.Status = 8;
                var response = await _apiService.PutAsync(CustomerApiString.ORDER_STATUS(), updateOrderDto);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        success = true
                    });
                } else
                {
                    return Ok(new {
                        success = false,
                        message = "Đã xảy ra lỗi"
                    });
                }
                
            }
            catch (Exception ex)
            { 
                return BadRequest();
            }
        }

    }
}
