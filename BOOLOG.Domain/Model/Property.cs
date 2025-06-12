using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Domain.Model
{
    public class Property
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public PropertyTypes Type { get; set; } //rend,buy,lease
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; } 
        public Guid LocationId { get; set; }
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
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
}
public enum PropertyTypes
{
    [Display(Name = "For Rent")]
    Rent,

    [Display(Name = "For Lease")]
    Lease,

    [Display(Name = "For Sale")]
    Buy
}