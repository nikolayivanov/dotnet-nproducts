using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NProducts.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAsync(int page, int pagesize, string orderbyfieldname, string orderbydirection, Func<T, Boolean> filter);

        T Get(int id);
        Task<T> GetAsync(int id);

        IEnumerable<T> Find(Func<T, Boolean> predicate);
        
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
