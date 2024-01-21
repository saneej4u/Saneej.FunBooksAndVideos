using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Shipping;

namespace Saneej.FunBooksAndVideos.Services.Tests.Shipping
{
    public class ShippingServiceTests
    {
        private Fixture _fixture;
        private ShippingService _sut;

        private IUnitOfWork _unitOfWorkStub;


        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _unitOfWorkStub = Substitute.For<IUnitOfWork>();

            _sut = new ShippingService(_unitOfWorkStub);
        }

        [Test]
        public async Task GenerateShippingSlip_With_NoOrderItems_Returns_Order_Not_Exist()
        {
            // Arrange

            // Act
            var result = await _sut.GenerateShippingSlip(1, 2);

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.ShippingCommandRepository.DidNotReceive().AddShippings(new List<Data.Entities.Shipping>());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Order not exist");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task GenerateShippingSlip_With_OrderItems_Has_NoMembershipLine_Returns_No_Membership_To_Activate()
        {
            // Arrange
            var order = _fixture.Create<Data.Entities.PurchaseOrder>();
            foreach (var purchaseOrderLine in order.PurchaseOrderLines)
            {
                purchaseOrderLine.IsPhysicalProduct = false;
            }
            _unitOfWorkStub.PurchaseOrderQueryRepository.FindByIdAsync(default, default).ReturnsForAnyArgs(order);

            // Act
            var result = await _sut.GenerateShippingSlip(order.CustomerId, order.PurchaseOrderId);

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.ShippingCommandRepository.DidNotReceiveWithAnyArgs().AddShippings(new List<Data.Entities.Shipping>());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("There no physical product to be shipped.");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }
        [Test]
        public async Task GenerateShippingSlip_With_PhysicalProduct_Returns_Successfully_Shipped()
        {
            // Arrange
            var order = _fixture.Create<Data.Entities.PurchaseOrder>();

            foreach(var purchaseOrderLine in order.PurchaseOrderLines)
            {
                purchaseOrderLine.IsPhysicalProduct = true;
            }

            _unitOfWorkStub.PurchaseOrderQueryRepository.FindByIdAsync(default, default).ReturnsForAnyArgs(order);

            var productToBeShipped = order.PurchaseOrderLines.Where(p => p.IsPhysicalProduct).ToList();

            var shippings = productToBeShipped.Select(s => new Data.Entities.Shipping()
            {
                CustomerId = order.CustomerId,
                OrderId = order.PurchaseOrderId,
                OrderLineId = s.PurchaseOrderLineId,
                ShippingStatus = "Created"
            }).ToList();

            // Act
            var result = await _sut.GenerateShippingSlip(order.CustomerId, order.PurchaseOrderId);

            // Assert;
            await _unitOfWorkStub.ShippingCommandRepository.ReceivedWithAnyArgs(1).AddShippings(shippings);
            await _unitOfWorkStub.Received(1).SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeTrue();

            result.HasError.Should().BeFalse();
            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeFalse();
        }
    }
}
