using BlogApp.Models;
using BlogApp.Services.Interfaces;
using BlogApp.UnitOfWork.Interfaces;

namespace BlogApp.Services.Implementation
{
    public class PostService : IPostService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _unitOfWork.PostRepository.GetAllPosts();
        }
    }
}
