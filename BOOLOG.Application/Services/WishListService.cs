using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Services
{
    public class WishListService : IWishListService
    {
        private readonly IRepository<WishList> _wishRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Property> _proRepo;
        private readonly IMapper _mapper;

        public WishListService(IRepository<WishList> wishrepo, IMapper mapper, IRepository<Property> proRepo, IRepository<User> userRepo)
        {
            _mapper = mapper;
            _wishRepo = wishrepo;
            _userRepo = userRepo;
            _proRepo = proRepo;
        }

        public async Task<ApiResponse<string>> AddWishList(Guid UserId, Guid PropertyId)
        {
            var user = await _userRepo.GetByIdAsync(UserId);
            var property = await _proRepo.GetByIdAsync(PropertyId);

            if (user == null || property == null)
                return new ApiResponse<string>(404, "User or Property not found");

            var allWishes = await _wishRepo.GetAllAsync();
            var alreadyExists = allWishes.Any(w =>
                w.UserId == UserId && w.PropertyId == PropertyId);

            if (alreadyExists)
                return new ApiResponse<string>(400, "Property already in wishlist");

            var map = new WishList
            {
                UserId = UserId,
                PropertyId = PropertyId,
                DateTime = DateTime.UtcNow
            };

            await _wishRepo.AddAsync(map);
            return new ApiResponse<string>(200, "Added to wishlist successfully");
        }

        public async Task<ApiResponse<string>> RemoveWishList(Guid id)
        {
            var result = await _wishRepo.GetByIdAsync(id);
            if (result == null)
                return new ApiResponse<string>(404, "Wishlist Id not found.");

            await _wishRepo.DeleteAsync(id);
            return new ApiResponse<string>(200, "Deleted Successfully");
        }
        public async Task<ApiResponse<List<WishlistDto>>> GetWishlist(Guid UserId)
        {
            var allWishes = await _wishRepo.GetAllAsync();
            var userWishes = allWishes.Where(w => w.UserId == UserId).ToList();

            var map = _mapper.Map<List<WishlistDto>>(userWishes);
            return new ApiResponse<List<WishlistDto>>(200, "Wishlist retrieved successfully", map);
        }

    }
}
