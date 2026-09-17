using GlobalExceptionPractice.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) 
        { 
            if (id != 1) 
            { 
                throw new NotFoundException($"Product with ID {id} was not found."); 
            } 
            return Ok(new 
            { 
                id = 1, 
                name = "Laptop", 
                price = 25000 
            }); 
        }

        [HttpGet("error")] 
        public IActionResult GetError() 
        { 
            throw new Exception("Something unexpected happened."); 
        }
        [HttpGet("badRequest")]
        public IActionResult GetBadRequest()
        {
            throw new BadRequestException("Bad request.");
        }
    }
}
