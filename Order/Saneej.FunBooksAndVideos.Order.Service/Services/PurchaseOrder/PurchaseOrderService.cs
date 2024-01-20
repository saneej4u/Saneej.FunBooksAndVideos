using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Customer;
using Saneej.FunBooksAndVideos.Service.Extensions;
using Saneej.FunBooksAndVideos.Service.Mappers;
using Saneej.FunBooksAndVideos.Service.Models;
using Saneej.FunBooksAndVideos.Service.Services.Integration;
using Saneej.FunBooksAndVideos.Service.Shipping;

namespace Saneej.FunBooksAndVideos.Service.PurchaseOrder
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationHttpService _integrationHttpService;
        private readonly IMembershipService _customerService;
        private readonly IShippingService _shippingService;
        private readonly IPurchaseOrderMapper _purchaseOrderMapper;

        public PurchaseOrderService(IUnitOfWork unitOfWork,
            IMembershipService customerService,
            IIntegrationHttpService integrationHttpService,
            IShippingService shippingService,
            IPurchaseOrderMapper purchaseOrderMapper)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _integrationHttpService = integrationHttpService;
            _shippingService = shippingService;
            _purchaseOrderMapper = purchaseOrderMapper;
        }

        public async Task<ResponseWrapper<PurchaseOrderResponse>> ProcessOrder(BasketRequest basket)
        {
            try
            {
                if (basket == null)
                {
                    return ResponseWrapper.CreateNotFoundError("Invalid call - no basket exist");
                }

                if (basket.BasketItems == null)
                {
                    return ResponseWrapper.CreateClientError("Basket is empty, cannot process the order");
                }

                _unitOfWork.BeginTransaction();

                //Get all the products by id from microservice
                var products = await GetAllFromProductService<ProductViewModel>(basket.BasketItems);

                var purchaseOrderLines = new List<PurchaseOrderLine>();

                foreach (var basketItem in basket.BasketItems)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == basketItem.ProductId);

                    if (product != null)
                    {
                        var orderLine = product.ToPurchaseOrderLineModel(basketItem.Quantity); // TODO: Convert to mapper
                        purchaseOrderLines.Add(orderLine);
                    }
                }

                if (!purchaseOrderLines.Any())
                {
                    return ResponseWrapper.CreateClientError("Products are out of stock.");
                }

                // TODO: Create an Order - Covert to mapper
                var orderEntity = purchaseOrderLines.ToCreateOrderModel(basket.CustomerId);

                await _unitOfWork.PurchaseOrderCommandRepository.AddOrder(orderEntity);

                await _unitOfWork.SaveChanges();

                _unitOfWork.CommitTransaction();

                // BR1.If the purchase order contains a membership, it has to be activated in the customer account immediately.
                // Option 1 : Send a message to ServiceBus, which will be consumed by another Shipping Microservice 
                // Option 2: See below implementation
                await _customerService.ActivateMembership(orderEntity.CustomerId, orderEntity.PurchaseOrderId);

                // BR2.If the purchase order contains a physical product a shipping slip has to be generated.
                // Option 1 : Send a message to ServiceBus, which will be consumed by another Shipping Microservice 
                // Option 2: See below implementation
                await _shippingService.GenerateShippingSlip(orderEntity.CustomerId, orderEntity.PurchaseOrderId);

                var purchaseOrderResponse = _purchaseOrderMapper.MapToPurchaseOrderResponseFromEntity(orderEntity);
                return ResponseWrapper.CreateSuccess(purchaseOrderResponse);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private async Task<List<ProductViewModel>> GetAllFromProductService<TResponse>(List<BasketItemRequest> basketItems)
        {
            var productIds = basketItems.Select(b => b.ProductId).ToList();

            //TODO : make it configurable
            var path = "";
            var baseUrl = "";

            var response = await _integrationHttpService.PostAsync<List<ProductViewModel>>($"{baseUrl}/{path}", productIds);

            return response;
        }
    }
}
