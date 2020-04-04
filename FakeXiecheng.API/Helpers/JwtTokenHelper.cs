using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Helpers
{
    public static class JwtTokenHelper
    {
        public static SymmetricSecurityKey GenerateSigningKey(string secretKey)
        {
            var secretByte = Encoding.UTF8.GetBytes(secretKey);
            return new SymmetricSecurityKey(secretByte);
        }

        public static string GenerateTokenJson(
            string secretKey,
            string issuer = null,
            string audience= null,
            IEnumerable<Claim> claims = null, 
            int expiresDays = 0
        )
        {
            var signingKey = GenerateSigningKey(secretKey);
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(expiresDays),
                signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
