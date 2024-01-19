using Saneej.FunBooksAndVideos.Repository;

namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public class ShippingService : IShippingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShippingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> GenerateShippingSlip(int customerId, int orderId)
        {
            var order = await _unitOfWork.PurchaseOrderQueryRepository.FindByIdAsync(orderId, customerId);

            if (order is not null)
            {
                var getAllPhysicalProduct = order.PurchaseOrderLines.Where(p => p.IsPhysicalProduct).ToList();
                if (getAllPhysicalProduct.Any())
                {
                    // TODO: update database
                    //Raise an event to ship the product.
                }

                return true;
            }

            return false;
        }
    }
}
