using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PathInterview.Core.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static (bool login, string message) LoginExists(this IHttpContextAccessor httpContextAccessor)
        {
            if (string.IsNullOrEmpty(httpContextAccessor.AccessToken().userId))
                return (false, "Giriş yapılmadı");

            return (true, string.Empty);
        }

        public static (string accessToken, string userId, List<Claim> claims) AccessToken(this IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            HttpRequest request = httpContext?.Request;

            string accessToken = request?.Headers["Authorization"].FirstOrDefault();
            string idToken = request?.Headers["IdToken"].FirstOrDefault();

            if (string.IsNullOrEmpty(accessToken))
                if (request.Query.TryGetValue("access_token", out StringValues token))
                    accessToken = $"Bearer {token}";

            string userId = "";
            List<Claim> claims = null;
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (accessToken.StartsWith("Bearer")) accessToken = accessToken.Replace("Bearer", "").Trim();

                JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

                if (securityToken.Claims.Any())
                {
                    List<Claim> claim = securityToken.Claims.ToList();
                    userId = claim.FirstOrDefault()?.Value;
                }
            }

            if (string.IsNullOrEmpty(idToken)) return (accessToken, userId, claims);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(idToken)) claims = tokenHandler.ReadJwtToken(idToken).Claims.ToList();

            return (accessToken, userId, claims);
        }
    }
}

