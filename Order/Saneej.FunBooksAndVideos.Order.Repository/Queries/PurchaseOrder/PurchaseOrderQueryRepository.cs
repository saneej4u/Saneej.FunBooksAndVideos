using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Context;

namespace Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder
{
    public class PurchaseOrderQueryRepository : IPurchaseOrderQueryRepository
    {
        private readonly FunBooksAndVideosContext _dbContext;

        public PurchaseOrderQueryRepository(FunBooksAndVideosContext context)
        {
            _dbContext = context;
        }
        public async Task<Data.Entities.PurchaseOrder> FindByIdAsync(int orderId, int customerId)
        {
            var order = await _dbContext.PurchaseOrders.FirstOrDefaultAsync(po => po.PurchaseOrderId == orderId && po.CustomerId == customerId);
            return order;
        }
    }
}
