using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping;
using Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder;

namespace Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        // Purchase
        IPurchaseOrderQueryRepository PurchaseOrderQueryRepository { get; }
        IPurchaseOrderCommandRepository PurchaseOrderCommandRepository { get; }

        // Shipping
        IShippingCommandRepository ShippingCommandRepository { get; }

        // Membership
        IMembershipCommandRepository MembershipCommandRepository { get; }

        FunBooksAndVideosContext ReadContext { get; }
        DatabaseFacade Database();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChanges();
    }
}
