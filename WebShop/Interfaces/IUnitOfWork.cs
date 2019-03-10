using System;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<Product> Products { get; }
        IRepository<Cart> Carts { get; }
        Task SaveAsync();
    }
}
