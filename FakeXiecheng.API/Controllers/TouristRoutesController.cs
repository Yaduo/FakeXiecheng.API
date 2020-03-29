using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TouristRoutesController : Controller //ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public IActionResult GetTouristRoutes([FromQuery] string keyword)
        {
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(keyword);
            // use for loop 
            //var touristRoutes = new List<TouristRouteDto>();
            //foreach(var touristRoute in touristRoutesFromRepo)
            //{
            //    var touristRoutePictures = new List<TouristRoutePictureDto>();
            //    foreach (var picture in touristRoute.TouristRoutePictures)
            //    {
            //        touristRoutePictures.Add(new TouristRoutePictureDto()
            //        {
            //            Url = picture.Url
            //        });
            //    }
            //    touristRoutes.Add(new TouristRouteDto()
            //    {
            //        Id = touristRoute.Id,
            //        Title = touristRoute.Title,
            //        Description = touristRoute.Description,
            //        OriginalPrice = touristRoute.OriginalPrice,
            //        DiscountPercent = touristRoute.DiscountPercent,
            //        Price = touristRoute.OriginalPrice * (decimal)(touristRoute.DiscountPercent ??= 1),
            //        Coupons = touristRoute.Coupons,
            //        Points = touristRoute.Points,
            //        Rating = touristRoute.Rating,
            //        Features = touristRoute.Features,
            //        Fees = touristRoute.Fees,
            //        Notes = touristRoute.Notes,
            //        Pictures = touristRoutePictures
            //    });
            //}

            // using Linq
            //var touristRoutes = touristRoutesFromRepo.Select(t => new TouristRouteDto()
            //{
            //    Id = t.Id,
            //    Title = t.Title,
            //    Description = t.Description,
            //    OriginalPrice = t.OriginalPrice,
            //    DiscountPercent = t.DiscountPercent,
            //    Price = t.OriginalPrice * (decimal)(t.DiscountPercent ?? 1),
            //    Coupons = t.Coupons,
            //    Points = t.Points,
            //    Rating = t.Rating,
            //    Features = t.Features,
            //    Fees = t.Fees,
            //    Notes = t.Notes,
            //    Pictures = t.TouristRoutePictures.Select(p => new TouristRoutePictureDto()
            //    {
            //        Url = p.Url
            //    }).ToList()
            //}).ToList();

            // using auto mapper
            var touristRoutes = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            return Ok(touristRoutes);
        }

        [HttpGet("{routeId}")]
        public IActionResult GetTouristRoute(Guid routeId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(routeId);

            if (touristRouteFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TouristRouteDto>(touristRouteFromRepo));
        }
    }
}
