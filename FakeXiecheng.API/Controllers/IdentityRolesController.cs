using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public IdentityRolesController(
            RoleManager<IdentityRole> roleManager,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _roleManager = roleManager;
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
    }
}