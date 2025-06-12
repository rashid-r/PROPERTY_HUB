using System.Text.Json;
using AutoMapper;
using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.AutoMapper
{
    public class Auto_Mapper : Profile
    {
        public Auto_Mapper() 
        {
            CreateMap<User, GetAllUserDto>()
                .ForMember(dest => dest.Role,
                       opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<User, RegisterDto>().ReverseMap();

            CreateMap<Property, PropertyDto>();
            CreateMap<Property, GetAllPropertyDto>()
            .ForMember(dest => dest.Type,
                       opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status,
                       opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Location, LocationDto>().ReverseMap();

            CreateMap<UserProfile, GetallUserProfileDto>();
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();

            CreateMap<AddFeedbackDto, Feedback>().ReverseMap();
            CreateMap<UpdateFeedbackDto, Feedback>();
            CreateMap<Feedback, GetAllFeedbackDto>().ReverseMap();

            CreateMap<WishlistDto, WishList>().ReverseMap();
            CreateMap<GetAllRazorpayDto, RazorPay>().ReverseMap();

        }
    }
}
