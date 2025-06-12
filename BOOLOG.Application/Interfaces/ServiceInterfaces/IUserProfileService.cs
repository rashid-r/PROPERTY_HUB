using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IUserProfileService
    {
        Task<ApiResponse<List<GetallUserProfileDto>>> GetAllUserProfile();
        Task<ApiResponse<GetallUserProfileDto>> GetUserProfileById(Guid UserId);
        Task<ApiResponse<string>> AddUserProfile(UserProfileDto dto, Guid UserId);
        Task<ApiResponse<string>> UpdateUserProfile(UserProfileDto dto, Guid id);
        Task<ApiResponse<string>> DeleteUserProfile(Guid UserId);
    }
}
