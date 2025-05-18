using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace WebMvc.Config
{
    public class JwtCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtCookieAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                // Lấy JWT từ cookie
                if (!Request.Cookies.TryGetValue("Jwt", out var token) || string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.Fail("JWT Token không có trong cookie");
                }

                // Log token để debug
                Logger.LogInformation($"JWT Token từ cookie: {token}");

                // Xác thực token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt key missing"));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // Xác thực token và lấy claims
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Tạo AuthenticationTicket từ ClaimsPrincipal
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (SecurityTokenExpiredException)
            {
                return AuthenticateResult.Fail("Token đã hết hạn");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return AuthenticateResult.Fail("Token có chữ ký không hợp lệ");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Lỗi xác thực JWT");
                return AuthenticateResult.Fail($"Lỗi xác thực token: {ex.Message}");
            }
        }
    }
}
