using AutoMapper;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOG.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;



namespace BOOLOG.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepository<Property> _proRepo;
        private readonly IRepository<Location> _locRepo;
        private readonly IRepository<Category> _catrepo;
        private readonly IPropertyRepository _filterRepo;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        

        public PropertyService(
            IRepository<Property> repository,
            IMapper mapper,
            IRepository<Category> catrepo,
            IPropertyRepository propertyrepo,
            IRepository<Location> locRepo,
            IHubContext<NotificationHub> hubContext
            )
        {
            _proRepo = repository;
            _mapper = mapper;
            _catrepo = catrepo;
            _filterRepo = propertyrepo;
            _locRepo = locRepo;
            _hubContext = hubContext;
        }

        public async Task<ApiResponse<List<GetAllPropertyDto>>> GetAllAsync()
        {
            var all = await _proRepo.GetAllAsync();
            var map = _mapper.Map<List<GetAllPropertyDto>>(all);
            return new ApiResponse<List<GetAllPropertyDto>>(200,"Propert Retrived Successfully",map);
        }

        public async Task<ApiResponse<GetAllPropertyDto>> GetByIdAsync(Guid id)
        {
            var pro = await _proRepo.GetByIdAsync(id);  
            if(pro == null) return new ApiResponse<GetAllPropertyDto>(404, $"Category with name '{id} not found.");

            var map = _mapper.Map<GetAllPropertyDto>(pro);
            return new ApiResponse<GetAllPropertyDto>
            (
                200,
                "Property retrieved successfully.",
                map
            );
        }

        public async Task<ApiResponse<string>> AddPropertyAsync(PropertyDto dto, Guid userId)
        {
            var cat = await _catrepo.GetByIdAsync(dto.CategoryId);
            if (cat == null)
            {
                return new ApiResponse<string>(404, $"CategoryId '{dto.CategoryId}' not listed in the category list.");
            }

            var loc = await _locRepo.GetByIdAsync(dto.LocationId);
            if (loc == null)
            {
                return new ApiResponse<string>(406, $"Location with '{dto.LocationId}' not Acceptable.");
            }

            var propertyEntity = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                Status = dto.Status,
                Price = dto.Price,
                CreatedDate = DateTime.UtcNow,
                CategoryId = dto.CategoryId,
                LocationId = dto.LocationId,
                UserId = userId,
            };

            await _proRepo.AddAsync(propertyEntity);

            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification",
                    "Property Added",
                    $"Your property '{dto.Title}' has been added successfully to Property Hub. Our team will verify and list it soon.");

            return new ApiResponse<string>(200, "Property added successfully");
        }

        public async Task<ApiResponse<string>> UpdatePropertyAsync(PropertyDto propertydto, Guid id)
        {
            var Property = await _proRepo.GetByIdAsync(id);
            if(Property==null) return new ApiResponse<string> (404, "Property not found... Please check the Id");

            var map = _mapper.Map(propertydto, Property);

            await _proRepo.UpdateAsync(map);

            return new ApiResponse<string> (200, "Property Updated Successfully");
        }
        public async Task<ApiResponse<string>> DeletePropertyAsync(Guid id)
        {
            var pro = await _proRepo.GetByIdAsync(id);
            if (pro == null)
                return new ApiResponse<string> (404, "Property not found.");
            await _proRepo.DeleteAsync(id);
            return new ApiResponse<string> ( 200,$"Property has been deleted successfully." );
        }

        public async Task<ApiResponse<List<PropertyDto>>> FilterProperty(PropertyQueryDto query)
        {
            var properties = await _filterRepo.PropertyFilter(query);
            var map = _mapper.Map<List<PropertyDto>>(properties);
            return new ApiResponse<List<PropertyDto>>(200, "Filtered Successful", map);
        }
    }
}
