
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        //Task<IEnumerable<CategoryEntity>> GetAllCategoryAsync();
        Task<CategoryEntity> GetCategoryByNameAsync(CategoryEntity category);
        Task AddCategoryAsync(CategoryEntity category);
        Task UpdateCategoryAsync(CategoryEntity category);
        //Task DeleteCategoryAsync(Guid id);
    }
}
