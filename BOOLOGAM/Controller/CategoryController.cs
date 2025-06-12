using System.Text.RegularExpressions;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _category;

        public CategoryController(ICategoryServices category)
        {
            _category = category;
        }
        
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _category.GetAllCategoryAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        
        [HttpGet("GetByCategoryName")]
        public async Task<IActionResult> GetByCategory(Guid Id)
        {
            
            var result = await _category.GetCategoryByIdAsync(Id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm]string name)
        {
            var regex = new Regex(@"^[a-zA-Z][a-zA-Z\s]*$");
            if (!regex.IsMatch(name))
            {
                return BadRequest("Name must start with a letter and contain only letters and spaces.");
            }
            var result = await _category.AddCategoryAsync(name);
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
        
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromForm]CategoryDto dto)
        {
            
            var result = await _category.UpdateCategoryAsync(dto);
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
        
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromForm] Guid id)
        {
            var result = await _category.DeleteCategoryAsync(id);
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