using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakeVanderHttpRequestController : Controller
    {
        [HttpGet()]
        public async Task<IActionResult> FakeGetRequest( bool returnFault = false)
        {
            // if returnFault is true, wait 500ms and
            // return an Internal Server Error
            if (returnFault)
            {
                await Task.Delay(600);
                return new StatusCodeResult(500);
            }

            // generate a byte array between 2 and 10MB
            var random = new Random();
            int bytes = random.Next(2097152, 10485760);
            byte[] results = new byte[bytes];
            random.NextBytes(results);

            return Ok(new
            {
                Content = results
            });
        }
    }
}
