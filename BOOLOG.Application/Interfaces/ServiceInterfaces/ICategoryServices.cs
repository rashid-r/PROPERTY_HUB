using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface ICategoryServices
    {
        Task<ApiResponse<List<CategoryDto>>> GetAllCategoryAsync();
        Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(Guid id);
        Task<ApiResponse<string>> AddCategoryAsync(string name);
        Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto dto);
        Task<ApiResponse<string>> DeleteCategoryAsync(Guid id);

    }
}
