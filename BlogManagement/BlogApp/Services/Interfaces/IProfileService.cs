using BlogApp.Contract;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<Result<int>> EditUserInfoAsync(UserInfoDto userInfoDto, int userId);
        public Task<Result<int>> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId);
        public Task<User?> GetCurrentUser(int userId);
        public Task<List<Country>> GetCountries();
    }
}
