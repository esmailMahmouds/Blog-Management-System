using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public ProfileService(IUnitOfWork unitOfWork, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
            
        }

        public async Task<bool> EditUserInfoAsync(UserInfoDto userInfoDto,User currentUser)
        {
            // var currentUser = await _unitOfWork.UserRepository.GetUserById(userId);
            if (currentUser != null)
            {
                currentUser.Name = userInfoDto.Name;
                currentUser.Email = userInfoDto.Email;
                currentUser.DateOfBirth = userInfoDto.DateOfBirth;
                currentUser.CountryId = userInfoDto.CountryId;

                if (userInfoDto.ProfileImage != null && userInfoDto.ProfileImage.Length > 0)
                {
                    // Read the IFormFile stream into a byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        await userInfoDto.ProfileImage.CopyToAsync(memoryStream);
                        currentUser.ProfileImage = memoryStream.ToArray();
                    }
                }

                await _unitOfWork.Save();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
