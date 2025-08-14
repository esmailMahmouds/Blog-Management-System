using BlogApp.Repositories.Interfaces;

namespace BlogApp.UnitOfWork.Interfaces
{

    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        public IUserRepository UserRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public ICountryRepository CountryRepository { get; }
        public IFollowRepository FollowRepository { get; }


        Task<int> Save();
    }

}
