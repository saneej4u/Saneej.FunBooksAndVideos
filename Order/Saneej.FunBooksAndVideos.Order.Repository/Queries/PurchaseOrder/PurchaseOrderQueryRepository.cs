using Microsoft.EntityFrameworkCore;

namespace Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder
{
    public class PurchaseOrderQueryRepository : IPurchaseOrderQueryRepository
    {
        private readonly DbSet<Data.Entities.PurchaseOrder> _dbContext;

        public PurchaseOrderQueryRepository(DbSet<Data.Entities.PurchaseOrder> dbSet)
        {
            _dbContext = dbSet;
        }
        public async Task<Data.Entities.PurchaseOrder> FindByIdAsync(int orderId, int customerId)
        {
            var order = await _dbContext.FirstOrDefaultAsync(po => po.PurchaseOrderId == orderId && po.CustomerId == customerId);
            return order;
        }
    }
}
