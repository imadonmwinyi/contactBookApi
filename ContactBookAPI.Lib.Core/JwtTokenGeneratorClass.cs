using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactBookAPI.Lib.Core
{

    public class JwtTokenGeneratorClass 
    {
        private readonly IConfiguration _config;
        public JwtTokenGeneratorClass(IConfiguration config)
        {
            _config = config;
        }
        public object GenerateToken(string userId, string Email, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityTokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials =new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:validIssuer").Value)),
                   SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenCreated = tokenHandler.CreateToken(securityTokenDesc);
            return new {
                 token = tokenHandler.WriteToken(tokenCreated),
                 id = userId
            };
        }
    }
}
