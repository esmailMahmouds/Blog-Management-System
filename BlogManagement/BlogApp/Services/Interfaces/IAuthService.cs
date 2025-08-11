using BlogApp.Contract;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserSignUpResponseDto?>> UserSignUpAsync(UserSignUpDto request);
        Task<Result<TokenResponseDto?>> UserSignInAsync(UserSignInDto request);
    }
}
