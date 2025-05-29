using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Models;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]

    public class BrandAdminApiController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandAdminApiController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(id);
                var brandDto = _mapper.Map<DetailBrandDto>(brand);
                return Ok(brandDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var brands = await _brandService.GetAllAsync();
                var brandDtos = _mapper.Map<IEnumerable<IndexBrandDto>>(brands);
                return Ok(brandDtos);
            } 
            catch(Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("page/{pageIndex}")]
        public async Task<IActionResult> GetPageResult(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _brandService.GetPageResultAsync(pageIndex, pageSize);
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

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddBrandDto brandDto)
        {
            try
            {
                var brand = await _brandService.AddAsync(brandDto);
                return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand.Id);
            } 
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBrandDto brandDto)
        {
            try
            {
                var brand = await _brandService.UpdateAsync(brandDto);
                return Ok(brand.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _brandService.DeleteAsync(id);
                if (!result)
                {
                    throw new Exception("Failed to delete brand");
                }
                return Ok("Brand deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut("ChangeActive/{id}")]
        public async Task<IActionResult> ChangeActive(int id, bool isActive)
        {
            try
            {
                var brand = await _brandService.ChangeActive(id, isActive);
                return Ok("Brand status updated successfully.");
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
