using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;


namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyEntity>> GetAllAsync();
        Task<Propertydto> GetByIdAsync(Guid id);
        Task<ResponseDto> AddPropertyAsync(Propertydto propertydto, Guid userId);
        Task<ResponseDto> UpdatePropertyAsync(Propertydto propertydto, Guid id);
        Task<ResponseDto> DeletePropertyAsync(Guid id);
    }
}
