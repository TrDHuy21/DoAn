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



        // GET: api/ProductAttributeAdminApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAttribute>>> GetAll()
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

        // GET: api/ProductAttributeAdminApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAttribute>> GetById(int id)
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

        // POST: api/ProductAttributeAdminApi
        [HttpPost]
        public async Task<ActionResult<ProductAttribute>> Create([FromBody] AddProductAttributeDto productAttributeDto)
        {
            try
            {
                var createdProductAttribute = await _productAttributeService.AddAsync(productAttributeDto);
                return CreatedAtAction(nameof(GetById), new { id = createdProductAttribute.Id }, createdProductAttribute.Id);
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

        // PUT: api/ProductAttributeAdminApi
        [HttpPut]
        public async Task<ActionResult<ProductAttribute>> Update([FromBody] UpdateProductAttributeDto productAttributeDto)
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

        // DELETE: api/ProductAttributeAdminApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
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

        // PATCH: api/ProductAttributeAdminApi/ChangeActive/5
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

        // PATCH: api/ProductAttributeAdminApi/ChangeDisplay/5
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

        // PATCH: api/ProductAttributeAdminApi/ChangeFilter/5
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
