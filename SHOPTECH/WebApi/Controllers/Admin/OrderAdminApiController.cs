using Application.Dtos.OrderDtos;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]

    public class OrderAdminApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderAdminApiController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? status = null)
        {
            try
            {
                var orders = await _orderService.GetAll(status);
                var orderDtos = _mapper.Map<IEnumerable<IndexOrderDto>>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllByCustomerId/{customerId}")]
        public async Task<IActionResult> GetAllByCustomerId(int customerId)
        {
            try
            {
                var orders = await _orderService.GetAllByCustomerId(customerId);
                var orderDtos = _mapper.Map<IEnumerable<DetailOrderDto>>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetAllByEmployeeId(int employeeId)
        {
            try
            {
                var orders = await _orderService.GetAllByEmployeeId(employeeId);
                var orderDtos = _mapper.Map<IEnumerable<DetailOrderDto>>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
