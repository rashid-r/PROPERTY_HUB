using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class Propertydto
    {
        [Required]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(PropertyType), ErrorMessage = "Type must be Rent, Lease, or Buy.")]
        public PropertyType PropertyPurpose { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number (e.g., 1000 or 1000.00)")]
        public Decimal Price { get; set; }

        [Required(ErrorMessage = "Created date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Created date must be a valid date and time")]
        public DateTime CreatedDate { get; set; }
    }
}

