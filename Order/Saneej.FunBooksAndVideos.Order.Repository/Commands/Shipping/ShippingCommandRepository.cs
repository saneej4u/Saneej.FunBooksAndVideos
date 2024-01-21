using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Context;

namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping
{
    public class ShippingCommandRepository : IShippingCommandRepository
    {
        private readonly FunBooksAndVideosContext _dbContext;

        public ShippingCommandRepository(FunBooksAndVideosContext context)
        {
            _dbContext = context;
        }

        public async Task AddShippings(List<Data.Entities.Shipping> shippings)
        {
            await _dbContext.AddRangeAsync(shippings);
        }
    }
}
