using Application.Dtos.ProductDtos;
using Application.Models;
using Application.Service.Implementation;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAdminApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductAdminApiController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                var productDtos = _mapper.Map<IEnumerable<IndexProductDto>>(products);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var productDto = _mapper.Map<DetailProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Failed to retrieve product: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddProductDto productDto)
        {
            try
            {
                var product = await _productService.AddAsync(productDto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Failed to add product: {ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto productDto)
        {
            try
            {
                var product = await _productService.UpdateAsync(productDto);
                return Ok(product.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                if (!result)
                {
                    throw new Exception("Failed to delete category");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpGet("page/{pageIndex}")]
        public async Task<IActionResult> GetPageResult(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _productService.GetPageResultAsync(pageIndex, pageSize);
                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPatch("{id}/active")]
        public async Task<IActionResult> ChangeActive(int id, [FromBody] bool isActive)
        {
            try
            {
                var product = await _productService.ChangeActiveAsync(id, isActive);
                return Ok("Product status updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }
    }
}
