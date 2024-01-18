using Microsoft.EntityFrameworkCore.Infrastructure;
using Saneej.FunBooksAndVideos.Product.Data.Context;
using Saneej.FunBooksAndVideos.Product.Repository.Queries;

namespace Saneej.FunBooksAndVideos.Product.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ProductContext context)
        {
            Context = context;
            ProductQueryRepository = new ProductQueryRepository(context.Products);
        }
        public ProductContext ReadContext => Context;
        public IProductQueryRepository ProductQueryRepository { get; }
        protected ProductContext Context { get; }
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
