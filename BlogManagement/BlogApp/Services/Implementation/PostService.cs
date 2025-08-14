using BlogApp.Enums;
using BlogApp.Models.DomainClasses;
using BlogApp.Models.Dtos;
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

        public async Task CreatePost(CreatePostDto createPostDto, int userId)
        {
            var post = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                CategoryId = createPostDto.CategoryId,
                UserId = userId,
                Status = PostStatus.Pending,
                LikeCount = 0,
                AverageRate = 0,
                RateCount = 0,
                CreateDate = DateTime.Now
            };

            await _unitOfWork.PostRepository.CreatePostAsync(post);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.CategoryRepository.GetAllCategories();
        }


        public async Task<bool> UpdatePost(EditPostDto editPostDto, int userId)
        {
            // Get the existing post to verify ownership
            var existingPost = await _unitOfWork.PostRepository.GetPostById(editPostDto.Id);
            if (existingPost == null)
                return false;

            // Check if the user is the owner of the post
            if (existingPost.UserId != userId)
                return false;

            var post = new Post
            {
                Id = editPostDto.Id,
                Title = editPostDto.Title,
                Content = editPostDto.Content,
                CategoryId = editPostDto.CategoryId
            };

            var updated = await _unitOfWork.PostRepository.UpdatePostAsync(post);
            if (!updated)
                return false;

            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> DeletePost(int postId, int userId)
        {
            var existingPost = await _unitOfWork.PostRepository.GetPostById(postId);
            if (existingPost == null)
                return false;

            if (existingPost.UserId != userId)
                return false;

            var deleted = await _unitOfWork.PostRepository.DeletePostAsync(postId);
            if (!deleted)
                return false;

            await _unitOfWork.Save();
            return true;
        }

        //admin specific methods
        public async Task<IEnumerable<Post>> GetAllPostsForAdmin()
        {
            return await _unitOfWork.PostRepository.GetAllPostsIncludingPending();
        }

        public async Task<IEnumerable<Post>> GetPendingPosts()
        {
            return await _unitOfWork.PostRepository.GetPendingPosts();
        }

        public async Task<bool> ApprovePost(int postId)
        {
            var approved = await _unitOfWork.PostRepository.ApprovePost(postId);
            if (!approved)
                return false;

            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> RejectPost(int postId)
        {
            var rejected = await _unitOfWork.PostRepository.RejectPost(postId);
            if (!rejected)
                return false;

            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> AdminDeletePost(int postId)
        {
            var deleted = await _unitOfWork.PostRepository.AdminDeletePost(postId);
            if (!deleted)
                return false;

            await _unitOfWork.Save();
            return true;
        }
        public async Task<int> GetPendingPostsCount()
        {
            return await _unitOfWork.PostRepository.GetPendingPostsCount();
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _unitOfWork.PostRepository.GetTotalPostsCount();
        }

        public async Task<int> GetApprovedPostsCount()
        {
            return await _unitOfWork.PostRepository.GetApprovedPostsCount();
        }
    }
}
