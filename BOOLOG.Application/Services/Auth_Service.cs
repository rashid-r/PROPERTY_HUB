
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BOOLOG.Application.Dto.AuthDto;
using BOOLOG.Application.Dto.GetAllDto;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOG.Infrastructure.SignalR;
using BOOLOGAM.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BOOLOG.Application.Services
{
    public class Auth_Service : IAuth_Service
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<UserProfile> _userPro;
        private readonly IUserprofileRepository _userprofileRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public Auth_Service(IRepository<User> irepository, IMapper mapper, IConfiguration configuration, IRepository<UserProfile> userPro, IUserprofileRepository userprofileRepository, IHubContext<NotificationHub> hubContext)
        {
            _userRepo = irepository;
            _mapper = mapper;
            _config = configuration;
            _userPro = userPro;
            _userprofileRepository = userprofileRepository;
            _hubContext = hubContext;
        }

        public async Task<AuthResponse> Registeration(RegisterDto registerDto)
        {
            try
            {
                var userExist = (await _userRepo.GetAllAsync()).FirstOrDefault(x => x.Contact == registerDto.Contact);
                if (userExist != null) return new AuthResponse (406, "Not Acceptable!!...User is Already Exists");

                var map = _mapper.Map<User>(registerDto);

                var passwordHasher = new PasswordHasher<User>();
                map.Password = passwordHasher.HashPassword(map, registerDto.Password);

                await _userRepo.AddAsync(map);
                return new AuthResponse (200, "Thanks for signing up! You're officially a BOOLOG member now.");
            }
            catch (Exception ex)
            {
                return new AuthResponse (400, "Registration failed: " + ex.Message );
            }
        }

        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            try
            {
                var user = (await _userRepo.GetAllAsync())
                            .FirstOrDefault(u => u.Contact == loginDto.Contact);

                if (user == null)
                    return new AuthResponse(404, "User Not Found");

                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

                if (result == PasswordVerificationResult.Failed)
                    return new AuthResponse(400, "Password is incorrect");

                if (user.IsBlocked == true)
                    return new AuthResponse(403, "Your account is blocked. Please contact support.");

                var token = GenerateJwtToken(user);

                await _hubContext.Clients.User(user.Id.ToString())
                    .SendAsync("ReceiveNotification",
                        "Welcome to BOOLOG!",
                        $"Hello {user.UserName}, welcome to Property Hub! " +
                        "Here you can explore, buy, and sell properties with ease. " +
                        "Don't forget to complete your profile and KYC to unlock full access.");

                return new AuthResponse
                (
                    statusCode: 200,
                    userName: user.UserName,
                    role: user.Role,
                    message: "Login successful. Welcome to BOOLOG!",
                    token: token
                );
            }
            catch (Exception ex)
            {
                return new AuthResponse(500, "Something went wrong. Please try again later.");
            }
        }


        public async Task<ApiResponse<List<GetAllUserDto>>> GetAllUsers()
        {
            var get = await _userRepo.GetAllAsync();
            var mapped = _mapper.Map<List<GetAllUserDto>>(get);
            return new ApiResponse<List<GetAllUserDto>>(200,"Users Retrived Successfully",mapped);
        }

       
        public async Task<ApiResponse<string>> ApproveKyc(bool IsApproved, Guid UserId)
        {
            var user = await _userRepo.GetByIdAsync(UserId);
            if (user == null)
                return new ApiResponse<string>(404, "User Does Not Exist");

            var userProfile = await _userPro.GetByIdAsync(UserId);
            if (userProfile == null)
                return new ApiResponse<string>(404, "UserProfile Does Not Exist");

            await _userprofileRepository.ApproveKyc(IsApproved, userProfile.Id, UserId);

            string title, message;
            if (IsApproved)
            {
                title = "KYC Approved";
                message = $"Hi {user.UserName}, your KYC has been successfully approved. You can now access full seller features in Property Hub.";
            }
            else
            {
                title = "KYC Rejected";
                message = $"Hi {user.UserName}, unfortunately your KYC has been rejected. Please review your submitted documents and try again.";
            }

            await _hubContext.Clients.User(user.Id.ToString())
                .SendAsync("ReceiveNotification", title, message);

            return new ApiResponse<string>(200, IsApproved ? "Approved KYC" : "Rejected KYC");
        }

        public async Task<ApiResponse<string>> BlockUnblockUserAsync(Guid UserId)
        {
            var user = await _userRepo.GetByIdAsync(UserId);
            if (user == null) return new ApiResponse<string>(404, "User Does Not Exist");
            
            var result = await _userprofileRepository.BlockUnblockUser(UserId);
            if (result)
            {
                return new ApiResponse<string>(200, "Blocked User Successfully");
            }
            else
            {
                return new ApiResponse<string>(200, "UnBlocked User Successfully");
            }
            
        }

        



        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("Role", nameof(user.Role))
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
