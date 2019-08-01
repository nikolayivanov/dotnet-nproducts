using NProducts.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NProducts.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Products> Products { get; }
        IRepository<Categories> Categories { get; }
        IRepository<Suppliers> Suppliers { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
