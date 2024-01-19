using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder;
using Saneej.FunBooksAndVideos.Repository.Commands;

namespace Saneej.FunBooksAndVideos.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(FunBooksAndVideosContext context)
        {
            Context = context;
            PurchaseOrderCommandRepository = new PurchaseOrderCommandRepository(context.PurchaseOrders);
            PurchaseOrderQueryRepository = new PurchaseOrderQueryRepository(context.PurchaseOrders);
        }
        public FunBooksAndVideosContext ReadContext => Context;
        public IPurchaseOrderCommandRepository PurchaseOrderCommandRepository { get; }
        public IPurchaseOrderQueryRepository PurchaseOrderQueryRepository { get; }
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
