namespace Saneej.FunBooksAndVideos.Product.Service.Services.Product
{
    public interface IProductService
    {
        public Task<List<Data.Entities.Product>> GetAllProductByIdsAsync(List<int> productIds);
        public Task<Data.Entities.Product> GetSingleProductByIdAsync(int productId);
    }
}
