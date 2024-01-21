using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Order.Service.Models;
using Saneej.FunBooksAndVideos.Service.Constants;
using Saneej.FunBooksAndVideos.Service.Customer;
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

                //Get all the products by ids from Product microservice - calling using HttpClient
                var products = await GetAllFromProductService<ProductViewModel>(basket.BasketItems);

                var purchaseOrderLines = new List<PurchaseOrderLine>();

                foreach (var basketItem in basket.BasketItems)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == basketItem.ProductId);

                    if (product != null)
                    {
                        var orderLine = _purchaseOrderMapper.MapToPurchaseOrderLine(product, basketItem.Quantity);
                        purchaseOrderLines.Add(orderLine);
                    }
                }

                if (!purchaseOrderLines.Any())
                {
                    return ResponseWrapper.CreateClientError("Products are out of stock.");
                }

                var total = purchaseOrderLines.Sum(item => item.UnitPrice * item.Quantity);

                var purchaseOrder = _purchaseOrderMapper.MapToPurchaseOrder(purchaseOrderLines, PurchaseOrderConstants.OrderPlaced, total, basket.CustomerId);

                await _unitOfWork.PurchaseOrderCommandRepository.AddPurchaseOrder(purchaseOrder);

                await _unitOfWork.SaveChanges();

                _unitOfWork.CommitTransaction();

                // BR1.If the purchase order contains a membership, it has to be activated in the customer account immediately.
                // Option 1 : Send a message to ServiceBus, which will be consumed by another Shipping Microservice 
                // Option 2: See below implementation
                await _customerService.ActivateMembership(purchaseOrder.CustomerId, purchaseOrder.PurchaseOrderId);

                // BR2.If the purchase order contains a physical product a shipping slip has to be generated.
                // Option 1 : Send a message to ServiceBus, which will be consumed by another Shipping Microservice 
                // Option 2: See below implementation
                await _shippingService.GenerateShippingSlip(purchaseOrder.CustomerId, purchaseOrder.PurchaseOrderId);

                var purchaseOrderResponse = _purchaseOrderMapper.MapToPurchaseOrderResponse(purchaseOrder);
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

            //TODO : Inject via constructor 
            // Reading from app config
            var _funBooksAndVideosOptions = new FunBooksAndVideosOptions();

            var path = _funBooksAndVideosOptions.ProductServicePathUrl;
            var baseUrl = _funBooksAndVideosOptions.BaseUrl;

            var response = await _integrationHttpService.PostAsync<List<ProductViewModel>>($"{baseUrl}/{path}", productIds);

            return response;
        }
    }
}
