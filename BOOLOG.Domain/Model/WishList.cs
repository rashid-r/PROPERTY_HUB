using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Domain.Model
{
    public class WishList
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
        public User user { get; set; }
        public Guid PropertyId { get; set; }
        public Property property { get; set; }

    }
}
