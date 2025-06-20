using Application.Dtos.LoginDtos;
using Application.Dtos.UserDtos;
using Application.Helper;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;

        public AccountController(HttpClient httpClient, IApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine(AdminApiString.LOGIN_URL());
                    var response = await _httpClient.PostAsJsonAsync(AdminApiString.LOGIN_URL(), loginDto);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                        Response.Cookies.Append("Jwt", tokenResponse?.Token ?? "", new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Lax
                        });

                        // Kiểm tra vai trò và điều hướng tương ứng
                        var roleClaim = JwtHelper.GetClaim(tokenResponse, "role");
                        if (roleClaim == "Admin")
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    TempData["ErrorMessage"] = "Login failed";
                    return BadRequest(ex);
                }
            }
            return View("Login", loginDto);
        }


        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("Jwt");
            }
            catch { }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var response = await _apiService.GetAsync(CustomerApiString.USER_ADMIN_PROFILE());
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

        [HttpGet()]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var response = await _apiService.PostAsync(CustomerApiString.USER_REGISTER(), registerUser);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Registration successful. Please login to continue.";
                    // redirect to login page
                    return RedirectToAction("Login", "Account");

                }
                else
                {
                    // get error message in response and set to model state
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new Exception(errorResponse?.Message ?? "Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // ad model state error
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(registerUser);
            }
        }
    }
}
