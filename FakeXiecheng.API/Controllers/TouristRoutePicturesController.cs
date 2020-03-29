using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/TouristRoutes/{touristRouteId}/pictures")]
    public class TouristRoutePicturesController : Controller
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutePicturesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // GET: /<controller>/
        [HttpGet(Name = "GetPictureListForTouristRoute")]
        public IActionResult GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("no tourist route found");
            }

            var picturesFromRepo = _touristRouteRepository.GetPicturesByTouristRouteId(touristRouteId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("no picture found");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(picturesFromRepo));
        }

        [HttpGet("{pictureId}", Name = "GetPictureForTouristRoute")]
        public IActionResult GetPictures(Guid touristRouteId, int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("no tourist route found");
            }
            var pictureFromRepo = _touristRouteRepository.GetPicturesByTouristRouteIdAndPictureId(touristRouteId, pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("no picture found");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(pictureFromRepo));
        }


        [HttpPost]
        public ActionResult CreatePictureForAuthor(Guid touristRouteId, TouristRoutePictureForCreationDto picture)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("no tourist route found");
            }

            var pictureModel = _mapper.Map<TouristRoutePicture>(picture);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            _touristRouteRepository.Save();
            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                    "GetPictureForTouristRoute",
                    new { touristRouteId, pictureId = pictureToReturn.Id },
                    pictureToReturn
                );
        }

        [HttpPost("collection")]
        public ActionResult CreatePictureListForAuthor(Guid touristRouteId, IEnumerable<TouristRoutePictureForCreationDto> pictureList)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("no tourist route found");
            }

            var pictureListModel = _mapper.Map<IEnumerable<TouristRoutePicture>>(pictureList);
            foreach (var picture in pictureListModel) {
                _touristRouteRepository.AddTouristRoutePicture(touristRouteId, picture);
            }
            _touristRouteRepository.Save();
            var pictureListToReturn = _mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictureListModel);
            return CreatedAtRoute("GetPictureListForTouristRoute",
             new { touristRouteId },
             pictureListToReturn);
        }
    }
}
