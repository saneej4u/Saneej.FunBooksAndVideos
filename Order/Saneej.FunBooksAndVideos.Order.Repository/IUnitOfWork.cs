using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder;
using Saneej.FunBooksAndVideos.Repository.Commands;

namespace Saneej.FunBooksAndVideos.Repository
{
    public interface IUnitOfWork
    {
        IPurchaseOrderQueryRepository PurchaseOrderQueryRepository { get; }
        IPurchaseOrderCommandRepository PurchaseOrderCommandRepository { get; }
        FunBooksAndVideosContext ReadContext { get; }
        DatabaseFacade Database();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChanges();
    }
}
