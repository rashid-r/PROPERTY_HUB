using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;
using Microsoft.AspNetCore.Http;

namespace BOOLOG.Application.Services
{
    public class UserProfileServices : IUserProfileService
    {
        private readonly IRepository<UserProfile> _UserProRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public UserProfileServices(IRepository<UserProfile> UserProRepo, IMapper mapper, IRepository<User> userRepo)
        {
            _mapper = mapper;
            _UserProRepo = UserProRepo;
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<List<GetallUserProfileDto>>> GetAllUserProfile()
        {
            var all = await _UserProRepo.GetAllAsync();
            var map = _mapper.Map<List<GetallUserProfileDto>>(all);
            return new ApiResponse<List<GetallUserProfileDto>>
            (
                200,
                "UserProfile retrieved successfully.",
                map
            );
        }
        public async Task<ApiResponse<GetallUserProfileDto>> GetUserProfileById(Guid UserId) 
        {
            var getId = await _UserProRepo.GetByIdAsync(UserId);
            if(getId==null)
            return new ApiResponse <GetallUserProfileDto> (404,$"Pls Check Your User Id {UserId}");
            var map = _mapper.Map<GetallUserProfileDto>(getId);
            return new ApiResponse<GetallUserProfileDto>
            (
                200,
                "UserProfile retrieved successfully.",
                map
            );

        }
        public async Task<ApiResponse<string>> AddUserProfile(UserProfileDto dto, Guid UserId)
        {
            var add = await _UserProRepo.GetByIdAsync(UserId); 
            if(add!=null) return new ApiResponse<string>(406,"Not Acceptable....UserProfile Is Already listed");

            var buyer = await _userRepo.GetByIdAsync(UserId);
            if (buyer != null && buyer.Role != Roles.Buyer) return new ApiResponse<string>(401, "Only Buyer can create UserProfile");

            var UserPro = new UserProfile
            {
                UserId = UserId,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                AadhaarIdNumber = dto.AadhaarIdNumber,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode
            };

            await _UserProRepo.AddAsync(UserPro);
            return new ApiResponse<string>
            (
                200,
                "UserProfile Created Successfully."
            );
        }
        public async Task<ApiResponse<string>> UpdateUserProfile(UserProfileDto dto,Guid UserId)
        {
            var update = await _UserProRepo.GetByIdAsync(UserId); 
                if(update==null)return new ApiResponse<string> (406,$"Property not found {UserId} Please check the Id");
            
            update.DateOfBirth = dto.DateOfBirth;
            update.Gender = dto.Gender;
            update.AadhaarIdNumber = dto.AadhaarIdNumber;
            update.Address = dto.Address;
            update.City = dto.City;
            update.State = dto.State;
            update.Country = dto.Country;
            update.PostalCode = dto.PostalCode;

            await _UserProRepo.UpdateAsync(update);
            return new ApiResponse<string> (200,"UserProfile Updated Successfully.");
        } 
        public async Task<ApiResponse<string>> DeleteUserProfile(Guid UserId)
        {
            var category = await _UserProRepo.GetByIdAsync(UserId);
            if (category == null)
                return new ApiResponse<string>(404, "User Profile not found.");
            await _UserProRepo.DeleteAsync(UserId);
            return new ApiResponse<string>
            (
                200,
                "UserProfile Deleted Successfully"
            );
        }
        
    }
}
