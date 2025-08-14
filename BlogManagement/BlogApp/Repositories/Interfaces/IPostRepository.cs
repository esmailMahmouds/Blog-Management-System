using BlogApp.Models.DomainClasses;

namespace BlogApp.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<(IEnumerable<Post>, int)> GetAllPosts(int page, int pageSize);
        Task<Post?> GetPostById(int id);
        Task<bool> AddLike(int postId, int userId);
        Task<bool> AddRating(int postId, int userId, double rating);
        Task<bool> AddComment(int postId, int userId, string content);
        Task<Post> CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int postId);
        Task<IEnumerable<Post>> GetPostsByUserId(int userId);

        //admin specific methods
        Task<IEnumerable<Post>> GetAllPostsIncludingPending();
        Task<IEnumerable<Post>> GetPendingPosts();
        Task<bool> ApprovePost(int postId);
        Task<bool> RejectPost(int postId);
        Task<bool> AdminDeletePost(int postId);


        Task<int> GetPendingPostsCount();
        Task<int> GetTotalPostsCount();
        Task<int> GetApprovedPostsCount();

        Task<Comment?> GetCommentById(int id);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Comment comment);
    }
}
