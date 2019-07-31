using NProducts.Data.Context;
using NProducts.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NProducts.DAL.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using NProducts.Data.Common;
using Microsoft.Extensions.Options;

namespace NProducts.DAL.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsRepository: IRepository<Products>
    {
        private NorthwindContext db;
        private IOptions<NProductsOptions> nproductsoptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public ProductsRepository(NorthwindContext db, IOptions<NProductsOptions> nproductsoptions)
        {
            this.db = db;
            this.nproductsoptions = nproductsoptions;
        }

        public void Create(Products item)
        {
            db.Products.Add(item);
        }

        public void Delete(int id)
        {
            Products product = db.Products.Find(id);
            if (product != null)
                db.Products.Remove(product);
        }

        public IEnumerable<Products> Find(Func<Products, bool> predicate)
        {
            return db.Products.Where(predicate).ToList();
        }       

        public Products Get(int id)
        {
            return this.db.Products.Find(id);
        }

        public async Task<Products> GetAsync(int id)
        {
            return await this.db.Products.FindAsync(id);
        }

        public IEnumerable<Products> GetAll()
        {
            return this.db.Products;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await this.db.Products.Include(a => a.Category).Include(a => a.Supplier).ToListAsync();
        }       

        public void Update(Products item)
        {
            db.Set<Products>().Attach(item);
            db.Entry(item).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Products>> GetAsync(int page, int pagesize, string orderbyfieldname, string orderbydirection, Func<Products, bool> filter)
        {
            if (string.IsNullOrEmpty(orderbyfieldname))
            {
                var p = new Products();
                orderbyfieldname = nameof(p.ProductId);
                orderbydirection = "ASC";
            }

            var query = from t in this.db.Products.Include(a => a.Category).Include(a => a.Supplier) select t;
            query = query.Where(a => filter(a));
            if (orderbydirection == "ASC")
            {
                query = query.OrderByProperty(orderbyfieldname);
            }
            else
            {
                query = query.OrderByPropertyDescending(orderbyfieldname);
            }

            query = query.Skip((page - 1) * page);
            query = query.Take(pagesize);
            return await query.ToListAsync();
        }
    }
}
