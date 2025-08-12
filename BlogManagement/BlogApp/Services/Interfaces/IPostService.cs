using BlogApp.Models;

namespace BlogApp.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post?> GetPostById(int id);
        Task<bool> LikePost(int postId, int userId);
        Task<bool> RatePost(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);
    }
}
