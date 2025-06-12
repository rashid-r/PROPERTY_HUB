using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class PropertyDto
    {
        [Required]
        public Guid LocationId { get; set; }
        
        [Required]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [EnumDataType(typeof(PropertyTypes), ErrorMessage = "Type must be Rent, Lease, or Buy.")]
        public PropertyTypes Type { get; set; }
        [Required]
        public PropertyStatus Status { get; set; } 

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number (e.g., 1000 or 1000.00)")]
        public decimal Price { get; set; }
        
    }
}

