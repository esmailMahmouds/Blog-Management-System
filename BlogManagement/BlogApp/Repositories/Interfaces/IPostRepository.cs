using BlogApp.Models;

namespace BlogApp.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post?> GetPostById(int id);
        Task<bool> AddLike(int postId, int userId);
        Task<bool> AddRating(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);

    }
}
