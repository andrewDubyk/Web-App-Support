using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Repositories;
using WebShop.Extensions.IdentityExtensions;

namespace WebShop.Managers
{
    public class UserProfileManager<TUserProfile> : IDisposable where TUserProfile : class
    {
        private readonly WorkContext _context;
        public const string FOLDERSSTRUCTURE = @"Storage\UserProfileImages";

        public UserProfileManager(WorkContext context)
        {
            _context = context;
        }

        #region Product Owners management

        public Cart GetCartByOwner(ClaimsPrincipal principal)
        {
            var userProfile = GetUserProfile(principal);
            return userProfile.Cart;
        }

        public async Task AddProductToCart(ClaimsPrincipal principal, Product product)
        {
            var userProfile = GetUserProfile(principal);
            userProfile.Cart.ProductInCarts.Add(new ProductInCart { Product = product, Cart = userProfile.Cart });
            await UpdateSaveAsync(userProfile);
        }
    
        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return _context.UserProfiles.GetAll();
        }

        public async Task RemoveProductFromCart(ClaimsPrincipal principal, Product product)
        {
            var userProfile = GetUserProfile(principal);
            var productInCart = userProfile
                .Cart
                .ProductInCarts
                .FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (productInCart != null)
            {
                userProfile.Cart.ProductInCarts.Remove(productInCart);
                await UpdateSaveAsync(userProfile);
            }
        }

        public async Task AddProdcutToOwner(ClaimsPrincipal principal, Product product)
        {
            var userProfile = GetUserProfile(principal);
            userProfile.Products.Add(product);
            await UpdateSaveAsync(userProfile);
        }

        public bool IsProductInCart(ClaimsPrincipal principal, Product product)
        {
            UserProfile userProfile = _context.UserProfiles.Find(principal.GetUserId());
            return userProfile.Cart.ProductInCarts
                .Any(p => p.Product.ProductId == product.ProductId);
        }

        public IEnumerable<Product> GetAllProductByOwner(ClaimsPrincipal principal)
        {
            return GetUserProfile(principal).Products;
        }

        public bool IsUserOwnProduct(ClaimsPrincipal principal, Product product)
        {
            return GetUserProfileId(principal) == product.UserProfileId;
        }

        #endregion

        #region UserProfile entities management

        public async Task ChangeFirstNameAsync(UserProfile user, string firstName)
        {
            user.FirstName = firstName;
            _context.UserProfiles.Update(user);
            await _context.SaveAsync();
        }

        public async Task ChangeFirstNameAsync(ClaimsPrincipal principal, string firstName)
        {
            var user = GetUserProfile(principal);
            await ChangeFirstNameAsync(user, firstName);
        }

        public async Task ChangeSecondNameAsync(UserProfile user, string secondName)
        {
            user.SecondName = secondName;
            _context.UserProfiles.Update(user);
            await _context.SaveAsync();
        }

        public async Task ChangeSecondNameAsync(ClaimsPrincipal principal, string secondName)
        {
            var user = GetUserProfile(principal);
            await ChangeSecondNameAsync(user, secondName);
        }

        public async Task<bool> ChangeUserProfilePhotoAsync(UserProfile user, IFormFile file, string webRootPath)
        {
            if (file == null)
            {
                return false;
            }

            string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string filePath = Path.Combine(webRootPath, "UserProfile", FOLDERSSTRUCTURE ,filename);

            if (!Directory.Exists(webRootPath + "\\" + "UserProfile" + "\\" + FOLDERSSTRUCTURE))
            {
                Directory.CreateDirectory(webRootPath + "\\" + "UserProfile" + "\\" + FOLDERSSTRUCTURE);
            }

            using (FileStream fs = File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            user.ImageName = file.FileName;

            _context.UserProfiles.Update(user);
            await _context.SaveAsync();

            return true;
        }

        public async Task<bool> ChangeUserProfilePhotoAsync(ClaimsPrincipal principal, IFormFile file, string webRootPath)
        {
            var user = GetUserProfile(principal);
            if (file == null)
            {
                return false;
            }

            await ChangeUserProfilePhotoAsync(user, file, webRootPath);

            return true;
        }

        public UserProfile GetUserProfile(ClaimsPrincipal principal)
        {
            UserProfile userProfile = _context.UserProfiles.Find(principal.GetUserId());
            return userProfile;
        }

        public UserProfile GetUserProfileById(string id)
        {
            UserProfile userProfile = _context.UserProfiles.Find(id);
            return userProfile;
        }

        public string GetUserProfileId(ClaimsPrincipal principal)
        {
            return principal.GetUserId();
        }

        public async Task UpdateSaveAsync(UserProfile user)
        {
            _context.UserProfiles.Update(user);
            await _context.SaveAsync();
        }

        #endregion

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
