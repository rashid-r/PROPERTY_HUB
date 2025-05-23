
using BOOLOG.Application.Repository.CategoryRepository;
using BOOLOG.Domain.Model;
using BOOLOGAM.Infrastructure.Db_Context;

namespace BOOLOG.Infrastructure.Repository.Repository_Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<IEnumerable<CategoryEntity>> GetAllCategoryAsync()
        //{
        //    return await _dbContext.CategoryModel.ToListAsync();
        //}
        public async Task<CategoryEntity> GetCategoryByNameAsync(CategoryEntity category)
        {
            return _dbContext.CategoryModel.FirstOrDefault(x => x.CategoryName == category.CategoryName);
        }
        public async Task AddCategoryAsync(CategoryEntity category)
        {
            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(CategoryEntity category)
        {
            _dbContext.Update(category);
            await _dbContext.SaveChangesAsync();
        }
        //public async Task DeleteCategoryAsync(Guid id)
        //{
        //    _dbContext.Remove(id);
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
