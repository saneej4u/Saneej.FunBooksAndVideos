using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Constants;
using Saneej.FunBooksAndVideos.Service.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saneej.FunBooksAndVideos.Services.Tests.Membership
{
    public class MembershipServiceTests
    {
        private Fixture _fixture;
        private MembershipService _sut;

        private IUnitOfWork _unitOfWorkStub;


        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _unitOfWorkStub = Substitute.For<IUnitOfWork>();
            _sut = new MembershipService(_unitOfWorkStub);
        }

        [Test]
        public async Task ActivateMembership_With_NoOrderItems_Returns_Order_Not_Exist()
        {
            // Arrange

            // Act
            var result = await _sut.ActivateMembership(1, 2);

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.MembershipCommandRepository.DidNotReceive().AddMembership(new Data.Entities.Membership());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Order not exist");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ActivateMembership_With_OrderItems_Has_NoMembershipLine_Returns_No_Membership_To_Activate()
        {
            // Arrange
            var order = _fixture.Create<Data.Entities.PurchaseOrder>();

            _unitOfWorkStub.PurchaseOrderQueryRepository.FindByIdAsync(default, default).ReturnsForAnyArgs(order);

            // Act
            var result = await _sut.ActivateMembership(order.CustomerId, order.PurchaseOrderId);

            // Assert;

            // Should not call database changes
            await _unitOfWorkStub.MembershipCommandRepository.DidNotReceiveWithAnyArgs().AddMembership(new Data.Entities.Membership());
            await _unitOfWorkStub.DidNotReceive().SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeFalse();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("There is no membership to activate");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }

        [Test]
        public async Task ActivateMembership_With_PremiumMembership_Returns_Membership_Successfully_Added()
        {
            // Arrange
            var order = _fixture.Create<Data.Entities.PurchaseOrder>();

            foreach (var purchaseOrderLine in order.PurchaseOrderLines)
            {
                purchaseOrderLine.ProductTypeCode = ProductTypeConstants.PREMIUM_MEMBERSHIP;
            }

            _unitOfWorkStub.PurchaseOrderQueryRepository.FindByIdAsync(default, default).ReturnsForAnyArgs(order);

            // Act
            var result = await _sut.ActivateMembership(order.CustomerId, order.PurchaseOrderId);

            // Assert;
            await _unitOfWorkStub.MembershipCommandRepository.ReceivedWithAnyArgs(1).AddMembership(new Data.Entities.Membership());
            await _unitOfWorkStub.Received(1).SaveChanges();

            result.Should().NotBeNull();
            result.Data.Should().BeTrue();

            result.HasError.Should().BeFalse();
            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeFalse();
        }
    }
}
