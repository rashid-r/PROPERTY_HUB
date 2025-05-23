using AutoMapper;
using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.AutoMapper
{
    public class Auto_Mapper : Profile
    {
        public Auto_Mapper() 
        {
            CreateMap<UserEntity, RegisterDto>().ReverseMap();
            CreateMap<PropertyEntity, Propertydto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
                
                
        }
    }
}
