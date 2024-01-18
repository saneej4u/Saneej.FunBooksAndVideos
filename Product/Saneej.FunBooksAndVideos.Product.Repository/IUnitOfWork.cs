using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Product.Data.Context;
using Saneej.FunBooksAndVideos.Product.Repository.Queries;

namespace Saneej.FunBooksAndVideos.Product.Repository
{
    public interface IUnitOfWork
    {
        IProductQueryRepository ProductQueryRepository { get; }
        ProductContext ReadContext { get; }
        DatabaseFacade Database();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChanges();
    }
}
