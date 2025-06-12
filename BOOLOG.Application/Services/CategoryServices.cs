using System.Collections.Immutable;
using AutoMapper;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;

using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;


namespace BOOLOG.Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IRepository<Category> _repository;
        //private readonly IRepository<LocationEntity> _locRepo;
        private readonly IMapper _mapper;

        public CategoryServices(IRepository<Category> repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllCategoryAsync()
        {
            var all = await _repository.GetAllAsync();
            var map = _mapper.Map<List<CategoryDto>>(all);
            return new ApiResponse<List<CategoryDto>>
            (
                200,
                "Categories retrieved successfully.",
                map
            );
        }

        public async Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id); 
                if(category == null)
                return new ApiResponse<CategoryDto> (404, $"CategoryId '{id}' not found.");

            var map = _mapper.Map<CategoryDto>(category);
            return new ApiResponse<CategoryDto> 
            (
                200,
                "Category found successfully.",
                map
            );
        }

        public async Task<ApiResponse<string>> AddCategoryAsync(string name)
        {
            var cat = (await _repository.GetAllAsync()).FirstOrDefault(x => x.CategoryName.ToLower() == name.ToLower());

            if (cat != null)
            {
                return new ApiResponse<string> (406,$"Not Acceptable!!..Category with name '{name}' already exists." );
            }

            //var loc = new LocationEntity();
           
            var category = new Category { CategoryName = name};
            await _repository.AddAsync(category);
            return new ApiResponse<string> 
            (
                200,
                "Category Created Successfully"
            );
        }

        public async Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto dto)
        {
            var Cat = await _repository.GetByIdAsync(dto.Id);
            if (Cat != null)
            {
                Cat.CategoryName = dto.CategoryName;
                await _repository.UpdateAsync(Cat);
                return new ApiResponse<CategoryDto> (200,"Category Updated Successfully" );   
            }

            return new ApiResponse<CategoryDto> (404, $"Category with name '{dto.CategoryName}' not exists. Pls enter a valid category Id");
        }

        public async Task<ApiResponse<string>> DeleteCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) 
                return new ApiResponse<string> (404, "Category not found.");
            await _repository.DeleteAsync(id);
            return new ApiResponse<string> (200, "Category Deleted Successfully");
        }
    }
}