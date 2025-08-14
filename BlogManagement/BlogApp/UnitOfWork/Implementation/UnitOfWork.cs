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

        private IUserRepository _userRepository;

        private ICountryRepository _countryRepository;

        private IFollowRepository _followRepository;


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

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                return _countryRepository ??= new CountryRepository(_context);
            }
        }

		public IFollowRepository FollowRepository
		{
			get
			{
				return _followRepository ??= new FollowRepository(_context);
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
