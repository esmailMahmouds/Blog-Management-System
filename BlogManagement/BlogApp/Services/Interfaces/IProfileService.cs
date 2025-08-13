using BlogApp.Contract;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<bool> EditUserInfoAsync(UserInfoDto userInfoDto, int userId);
        public Task<User?> GetCurrentUser(int userId);
        public Task<List<Country>> GetCountries();
    }
}
