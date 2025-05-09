using Application.Dtos.ProductDetailDtos;
using Application.Dtos;
using AutoMapper;
using Domain.Enity;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Service.Interface;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailAdminApiController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        private readonly IMapper _mapper;

        public ProductDetailAdminApiController(IProductDetailService productDetailService, IMapper mapper)
        {
            _productDetailService = productDetailService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndexProductDetailDto>>> GetAll()
        {
            try
            {
                var products = await _productDetailService.GetAllAsync();
                if (products == null)
                {
                    throw new Exception("No product details found");
                }

                var productDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(products);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { 
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetail>> GetById(int id)
        {
            try
            {
                var product = await _productDetailService.GetByIdAsync(id);
                var productDto = _mapper.Map<DetailProductDetailDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<IndexProductDetailDto>>> GetByProductId(int productId)
        {
            try
            {
                var products = await _productDetailService.GetByProductIdAsync(productId);
                if (products == null)
                {
                    throw new Exception("No product details found");
                }
                var productDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(products);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetail>> Add([FromForm]AddProductDetailDto productDetailDto)
        {
            try
            {
                var product = await _productDetailService.AddAsync(productDetailDto);
                if (product == null)
                {
                    throw new Exception("Failed to add product detail");
                }

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ProductDetail>> Update([FromForm] UpdateProductDetailDto productDetailDto)
        {
            try
            {
                var product = await _productDetailService.UpdateAsync(productDetailDto);
                if (product == null)
                {
                    throw new Exception($"Product detail with id {productDetailDto.Id} not found");
                }

                return Ok(product.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _productDetailService.DeleteAsync(id);
                if (!result)
                {
                    throw new Exception($"Product detail with id {id} not found");
                }
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("page")]
        public async Task<ActionResult<PageResultDto<IndexProductDetailDto>>> GetPageResult(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _productDetailService.GetPageResultAsync(pageIndex, pageSize);
                if (pageResult == null)
                {
                    throw new Exception("No product details found");
                }

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}/active")]
        public async Task<ActionResult<ProductDetail>> ChangeActive(int id, [FromBody] bool isActive)
        {
            try
            {
                var product = await _productDetailService.ChangeActive(id, isActive);
                if (product == null)
                {
                    throw new Exception($"Product detail with id {id} not found");
                }
                 return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = ex.Message
                });
            }
        }
    }
}

