using BlogApp.Models.DomainClasses;
using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;

namespace BlogApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _unitOfWork.UserRepository.GetUserById(id);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmail(email);
        }

        // Admin specific methods
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _unitOfWork.UserRepository.GetAllUsers();
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var deleted = await _unitOfWork.UserRepository.DeleteUser(userId);
            if (!deleted)
                return false;

            await _unitOfWork.Save();
            return true;
        }
    }
}