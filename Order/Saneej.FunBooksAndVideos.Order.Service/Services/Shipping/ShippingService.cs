using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public class ShippingService : IShippingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShippingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseWrapper<bool>> GenerateShippingSlip(int customerId, int orderId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var order = await _unitOfWork.PurchaseOrderQueryRepository.FindByIdAsync(orderId, customerId);

                if (order == null)
                {
                    return ResponseWrapper.CreateClientError("Order not exist");
                }

                var productToBeShipped = order.PurchaseOrderLines.Where(p => p.IsPhysicalProduct).ToList();

                if (!productToBeShipped.Any())
                {
                    return ResponseWrapper.CreateClientError("There no physical product to be shipped.");
                }

                var shippings = productToBeShipped.Select(s => new Data.Entities.Shipping()
                {
                    CustomerId = customerId,
                    OrderId = orderId,
                    OrderLineId = s.PurchaseOrderLineId,
                    ShippingStatus = "Created"
                }).ToList();

                await _unitOfWork.ShippingCommandRepository.AddShippings(shippings);

                await _unitOfWork.SaveChanges();

                _unitOfWork.CommitTransaction();

                // TODO : Inject mapper and return shipping objects
                return ResponseWrapper.CreateSuccess(true);

            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
