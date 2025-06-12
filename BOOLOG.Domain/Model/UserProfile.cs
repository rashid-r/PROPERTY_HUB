using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Domain.Model
{
    public class UserProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Genders Gender { get; set; }
        public long AadhaarIdNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public double PostalCode { get; set; }

        public KycStatus KycStatus { get; set; } = KycStatus.Pending;
        public DateTime SubmittedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

    }

    public enum Genders
    {
        Male,
        Female,
        Other
    }
    public enum KycStatus
    {
        Pending,
        Approved,
        Rejected
    }

}
