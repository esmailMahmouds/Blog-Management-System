using BlogApp.Repositories.Interfaces;

namespace BlogApp.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        Task<int> Save();
    }
}
