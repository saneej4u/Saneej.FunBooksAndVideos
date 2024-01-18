using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Repository.Commands;

namespace Saneej.FunBooksAndVideos.Repository
{
    public interface IUnitOfWork
    {
        IPurchaseOrderCommandRepository PurchaseOrderCommandRepository { get; }
        FunBooksAndVideosContext ReadContext { get; }
        DatabaseFacade Database();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChanges();
    }
}
