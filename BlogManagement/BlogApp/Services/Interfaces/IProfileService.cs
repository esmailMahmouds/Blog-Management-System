using BlogApp.Contract;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.Interfaces
{
    public interface IProfileService
    {
        public  Task<bool> EditUserInfoAsync(UserInfoDto userInfoDto, User currentUser);
    }
}
