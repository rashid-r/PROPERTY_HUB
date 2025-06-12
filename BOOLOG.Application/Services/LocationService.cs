using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locRepo;
        private readonly IMapper _mapper;

        public LocationService(IRepository<Location> locRepo, IMapper mapper)
        {
            _locRepo = locRepo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<LocationDto>>> GetAllLocationAsync()
        {
            var all = await _locRepo.GetAllAsync();
            var map = _mapper.Map<List<LocationDto>>(all);

            return new ApiResponse<List<LocationDto>>
            (
                200,
                "Locations retrieved successfully.",
                map
            );
        }

        public async Task<ApiResponse<LocationDto>> GetLocationById(Guid id)
        {
            var get = await _locRepo.GetByIdAsync(id);
            if(get==null) return new ApiResponse<LocationDto> (404, $"Location with ID {id} not found." );
            var map = _mapper.Map<LocationDto>(get);    
            return new ApiResponse<LocationDto>
            (
                200,
                "Location retrieved successfully.",
                map
            );
        }
        public async Task<ApiResponse<string>> AddLocationAsync(string name)
        {
            var add = (await _locRepo.GetAllAsync()).FirstOrDefault(x => x.LocationName.ToLower().Trim() == name.ToLower().Trim() );
            if (add != null)
                return new ApiResponse<string>(406,$"Not acceptable...This location {name} is already added");

            var map = new Location
            {
                LocationName = name.ToLower()
            };
            await _locRepo.AddAsync(map);
            return new ApiResponse<string>
            (200,"Location Added Successfully");
        }
        public async Task<ApiResponse<string>> UpdateLocationAsync(LocationDto dto)
        {
            var update = await _locRepo.GetByIdAsync(dto.Id); 

            if (update == null)
                return new ApiResponse<string> (404, $"Location with ID {dto.Id} not found.");
                

            //update.LocationId = dto.LocationId;
            update.LocationName = dto.LocationName.ToLower();

            await _locRepo.UpdateAsync(update);

            return new ApiResponse<string>
            (
                200,
                "Location Updated Successfully"
            );
        }
        public async Task<ApiResponse<string>> DeleteLocationAsync(Guid id)
        {
            var category = await _locRepo.GetByIdAsync(id);
            if (category == null)
                return new ApiResponse<string>(404, "Location not found.");
            await _locRepo.DeleteAsync(id);
            return new ApiResponse<string>
            (
                200,
                "Location Deleted Successfully"
            );
        }

    }
}
 