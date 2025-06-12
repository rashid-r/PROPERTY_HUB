using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Domain.Model
{
    public class Location
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LocationName { get; set; }
    }
}
