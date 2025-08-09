using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;

namespace BlogApp.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        //implement authentication methods, such as Register, Login, ..etc.
    }
}