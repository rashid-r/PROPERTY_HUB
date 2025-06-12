using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Dto.AuthDto
{
    public class AuthResponse
    {
        public int StatusCode { get; set; }
        public bool IsBlocked { get; set; }
        public string UserName { get; set; }
        public Roles Role { get; set; } = Roles.Buyer;
        public string Message { get; set; }
        public string Token { get; set; }

        public AuthResponse(int statusCode, string userName = null, Roles role = Roles.Buyer , string message = null, string token = null)
        {
            StatusCode = statusCode;
            UserName = userName;
            Role = role;
            Message = message;
            Token = token;
        }
    }

}
