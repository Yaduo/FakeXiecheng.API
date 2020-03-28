using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TouristRoutesController : Controller //ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository)
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
        }

        [HttpGet]
        public IActionResult GetTouristRoutes()
        {
            var touristRoutes = _touristRouteRepository.GetAllTouristRoutes();
            return Ok(touristRoutes);
        }

        [HttpGet("{routeId}")]
        public IActionResult GetTouristRoute(Guid routeId)
        {
            var authorFromRepo = _touristRouteRepository.GetTouristRoute(routeId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(authorFromRepo);
        }
    }
}
