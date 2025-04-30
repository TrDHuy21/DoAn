using Application.Dtos.ProductAttributeDtos;
using Application.Models;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeAdminApiController : ControllerBase
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly IMapper _mapper;

        public ProductAttributeAdminApiController(IProductAttributeService productAttributeService, IMapper mapper)
        {
            _productAttributeService = productAttributeService;
            _mapper = mapper;
        }



        // GET: api/ProductAttribute
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAttribute>>> GetAllProductAttributes()
        {
            try
            {
                var productAttributes = await _productAttributeService.GetAllAsync();
                var productAttributeDtos = _mapper.Map<IEnumerable<IndexProductAttributeDto>>(productAttributes);
                return Ok(productAttributeDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // GET: api/ProductAttribute/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAttribute>> GetProductAttribute(int id)
        {
            try
            {
                var productAttribute = await _productAttributeService.GetByIdAsync(id);
                var productAttributeDto = _mapper.Map<DetailProductAttributeDto>(productAttribute);
                return Ok(productAttributeDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // POST: api/ProductAttribute
        [HttpPost]
        public async Task<ActionResult<ProductAttribute>> CreateProductAttribute([FromBody] AddProductAttributeDto productAttributeDto)
        {
            try
            {
                var createdProductAttribute = await _productAttributeService.AddAsync(productAttributeDto);
                return CreatedAtAction(nameof(GetProductAttribute), new { id = createdProductAttribute.Id }, createdProductAttribute.Id);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // PUT: api/ProductAttribute
        [HttpPut]
        public async Task<ActionResult<ProductAttribute>> UpdateProductAttribute([FromBody] UpdateProductAttributeDto productAttributeDto)
        {
            try
            {
                var updatedProductAttribute = await _productAttributeService.UpdateAsync(productAttributeDto);
                return Ok(updatedProductAttribute.Id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // DELETE: api/ProductAttribute/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAttribute(int id)
        {
            try
            {
                await _productAttributeService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // PATCH: api/ProductAttribute/ChangeActive/5
        [HttpPatch("ChangeActive/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeActiveStatus(int id, [FromBody] bool isActive)
        {
            try
            {
                var updatedProductAttribute = await _productAttributeService.ChangeActiveAsync(id, isActive);
                return Ok(updatedProductAttribute.Id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // PATCH: api/ProductAttribute/ChangeDisplay/5
        [HttpPatch("ChangeDisplay/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeDisplayStatus(int id, [FromBody] bool isDisplay)
        {
            try
            {
                var updatedProductAttribute = await _productAttributeService.ChangeDisplayAsync(id, isDisplay);
                return Ok(updatedProductAttribute.Id);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        // PATCH: api/ProductAttribute/ChangeFilter/5
        [HttpPatch("ChangeFilter/{id}")]
        public async Task<ActionResult<ProductAttribute>> ChangeFilterStatus(int id, [FromBody] bool isFilter)
        {
            try
            {
                var updatedProductAttribute = await _productAttributeService.ChangeFilterAsync(id, isFilter);
                return Ok(updatedProductAttribute.Id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }
    }
}
