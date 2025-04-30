using Application.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var result = await _brandService.GetByIdAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Brand not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        //get all
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var result = await _brandService.GetAllAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No brands found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
