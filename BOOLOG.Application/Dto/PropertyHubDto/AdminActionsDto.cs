using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Dto.PropertyHubDto
{
    public class AdminActionsDto
    {
        public Guid UserId { get; set; }
        public bool IsBlocked { get; set; }
        public KycStatus KycStatus { get; set; }
    }
}
