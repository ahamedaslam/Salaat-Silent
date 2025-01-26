using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NamazSchedulerApp.API.Utils
{
    
    
        public class GenerateToken
        {
            private readonly string _secretKey;
            private readonly string _issuer;
            private readonly string _audience;

            public GenerateToken(string secretKey, string issuer, string audience)
            {
                _secretKey = secretKey;
                _issuer = issuer;
                _audience = audience;
            }

            public string CreateJwtToken(Guid userId)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    
}
