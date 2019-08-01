using NProducts.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using NProducts.Data.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NProducts.Data.Common;
using NProducts.DAL.Context;
using NProducts.Data.Interfaces;

namespace NProducts.DAL
{
    public class NorthwindUnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposed = false;
        private NorthwindContext db;
        private IOptionsSnapshot<NProductsOptions> nproductsoptions;
        private IRepository<Products> productsRepository;
        private IRepository<Categories> categoriesRepository;
        private IRepository<Suppliers> suppliersRepository;

        public NorthwindUnitOfWork(IConfiguration configuration, IOptionsSnapshot<NProductsOptions> nproductsoptions)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NorthwindDB"));
            this.db = new NorthwindContext(optionsBuilder.Options);
            this.nproductsoptions = nproductsoptions;
        }

        public IRepository<Products> Products
        {
            get
            {
                if (productsRepository == null)
                    productsRepository = new ProductsRepository(db, this.nproductsoptions);
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

        public IRepository<Suppliers> Suppliers
        {
            get
            {
                if (suppliersRepository == null)
                    suppliersRepository = new SuppliersRepository(db);
                return suppliersRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync();
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
