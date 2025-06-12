using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Dto.GetAllDto
{
    public class GetAllRazorpayDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PropertyId { get; set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }   // Razorpay Order ID
        public DateTime Date { get; set; }
    }
}
