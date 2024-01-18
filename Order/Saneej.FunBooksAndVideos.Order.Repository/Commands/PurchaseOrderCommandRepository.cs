using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Entities;

namespace Saneej.FunBooksAndVideos.Repository.Commands
{
    public class PurchaseOrderCommandRepository : IPurchaseOrderCommandRepository
    {
        private readonly DbSet<PurchaseOrder> _dbContext;

        public PurchaseOrderCommandRepository(DbSet<PurchaseOrder> dbSet)
        {
            _dbContext = dbSet;
        }
        public async Task AddOrder(PurchaseOrder order)
        {
            await _dbContext.AddAsync(order);
        }
    }
}
