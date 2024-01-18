using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Repository;
using Saneej.FunBooksAndVideos.Service.Constants;
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
        private readonly ICustomerService _customerService;
        private readonly IShippingService _shippingService;
        private readonly IPurchaseOrderMapper _purchaseOrderMapper;

        public PurchaseOrderService(IUnitOfWork unitOfWork,
            ICustomerService customerService,
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
            if (basket == null)
            {
                return ResponseWrapper.CreateNotFoundError();
            }

            if (!basket.BasketItems.Any())
            {
                return ResponseWrapper.CreateClientError("Basket is empty, cannot process the order");
            }

            try
            {
                _unitOfWork.BeginTransaction();

                //Get all the products by id from microservice
                var products = await GetAllFromProductService<ProductViewModel>(basket.BasketItems);

                var purchaseOrderLines = new List<PurchaseOrderLine>();

                foreach (var basketItem in basket.BasketItems)
                {
                    var productItem = products.FirstOrDefault(x => x.ProductId == basketItem.ProductId);

                    if (productItem != null)
                    {
                        var orderItem = productItem.ToPurchaseOrderLineModel(basketItem.Quantity);
                        purchaseOrderLines.Add(orderItem);
                    }
                }

                if (!purchaseOrderLines.Any())
                {
                    return ResponseWrapper.CreateClientError("Products are out of stock.");
                }

                // Create an Order
                var orderEntity = purchaseOrderLines.ToCreateOrderModel(basket.CustomerId);

                await _unitOfWork.PurchaseOrderCommandRepository.AddOrder(orderEntity);

                await _unitOfWork.SaveChanges();

                _unitOfWork.CommitTransaction();

                var order = _purchaseOrderMapper.MapOrderDetailsFromEntity(orderEntity);

                // BR1.If the purchase order contains a membership, it has to be activated in the customer account immediately.
                var purchasedMemberShip = order.PurchaseOrderLines
                                                .FirstOrDefault(p => GetMembershipCodes().Contains(p.ProductTypeCode));
                //Raise an event to activate customer account.
                if (purchasedMemberShip != null)
                {
                    _customerService.ActivateMembership(order.CustomerId, purchasedMemberShip.ProductTypeCode);
                }

                // BR2.If the purchase order contains a physical product a shipping slip has to be generated.
                // Raise an event to generate a shipping slip.
                _shippingService.GenerateShippingSlip(order.CustomerId, order.PurchaseOrderId);

                return ResponseWrapper.CreateSuccess(order);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private static List<string> GetMembershipCodes()
        {
            return new List<string>
                    {
                        ProductTypeConstants.BOOK_MEMBERSHIP,
                        ProductTypeConstants.VIDEO_MEMBERSHIP,
                        ProductTypeConstants.PREMIUM_MEMBERSHIP
                    };
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
