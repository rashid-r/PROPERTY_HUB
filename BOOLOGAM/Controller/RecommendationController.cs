
using BOOLOG.Application.Interfaces.ServiceInterfaces; 
using BOOLOG.Application.Dto.PropertyHubDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("TrainModel")]
        public async Task<IActionResult> TrainModel()
        {
            var result = await _recommendationService.TrainModelAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(result.StatusCode, result);
            }
        }

        [HttpGet("ForUser")]
        public async Task<IActionResult> GetRecommendationsForCurrentUser([FromQuery] int count = 5)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ApiResponse<string>(401, "User ID claim is missing from token."));
            }

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return BadRequest(new ApiResponse<string>(400, "Invalid User ID format in token."));
            }

            if (!_recommendationService.IsModelTrained())
            {
                return StatusCode(503, new ApiResponse<string>(503, "Recommendation model is not yet trained. Please try again later or train the model first."));
            }

            var result = await _recommendationService.GetRecommendedPropertiesForUserAsync(userId,count);

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
                return StatusCode(result.StatusCode, result);
            }
        }

        [HttpGet("ForUser/{userId}")]
        public async Task<IActionResult> GetRecommendationsForSpecificUser(Guid userId, [FromQuery] int count = 5)
        {
            if (!_recommendationService.IsModelTrained())
            {
                return StatusCode(503, new ApiResponse<string>(503, "Recommendation model is not yet trained. Please try again later or train the model first."));
            }

            var result = await _recommendationService.GetRecommendedPropertiesForUserAsync(userId, count);

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
                return StatusCode(result.StatusCode, result);
            }
        }
    }
}
