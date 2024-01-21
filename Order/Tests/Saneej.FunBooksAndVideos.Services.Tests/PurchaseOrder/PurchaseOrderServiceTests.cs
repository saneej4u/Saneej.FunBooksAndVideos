using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Constants;
using Saneej.FunBooksAndVideos.Service.Customer;
using Saneej.FunBooksAndVideos.Service.Mappers;
using Saneej.FunBooksAndVideos.Service.Models;
using Saneej.FunBooksAndVideos.Service.PurchaseOrder;
using Saneej.FunBooksAndVideos.Service.Services.Integration;
using Saneej.FunBooksAndVideos.Service.Shipping;

namespace Saneej.FunBooksAndVideos.Services.Tests.PurchaseOrder
{
    [TestFixture]
    public class PurchaseOrderServiceTests
    {
        private Fixture _fixture;
        private PurchaseOrderService _sut;

        private IUnitOfWork _unitOfWorkStub;
        private IIntegrationHttpService _integrationHttpServiceStub;
        private IMembershipService _customerServiceStub;
        private IShippingService _shippingServiceStub;
        private IPurchaseOrderMapper _purchaseOrderMapperStub;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _unitOfWorkStub = Substitute.For<IUnitOfWork>();
            _integrationHttpServiceStub = Substitute.For<IIntegrationHttpService>();
            _customerServiceStub = Substitute.For<IMembershipService>();
            _shippingServiceStub = Substitute.For<IShippingService>();
            _purchaseOrderMapperStub = Substitute.For<IPurchaseOrderMapper>();

            _sut = new PurchaseOrderService(
                _unitOfWorkStub,
                _customerServiceStub,
                _integrationHttpServiceStub,
                _shippingServiceStub,
                _purchaseOrderMapperStub);
        }

        [Test]
        public async Task ProcessOrder_With_Empty_Basket_Returns_NotFoundError()
        {
            // Act
            var result = await _sut.ProcessOrder(null);

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.PurchaseOrderCommandRepository.DidNotReceive().AddPurchaseOrder(new Data.Entities.PurchaseOrder());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Invalid call - no basket exist");

            result.IsNotFound.Should().BeTrue();
            result.IsClientError.Should().BeFalse();
        }

        [Test]
        public async Task ProcessOrder_With_Empty_BasketItems_Returns_ClientError()
        {
            // Act
            var result = await _sut.ProcessOrder(new BasketRequest());

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.PurchaseOrderCommandRepository.DidNotReceive().AddPurchaseOrder(new Data.Entities.PurchaseOrder());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Basket is empty, cannot process the order");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ProcessOrder_With_Product_NotMatches_BasketItems_Returns_ClientError()
        {
            // Arrange

            var basketRequest = _fixture.Create<BasketRequest>();
            var productId = 1;
            basketRequest.BasketItems.ForEach(bi =>
            {
                bi.ProductId = productId + 1;
            });

            var productViewModels = basketRequest.BasketItems.Select(bi => new ProductViewModel { ProductId = 0, IsPhysicalProduct = true }).ToList();

            // Mock HTTP Client call
            _integrationHttpServiceStub.PostAsync<List<ProductViewModel>>(default, default, default).ReturnsForAnyArgs(productViewModels);

            // Act
            var result = await _sut.ProcessOrder(basketRequest);


            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.PurchaseOrderCommandRepository.DidNotReceive().AddPurchaseOrder(new Data.Entities.PurchaseOrder());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Products are out of stock.");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ProcessOrder_With_Default_Basket_Returns_OrderResponse()
        {
            // Arrange
            var basketRequest = _fixture.Create<BasketRequest>();
            var productId = 1;
            basketRequest.BasketItems.ForEach(bi =>
            {
                bi.ProductId = productId + 1;
            });

            var productViewModels = basketRequest.BasketItems.Select(bi => new ProductViewModel { ProductId = bi.ProductId, IsPhysicalProduct = true }).ToList();

            // Mock HTTP Client call
            _integrationHttpServiceStub.PostAsync<List<ProductViewModel>>(default, default, default).ReturnsForAnyArgs(productViewModels);

            var codes = new List<string> { ProductTypeConstants.BOOK_MEMBERSHIP, ProductTypeConstants.VIDEO_MEMBERSHIP, ProductTypeConstants.PREMIUM_MEMBERSHIP };
            var purchaseOrderLines = _fixture.Build<PurchaseOrderLineResponse>();

            foreach (var productViewModel in productViewModels)
            {
                _purchaseOrderMapperStub.MapToPurchaseOrderLine(productViewModel, 1).ReturnsForAnyArgs(new Data.Entities.PurchaseOrderLine());
            }

            var purchaseOrderResponse = _fixture.Create<PurchaseOrderResponse>();
            var orderLineResponses = codes.Select(x => purchaseOrderLines.With(x => x.ProductTypeCode, x).Create());
            purchaseOrderResponse.PurchaseOrderLines.AddRange(orderLineResponses);
            _purchaseOrderMapperStub.MapToPurchaseOrderResponse(default).ReturnsForAnyArgs(purchaseOrderResponse);

            _purchaseOrderMapperStub.MapToPurchaseOrder(default, default, default, default).ReturnsForAnyArgs(new Data.Entities.PurchaseOrder());

            // Act
            var result = await _sut.ProcessOrder(basketRequest);

            // Assert;


            // Should call database changes
            await _unitOfWorkStub.PurchaseOrderCommandRepository.ReceivedWithAnyArgs(1).AddPurchaseOrder(new Data.Entities.PurchaseOrder());
            await _unitOfWorkStub.Received(1).SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();

            result.HasError.Should().BeFalse();
            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeFalse();
        }
    }
}
