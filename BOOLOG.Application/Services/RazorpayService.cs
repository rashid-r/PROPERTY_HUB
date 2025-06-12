using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Razorpay.Api;


namespace BOOLOG.Application.Services
{
    public class RazorpayService : IRazorpayService
    {
        private readonly IRepository<RazorPay> _RazorRepo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public RazorpayService(IRepository<RazorPay> repo, IConfiguration config, IMapper mapper)
        {
            _RazorRepo = repo;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddPaymentAsync(Guid userId, Guid propertyId, decimal amount)
        {
            RazorpayClient client = new RazorpayClient(_config["Razorpay:Key"], _config["Razorpay:Secret"]);

            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount * 100 },
            { "currency", "INR" },
            { "payment_capture", 1 }
        };

            Razorpay.Api.Order order = client.Order.Create(options);

            RazorPay map = new RazorPay
            {
                UserId = userId,
                PropertyId = propertyId,
                Amount = amount,
                OrderId = order["id"].ToString(),
                Date = DateTime.UtcNow
            };

            await _RazorRepo.AddAsync(map);
            
            var id = order["id"].ToString();
            return new ApiResponse<string>(200, "Payment recorded successfully", id);
        }

        public async Task<ApiResponse<List<GetAllRazorpayDto>>> GetAllPayments()
        {
            var result = await _RazorRepo.GetAllAsync();
            var map = _mapper.Map<List<GetAllRazorpayDto>>(result);
            return new ApiResponse<List<GetAllRazorpayDto>>(200, "Payments retrived successfully", map);
        }
    }
}
