﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TouristRoutesController : Controller //ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;
        private IUrlHelper _urlHelper;

        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
        )
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private string CreateAuthorsResourceUri(
            TouristRouteFilterParameters parameters,
            ResourceUriType type
        )
        {
            return type switch
            {
                ResourceUriType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                    new {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    }),
                ResourceUriType.NextPage => _urlHelper.Link("GetTouristRoutes",
                    new {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    }),
                _ => _urlHelper.Link("GetTouristRoutes",
                    new {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    })
            };
        }

        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteFilterParameters parameters)
        {
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(parameters);
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
            if(touristRoutes.Count() <= 0)
            {
                return NotFound("no tourist routes found");
            }

            // create Header
            var previousPageLink = touristRoutesFromRepo.HasPrevious ?
                    CreateAuthorsResourceUri(parameters,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = touristRoutesFromRepo.HasNext ?
                CreateAuthorsResourceUri(parameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(touristRoutes);
        }

        [HttpGet("{routeId}", Name = "GetTouristRouteById")]
        public IActionResult GetTouristRouteById(Guid routeId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(routeId);

            if (touristRouteFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TouristRouteDto>(touristRouteFromRepo));
        }

        [HttpPost]
        public IActionResult CreateAuthor(TouristRouteForCreationDto touristRouteDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                    "GetTouristRouteById",
                    new { routeId = touristRouteToReturn.Id },
                    touristRouteToReturn
                );
        }
    }
}