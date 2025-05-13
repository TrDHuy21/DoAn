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
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
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
                    var response = await _httpClient.PostAsJsonAsync(AdminApiString.LOGIN_URL, loginDto);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                        var cookieOptions = new CookieOptions();
                        Response.Cookies.Append("Jwt", tokenResponse?.Token ?? "", cookieOptions);

                        // Kiểm tra vai trò và điều hướng tương ứng
                        var roleClaim = JwtHelper.GetClaim(tokenResponse, "role");
                        if (roleClaim == "Admin" || roleClaim == "Saler")
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

                        string errorMessage = "Login failed";
                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View("Index", loginDto);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return BadRequest("error");
                }
            }

            return View("Index", loginDto);
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
    }
}
