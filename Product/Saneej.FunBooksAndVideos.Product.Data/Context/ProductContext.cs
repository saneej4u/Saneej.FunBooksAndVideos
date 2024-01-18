using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Product.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saneej.FunBooksAndVideos.Product.Data.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext()
        {
        }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Entities.Product> Products { get; set; }
    }
}
