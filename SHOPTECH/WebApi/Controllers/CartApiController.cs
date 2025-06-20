using Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartApiController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _cartService.GetAysnc();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add/{productDetailId}")]
        public async Task<IActionResult> Add(int productDetailId)
        {
            try
            {
                var result = await _cartService.AddAysnc(productDetailId);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    throw new Exception("Lỗi");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
        }
        [HttpDelete("remove/{productDetailId}")]
        public async Task<IActionResult> Remove(int productDetailId)
        {
            try
            {
                await _cartService.RemoveAysnc(productDetailId);
                return Ok("Thanh cong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("change-quantity/{productDetailId}")]
        public async Task<IActionResult> ChangeQuantity(int productDetailId,[FromQuery] int quantity)
        {
            try
            {
                var result = await _cartService.ChangeQuantityAysnc(productDetailId, quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
