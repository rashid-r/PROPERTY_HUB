using System.Security.Claims;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfile;

        public UserProfileController(IUserProfileService userProfile)
        {
            _userProfile = userProfile;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUserProfile()
        {
            var all = await _userProfile.GetAllUserProfile();
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }
            else
            {
                return BadRequest(all);
            }
        }
        [HttpGet("UserProfileById")]
        public async Task<IActionResult> GetUserProfileById(Guid id)
        {
            var all = await _userProfile.GetUserProfileById(id);
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }else if (all.StatusCode == 404)
            {
                return NotFound(all);
            }
            else
            {
                return BadRequest(all);
            }
        }
        [HttpPost("AddUserProfile")]
        public async Task<IActionResult> AddUserProfile([FromForm]UserProfileDto dto)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);
            var all = await _userProfile.AddUserProfile(dto,UserId);
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }
            else if (all.StatusCode == 406)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable,all);
            }
            else
            {
                return BadRequest(all);
            }
        }


        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileDto dto,Guid id)
        {
            var all = await _userProfile.UpdateUserProfile(dto,id);
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }
            else if (all.StatusCode == 406)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, all);
            }
            else
            {
                return BadRequest(all);
            }
        }
        [HttpDelete("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile([FromForm] Guid id)
        {
            var all = await _userProfile.DeleteUserProfile(id);
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }
            else if (all.StatusCode == 404)
            {
                return NotFound(all);
            }
            else
            {
                return BadRequest(all);
            }
        }
    }
}
