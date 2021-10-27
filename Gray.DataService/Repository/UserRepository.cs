using Gray.DataService.Data;
using Gray.DataService.IRepository;
using Gray.Entities.DbSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gray.DataService.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) :
            base(context, logger)
        {
        }

        public override async Task<IEnumerable<User>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Logger
                    .LogError(ex,
                    "{Repo} All Method has generated an error",
                    typeof (UserRepository));

                return new List<User>();
            }
        }

      
    }
}
