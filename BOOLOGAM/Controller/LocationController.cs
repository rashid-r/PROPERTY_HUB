using System.Text.RegularExpressions;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("GetAllLocation")]
        public async Task<IActionResult> GetAllAsync()
        {
            var allpro = await _locationService.GetAllLocationAsync();
            if (allpro.StatusCode == 200)
            {
                return Ok(allpro);
            }
            else
            {
                return BadRequest(allpro);
            }
        }

        [HttpGet("GetLocationById")]
        public async Task<IActionResult> GetLocationById(Guid id)
        {
            var result = await _locationService.GetLocationById(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] string name)

        {

            var regex = new Regex(@"^[a-zA-Z][a-zA-Z\s]*$");
            if (!regex.IsMatch(name))
            {
                return BadRequest("Name must start with a letter and contain only letters and spaces.");
            }
            var result = await _locationService.AddLocationAsync(name);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            } else if (result.StatusCode == 406)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable,result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPut("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation([FromForm]LocationDto dto)
        {
            var result = await _locationService.UpdateLocationAsync(dto);
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
                return StatusCode(StatusCodes.Status406NotAcceptable,result);
            }
            else 
            {
                return BadRequest(result);
            }
        }
        [HttpDelete("DeleteLocatio")]
        public async Task<IActionResult> DeleteLocation([FromForm]Guid id)
        {
            var result = await _locationService.DeleteLocationAsync(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }else if (result.StatusCode == 404)
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
