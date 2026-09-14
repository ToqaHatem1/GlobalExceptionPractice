using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            throw new Exception("Something went wrong!");
        }
    }
}
