using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Dto.PropertyHubDto
{
    public class MultiUserNotificationDto
    {
        public List<string> UserIds { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
