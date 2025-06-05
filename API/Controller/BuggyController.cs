using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseAPIController
    {
        [HttpGet("unauthorized")] // 401
        public IActionResult GetUnauthorized()
        {
            return Unauthorized("You are not authorized to access this resource.");
        }

        [HttpGet("badrequest")] // 400
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a god request.");
        }

        [HttpGet("notfound")] // 404
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("internalerror")]// 500
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a server error.");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(ProductDTO productdto)
        {
            return Ok();
        }
    }
}
