using BlogApp.Repositories.Interfaces;

namespace BlogApp.UnitOfWork.Interfaces
{

    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        public IUserRepository UserRepository { get; }

        Task<int> Save();
    }

}
