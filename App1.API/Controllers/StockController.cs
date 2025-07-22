using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStock()
        {
            var UserIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok(UserIdClaim.Value);
        }
    }
}
