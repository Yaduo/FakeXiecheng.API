using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityRolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public IdentityRolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //public async Task<IActionResult> GetIdentityRoles()
        public async Task<IActionResult> GetIdentityRolesAsync()
        {
            //var roles = _roleManager.Roles.ToList();
            var roles = await _touristRouteRepository.GetAllIdentityRolesAsync();
            return Ok(_mapper.Map<IEnumerable<IdentityRoleDto>>(roles));
        }

        [HttpPost]
        public async Task<IActionResult> AddIdentityRolesAsync(IdentityRoleForCreationDto dto)
        {
            var role = _mapper.Map<IdentityRole>(dto);
            await _roleManager.CreateAsync(role);
            return NoContent();
        }

        [HttpPost("{roleName}/user/{userName}")]
        public async Task<IActionResult> AddRoleToUserAsync(string roleName, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
            {
                return NotFound("User not found");
            }
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role {roleName} not found");
            }
            await _userManager.AddToRoleAsync(user, roleName);

            return NoContent();
        }

    }
}