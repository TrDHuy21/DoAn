using Application.Models;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeApiController : ControllerBase
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly IMapper _mapper;

        public ProductAttributeApiController(IProductAttributeService productAttributeService, IMapper mapper)
        {
            _productAttributeService = productAttributeService;
            _mapper = mapper;
        }

        [HttpGet("FilterMenu/{categoryName}")]
        public async Task<IActionResult> GetFilterMenu(string categoryName)
        {
            try
            {
                var filterMenus = await _productAttributeService.GetFilterMenu(categoryName);
                return Ok(filterMenus);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("CurrentFilter/{categoryName}")]
        public async Task<IActionResult> GetCurrentFilter(string categoryName,[FromQuery] Dictionary<string, string> queryParams = null)
        {
            try
            {
                var filterMenus = await _productAttributeService.GetCurrentFilter(categoryName, queryParams);
                return Ok(filterMenus);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}
