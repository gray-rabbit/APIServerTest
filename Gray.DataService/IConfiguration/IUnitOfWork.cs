using Gray.DataService.IRepository;

namespace Gray.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();
    }
}
