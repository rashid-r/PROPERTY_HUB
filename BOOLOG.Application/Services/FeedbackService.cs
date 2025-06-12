using AutoMapper;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Services
{
    public class FeedbackService : IFeedbackServices
    {
        private readonly IRepository<Feedback> _feedrepo;
        private readonly IRepository<Property> _proRepo;
        private readonly IMapper _mapper;

        public FeedbackService(IRepository<Feedback> profeedrepo, IMapper mapper, IRepository<Property> proRepo)
        {
            _mapper = mapper;
            _feedrepo = profeedrepo;
            _proRepo = proRepo;
        }

        public async Task<ApiResponse<List<GetAllFeedbackDto>>> GetAllFeedbacks()
        {
            var result = await _feedrepo.GetAllAsync();
            var map = _mapper.Map<List<GetAllFeedbackDto>>(result);
            return new ApiResponse<List<GetAllFeedbackDto>>(200, "All Property Feedbacks", map);
        }
        public async Task<ApiResponse<GetAllFeedbackDto>> GetFeedbackById(Guid id)
        {

            var result = await _feedrepo.GetByIdAsync(id);
            if (result == null)
                return new ApiResponse<GetAllFeedbackDto>(404, $"Feedback with ID {id} not found.");

            var map = _mapper.Map<GetAllFeedbackDto>(result);
            return new ApiResponse<GetAllFeedbackDto>(200, "Feedback retreived successfully", map);
        }
        public async Task<ApiResponse<string>> AddFeedbacks(AddFeedbackDto dto,Guid UserId)
        {
            if (dto == null || dto.Rating < 1 || dto.Rating > 5 || string.IsNullOrWhiteSpace(dto.Comment))
            {
                return new ApiResponse<string>(400, "Invalid feedback data. Rating must be between 1 and 5, and comment cannot be empty.");
            }
            var result = await _proRepo.GetByIdAsync(dto.PropertyId);
            if (result == null) return new ApiResponse<string>
                    (404, $"Feedback with PropertyId {dto.PropertyId} not found.");
            var fb = _mapper.Map<Feedback>(dto);
            fb.CreatedAt = DateTime.UtcNow;
            fb.UserId = UserId;
            await _feedrepo.AddAsync(fb);
            return new ApiResponse<string>(200, "Property Feedbacks Added Successfully");
        }
        public async Task<ApiResponse<string>> UpdateFeedbacks(UpdateFeedbackDto dto,Guid fbId, Guid UserId)
        {
            if (dto == null || dto.Rating < 1 || dto.Rating > 5 || string.IsNullOrWhiteSpace(dto.Comment))
            {
                return new ApiResponse<string>(400, "Invalid feedback data. Rating must be between 1 and 5, and comment cannot be empty.");
            }
            var oldfb = await _feedrepo.GetByIdAsync(fbId);
            if (oldfb == null)
            {
                return new ApiResponse<string>(404, $"Feedback with Id {fbId} not found.");
            }

            var newfb = _mapper.Map(dto, oldfb);
            await _feedrepo.UpdateAsync(newfb);
            return new ApiResponse<string>(200, "Property Feedbacks Updated Successfully");
        }
        public async Task<ApiResponse<string>> DeleteFeedbacks(Guid id)
        {
            var pro = await _feedrepo.GetByIdAsync(id);
            if (pro == null)
                return new ApiResponse<string>(404, "Feedback not found.");
            await _feedrepo.DeleteAsync(id);
            return new ApiResponse<string>(200, $"Property has been deleted successfully.");
        }
    }

}