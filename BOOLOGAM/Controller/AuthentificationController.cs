using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuth_Service _auth_service;

        public AuthentificationController(IAuth_Service auth_Service)
        {
            _auth_service = auth_Service;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromForm]RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _auth_service.Registeration(registerDto);
            if(res.StatusCode == 200)
            {
                return Ok(res);
            }else if (res.StatusCode == 406)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, res);
            }
            else
            {
                return BadRequest(res);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var result = await _auth_service.Login(loginDto);
            if (string.IsNullOrEmpty(result.Token))
            {
                return Unauthorized(result);
            }
            if(result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else if(result.StatusCode == 403)
            {
                return StatusCode(StatusCodes.Status403Forbidden, result);
            }
            else if(result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.StatusCode == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return BadRequest(result);

        }

        //[Authorize(Roles = "Admin")]
        [HttpPatch("ApproveKyc")]
        public async Task<IActionResult> ApproveKyc([FromForm]bool IsApproved,Guid UserId)
        {
            
            var result = await _auth_service.ApproveKyc(IsApproved, UserId);
            return StatusCode(result.StatusCode, result.Message);

        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _auth_service.GetAllUsers();
            if (result.StatusCode == 200) 
            { 
                return Ok(result); 
            }
            else
            {
                return BadRequest(result);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPatch("Block")]
        public async Task<IActionResult> BlockUnBlock(Guid UserId)
        {
            var result = await _auth_service.BlockUnblockUserAsync(UserId);
            return StatusCode(result.StatusCode, result.Message);
        }

    }
}
