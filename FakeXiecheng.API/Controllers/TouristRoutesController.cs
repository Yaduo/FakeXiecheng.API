using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.ActionConstraints;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
    public class TouristRoutesController : Controller //ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IPropertyCheckerService propertyCheckerService
        )
        {
            _touristRouteRepository = touristRouteRepository ??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyCheckerService = propertyCheckerService ??
              throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        private string CreateAuthorsResourceUri(
            TouristRouteFilterParameters parameters,
            ResourceUriType type
        )
        {
            return type switch
            {
                ResourceUriType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                    }),
                ResourceUriType.NextPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                    }),
                _ => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                    })
            };
        }

        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        //[ResponseCache(Duration = 120)]
        public async Task<IActionResult> GetTouristRoutes([FromQuery] TouristRouteFilterParameters parameters)
        {
            if (!_propertyCheckerService.TypeHasProperties<TouristRouteDto>(parameters.Fields))
            {
                return BadRequest();
            }

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(parameters);
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
            if (touristRoutes.Count() <= 0)
            {
                return NotFound("no tourist routes found");
            }

            // create Header
            var previousPageLink = touristRoutesFromRepo.HasPrevious
                ? CreateAuthorsResourceUri(parameters,ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = touristRoutesFromRepo.HasNext
                ? CreateAuthorsResourceUri(parameters, ResourceUriType.NextPage)
                : null;

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

            var links = CreateLinksForTouristRouteList(
                    parameters,
                    touristRoutesFromRepo.HasNext,
                    touristRoutesFromRepo.HasPrevious
                );

            var shapedAuthors = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo)
                               .ShapeData(parameters.Fields);

            var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForTouristRoute((Guid)authorAsDictionary["Id"], null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedAuthorsWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        [Produces(
            "application/json",
            "application/xml",
            "application/vnd.fakeXiecheng.hateoas+json",
            "application/vnd.fakeXiecheng.simplify+json",
            "application/vnd.fakeXiecheng.simplify.hateoas+json"
        )]
        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(
            Guid touristRouteId,
            string fields,
            [FromHeader(Name = "Accept")] string mediaType
        )
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<TouristRouteDto>(fields))
            {
                return BadRequest();
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound();
            }

            //if(parsedMediaType.MediaType == "application/vnd.fakeXiecheng.hateoas+json")
            //{
            //    var links = CreateLinksForTouristRoute(touristRouteId, fields);

            //    var linkedResourceToReturn =
            //        _mapper.Map<TouristRouteDto>(touristRouteFromRepo).ShapeData(fields)
            //        as IDictionary<string, object>;

            //    linkedResourceToReturn.Add("links", links);

            //    return Ok(linkedResourceToReturn);
            //}

            //return Ok(_mapper.Map<TouristRouteDto>(touristRouteFromRepo).ShapeData(fields));

            bool includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            IEnumerable<LinkDto> links = new List<LinkDto>();
            if (includeLinks)
            {
                links = CreateLinksForTouristRoute(touristRouteId, fields);
            }

            var primaryMediaType = includeLinks
                ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;

            // simplify TouristRouteDto
            if (primaryMediaType == "vnd.fakeXiecheng.simplify")
            {
                var simplifyResults = _mapper.Map<SimpleTouristRouteDto>(touristRouteFromRepo)
                    .ShapeData(fields) as IDictionary<string, object>;

                if (includeLinks)
                {
                    simplifyResults.Add("links", links);
                }

                return Ok(simplifyResults);
            }

            // normal TouristRouteDto
            var results = _mapper.Map<TouristRouteDto>(touristRouteFromRepo)
                .ShapeData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                results.Add("links", links);
            }

            var getFakeImageExternalUrl = _urlHelper.Link("GetFakeImageRequest", null);

            var fakeImageContent = await _touristRouteRepository.GetFakeImageContentFromExternalAPI(getFakeImageExternalUrl);

            //return Ok(results);
            return Ok(new
            {
                results,
                fakeImageContent
            });
        }

        [HttpGet("collection/({ids})", Name = "GetAuthorCollection")]
        public async Task<IActionResult> GetAuthorCollection(
            [FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var touristRoute = await _touristRouteRepository.GetTouristRoutesByIdListAsync(ids);

            if (ids.Count() != touristRoute.Count())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TouristRouteDto>>(touristRoute));
        }


        // 注意，次函数必须排在CreateTouristRoute前面，否则没法匹配Consumes
        [HttpPost(Name = "CreateTouristRouteWithTripAttribute")]
        [RequestHeaderMatchesMediaType(
            "Content-Type",
            "application/vnd.fakeXiecheng.createTouristRouteWithTripAttribute+json"
        )]
        [Consumes("application/vnd.fakeXiecheng.createTouristRouteWithTripAttribute+json")]
        public async Task<IActionResult> CreateTouristRouteWithTripAttribute(TouristRouteForCreationWithTripAttributeDto touristRouteDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteDto);

            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            var links = CreateLinksForTouristRoute(touristRouteToReturn.Id, null);

            var linkedResourceToReturn =
                _mapper.Map<TouristRouteDto>(touristRouteToReturn).ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute(
                    "GetTouristRouteById",
                    new { touristRouteId = linkedResourceToReturn["Id"] },
                    linkedResourceToReturn
                );
        }

        [HttpPost(Name = "CreateTouristRoute")]
        [RequestHeaderMatchesMediaType(
            "Content-Type",
            "application/json",
            "application/vnd.fakeXiecheng.createTouristRoute+json"
        )]
        [Consumes("application/json", "application/vnd.fakeXiecheng.createTouristRoute+json")]
        //[Authorize]
        public async Task<IActionResult> CreateTouristRoute(TouristRouteForCreationDto touristRouteDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteDto);

            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            var links = CreateLinksForTouristRoute(touristRouteToReturn.Id, null);

            var linkedResourceToReturn =
                _mapper.Map<TouristRouteDto>(touristRouteToReturn).ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute(
                    "GetTouristRouteById",
                    new { touristRouteId = linkedResourceToReturn["Id"] },
                    linkedResourceToReturn
                );
        }

        [HttpOptions]
        public IActionResult GetTouristRouteOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTION, POST");
            return Ok();
        }

        [HttpPut("{touristRouteId}")]
        public async Task<IActionResult> UpdateTouristRouteById(Guid touristRouteId, TouristRouteForUpdateDto touristRouteDto)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                var touristRouteToAdd = _mapper.Map<TouristRoute>(touristRouteDto);
                touristRouteToAdd.Id = touristRouteId;
                _touristRouteRepository.AddTouristRoute(touristRouteToAdd);
                await _touristRouteRepository.SaveAsync();
                var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteToAdd);
                return CreatedAtRoute(
                    "GetTouristRouteById",
                    new { touristRouteId = touristRouteToReturn.Id },
                    touristRouteToReturn
                );
            }

            // map the entity to a CourseForUpdateDto
            // apply the updated field values to that dto
            // map the CourseForUpdateDto back to an entity
            _mapper.Map(touristRouteDto, touristRouteFromRepo);
            _touristRouteRepository.UpdateTouristRoute(touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }


        [HttpPatch("{touristRouteId}")]
        public async Task<IActionResult> PartiallyUpdateTouristRouteById(Guid touristRouteId,
            JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                var touristRouteDto = new TouristRouteForUpdateDto();
                patchDocument.ApplyTo(touristRouteDto, ModelState);
                if (!TryValidateModel(touristRouteDto))
                {
                    return ValidationProblem(ModelState);
                }

                var touristRouteToAdd = _mapper.Map<TouristRoute>(touristRouteDto);
                touristRouteToAdd.Id = touristRouteId;

                _touristRouteRepository.AddTouristRoute(touristRouteToAdd);
                await _touristRouteRepository.SaveAsync();

                var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteToAdd);
                return CreatedAtRoute(
                    "GetTouristRouteById",
                    new { touristRouteId = touristRouteToReturn.Id },
                    touristRouteToReturn
                );
            }

            // apply patch
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);

            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // update model by patch
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);

            // update database
            _touristRouteRepository.UpdateTouristRoute(touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        [HttpDelete("{touristRouteId}", Name = "DeleteTouristRoute")]
        public async Task<IActionResult> DeleteTouristRoute(Guid touristRouteId)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound();

            }

            _touristRouteRepository.DeleteTouristRoute(touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForTouristRoute(Guid touristRouteId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link("GetTouristRouteById", new { touristRouteId }) ,"self", "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(Url.Link("GetTouristRouteById", new { touristRouteId, fields }), "self", "GET"));
            }

            links.Add(new LinkDto(Url.Link("DeleteTouristRoute", new { touristRouteId }), "delete_tourist_route", "DELETE"));

            links.Add(new LinkDto(Url.Link("CreateTouristRoute", new { touristRouteId }), "create_tourist_route", "POST"));

            links.Add(new LinkDto(Url.Link("GetPictureListForTouristRoute", new { touristRouteId }), "prictures", "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForTouristRouteList(
            TouristRouteFilterParameters parameters,
            bool hasNext,
            bool hasPrevious
        )
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(CreateAuthorsResourceUri(parameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(new LinkDto(CreateAuthorsResourceUri(parameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateAuthorsResourceUri(parameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return links;
        }
    }
}
