using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PathInterview.Core.Extensions;
using PathInterview.Core.Security.Encryption;
using PathInterview.Entities.Entity;

namespace PathInterview.Core.Security
{
    public class JwtHelper : ITokenHelper
    {
        private IConfiguration Configuration { get; }
        private readonly TokenOptionsModel _tokenOptions;
        private readonly DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptionsModel>();
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }

        public AccessToken CreateToken(User user)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptionsModel tokenOptions, User user, SigningCredentials signingCredentials)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user),
                signingCredentials: signingCredentials
            );
            return jwt;
        }
        
        private static IEnumerable<Claim> SetClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id);
            claims.AddEmail(user.Email);

            return claims;
        }
    }
}