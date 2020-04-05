using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateController(
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

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
        public async Task<IActionResult> LoginByJWT(LoginDto loginDto)
        {

            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            if (!loginResult.Succeeded)
            {
                return Forbid();
            }

            var user = await _userManager.FindByNameAsync(loginDto.Email);
            var roleNames = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var roleName in roleNames)
            {
                var roleClaims = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaims);
            }

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            //    new Claim("custome", "something_2"),
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.Role, "Admin"),
            //    new Claim(ClaimTypes.Role, "Author"),
            //};

            /*
            var secret = _configuration["Authentication:SecretKey"];
            var secretByte = Encoding.UTF8.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"], 
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
            );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            */

            var tokenJson = JwtTokenHelper.GenerateTokenJson(
                secretKey: _configuration["Authentication:SecretKey"],
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                expiresDays: Int32.TryParse(_configuration["Authentication:ExpiresDays"], out int expiresDays) ? expiresDays : 0
            );

            return Ok(new
            {
                access_token = tokenJson
            });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // 创建新用户，并向数据库写入用户数据
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var user = new IdentityUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                return Forbid();
            }

            return NoContent();
        }



    }
}
