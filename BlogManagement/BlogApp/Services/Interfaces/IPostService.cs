using BlogApp.Models;

namespace BlogApp.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
    }
}
