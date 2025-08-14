using BlogApp.Models.DomainClasses;

namespace BlogApp.Services.Interfaces
{
    public interface IPostService
    {
        Task<(IEnumerable<Post> Posts, int TotalCount)> GetAllPosts(int page, int pageSize);
        Task<Post?> GetPostById(int id);
        Task<bool> LikePost(int postId, int userId);
        Task<bool> RatePost(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);
        Task<bool> EditComment(int commentId, string newContent, int userId, bool isAdmin);
        Task<bool> DeleteComment(int commentId, int userId, bool isAdmin);
    }
}
