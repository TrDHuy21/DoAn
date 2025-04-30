using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Helper
{
    public class JwtHelper
    {
        public static string? GetClaim(TokenResponse? tokenResponse, string claimName)
        {
            if(tokenResponse == null)
            {
                return string.Empty;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResponse.Token);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
        }

    }
}
