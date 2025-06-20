using Application.Dtos.OrderDetailDtos;
using Application.Dtos.OrderDtos;
using Application.Dtos.ProductDetailDtos;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IExportBill _exportBill;
        private readonly IMapper _mapper;

        public OrderApiController(IOrderService orderService, IExportBill exportBill, IMapper mapper)
        {
            _orderService = orderService;
            _exportBill = exportBill;
            _mapper = mapper;
        }




        /*
            1. getbyid
            2. getOrderOfCustomer(int customerId)
            3. getOrderOfEmployee(int employeeId)
            4. getAllOrder()
            5. getMyOrder()
         */

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetById(id);
                var orderDto = _mapper.Map<DetailOrderDto>(order);
                orderDto.OrderDetails = new List<IndexOrderDetailDto>();
                foreach (var item in order.OrderDetails)
                {
                    orderDto.OrderDetails.Add(new IndexOrderDetailDto()
                    {
                       Quantity = item.Quantity,
                       Price = item.Price,
                        ProductDetail = _mapper.Map<IndexProductDetailDto>(item.ProductDetail)
                    });
                }
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MyOrder")]
        [Authorize]
        public async Task<IActionResult> GetMyOrder()
        {
            try
            {
                var orders = await _orderService.GetMyOrder();
                var orderDtos = _mapper.Map<IEnumerable<IndexOrderDto>>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateOrder/online")]
        [Authorize(Policy = "RequireCustomerRole")]
        public async Task<IActionResult> CreateOrderOnline([FromBody] CreateOnlineOrderDto createOrderOnlineDto)
        {
            try
            {
                var order = await _orderService.CreateOnlineOrder(createOrderOnlineDto);
                var orderDto = _mapper.Map<DetailOrderDto>(order);

                await _orderService.SendNewOrderNotificationForCustomerAsync(order);
                await _orderService.SendNewOrderNotificationForAdminAsync(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("CreateOrder/offline")]
        [Authorize(Policy = "RequireSalerRole")]
        public async Task<IActionResult> Offline([FromBody] CreateOfflineOrderDto createOfflineOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOfflineOrder(createOfflineOrderDto);
                var orderDto = _mapper.Map<DetailOrderDto>(order);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);

                return BadRequest(new { Message = ex.Message });

            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderDto updateOrder)
        {
            try
            {
                var order = await _orderService.UpdateStatus(updateOrder);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("ExportBill/{id}")]
        public async Task<IActionResult> ExportBill(int id)
        {
            var pdfBytes = _exportBill.GenerateInvoicePdf(id);
            return File(pdfBytes, "application/pdf", $"HoaDon_{id}.pdf");
        }

    }
}
