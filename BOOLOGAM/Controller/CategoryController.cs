using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "User")]
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var all = await _category.GetAllCategoryAsync();
            return Ok(all);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetByCategory(string name)
        {
            
            var cat = await _category.GetByCategoryAsync(name);
            return Ok(cat);
        }
        [Authorize(Roles = "User")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto name)
        {
            var cat = await _category.AddCategoryAsync(name);
            return Ok(cat);
        }
        [Authorize(Roles = "User")]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Guid id, string CategoryName)
        {
            
            var cat = await _category.UpdateCategoryAsync(id, CategoryName);
            return Ok(cat);
        }
        [Authorize(Roles = "User")]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromForm] Guid id)
        {
            var cat = await _category.DeleteCategoryAsync(id);
            return Ok(cat);
        }
    }
}