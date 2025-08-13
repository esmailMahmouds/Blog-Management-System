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
            //verify ownership
            var existingPost = await _unitOfWork.PostRepository.GetPostById(editPostDto.Id);
            if (existingPost == null)
                return false;

            //check that user is the owner of post
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
    }
}
