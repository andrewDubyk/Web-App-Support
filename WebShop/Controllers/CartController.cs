using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.EF;
using WebShop.Managers;
using WebShop.Models;
using WebShop.Repositories;

namespace WebShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly WorkContext _context;
        private readonly UserProfileManager<UserProfile> _userProfileManager;
        private readonly CartsManager<Cart> _cartsManager;
        private readonly ProductsManager<Product> _productsManager;

        public CartController(ApplicationDbContext context)
        {
            _context = new WorkContext(context);
            _cartsManager = new CartsManager<Cart>(_context);
            _userProfileManager = new UserProfileManager<UserProfile>(_context);
            _productsManager = new ProductsManager<Product>(_context);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddProduct(string id)
        {
            bool result = false;
            var product = _productsManager.Find(id);
            if (product != null)
            {
                if (!_cartsManager.IsProductInCart(User, product))
                {
                    await _userProfileManager.AddProductToCart(User, product);
                    result = true;
                }
            }

            return Json(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<JsonResult> RemoveProduct(string id)
        {
            bool result = false;
            var product = _productsManager.Find(id);
            if (product != null)
            {
                if (_cartsManager.IsProductInCart(User, product))
                {
                    await _userProfileManager.RemoveProductFromCart(User, product);
                    result = true;
                }
            }

            return Json(result);
        }

        // GET: Cart/Details
        [HttpGet]
        [Authorize]
        public IActionResult Details()
        {
            var cart = _userProfileManager.GetCartByOwner(User);
            if (cart == null)
            {
                return RedirectToAction(nameof(EmptyCart));
            }

            return View(cart.ProductInCarts.Select(p => p.Product).ToList());
        }

        public IActionResult EmptyCart()
        {
            return View();
        }
    }
}
