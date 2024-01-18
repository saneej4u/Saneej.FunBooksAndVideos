using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Entities;

namespace Saneej.FunBooksAndVideos.Data.Context
{
    public class FunBooksAndVideosContext : DbContext
    {
        public FunBooksAndVideosContext()
        {
        }

        public FunBooksAndVideosContext(DbContextOptions<FunBooksAndVideosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        //public virtual DbSet<ProductType> ProductTypes { get; set; }
        //public virtual DbSet<Product> Products { get; set; }
    }
}
