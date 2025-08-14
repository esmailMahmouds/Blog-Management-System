using BlogApp.Context;
using BlogApp.Enums;
using BlogApp.Models.DomainClasses;
using BlogApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Post>, int)> GetAllPosts(int page, int pageSize)
        {
            if (page < 1) page = 1;

            var totalCount = await _context.Posts
                .Where(p => p.Status == PostStatus.Approved)
                .CountAsync();

            var posts = await _context.Posts
                .Where(p => p.Status == PostStatus.Approved)
                .Include(p => p.User)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreateDate = p.CreateDate,
                    LikeCount = p.LikeCount,
                    AverageRate = p.AverageRate,
                    RateCount = p.RateCount,
                    Status = p.Status,
                    UserId = p.UserId,
                    CategoryId = p.CategoryId,
                    User = new User { Id = p.User.Id, Name = p.User.Name, ImageURL = p.User.ImageURL, ProfileImage = p.User.ProfileImage },
                    Category = new Category { Id = p.Category.Id, Name = p.Category.Name }
                })
                .ToListAsync();

            return (posts, totalCount);
        }

        public async Task<Post?> GetPostById(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Ratings)
                .Include(p => p.Likes)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> AddLike(int postId, int userId)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            if (existingLike != null)
            {
                // Unlike
                post.LikeCount = Math.Max(0, post.LikeCount - 1);
                _context.Likes.Remove(existingLike);
            }
            else
            {
                // Like
                post.LikeCount++;
                _context.Likes.Add(new Like { PostId = postId, UserId = userId });
            }

            return true;
        }
        public async Task<bool> AddRating(int postId, int userId, double rating)
        {
            var post = await _context.Posts
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return false;

            var existingRating = post.Ratings.FirstOrDefault(r => r.UserId == userId);

            if (existingRating != null)
            {
                // Update rating
                existingRating.Value = rating;
            }
            else
            {
                // Add new rating
                var newRating = new Rating { PostId = postId, UserId = userId, Value = rating };
                _context.Ratings.Add(newRating);
                post.RateCount++;
                post.Ratings = post.Ratings.Append(newRating).ToList();
            }

            // Recalculate average
            post.AverageRate = post.Ratings.Average(r => r.Value);

            return true;
        }
        public async Task<bool> AddComment(int postId, int userId, string content)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            _context.Comments.Add(new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content
            });

            return true;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            return post;
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            var existingPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existingPost == null)
                return false;

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.CategoryId = post.CategoryId;
            existingPost.Status = PostStatus.Pending;

            _context.Posts.Update(existingPost);
            return true;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return false;

            if (post.Comments?.Any() == true)
            {
                _context.Comments.RemoveRange(post.Comments);
            }

            if (post.Likes?.Any() == true)
            {
                _context.Likes.RemoveRange(post.Likes);
            }

            if (post.Ratings?.Any() == true)
            {
                _context.Ratings.RemoveRange(post.Ratings);
            }

            _context.Posts.Remove(post);
            return true;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserId(int userId)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 150 ? p.Content.Substring(0, 150) + "..." : p.Content,
                    CreateDate = p.CreateDate,
                    LikeCount = p.LikeCount,
                    AverageRate = p.AverageRate,
                    RateCount = p.RateCount,
                    Status = p.Status,
                    UserId = p.UserId,
                    CategoryId = p.CategoryId,
                    Category = new Category { Id = p.Category.Id, Name = p.Category.Name }
                })
                .ToListAsync();
        }

        //admin specific methods - optimized for performance
        public async Task<IEnumerable<Post>> GetAllPostsIncludingPending()
        {
            return await _context.Posts
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 150 ? p.Content.Substring(0, 150) + "..." : p.Content,
                    CreateDate = p.CreateDate,
                    LikeCount = p.LikeCount,
                    AverageRate = p.AverageRate,
                    RateCount = p.RateCount,
                    Status = p.Status,
                    UserId = p.UserId,
                    CategoryId = p.CategoryId,
                    User = new User { Id = p.User.Id, Name = p.User.Name },
                    Category = new Category { Id = p.Category.Id, Name = p.Category.Name }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPendingPosts()
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.Status == PostStatus.Pending)
                .Include(p => p.User)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 150 ? p.Content.Substring(0, 150) + "..." : p.Content,
                    CreateDate = p.CreateDate,
                    LikeCount = p.LikeCount,
                    AverageRate = p.AverageRate,
                    RateCount = p.RateCount,
                    Status = p.Status,
                    UserId = p.UserId,
                    CategoryId = p.CategoryId,
                    User = new User { Id = p.User.Id, Name = p.User.Name },
                    Category = new Category { Id = p.Category.Id, Name = p.Category.Name }
                })
                .ToListAsync();
        }

        public async Task<bool> ApprovePost(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            post.Status = PostStatus.Approved;
            _context.Posts.Update(post);
            return true;
        }

        public async Task<bool> RejectPost(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return false;

            post.Status = PostStatus.Rejected;
            _context.Posts.Update(post);
            return true;
        }

        public async Task<bool> AdminDeletePost(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return false;

            if (post.Comments?.Any() == true)
            {
                _context.Comments.RemoveRange(post.Comments);
            }

            if (post.Likes?.Any() == true)
            {
                _context.Likes.RemoveRange(post.Likes);
            }

            if (post.Ratings?.Any() == true)
            {
                _context.Ratings.RemoveRange(post.Ratings);
            }

            _context.Posts.Remove(post);
            return true;
        }


        public async Task<int> GetPendingPostsCount()
        {
            return await _context.Posts
                .Where(p => p.Status == PostStatus.Pending)
                .CountAsync();
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _context.Posts.CountAsync();
        }

        public async Task<int> GetApprovedPostsCount()
        {
            return await _context.Posts
                .Where(p => p.Status == PostStatus.Approved)
                .CountAsync();
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);

        }
        public async Task DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);

        }
    }
}
