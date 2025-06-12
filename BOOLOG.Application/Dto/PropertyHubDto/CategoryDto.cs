using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyDto
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Category name must be between 5 and 100 characters.")]
        public string CategoryName { get; set; }

    }
}