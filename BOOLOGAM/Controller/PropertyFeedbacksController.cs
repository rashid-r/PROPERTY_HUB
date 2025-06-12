using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyFeedbacksController : ControllerBase
    {
        private readonly IFeedbackServices _profeeds;
        public PropertyFeedbacksController(IFeedbackServices profeeds)
        {
            _profeeds = profeeds;
        }

        [HttpGet("GetAllFeedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _profeeds.GetAllFeedbacks();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("GetFeedbackById")]
        public async Task<IActionResult> GetFeedbackById(Guid FeedbackId)
        {
            var result = await _profeeds.GetFeedbackById(FeedbackId);
            if (result.StatusCode == 200) 
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("AddFeedbacks")]
        public async Task<IActionResult> AddFeedbacks([FromForm]AddFeedbackDto dto)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);

            var result = await _profeeds.AddFeedbacks(dto, UserId);
            if (result.StatusCode == 200) 
            {
                return Ok(result);
            }else if(result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPut("UpdateFeedback")]
        public async Task<IActionResult> UpdateFeedback([FromForm]UpdateFeedbackDto dto, Guid fbId)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);

            var result = await _profeeds.UpdateFeedbacks(dto,fbId, UserId);
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
        [HttpDelete("DeleteFeedbacks")]
        public async Task<IActionResult> DeleteFeedbacks(Guid id)
        {
            var result = await _profeeds.DeleteFeedbacks(id);
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
