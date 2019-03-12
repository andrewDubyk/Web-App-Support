using System;
using System.Threading.Tasks;
using WebShop.EF;
using WebShop.Models;
using WebShop.Interfaces;

namespace WebShop.Repositories
{
    public class WorkContext : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private UserProfileRepository _userProfileRepositiry;
        private ProductRepository _productRepository;
        private CartRepository _cartRepository;

        public WorkContext(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<UserProfile> UserProfiles
        {
            get
            {
                if (_userProfileRepositiry == null)
                    _userProfileRepositiry = new UserProfileRepository(_context);
                return _userProfileRepositiry;
            }
        }

        public IRepository<Product> Products
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        //public IRepository<ProductOwner> ProductOwners
        //{
        //    get
        //    {
        //        if (_productOwnerRepository == null)
        //            _productOwnerRepository = new ProductOwnerRepository(_context);
        //        return _productOwnerRepository;
        //    }
        //}

        public IRepository<Cart> Carts
        {
            get
            {
                if (_cartRepository == null)
                    _cartRepository = new CartRepository(_context);
                return _cartRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
