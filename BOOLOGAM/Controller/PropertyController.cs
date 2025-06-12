using System.Security.Claims;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _property;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(IPropertyService property , ILogger<PropertyController> logger)
        {
            _property = property;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _property.GetAllAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _property.GetByIdAsync(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("FilterProperty")]
        public async Task<IActionResult> FilterAsync([FromQuery] PropertyQueryDto query)
        {
            
            var result = await _property.FilterProperty(query);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [Authorize]
        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddPropertyAsync([FromForm] PropertyDto propertydto)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }
            var userId = Guid.Parse(userIdClaim);

            var result = await _property.AddPropertyAsync(propertydto, userId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.StatusCode == 406)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateProperty")]
        public async Task<IActionResult> UpdateProperyAsync([FromForm]PropertyDto propertydto, Guid id)
        {
            var result = await _property.UpdatePropertyAsync(propertydto, id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePropertyAsync(Guid id)
        {
            var result = await _property.DeletePropertyAsync(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
