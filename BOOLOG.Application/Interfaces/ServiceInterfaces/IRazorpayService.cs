using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.GetAllDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IRazorpayService
    {
        Task<ApiResponse<string>> AddPaymentAsync(Guid userId, Guid propertyId, decimal amount);
        Task<ApiResponse<List<GetAllRazorpayDto>>> GetAllPayments();
    }
}
