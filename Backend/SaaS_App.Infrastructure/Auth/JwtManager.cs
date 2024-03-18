using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace SaaS_App.Infrastructure.Auth
{
    public class JwtManager
    {
        private readonly JwtAutheticationOptions _jwtAutheticationOptions;
        public const string UserIdClaim = "UserId";

        public JwtManager(IOptions<JwtAutheticationOptions> jwtAutheticationOptions)
        {
            _jwtAutheticationOptions = jwtAutheticationOptions.Value;
        }

        public string GenerateJwtToken(int userId)
        {
            var claims = new Claim[]
            {
                new Claim(UserIdClaim, userId.ToString())
            };
            return GenerateJwtTokenWithClaims(claims);
        }
        private string GenerateJwtTokenWithClaims(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = GetSecurityKey();
            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_jwtAutheticationOptions.ExpireInDays),
                Issuer = _jwtAutheticationOptions.Issuer,
                Audience = _jwtAutheticationOptions.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesciptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityKey GetSecurityKey()
        {
            if (string.IsNullOrWhiteSpace(_jwtAutheticationOptions.Secret))
            {
                throw new ArgumentException("JWT options secret is empty !");
            }

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtAutheticationOptions.Secret));
        }
        public bool ValidateJwtToken(string jwtSecurityToken)
        {
            if (string.IsNullOrEmpty(jwtSecurityToken))
            {
                return false;
            }

            var mySecurityKey = GetSecurityKey();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(jwtSecurityToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtAutheticationOptions.Issuer,
                    ValidAudience = _jwtAutheticationOptions.Audience
                }, out SecurityToken validatedToken);
            }
            catch 
            {
                return false;
            }

            return true;
        }

        public string? GetClaimFromJwtToken(string jwtToken, string claimType)
        { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            if (token == null)
            {
                return null;
            }

            var stringClaimValue = token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            return stringClaimValue;
        }
    }
}
