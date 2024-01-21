using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Context;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder
{
    public class PurchaseOrderCommandRepository : IPurchaseOrderCommandRepository
    {
        private readonly FunBooksAndVideosContext _dbContext;

        public PurchaseOrderCommandRepository(FunBooksAndVideosContext context)
        {
            _dbContext = context;
        }
        public async Task AddPurchaseOrder(Data.Entities.PurchaseOrder order)
        {
            await _dbContext.AddAsync(order);
        }
    }
}
