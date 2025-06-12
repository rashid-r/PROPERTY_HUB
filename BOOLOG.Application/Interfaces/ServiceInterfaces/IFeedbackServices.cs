using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface IFeedbackServices
    {
        Task<ApiResponse<List<GetAllFeedbackDto>>> GetAllFeedbacks();
        Task<ApiResponse<GetAllFeedbackDto>> GetFeedbackById(Guid id);
        Task<ApiResponse<string>> AddFeedbacks(AddFeedbackDto dto, Guid UserId);
        Task<ApiResponse<string>> UpdateFeedbacks(UpdateFeedbackDto dto,Guid fbId, Guid UserId);
        Task<ApiResponse<string>> DeleteFeedbacks(Guid id);
    }
}
