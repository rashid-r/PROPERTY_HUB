using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Dto.GetAllDto
{
    public class GetAllUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Contact { get; set; }
        public bool IsBlocked { get; set; }
        public Roles Role { get; set; } 
    }
}
