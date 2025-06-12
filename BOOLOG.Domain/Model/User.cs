using System.Collections;
using System.ComponentModel.DataAnnotations;
using BOOLOG.Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BOOLOGAM.Domain.Model

{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public bool IsBlocked { get; set; } = false;
        public Roles Role { get; set; } = Roles.Buyer;
        public ICollection<Property> Properties { get; set; }
        public UserProfile UserProfile { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
       
    }

    public enum Roles
    {
        Buyer,
        Seller,
        Admin
    }
}
 