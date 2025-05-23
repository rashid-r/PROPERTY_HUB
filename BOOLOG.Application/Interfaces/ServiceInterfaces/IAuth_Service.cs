using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.ResponseDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IAuth_Service
    {
        Task<ResponseDto> Registeration(RegisterDto registerDto);
        Task<ResponseDto> Login(LoginDto loginDto);
    }
}
