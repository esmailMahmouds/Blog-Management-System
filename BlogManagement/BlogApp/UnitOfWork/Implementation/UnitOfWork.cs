using BlogApp.Context;
using BlogApp.Repositories.Implementation;
using BlogApp.Repositories.Interfaces;
using BlogApp.UnitOfWork.Interfaces;

namespace BlogApp.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IPostRepository _postRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IPostRepository PostRepository
        {
            get
            {
                return _postRepository ??= new PostRepository(_context);
            }
        }
        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
