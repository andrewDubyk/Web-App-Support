using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.EF;
using WebShop.Interfaces;
using WebShop.Models;

namespace WebShop.Repositories
{
    public class UserProfileRepository : IRepository<UserProfile>
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.Any(predicate);
        }

        public void Create(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProfile> GetAllWhere(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.Include(p => p.ApplicationUser).Include(p => p.Cart).Include(p => p.Products).Where(predicate);
        }

        public UserProfile Find(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles
                .Include(p => p.ApplicationUser)
                .Include(p => p.Cart)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart)
                .Where(predicate)
                .FirstOrDefault();
        }

        public UserProfile Find(string id)
        {
            return _context.UserProfiles
                .Include(p => p.ApplicationUser)
                .Include(p => p.Cart)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart)
                .SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _context.UserProfiles
                .Include(p => p.ApplicationUser)
                .Include(p => p.Cart)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart);
        }

        public void Remove(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public UserProfile SingleOrDefault(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles
                .Include(p => p.ApplicationUser)
                .Include(p => p.Cart)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart)
                .SingleOrDefault(predicate);
        }

        public void Update(UserProfile item)
        {
            _context.UserProfiles.Update(item);
        }

        public IEnumerable<UserProfile> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);
            return _context.UserProfiles
                .Where(p => usersId.Contains(p.Id))
                .Include(p => p.ApplicationUser)
                .Include(p => p.Cart)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Product)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductInCarts)
                .ThenInclude(p => p.Cart);
        }
    }
}