
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOGAM.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BOOLOG.Application.Services
{
    public class Auth_Service : IAuth_Service
    {
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public Auth_Service(IRepository<UserEntity> irepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepo = irepository;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<ResponseDto> Registeration(RegisterDto registerDto)
        {
            try
            {
                var userExist = (await _userRepo.GetAllAsync()).FirstOrDefault(x => x.Contact == registerDto.Contact);
                if (userExist != null) return new ResponseDto { Message = "User is Already Exists" };

                var map = _mapper.Map<UserEntity>(registerDto);

                var passwordHasher = new PasswordHasher<UserEntity>();
                map.Password = passwordHasher.HashPassword(map, registerDto.Password);

                await _userRepo.AddAsync(map);
                return new ResponseDto { Message = "You are registration successfully" };
            }
            catch (Exception ex)
            {
                throw new Exception("Registration failed: " + ex.Message, ex);
            }
        }
        public async Task<ResponseDto> Login(LoginDto loginDto)
        {
            var response = new ResponseDto();

            try
            {
                var user = (await _userRepo.GetAllAsync())
                            .FirstOrDefault(u => u.Contact == loginDto.Contact);

                var passwordHasher = new PasswordHasher<UserEntity>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    response.Message = "Password is incorrect";
                    return response;
                }

                if (!string.IsNullOrEmpty(user.BlockedUser))
                {
                    response.Message = "You are blocked";
                    return response;
                }

                response.Message = "Login successful. Welcome to BOOLOG!";
                response.Token = GenerateJwtToken(user);
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Something went wrong. Please try again later.";
                return response;
            }
        }


        private string GenerateJwtToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
           
                new Claim("UserName", user.UserName),
                new Claim("Role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
