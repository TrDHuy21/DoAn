using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Application.Dtos.LoginDtos;
using Application.Helper;
using Application.Models;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                        TempData["ErrorMessage"] = "Login failed. Please check your account and password";
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
    }
}
