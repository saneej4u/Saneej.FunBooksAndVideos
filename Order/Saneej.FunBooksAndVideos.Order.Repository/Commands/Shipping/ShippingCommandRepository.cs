using Microsoft.EntityFrameworkCore;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping
{
    public class ShippingCommandRepository : IShippingCommandRepository
    {
        private readonly DbSet<Data.Entities.Shipping> _dbContext;

        public ShippingCommandRepository(DbSet<Data.Entities.Shipping> dbSet)
        {
            _dbContext = dbSet;
        }

        public async Task AddShippings(List<Data.Entities.Shipping> shippings)
        {
            await _dbContext.AddRangeAsync(shippings);
        }
    }
}
