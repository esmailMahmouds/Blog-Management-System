using BlogApp.Models.DomainClasses;
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

        public async Task<(IEnumerable<Post> Posts, int TotalCount)> GetAllPosts(int page, int pageSize)
        {
            return await _unitOfWork.PostRepository.GetAllPosts(page, pageSize);
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
        public async Task<bool> EditComment(int commentId, string newContent, int userId, bool isAdmin)
        {
            var comment = await _unitOfWork.PostRepository.GetCommentById(commentId);
            if (comment == null) return false;

            // Rule: User can edit their own, Admins can edit only their own
            if (comment.UserId != userId && comment.Post.UserId != userId) return false;

            comment.Content = newContent;
            comment.CreateDate = DateTime.UtcNow;

            await _unitOfWork.PostRepository.UpdateComment(comment);
            await _unitOfWork.Save();
            return true;
        }
        public async Task<bool> DeleteComment(int commentId, int userId, bool isAdmin)
        {
            var comment = await _unitOfWork.PostRepository.GetCommentById(commentId);
            if (comment == null) return false;

            // Rule: User deletes own, Admin can delete any
            if (comment.UserId != userId && !isAdmin) return false;

            await _unitOfWork.PostRepository.DeleteComment(comment);
            await _unitOfWork.Save();
            return true;
        }

    }
}
