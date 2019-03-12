using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.EF;
using WebShop.Interfaces;
using WebShop.Models;

namespace WebShop.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<Product, bool> predicate)
        {
            return _context.Products.Any(predicate);
        }

        public void Create(Product item)
        {
            _context.Products.Add(item);
        }

        public async Task CreateAsync(Product item)
        {
            await _context.Products.AddAsync(item);
        }

        public void Delete(string id)
        {
            Product p = _context.Products.Find(id);
            if (p != null)
                _context.Products.Remove(p);
        }

        public async Task DeleteAsync(string id)
        {
            Product p = await _context.Products.FindAsync(id);
            if (p != null)
                _context.Products.Remove(p);
        }

        public Product Find(string id)
        {
            return _context.Products
                .Include(p => p.UserProfile)
                .Include(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart)
                .Where(p => p.ProductId == id)
                .FirstOrDefault();
        }

        public Product Find(Func<Product, bool> predicate)
        {
            return _context.Products
               .Include(p => p.UserProfile)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Cart)
               .Where(predicate)
               .FirstOrDefault();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
              .Include(p => p.UserProfile)
              .Include(p => p.ProductInCarts)
              .ThenInclude(p => p.Cart)
              .ToList();
        }

        public IEnumerable<Product> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> productIds = new HashSet<string>(ids);
            return _context.Products
               .Where(p => productIds.Contains(p.ProductId))
               .Include(p => p.UserProfile)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Cart)
               .ToList();
        }

        public IEnumerable<Product> GetAllWhere(Func<Product, bool> predicate)
        {
            return _context.Products
               .Include(p => p.UserProfile)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Cart)
               .Where(predicate)
               .ToList();
        }

        public void Remove(Product item)
        {
            _context.Products.Remove(item);
        }

        public Product SingleOrDefault(Func<Product, bool> predicate)
        {
            return _context.Products.SingleOrDefault(predicate);
        }

        public void Update(Product item)
        {
            _context.Products.Update(item);
        }
    }
}
