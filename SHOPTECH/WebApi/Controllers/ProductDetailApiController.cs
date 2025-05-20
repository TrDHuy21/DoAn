using Application.Dtos.ProductDetailDtos;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailApiController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        private readonly IMapper _mapper;

        public ProductDetailApiController(IProductDetailService productDetailService, IMapper mapper)
        {
            _productDetailService = productDetailService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var productDetail = await _productDetailService.GetByIdAsync(id);
                if (productDetail == null)
                {
                    return NotFound(new { Message = "Product detail not found." });
                }

                if (!productDetail.IsActive)
                {
                    return NotFound(new { Message = "Product detail not found." });
                }
                var productDetailDto = _mapper.Map<DetailClientProductDetail>(productDetail);
                return Ok(productDetailDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                if(ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException);
                }
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            try
            {
                var productDetails = await _productDetailService.GetByProductIdAsync(productId);
                if (productDetails == null)
                {
                    return NotFound(new { Message = "No product details found." });
                }
                productDetails = productDetails.Where(pd => pd.IsActive);
                var productDetailDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(productDetails);
                return Ok(productDetailDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("product/other/{productDetailId}")]
        public async Task<IActionResult> GetOtherByProductId(int productDetailId)
        {
            try
            {
                var productDetails = await _productDetailService.GetOtherByProductIdAsync(productDetailId);
                if (productDetails == null)
                {
                    return NotFound(new { Message = "No product details found." });
                }
                productDetails = productDetails.Where(pd => pd.IsActive);
                var productDetailDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(productDetails);
                return Ok(productDetailDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> GetCheckout([FromQuery] IEnumerable<int> productDetailIds)
        {
            try
            {
                var productDetails = await _productDetailService.GetCheckout(productDetailIds);
                if (productDetails == null)
                {
                    return NotFound(new { Message = "No product details found." });
                }
                var productDetailDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(productDetails);
                return Ok(productDetailDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("category/{categoryName}/page/{index}")]
        public async Task<IActionResult> GetPageResultWithFilter(string categoryName, int index = 1, int size = 10, [FromQuery] Dictionary<string, string> queryParams = null)
        {
            try
            {
                var pageResultDto = await _productDetailService.GetPageResultWithFilterAsync(categoryName,index, size, queryParams);
                if (pageResultDto == null)
                {
                    return NotFound(new { Message = "No product details found." });
                }

                pageResultDto.Items = pageResultDto.Items.Where(i => i.IsActive);
                return Ok(pageResultDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetWithFilter(string categoryName, [FromQuery] Dictionary<string, string> queryParams = null)
        {
            try
            {
                var productDetails = await _productDetailService.GetWithFilterAsync(categoryName, queryParams);
                if (productDetails == null)
                {
                    return NotFound(new { Message = "No product details found." });
                }

                productDetails = productDetails.Where(pd => pd.IsActive);

                var productDetailDtos = _mapper.Map<IEnumerable<IndexProductDetailDto>>(productDetails);
                return Ok(productDetailDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
