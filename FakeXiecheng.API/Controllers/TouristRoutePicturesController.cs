using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("no tourist route found");
            }

            var picturesFromRepo = await _touristRouteRepository.GetPicturesByTouristRouteIdAsync(touristRouteId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("no picture found");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(picturesFromRepo));
        }

        [HttpGet("{pictureId}", Name = "GetPictureForTouristRoute")]
        public async Task<IActionResult> GetPictures(Guid touristRouteId, int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("no tourist route found");
            }
            var pictureFromRepo = await _touristRouteRepository.GetPicturesByTouristRouteIdAndPictureIdAsync(touristRouteId, pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("no picture found");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(pictureFromRepo));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin, Author")]
        public async Task<IActionResult> CreatePictureForAuthor(Guid touristRouteId, TouristRoutePictureForCreationDto picture)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("no tourist route found");
            }

            var pictureModel = _mapper.Map<TouristRoutePicture>(picture);
            await _touristRouteRepository.AddTouristRoutePictureAsync(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();
            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                    "GetPictureForTouristRoute",
                    new { touristRouteId, pictureId = pictureToReturn.Id },
                    pictureToReturn
                );
        }

        [HttpPost("collection")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin, Author")]
        public async Task<IActionResult> CreatePictureListForAuthor(Guid touristRouteId, IEnumerable<TouristRoutePictureForCreationDto> pictureList)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("no tourist route found");
            }

            var pictureListModel = _mapper.Map<IEnumerable<TouristRoutePicture>>(pictureList);
            // for loop 进行IO操作, 严重影响效率，怎么办呢？
            //foreach (var picture in pictureListModel) {
            //    await _touristRouteRepository.AddTouristRoutePictureAsync(touristRouteId, picture);
            //}
            await _touristRouteRepository.AddRangeForTouristRoutePictureListAsync(touristRouteId, pictureListModel);
            await _touristRouteRepository.SaveAsync();
            var pictureListToReturn = _mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictureListModel);
            return CreatedAtRoute("GetPictureListForTouristRoute",
             new { touristRouteId },
             pictureListToReturn);
        }
    }
}
