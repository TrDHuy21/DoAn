using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebMvc.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Kiểm tra xem cookie JWT có tồn tại không
                if (context.Request.Cookies.TryGetValue("Jwt", out var token) && !string.IsNullOrEmpty(token))
                {
                    if (!context.Request.Headers.ContainsKey("Authorization"))
                    {
                        context.Request.Headers.Append("Authorization", $"Bearer {token}");
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            await _next(context);
        }
    }
}
