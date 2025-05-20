using Application.Dtos.BrandDtos;
using Application.Models;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandApiController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandApiController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }



        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var result = await _brandService.GetByIdAsync(id);

                if (result == null)
                {
                    throw new Exception("No brands found.");
                }

                if (!result.IsActive)
                {
                    throw new Exception("No brands found.");
                }

                var brandDtos = _mapper.Map<DetailBrandDto>(result);
                return Ok(brandDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        //get all
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var result = await _brandService.GetAllAsync();
                
                if(result == null)
                {
                    throw new Exception("No brands found.");
                }
                result = result.Where(b => b.IsActive);

                var brandDtos = _mapper.Map<IEnumerable<IndexBrandDto>>(result);
                return Ok(brandDtos);
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
