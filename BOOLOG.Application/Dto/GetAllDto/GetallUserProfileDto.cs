using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Dto.GetAllDto
{
    public class GetallUserProfileDto
    {
        public Guid UserProfileId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public long AadhaarIdNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public double PostalCode { get; set; }
    }
}
