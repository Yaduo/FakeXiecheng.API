using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            // create links for root
            var links = new List<LinkDto>();

            links.Add(new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"));

            links.Add(new LinkDto(Url.Link("GetTouristRoutes", new { }), "tourist_route_list", "GET"));

            links.Add(new LinkDto(Url.Link("CreateCreateTouristRoute", new { }), "create_tourist_route", "POST"));

            return Ok(links);
        }
    }
}
