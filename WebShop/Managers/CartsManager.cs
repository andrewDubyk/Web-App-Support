using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.Extensions.IdentityExtensions;
using WebShop.Models;
using WebShop.Repositories;

namespace WebShop.Managers
{
    public class CartsManager<TCart> : IDisposable where TCart : class
    {
        private readonly WorkContext _context;

        public CartsManager(WorkContext context)
        {
            _context = context;
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            return _context.Carts.GetAll();
        }

        public async Task CreateAsync(Cart service)
        {
            _context.Carts.Create(service);
            await _context.SaveAsync();
        }

        public Cart Find(string id)
        {
            return _context.Carts.Find(id);
        }

        public async Task UpdateAsync(Cart service)
        {
            _context.Carts.Update(service);
            await _context.SaveAsync();
        }

        public async Task RemoveAsync(Cart service)
        {
            _context.Carts.Remove(service);
            await _context.SaveAsync();
        }

        public bool IsCartExists(string id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }

        public bool IsProductInCart(ClaimsPrincipal principal, Product product)
        {
            UserProfile userProfile = _context.UserProfiles.Find(principal.GetUserId());

            if (userProfile == null || product == null)
            {
                return false;
            }

            return userProfile.Cart.ProductInCarts
                .Any(p => p.ProductId == product.ProductId);
        }

        public bool IsProductInCart(ClaimsPrincipal principal, string productId)
        {
            return true;
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
