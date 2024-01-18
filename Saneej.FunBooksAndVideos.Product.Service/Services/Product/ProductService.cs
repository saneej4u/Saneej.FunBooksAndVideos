using Saneej.FunBooksAndVideos.Product.Repository;

namespace Saneej.FunBooksAndVideos.Product.Service.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Data.Entities.Product>> GetAllProductByIdsAsync(List<int> productIds)
        {
            return await _unitOfWork.ProductQueryRepository.FindAllByIdsAsync(productIds);
        }

        public async Task<Data.Entities.Product> GetSingleProductByIdAsync(int productId)
        {
            return await _unitOfWork.ProductQueryRepository.FindByIdAsync(productId);
        }
    }
}
