using Microsoft.EntityFrameworkCore;

namespace Saneej.FunBooksAndVideos.Product.Repository.Queries
{
    public class ProductQueryRepository : IProductQueryRepository
    {
        private readonly DbSet<Data.Entities.Product> _dbContext;

        public ProductQueryRepository(DbSet<Data.Entities.Product> dbSet)
        {
            _dbContext = dbSet;
        }

        public async Task<List<Data.Entities.Product>> FindAllByIdsAsync(List<int> productIds)
        {
            return await _dbContext.Include(p => p.ProductType).Where(doc => productIds.Contains(doc.ProductId)).ToListAsync();
        }

        public async Task<Data.Entities.Product> FindByIdAsync(int productId)
        {
            var product = await _dbContext.Include(p => p.ProductType).FirstOrDefaultAsync(doc => doc.ProductId == productId);

            return product ?? throw new Exception("Product not found");
        }
    }
}
