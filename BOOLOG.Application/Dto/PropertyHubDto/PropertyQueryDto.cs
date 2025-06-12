using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Dto.PropertyDto
{
    public class PropertyQueryDto
    {
        public string? SearchText { get; set; }
        public List<Guid>? Locations { get; set; }
        public List<Guid>? Categories { get; set; }
        public decimal? MaxPrice { get; set; } 
        public decimal? MinPrice { get; set; }
    }
}
