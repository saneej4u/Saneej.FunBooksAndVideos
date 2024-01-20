using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping;
using Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder;

namespace Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(FunBooksAndVideosContext context)
        {
            Context = context;

            // Purchase order
            PurchaseOrderCommandRepository = new PurchaseOrderCommandRepository(context.PurchaseOrders);
            PurchaseOrderQueryRepository = new PurchaseOrderQueryRepository(context.PurchaseOrders);

            // Shipping
            ShippingCommandRepository = new ShippingCommandRepository(context.Shippings);

            // Membership
            MembershipCommandRepository = new MembershipCommandRepository(context.Memberships);
        }
        public FunBooksAndVideosContext ReadContext => Context;

        // Purchase order
        public IPurchaseOrderCommandRepository PurchaseOrderCommandRepository { get; }
        public IPurchaseOrderQueryRepository PurchaseOrderQueryRepository { get; }

        // Shipping
        public IShippingCommandRepository ShippingCommandRepository { get; }

        // Membership
        public IMembershipCommandRepository MembershipCommandRepository { get; }

        protected FunBooksAndVideosContext Context { get; }
        public DatabaseFacade Database()
        {
            return Context.Database;
        }
        public void BeginTransaction()
        {
            Context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            Context.Database.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            Context.Database.RollbackTransaction();
        }
        public Task<int> SaveChanges()
        {
            return Context.SaveChangesAsync();
        }
    }
}
