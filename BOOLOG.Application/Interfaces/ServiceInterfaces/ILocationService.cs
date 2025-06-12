using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface ILocationService
    {
        Task<ApiResponse<LocationDto>> GetLocationById(Guid id);
        Task<ApiResponse<string>> AddLocationAsync(string name);
        Task<ApiResponse<List<LocationDto>>> GetAllLocationAsync();
        Task<ApiResponse<string>> UpdateLocationAsync(LocationDto dto);
        Task<ApiResponse<string>> DeleteLocationAsync(Guid id);
    }
}
