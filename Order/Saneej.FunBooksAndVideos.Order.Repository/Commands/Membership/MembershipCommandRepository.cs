using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Context;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership
{
    public class MembershipCommandRepository : IMembershipCommandRepository
    {

        private readonly FunBooksAndVideosContext _dbContext;

        public MembershipCommandRepository(FunBooksAndVideosContext context)
        {
            _dbContext = context;
        }

        public async Task AddMembership(Data.Entities.Membership membership)
        {
            await _dbContext.AddAsync(membership);
        }
    }
}
