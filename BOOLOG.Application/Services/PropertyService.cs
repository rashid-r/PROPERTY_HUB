
using AutoMapper;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;



namespace BOOLOG.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepository<PropertyEntity> _proRepo;
        private readonly IRepository<CategoryEntity> _catrepo;
        private readonly IMapper _mapper;

        public PropertyService(IRepository<PropertyEntity> repository, IMapper mapper, IRepository<CategoryEntity> catrepo)
        {
            _proRepo = repository;
            _mapper = mapper;
            _catrepo = catrepo;
        }

        public async Task<IEnumerable<PropertyEntity>> GetAllAsync()
        {
            return await _proRepo.GetAllAsync();
        }

        public async Task<Propertydto> GetByIdAsync(Guid id)
        {
            var pro = await _proRepo.GetByIdAsync(id) ?? throw new Exception("Property with ID {id} not found.");

            var map = _mapper.Map<Propertydto>(pro);
            return map;
        }

        public async Task<ResponseDto> AddPropertyAsync(Propertydto dto,Guid userId)
        {
            var cat = (await _catrepo.GetAllAsync()).FirstOrDefault(x=>x.CategoryName == dto.CategoryName);

            if (cat == null)
            {
                return new ResponseDto { Message = $"Category with name '{dto.CategoryName}' not listed in the category list." };
            }
            //var map = _mapper.Map<PropertyEntity>(dto);
            //await _proRepo.AddAsync(map);
            //return new ResponseDto { Message = "Property added successfully" };
             var propertyEntity = new PropertyEntity
            {
                CategoryName = dto.CategoryName,
                Title = dto.Title,
                Description = dto.Description,
                PropertyPurpose = dto.PropertyPurpose,
                Price = dto.Price,
                CreatedDate = dto.CreatedDate,
                CategoryEntityId = cat.Id,
                 UserEntityId = userId,
                 Status = PropertyStatus.Pending,
             };
            await _proRepo.AddAsync(propertyEntity);
            return new ResponseDto { Message = "Property added successfully" };
        }
        public async Task<ResponseDto> UpdatePropertyAsync(Propertydto propertydto, Guid id)
        {
            var existingProperty = await _proRepo.GetByIdAsync(id)
                                    ?? throw new Exception("Property not found... Please check the Id");

            _mapper.Map(propertydto, existingProperty);

            await _proRepo.UpdateAsync(existingProperty);

            return new ResponseDto { Message = "Property Updated Successfully" };
        }
        public async Task<ResponseDto> DeletePropertyAsync(Guid id)
        {
            await _proRepo.DeleteAsync(id);
            return new ResponseDto { Message = $"Property has been deleted successfully." };
        }

    }
}
