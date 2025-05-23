using System.Collections;
using System.ComponentModel.DataAnnotations;
using BOOLOG.Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BOOLOGAM.Domain.Model

{
    public class UserEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string BlockedUser { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public ICollection<PropertyEntity> propertyEntries { get; set; }
    }
}
