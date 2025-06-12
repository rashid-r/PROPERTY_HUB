using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Dto.GetAllDto
{
    public class GetAllPropertyDto
    {
        public Guid Id { get; set; } 
        public Guid CategoryId { get; set; }
        public Guid LocationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } //rend,buy,lease
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
