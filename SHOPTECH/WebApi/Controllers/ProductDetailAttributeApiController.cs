using Application.Models;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailAttributeApiController : ControllerBase
    {
        private readonly IProductDetailAttributeService _productDetailAttributeService;
        private readonly IMapper _mapper;
        public ProductDetailAttributeApiController(IProductDetailAttributeService productDetailAttributeService, IMapper mapper)
        {
            _productDetailAttributeService = productDetailAttributeService;
            _mapper = mapper;
        }
        [HttpGet("{productDetailId}/{productAttributeId}")]
        public async Task<IActionResult> GetByProductId(int productDetailId, int productAttributeId)
        {
            try
            {
                var productAttributes = await _productDetailAttributeService.GetByProductDetailIdAndProductAttributeId(productDetailId, productAttributeId);
                return Ok(productAttributes);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var productAttributes = await _productDetailAttributeService.GetAllAsync();

                var productAttributeDtos = _mapper.Map<IEnumerable<ProductDetailAttribute>>(productAttributes);
                return Ok(productAttributeDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}
