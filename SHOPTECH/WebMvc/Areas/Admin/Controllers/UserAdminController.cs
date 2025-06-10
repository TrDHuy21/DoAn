using Application.Dtos.BrandDtos;
using Application.Dtos.UserDtos;
using Application.Helper;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Implementation;
using WebMvc.Service.Interface;

namespace WebMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Route("Admin/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]

    public class UserAdminController : Controller
    {
        private readonly IApiService _apiService;

        public UserAdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.USER_ADMIN());
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<IEnumerable<IndexUserDto>>()
                        ?? throw new Exception("");

                    ViewData["users"] = users;

                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _apiService.GetAsync(AdminApiString.USER_ADMIN(id));
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<DetailUserDto>()
                        ?? throw new Exception("");

                    return View(user);
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Model is not valid");
                }

                //  sử dụng MultipartFormDataContent để gửi cả data và file
                using (var multiContent = new MultipartFormDataContent())
                {
                    multiContent.Create(userDto);

                    // Gửi request
                    var response = await _apiService.PostAsync(AdminApiString.USER_ADMIN(), multiContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        throw new Exception(errorContent?.Message);
                    }

                    var userId = await response.Content.ReadFromJsonAsync<int>();
                    TempData["SuccessMessage"] = "Tạo tài khoảng thành công thành công !";
                    return RedirectToAction("Detail", new { id = userId });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(userDto);
            }
        }

        [HttpPost("changeActive")]
        public async Task<IActionResult> ChangeActive(int id,bool isActive)
        {
            try
            {
                var response =await _apiService.PutAsync(AdminApiString.USER_ADMIN_CHAGNE_ACTIVE(id), isActive);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    success = true,
                });
            } 
            catch
            {
                return Ok(new
                {
                    success = false,
                    message = "Đã xảy ra lỗi"
                });
            }
        }
    }   
}
