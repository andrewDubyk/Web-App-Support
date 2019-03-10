using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.EF;
using WebShop.Extensions;
using WebShop.Managers;
using WebShop.Models;
using WebShop.Repositories;

namespace WebShop.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly WorkContext _context;
        private readonly UserProfileManager<UserProfile> _userProfileManager;
        private readonly ProductsManager<Product> _productsManager;

        private readonly int ITEMS_ON_PAGE = 5;

        public ProductController(ApplicationDbContext context)
        {
            _context = new WorkContext(context);
            _productsManager = new ProductsManager<Product>(_context);
            _userProfileManager = new UserProfileManager<UserProfile>(_context);
        }

        // GET: Products
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Products()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult EmptyPage()
        {
            return View();
        }

        // GET: Product/Details/5
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsManager.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.UserProfileId = _userProfileManager.GetUserProfileId(User);
                product.CreatedOn = DateTime.Now;
                await _productsManager.CreateAsync(product);

                return RedirectToAction(nameof(Products));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsManager.Find(id);
            if (product == null || !_userProfileManager.IsUserOwnProduct(User, product))
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,ProductOwnerId,UserProfileId,Title,Description,Price")] Product product)
        {
            if (id != product.ProductId || !_userProfileManager.IsUserOwnProduct(User, product))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productsManager.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productsManager.IsProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Products));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsManager.Find(id);
            if (product == null || !_userProfileManager.IsUserOwnProduct(User, product))
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = _productsManager.Find(id);
            if (product == null || !_userProfileManager.IsUserOwnProduct(User, product))
            {
                return NotFound();
            }
            await _productsManager.RemoveAsync(product);

            return RedirectToAction(nameof(Products));
        }

        private IEnumerable<ProductViewModel> ConvertToViewModel(string userId, IEnumerable<Product> products)
        {
            List<ProductViewModel> productsViewModel = new List<ProductViewModel>();
            var userProfile = _userProfileManager.GetUserProfileById(userId);
            foreach (var item in products)
            {
                productsViewModel.Add(new ProductViewModel
                {
                    ProductId = item.ProductId,
                    Title = item.Title,
                    CreatedOn = item.CreatedOn,
                    Description = item.Description,
                    Price = item.Price,
                    IsInCart = userProfile != null ?
                    userProfile
                    .Cart
                    .ProductInCarts
                    .Any(p => p.ProductId == item.ProductId) : false
                });
            }

            return productsViewModel;
        }

        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult ProductPartial(string sortOrder, string currentFilter, string searchString, int? page, bool forOwner, string userId)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IEnumerable<Product> pr;
            if (forOwner)
            {
                if (userId != _userProfileManager.GetUserProfileId(User))
                {
                    throw new Exception("User ids doesn't match");
                }
                pr = _userProfileManager.GetAllProductByOwner(User);
            }
            else
            {
                pr = _productsManager.GetAll();
            }

            var products = ConvertToViewModel(_userProfileManager.GetUserProfileId(User), pr);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                products = products.Where(s => s.Description.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "date_asc":
                    products = products.OrderBy(p => p.CreatedOn);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(p => p.CreatedOn);
                    break;
                default:
                    products = products.OrderBy(p => p.Price);
                    break;
            }

            return PartialView(PaginatedList<ProductViewModel>.Create(products.AsQueryable(), page ?? 1, ITEMS_ON_PAGE));
        }
    }
}
