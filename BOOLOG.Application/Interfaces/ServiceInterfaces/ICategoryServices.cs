using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategoryAsync();
        Task<CategoryEntity> GetByCategoryAsync(string name);
        Task<ResponseDto> AddCategoryAsync(CategoryDto name);
        Task<ResponseDto> UpdateCategoryAsync(Guid id, string CategoryName);
        Task<ResponseDto> DeleteCategoryAsync(Guid id);

    }
}
