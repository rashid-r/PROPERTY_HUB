using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Domain.Model
{
    public class PropertyEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PropertyType PropertyPurpose { get; set; } //rend,buy,lease
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;
        public Decimal Price { get; set; }
        //public Guid LocationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserEntityId { get; set; }
        public virtual UserEntity userEntities { get; set; }
        public Guid CategoryEntityId { get; set; } 
        public virtual CategoryEntity categoryEntities { get; set; }
    }
}
public enum PropertyStatus
{
    [Display(Name = "Available")]
    Available,
    [Display(Name = "Not Available")]
    NotAvailable,
    [Display(Name = "Rented")]
    Rented,
    [Display(Name = "Sold")]
    Sold,
    [Display(Name = "Leased")]
    Leased,
    [Display(Name = "Pending")]
    Pending,
}
public enum PropertyType
{
    [Display(Name = "For Rent")]
    Rent,

    [Display(Name = "For Lease")]
    Lease,

    [Display(Name = "For Sale")]
    Buy
}