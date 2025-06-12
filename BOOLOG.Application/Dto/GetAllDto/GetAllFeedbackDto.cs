using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Dto.GetAllDto
{
    public class GetAllFeedbackDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
