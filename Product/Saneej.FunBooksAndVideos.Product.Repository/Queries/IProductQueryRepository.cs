namespace Saneej.FunBooksAndVideos.Product.Repository.Queries
{
    public interface IProductQueryRepository
    {
        Task<List<Data.Entities.Product>> FindAllByIdsAsync(List<int> productIds);
        Task<Data.Entities.Product> FindByIdAsync(int productId);
    }
}
