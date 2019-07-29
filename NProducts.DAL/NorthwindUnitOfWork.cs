using NProducts.DAL.Repository;
using NProducts.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using NProducts.DAL.Interfaces;
using NProducts.Data.Models;

namespace NProducts.DAL
{
    public class NorthwindUnitOfWork : IDisposable
    {
        private bool disposed = false;
        private NorthwindContext db;
        private IRepository<Products> productsRepository;
        private IRepository<Categories> categoriesRepository;

        public NorthwindUnitOfWork(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();            
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NorthwindDB"));
            this.db = new NorthwindContext(optionsBuilder.Options);
        }

        public IRepository<Products> Products
        {
            get
            {
                if (productsRepository == null)
                    productsRepository = new ProductsRepository(db);
                return productsRepository;
            }
        }

        public IRepository<Categories> Categories
        {
            get
            {
                if (categoriesRepository == null)
                    categoriesRepository = new CategoriesRepository(db);
                return categoriesRepository;
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
