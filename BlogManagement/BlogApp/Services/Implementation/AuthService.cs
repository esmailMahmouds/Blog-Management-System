using BlogApp.Contract;
using BlogApp.Extensions;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;
using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<Result<TokenResponseDto?>> UserSignInAsync(UserSignInDto request)
        {
            _logger.LogInformation("User sign-in attempt for {Email}", request.Email);

            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                _logger.LogWarning("Sign-in failed: User not found");
                return Result<TokenResponseDto?>.Fail("Invalid email or password.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Sign-in failed: Invalid password");
                return Result<TokenResponseDto?>.Fail("Invalid email or password.");
            }

            var token = new TokenResponseDto
            {
                AccessToken = _jwtService.GenerateToken(user),
                RefreshToken = "" //implement refresh token if needed        
            };

            _logger.LogInformation("User signed in successfully");
            return Result<TokenResponseDto?>.Ok(token);
        }

        public async Task<Result<UserSignUpResponseDto?>> UserSignUpAsync(UserSignUpDto request)
        {
            _logger.LogInformation("User sign-up attempt for {Email}", request.Email);

            if (await _unitOfWork.UserRepository.GetUserByEmail(request.Email) != null)
            {
                _logger.LogWarning("Sign-up failed: Email already exists");
                return Result<UserSignUpResponseDto?>.Fail("Email already exists.");
            }

            var user = request.ToUser();
            user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);

            await _unitOfWork.UserRepository.AddUser(user);
            await _unitOfWork.Save();

            _logger.LogInformation("User registered successfully");
            return Result<UserSignUpResponseDto?>.Ok(user.ToUserSignUpResponseDto());
        }
    }
}