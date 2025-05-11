using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("secret")]
        [Authorize]
        public IActionResult Secret()
        {
            return Ok("با توکن اجازه ورود به این صفحه را یافتید");
        }
    }
}

