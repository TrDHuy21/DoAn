using Application.Dtos.BrandDtos;
using Application.Dtos.UserDtos;
using Application.Models;
using Application.Service.Implementation;
using Application.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class UserAdminApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserAdminApiController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }
                var userDto = _mapper.Map<DetailUserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                if (users == null)
                {
                    return NotFound(new { Message = "User not found" });
                }
                var userDtos = _mapper.Map<IEnumerable<IndexUserDto>>(users);
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //add
        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto userDto)
        {
            try
            {
                var user = await _userService.AddAsync(userDto);
                if (user == null)
                {
                    return BadRequest(new { Message = "Error adding user" });
                }
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //update
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto userDto)
        {
            try
            {
                var brand = await _userService.UpdateAsync(userDto);
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
                var result = await _userService.DeleteAsync(id);
                if (!result)
                {
                    throw new Exception("Failed to delete User");
                }
                return Ok("User deleted successfully.");
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
                var brand = await _userService.ChangeActive(id, isActive);
                return Ok("User status updated successfully.");
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
