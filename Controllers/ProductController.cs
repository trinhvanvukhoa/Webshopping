using Microsoft.AspNetCore.Mvc;

namespace WebsiteShopping.Controllers
{
    /// <summary>
    /// Storefront controller for customers (NO [Area]).
    /// The admin part lives in Areas/Admin/Controllers/ProductController.cs
    ///
    /// Maps the 5 pages of the Karl template:
    ///   /                          -> Index          (karl/index.html)
    ///   /Product/Shop              -> Shop           (karl/shop.html)
    ///   /Product/ProductDetails/5  -> ProductDetails (karl/product-details.html)
    ///   /Product/Cart              -> Cart           (karl/cart.html)
    ///   /Product/Checkout          -> Checkout       (karl/checkout.html)
    ///
    /// The views in Views/Product are currently static markup copied from the
    /// Karl template, so the actions just render them. The database query code
    /// is intentionally left out: add it back per action once the pages need
    /// real data (Products / Categories via ApplicationDbContext).
    /// </summary>
    public class ProductController : Controller
    {
        // GET: /  - home page (karl/index.html)
        public IActionResult Index()
        {
            return View("index");
        }

        // GET: Product/Shop - product listing
        public IActionResult Shop()
        {
            return View();
        }

        // GET: Product/ProductDetails/5
        public IActionResult ProductDetails(int id)
        {
            return View("product-details");
        }

        // GET: Product/Cart
        public IActionResult Cart()
        {
            return View();
        }

        // GET: Product/Checkout
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
