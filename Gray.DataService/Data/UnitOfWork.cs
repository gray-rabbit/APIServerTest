using Gray.DataService.IConfiguration;
using Gray.DataService.IRepository;
using Gray.DataService.Repository;
using Microsoft.Extensions.Logging;

namespace Gray.DataService.Data
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDbContext context;


        private readonly ILogger logger;

        public IUserRepository Users { get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger("Db_Logger");
            Users  = new UserRepository(context, logger);
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
          context.Dispose();
        }
  }
}
