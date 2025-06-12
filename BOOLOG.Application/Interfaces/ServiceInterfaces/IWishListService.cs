using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.PropertyHubDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IWishListService
    {
        Task<ApiResponse<string>> AddWishList(Guid UserId, Guid PropertyId);
        Task<ApiResponse<string>> RemoveWishList(Guid id);
        Task<ApiResponse<List<WishlistDto>>> GetWishlist(Guid UserId);
    }
}
