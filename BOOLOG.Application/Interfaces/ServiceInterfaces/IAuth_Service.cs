using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Dto.ResponseDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IAuth_Service
    {
        Task<AuthResponse> Registeration(RegisterDto registerDto);
        Task<AuthResponse> Login(LoginDto loginDto);
        Task<ApiResponse<List<GetAllUserDto>>> GetAllUsers();
        Task<ApiResponse<string>> ApproveKyc(bool IsApproved, Guid UserId);
        Task<ApiResponse<string>> BlockUnblockUserAsync(Guid UserId);
    }
}
