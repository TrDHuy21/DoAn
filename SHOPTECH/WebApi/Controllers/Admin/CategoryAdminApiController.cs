using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Models;
using Application.Service.Implementation;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAdminApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryAdminApiController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                var categoryDto = _mapper.Map<DetailCategoryDto>(category);
                return Ok(categoryDto);
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
                var categories = await _categoryService.GetAllAsync();
                var categoryDtos = _mapper.Map<IEnumerable<IndexCategoryDto>>(categories);
                return Ok(categoryDtos);
            }
            catch (Exception ex)
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
                var pageResult = await _categoryService.GetPageResultAsync(pageIndex, pageSize);
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
        public async Task<IActionResult> Add([FromForm] AddCategoryDto categoryDto)
        {
            try
            {
                var category = await _categoryService.AddAsync(categoryDto);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category.Id);
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
        public async Task<IActionResult> Update(UpdateCategoryDto categoryDto)
        {

            try
            {
                var category = await _categoryService.UpdateAsync(categoryDto);
                return Ok(category.Id);
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
                var result = await _categoryService.DeleteAsync(id);
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
                var category = await _categoryService.ChangeActiveAsync(id, isActive);
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
