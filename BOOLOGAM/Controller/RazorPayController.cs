using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RazorPayController : ControllerBase
    {
        private readonly IRazorpayService _razorpay;

        public RazorPayController(IRazorpayService razorpay)
        {
            _razorpay = razorpay;
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> AddPayment(Guid propertyId, decimal amount)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID claim is missing from token.");
            }

            var UserId = Guid.Parse(userIdClaim);
            var result = await _razorpay.AddPaymentAsync(UserId, propertyId, amount);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetAllPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var all = await _razorpay.GetAllPayments();
            if (all.StatusCode == 200)
            {
                return Ok(all);
            }
            else
            {
                return BadRequest(all);
            }
        }
    }
}
