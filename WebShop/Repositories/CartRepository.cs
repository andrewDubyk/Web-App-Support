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
    public class CartRepository : IRepository<Cart>
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<Cart, bool> predicate)
        {
            return _context.Carts.Any(predicate);
        }

        public void Create(Cart item)
        {
            _context.Carts.Add(item);
        }

        public async Task CreateAsync(Cart item)
        {
            await _context.Carts.AddAsync(item);
        }

        public void Delete(string id)
        {
            Cart p = _context.Carts.Find(id);
            if (p != null)
                _context.Carts.Remove(p);
        }

        public async Task DeleteAsync(string id)
        {
            Cart p = await _context.Carts.FindAsync(id);
            if (p != null)
                _context.Carts.Remove(p);
        }

        public Cart Find(string id)
        {
            return _context.Carts
                .Include(p => p.UserProfile)
                .ThenInclude(p => p.Products)
                .Include(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Where(p => p.CartId == id)
                .FirstOrDefault();
        }

        public Cart Find(Func<Cart, bool> predicate)
        {
            return _context.Carts
                .Include(p => p.UserProfile)
                .ThenInclude(p => p.Products)
                .Include(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Where(predicate)
                .FirstOrDefault();
        }

        public IEnumerable<Cart> GetAll()
        {
            return _context.Carts
               .Include(p => p.UserProfile)
               .ThenInclude(p => p.Products)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Product)
               .ToList();
        }

        public IEnumerable<Cart> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> cartstIds = new HashSet<string>(ids);
            return _context.Carts
               .Where(p => cartstIds.Contains(p.CartId))
               .Include(p => p.UserProfile)
               .ThenInclude(p => p.Products)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Product)
               .ToList();
        }

        public IEnumerable<Cart> GetAllWhere(Func<Cart, bool> predicate)
        {
            return _context.Carts
               .Include(p => p.UserProfile)
               .ThenInclude(p => p.Products)
               .Include(p => p.ProductInCarts)
               .ThenInclude(p => p.Product)
               .Where(predicate)
               .ToList();
        }

        public void Remove(Cart item)
        {
            _context.Carts.Remove(item);
        }

        public Cart SingleOrDefault(Func<Cart, bool> predicate)
        {
            return _context.Carts.SingleOrDefault(predicate);
        }

        public void Update(Cart item)
        {
            _context.Carts.Update(item);
        }
    }
}
