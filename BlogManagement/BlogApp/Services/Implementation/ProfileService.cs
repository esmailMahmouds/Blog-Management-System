using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Azure.Core;
using BlogApp.Contract;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;

        public ProfileService(IUnitOfWork unitOfWork, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            
        }

        public async Task<Result<int>> EditUserInfoAsync(UserInfoDto userInfoDto, int userId)
        {
            var currentUser = await GetCurrentUser(userId);
            var result = new Result<int>(); 
            if (currentUser != null)
            {
                currentUser.Name = userInfoDto.Name;
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

                if (await _unitOfWork.UserRepository.GetUserByEmail(userInfoDto.Email) != null)
                {
                    _logger.LogWarning("Edit Email failed: Email already exists");
                    result = Result<int>.Fail("Email already exists.");
                }
                else
                {
                    currentUser.Email = userInfoDto.Email;
                }

                _unitOfWork.UserRepository.UpdateUser(currentUser);
                await _unitOfWork.Save();
            }
            else
            {
                result = Result<int>.Fail("User not Found");
            }

            return result;
        }

		public async Task<Result<int>> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
		{
            var currentUser = await GetCurrentUser(userId);

            if(currentUser != null)
            {
				var passwordHasher = new PasswordHasher<User>();
				var passwordVerificationResult = passwordHasher.VerifyHashedPassword(currentUser, currentUser.Password, changePasswordDto.OldPassword);

				if (passwordVerificationResult != PasswordVerificationResult.Success)
				{
					_logger.LogWarning("Old Password is Wrong");
                    return Result<int>.Fail("Old Password is Wrong");
				}
                else
                {
                    currentUser.Password = new PasswordHasher<User>().HashPassword(currentUser, changePasswordDto.NewPassword);
					_unitOfWork.UserRepository.UpdateUser(currentUser);
                    await _unitOfWork.Save();
                    return Result<int>.Ok(userId); ;
                }
			}
            else
            {
                _logger.LogWarning($"No user with id#{userId} exists");
                return Result<int>.Fail($"No user with id#{userId} exists");
            }
		}

		public async Task<User?> GetCurrentUser(int userId)
        {
            return await _unitOfWork.UserRepository.GetUserById(userId);
        }

        public async Task<List<Country>> GetCountries()
        {
            return (List<Country>) await _unitOfWork.CountryRepository.GetCountries();
        }
	}
}
