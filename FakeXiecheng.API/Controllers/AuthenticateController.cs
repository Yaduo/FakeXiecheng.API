using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : Controller
    {
        [AllowAnonymous]
        [HttpGet("loginByCookie")]
        public IActionResult LoginByCookie()
        {
            var identityClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Author"),
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim("DrivingLicense", "A+"),
            };

            var grandmaIdentity = new ClaimsIdentity(identityClaims, "Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("loginByJWT")]
        public IActionResult LoginByJWT()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id"),
                new Claim("custome", "something_2"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Author"),
            };

            var secret = "sui_bian_xie_dian_zifuchuan";
            var secretByte = Encoding.UTF8.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: "fakeXiecheng.com", 
                audience: "fakeXiecheng.com",
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
            );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { 
                access_token = tokenJson 
            });
        }


    }
}
