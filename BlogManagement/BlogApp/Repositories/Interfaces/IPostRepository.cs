using BlogApp.Models;

namespace BlogApp.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
    }
}
