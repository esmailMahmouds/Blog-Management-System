using BlogApp.Models.DomainClasses;

namespace BlogApp.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post?> GetPostById(int id);
        Task<bool> LikePost(int postId, int userId);
        Task<bool> RatePost(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);
        Task CreatePost(CreatePostDto createPostDto, int userId);
        Task<bool> UpdatePost(EditPostDto editPostDto, int userId);
        Task<bool> DeletePost(int postId, int userId);
        Task<IEnumerable<Category>> GetAllCategories();

        //admin specific methods
        Task<IEnumerable<Post>> GetAllPostsForAdmin();
        Task<IEnumerable<Post>> GetPendingPosts();
        Task<bool> ApprovePost(int postId);
        Task<bool> RejectPost(int postId);
        Task<bool> AdminDeletePost(int postId);
    }
}
