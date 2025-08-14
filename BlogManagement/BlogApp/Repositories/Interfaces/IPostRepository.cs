using BlogApp.Models.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<(IEnumerable<Post>, int)> GetAllPosts(int page, int pageSize);
        Task<Post?> GetPostById(int id);
        Task<bool> AddLike(int postId, int userId);
        Task<bool> AddRating(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);
        Task<Comment> GetCommentById(int id);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Comment comment);


    }
}
