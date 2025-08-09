using BlogApp.Repositories.Interfaces;

namespace BlogApp.UnitOfWork.Interfaces
{

    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }

        Task<int> Save();
    }


}
