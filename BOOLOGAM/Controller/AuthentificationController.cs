using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
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
            var res=await _auth_service.Registeration(registerDto);
            return Ok(res);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var result = await _auth_service.Login(loginDto);
            if (string.IsNullOrEmpty(result.Token))
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

    }
}
