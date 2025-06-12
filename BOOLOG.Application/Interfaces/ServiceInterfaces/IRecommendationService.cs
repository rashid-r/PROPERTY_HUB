using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.PropertyDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IRecommendationService
    {
        Task<ApiResponse<string>> TrainModelAsync();

        Task<ApiResponse<List<PropertyDto>>> GetRecommendedPropertiesForUserAsync(Guid userId, int numberOfRecommendations = 5);

        bool IsModelTrained();
    }
}
