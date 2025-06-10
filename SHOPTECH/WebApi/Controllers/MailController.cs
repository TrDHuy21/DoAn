using Application.Service.Interface;
using Domain.Enity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string body)
        {
            var rs = await _emailService.SendSimpleEmailAsync(
                 "trdhuy21@gmail.com",
                 body,
                 "<h3>Đơn hàng của bạn đã được xác nhận!</h3>"
             );
            if (rs)
            {
                return Ok("Email sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send email.");
            }
        }
    }
}
