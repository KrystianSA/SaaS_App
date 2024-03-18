using Microsoft.AspNetCore.Authentication;
using SaaS_App.Application.Interfaces;
using SaaS_App.Infrastructure.Auth;
using System.Runtime.InteropServices;

namespace SaaS_App.WebApi.Application.Auth
{
    public class JwtAutheticationDataProvider : IAuthenticationDataProvider
    {
        private readonly JwtManager _jwtManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAutheticationDataProvider(JwtManager jwtManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public int? GetUserId()
        {
            var claim = GetClaimFromToken(JwtManager.UserIdClaim);

            if (int.TryParse(claim, out var userId))
            {
                return userId;
            }
            
            return null;
        }
        private string? GetClaimFromToken(string claimType)
        {
            var token = GetTokenFromHeader();
            if (string.IsNullOrEmpty(token))
            {
                token = GetTokenFromCookie();
            }

            if (!string.IsNullOrEmpty(token) && _jwtManager.ValidateJwtToken(token))
            {
                return _jwtManager.GetClaimFromJwtToken(token, claimType);
            }

            return null;
        }
        private string? GetTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.CookieName];
        }
        private string? GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            var splited = authorizationHeader.Split(' ');
            if (splited.Length > 1 && splited[0] == "Bearer")
            {
                return splited[1];
            }

            return null;
        }
    }
}
