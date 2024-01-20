using Microsoft.EntityFrameworkCore;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder
{
    public class PurchaseOrderCommandRepository : IPurchaseOrderCommandRepository
    {
        private readonly DbSet<Data.Entities. PurchaseOrder> _dbContext;

        public PurchaseOrderCommandRepository(DbSet<Data.Entities.PurchaseOrder> dbSet)
        {
            _dbContext = dbSet;
        }
        public async Task AddPurchaseOrder(Data.Entities.PurchaseOrder order)
        {
            await _dbContext.AddAsync(order);
        }
    }
}
