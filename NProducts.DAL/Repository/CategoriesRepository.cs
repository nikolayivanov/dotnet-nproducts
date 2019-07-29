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

namespace NProducts.DAL.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoriesRepository : IRepository<Categories>
    {
        private NorthwindContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public CategoriesRepository(NorthwindContext db)
        {
            this.db = db;
        }

        public void Create(Categories item)
        {
            db.Categories.Add(item);
        }

        public void Delete(int id)
        {
            Categories item = db.Categories.Find(id);
            if (item != null)
                db.Categories.Remove(item);
        }

        public IEnumerable<Categories> Find(Func<Categories, bool> predicate)
        {
            return db.Categories.Where(predicate).ToList();
        }       

        public Categories Get(int id)
        {
            return this.db.Categories.Find(id);
        }

        public async Task<Categories> GetAsync(int id)
        {
            return await this.db.Categories.FindAsync(id);
        }

        public IEnumerable<Categories> GetAll()
        {
            return this.db.Categories;
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await this.db.Categories.ToListAsync();
        }

        public void Update(Categories item)
        {
            db.Entry(item).State = EntityState.Modified;
        }        

        public async Task<IEnumerable<Categories>> GetAsync(int page, int pagesize, string orderbyfieldname, string orderbydirection, Func<Categories, bool> filter)
        {
            if (string.IsNullOrEmpty(orderbyfieldname))
            {
                var p = new Categories();
                orderbyfieldname = nameof(p.CategoryId);
                orderbydirection = "ASC";
            }

            var query = from t in this.db.Categories select t;
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
