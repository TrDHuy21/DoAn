using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Service.Interface;

namespace WebMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenimiController : ControllerBase
    {
        private readonly IApiService _apiService;

        public GenimiController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("/api/chat/send")]
        public async Task<IActionResult> SendChat(ChatRequest chatRequest)
        {
            var response = await _apiService.PostAsync(CustomerApiString.CHAT(), chatRequest);
            if (!response.IsSuccessStatusCode)
                return BadRequest();
            var answer = await response.Content.ReadFromJsonAsync<ChatResponse>();
            return Ok(answer);
        }
    }
}
