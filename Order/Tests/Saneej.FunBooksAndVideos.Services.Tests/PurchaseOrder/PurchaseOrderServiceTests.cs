using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Saneej.FunBooksAndVideos.Data.Entities;
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
        public async Task ProcessOrder_With_Membership_Activate_Cuustomer()
        {
            var basketModel = _fixture.Create<BasketRequest>();
            basketModel.BasketItems.ForEach(bi =>
            {
                bi.ProductId = 1;
            });
            var purchaseOrderResponse = _fixture.Create<PurchaseOrderResponse>();

            var path = "";
            var baseUrl = "";
            var productIds = basketModel.BasketItems.Select(b => b.ProductId).ToList();
            _integrationHttpServiceStub.PostAsync<List<ProductViewModel>>($"{baseUrl}/{path}", productIds).ReturnsForAnyArgs(new List<ProductViewModel>
            {
                new ProductViewModel (1, "","","", ProductTypeConstants.BOOK_MEMBERSHIP, 12M,2,true)
            });

            _purchaseOrderMapperStub.MapOrderDetailsFromEntity(default).ReturnsForAnyArgs(purchaseOrderResponse);

            var result = await _sut.ProcessOrder(basketModel);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();

            result.Data.PurchaseOrderId.Should().BeGreaterThan(0);

            result.IsClientError.Should().BeFalse();
        }
    }
}
