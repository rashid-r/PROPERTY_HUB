using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;


namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IPropertyService
    {
        Task<ApiResponse<List<GetAllPropertyDto>>> GetAllAsync();
        Task<ApiResponse<GetAllPropertyDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<PropertyDto>>> FilterProperty(PropertyQueryDto query);
        Task<ApiResponse<string>> AddPropertyAsync(PropertyDto dto, Guid userId);
        Task<ApiResponse<string>> UpdatePropertyAsync(PropertyDto propertydto, Guid id);
        Task<ApiResponse<string>> DeletePropertyAsync(Guid id);
    }
}
