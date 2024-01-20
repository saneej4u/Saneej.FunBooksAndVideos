using Microsoft.EntityFrameworkCore;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership
{
    public class MembershipCommandRepository : IMembershipCommandRepository
    {

        private readonly DbSet<Data.Entities.Membership> _dbContext;

        public MembershipCommandRepository(DbSet<Data.Entities.Membership> dbSet)
        {
            _dbContext = dbSet;
        }
        public async Task AddMembership(Data.Entities.Membership membership)
        {
            await _dbContext.AddAsync(membership);
        }
    }
}
