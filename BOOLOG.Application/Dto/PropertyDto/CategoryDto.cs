using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyDto
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Category name must be between 5 and 100 characters.")]
        public string CategoryName { get; set; }
    }
}