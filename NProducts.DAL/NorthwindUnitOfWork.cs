using NProducts.DAL.Repository;
using NProducts.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace NProducts.DAL
{
    public class NorthwindUnitOfWork : IDisposable
    {
        private bool disposed = false;
        private NorthwindContext db;
        private ProductsRepository productsRepository;

        public NorthwindUnitOfWork(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();            
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NorthwindDB"));
            this.db = new NorthwindContext(optionsBuilder.Options);
        }

        public ProductsRepository Products
        {
            get
            {
                if (productsRepository == null)
                    productsRepository = new ProductsRepository(db);
                return productsRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }       

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
