using Application.Models;
using Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {

        private readonly IGeminiService _geminiService;

        public ChatController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest("Message cannot be empty");
                }
                string jwt = GetTokenFromRequest() ?? throw new Exception();
                var response = await _geminiService.SendMessageAsync(
                    request.Message,
                    jwt
                );
                return Ok(new ChatResponse
                {
                    Message = response,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "Chat service is running", Timestamp = DateTime.UtcNow });
        }

        private string GetTokenFromRequest()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }
            return null;
        }
      
    }
}
