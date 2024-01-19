﻿using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Saneej.FunBooksAndVideos.Repository;
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
        private ICustomerService _customerServiceStub;
        private IShippingService _shippingServiceStub;
        private IPurchaseOrderMapper _purchaseOrderMapperStub;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _unitOfWorkStub = Substitute.For<IUnitOfWork>();
            _integrationHttpServiceStub = Substitute.For<IIntegrationHttpService>();
            _customerServiceStub = Substitute.For<ICustomerService>();
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
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            
            result.IsNotFound.Should().BeTrue();
            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Invalid call - no basket exist");

            result.IsClientError.Should().BeFalse();
        }

        [Test]
        public async Task ProcessOrder_With_Empty_BasketItems_Returns_ClientError()
        {
            // Act
            var result = await _sut.ProcessOrder(null);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.IsNotFound.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Basket is empty, cannot process the order");
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ProcessOrder_With_Product_NotMatches_BasketItems_Returns_ClientError()
        {
            var basketRequest = _fixture.Create<BasketRequest>();
            var productId = 1;
            basketRequest.BasketItems.ForEach(bi =>
            {
                bi.ProductId = productId + 1;
            });

            var productViewModels = basketRequest.BasketItems.Select(bi => new ProductViewModel(0, "", "", "", "", 12M, 2, true)).ToList();

            // Mock HTTP Client call
            _integrationHttpServiceStub.PostAsync<List<ProductViewModel>>(default, default, default).ReturnsForAnyArgs(productViewModels);

            //var codes = new List<string> { ProductTypeConstants.BOOK_MEMBERSHIP, ProductTypeConstants.VIDEO_MEMBERSHIP, ProductTypeConstants.PREMIUM_MEMBERSHIP };
            //var purchaseOrderLines = _fixture.Build<PurchaseOrderLineResponse>();
            //var orderLineResponses = codes.Select(x => purchaseOrderLines.With(x => x.ProductTypeCode, x).Create());

            //var purchaseOrderResponse = _fixture.Create<PurchaseOrderResponse>();
            //purchaseOrderResponse.PurchaseOrderLines.AddRange(orderLineResponses);
            //_purchaseOrderMapperStub.MapToPurchaseOrderResponseFromEntity(default).ReturnsForAnyArgs(purchaseOrderResponse);

            // Act
            var result = await _sut.ProcessOrder(basketRequest);


            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.IsNotFound.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Products are out of stock.");
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ProcessOrder_With_Default_Basket_Returns_OrderResponse()
        {
            var basketRequest = _fixture.Create<BasketRequest>();
            var productId = 1;
            basketRequest.BasketItems.ForEach(bi =>
            {
                bi.ProductId = productId + 1;
            });

            var productViewModels = basketRequest.BasketItems.Select(bi => new ProductViewModel(bi.ProductId, "", "", "", "", 12M, 2, true)).ToList();

            // Mock HTTP Client call
            _integrationHttpServiceStub.PostAsync<List<ProductViewModel>>(default, default, default).ReturnsForAnyArgs(productViewModels);

            var codes = new List<string> { ProductTypeConstants.BOOK_MEMBERSHIP, ProductTypeConstants.VIDEO_MEMBERSHIP, ProductTypeConstants.PREMIUM_MEMBERSHIP };
            var purchaseOrderLines = _fixture.Build<PurchaseOrderLineResponse>();
            var orderLineResponses = codes.Select(x => purchaseOrderLines.With(x => x.ProductTypeCode, x).Create());

            var purchaseOrderResponse = _fixture.Create<PurchaseOrderResponse>();
            purchaseOrderResponse.PurchaseOrderLines.AddRange(orderLineResponses);
            _purchaseOrderMapperStub.MapToPurchaseOrderResponseFromEntity(default).ReturnsForAnyArgs(purchaseOrderResponse);

            // Act
            var result = await _sut.ProcessOrder(basketRequest);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeFalse();
        }
    }
}
