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
        public async Task<Post?> GetPostById(int id)
        {
            return await _unitOfWork.PostRepository.GetPostById(id);
        }
        public async Task<bool> LikePost(int postId, int userId)
        {
            var liked = await _unitOfWork.PostRepository
                .AddLike(postId, userId);

            if (!liked)
                return false;

            await _unitOfWork.Save();
            return true;
        }
        public async Task<bool> RatePost(int postId, int userId, double rating)
        {
            var rate = await _unitOfWork.PostRepository.AddRating(postId, userId, rating);
            if (!rate) return false;

            await _unitOfWork.Save();
            return true;
        }
        public async Task<bool> AddComment(int postId, int userId, string content)
        {
            var comment = await _unitOfWork.PostRepository.AddComment(postId, userId, content);
            if (!comment) return false;

            await _unitOfWork.Save();
            return true;
        }

    }
}
