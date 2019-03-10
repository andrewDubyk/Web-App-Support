using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Repositories;

namespace WebShop.Managers
{
    public class ProductsManager<TProduct> : IDisposable where TProduct : class
    {
        private readonly WorkContext _context;

        public ProductsManager(WorkContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.GetAll();
        }

        public async Task CreateAsync(Product service)
        {
            _context.Products.Create(service);
            await _context.SaveAsync();
        }

        public Product Find(string id)
        {
            return _context.Products.Find(id);
        }

        public async Task UpdateAsync(Product service)
        {
            _context.Products.Update(service);
            await _context.SaveAsync();
        }

        public async Task RemoveAsync(Product service)
        {
            _context.Products.Remove(service);
            await _context.SaveAsync();
        }

        public bool IsProductExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _context != null)
                {
                    _context.Dispose(true);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
