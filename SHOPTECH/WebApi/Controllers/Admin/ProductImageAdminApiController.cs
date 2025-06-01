using Application.Dtos.ProductImage;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageAdminApiController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly IMapper _mapper;

        public ProductImageAdminApiController(IProductImageService productImageService, IMapper mapper)
        {
            _productImageService = productImageService;
            _mapper = mapper;
        }

        // GET: api/ProductImageAdminApi/getbyproductid/5
        [HttpGet("getbyproductid/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await _productImageService.GetByProductIdAsync(productId);
            return Ok(result);
        }

        // GET: api/ProductImageAdminApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productImageService.GetAllAsync();
            return Ok(result);
        }

        // POST: api/ProductImageAdminApi
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] List<IFormFile> files,[FromQuery] int productId)
        {
            var success = await _productImageService.AddAsync(files, productId);
            if (success)
                return Ok();
            return BadRequest();
        }

        // PUT: api/ProductImageAdminApi
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductImageDto productImageDto)
        {
            var success = await _productImageService.UPdateAsync(productImageDto);
            if (success)
                return Ok();
            return BadRequest();
        }

        // DELETE: api/ProductImageAdminApi
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] ProductImageDto productImageDto)
        {
            // Debug
            var rawBody = "";
            using (var reader = new StreamReader(Request.Body))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            Console.WriteLine($"Raw body: {rawBody}");
            Console.WriteLine($"ImageFileId: {productImageDto?.ImageFileId}");
            Console.WriteLine($"ProductId: {productImageDto?.ProductId}");

            var success = await _productImageService.DeleteAsync(productImageDto);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
