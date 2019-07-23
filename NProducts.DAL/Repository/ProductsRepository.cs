using NProducts.Data.Context;
using NProducts.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NProducts.DAL.Repository
{
    public class ProductsRepository
    {
        private NorthwindContext db;

        public ProductsRepository(NorthwindContext db)
        {
            this.db = db;
        }

        public Products Get(int id)
        {
            return this.db.Products.Find(id);
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await this.db.Products.Include(a => a.Category).Include(a => a.Supplier).ToListAsync();
        }

        public void Create(Products product)
        {
            db.Products.Add(product);
        }

        public void Update(Products product)
        {
            db.Entry(product).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Products product = db.Products.Find(id);
            if (product != null)
                db.Products.Remove(product);
        }
    }
}
