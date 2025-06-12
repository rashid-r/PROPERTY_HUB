using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListPropertyController : ControllerBase
    {
        private readonly IWishListService _IWishListService;

        public WishListPropertyController(IWishListService IWishListService)
        {
            _IWishListService = IWishListService;
        }

        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> AddToWishlist(Guid PropertyId)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);
            var result = await _IWishListService.AddWishList(UserId,PropertyId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest("Something went wrong pls try again");
            }
        }

        [HttpGet("GetWishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);

            var result = await _IWishListService.GetWishlist(UserId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Something went wrong pls try again");
            }
        }

        [HttpDelete("RemoveWishlist")]
        public async Task<IActionResult> RemoveWishlist(Guid id)
        {
            var result = await _IWishListService.RemoveWishList(id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if(result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest("Something went wrong pls try again");
            }
        }
    }
}
