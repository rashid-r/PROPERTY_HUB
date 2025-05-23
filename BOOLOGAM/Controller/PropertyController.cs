using System.Security.Claims;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;


namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _property;

        public PropertyController(IPropertyService property)
        {
            _property = property;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var pro = await _property.GetByIdAsync(id);
            return Ok(pro);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var allpro =await _property.GetAllAsync();
            return Ok(allpro);
        }
        [HttpPost("AddProperty")]

        public async Task<IActionResult> AddPropertyAsync([FromForm]Propertydto propertydto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var add = await _property.AddPropertyAsync(propertydto, userId);
            return Ok(add);
        }
        [HttpPut("UpdateProperty")]
        public async Task<IActionResult> UpdateProperyAsync([FromForm]Propertydto propertydto, Guid id)
        {
            await _property.UpdatePropertyAsync(propertydto, id);
            return Ok("Property Updated Successfully");
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePropertyAsync(Guid id)
        {
            var pro = await _property.DeletePropertyAsync(id);
            return Ok(pro);
        }
    }
}
