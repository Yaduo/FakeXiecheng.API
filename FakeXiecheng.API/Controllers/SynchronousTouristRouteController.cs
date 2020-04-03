using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SynchronousTouristRouteController : Controller
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public SynchronousTouristRouteController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteByIdSynchronous")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            //var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId).Result;

            // _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId).Wait(); // -> Task<void>

            return Ok(_mapper.Map<TouristRouteDto>(touristRouteFromRepo));
        }
    }
}
