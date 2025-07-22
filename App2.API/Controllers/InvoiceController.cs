using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvoice()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier);
            return Ok(userIdClaim.Value);
        }
    }
}
