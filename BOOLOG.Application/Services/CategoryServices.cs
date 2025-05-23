using System.Collections.Immutable;
using AutoMapper;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.CategoryRepository;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;


namespace BOOLOG.Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _category;
        private readonly IRepository<CategoryEntity> _repository;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRepository repository, IRepository<CategoryEntity> repo, IMapper mapper)
        {
            _repository = repo;
            _category = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllCategoryAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CategoryEntity> GetByCategoryAsync(string name)
        {
            var category = (await _repository.GetAllAsync()).FirstOrDefault(n => n.CategoryName == name) ??
                throw new Exception($"Category with name '{name}' not found.");
            return category;
        }

        public async Task<ResponseDto> AddCategoryAsync(CategoryDto dto)
        {
            var cat = (await _repository.GetAllAsync()).FirstOrDefault(x => x.CategoryName == dto.CategoryName);

            if (cat != null)
            {
                return new ResponseDto { Message = $"Category with name '{dto.CategoryName}' already exists." };
            }
            
            var map = _mapper.Map<CategoryEntity>(dto);
            await _category.AddCategoryAsync(map);
            return new ResponseDto { Message = "Category Created Successfully" };
        }

        public async Task<ResponseDto> UpdateCategoryAsync(Guid id, string CategoryName)
        {
            var Cat = await _repository.GetByIdAsync(id);
            if (Cat != null)
            {
                //Cat.CategoryName= Name;
                await _category.UpdateCategoryAsync(Cat);
                return new ResponseDto { Message = "Category Updated Successfully" };   
            }

            return new ResponseDto { Message = $"Category with Id '{CategoryName}' not exists. Pls enter a valid category Id" };
        }

        public async Task<ResponseDto> DeleteCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Category not found.");
            await _repository.DeleteAsync(id);
            return new ResponseDto { Message = "Category Deleted Successfully" };
        }
    }
}