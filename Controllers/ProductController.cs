using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Data;

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
    /// </summary>
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /  - home page (karl/index.html)
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await LoadCategoriesAsync();

            // Newest products first, with the category loaded for the Isotope filter class.
            ViewBag.Products = await _db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();

            // The view file is named "index.cshtml" (lowercase) - name it explicitly
            // so it does not depend on the OS being case sensitive.
            return View("index");
        }

        // GET: Product/Shop?categoryId=3&search=dress - product listing, filtered
        public async Task<IActionResult> Shop(int? categoryId, string? search)
        {
            var query = _db.Products.AsNoTracking().Include(p => p.Category);

            if (categoryId.GetValueOrDefault() > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var keyword = search.Trim();
                query = query.Where(p => p.ProductName.Contains(keyword));
            }

            ViewBag.Categories = await LoadCategoriesAsync();
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.Products = await query.OrderBy(p => p.ProductId).ToListAsync();

            return View();
        }

        // GET: Product/ProductDetails/5
        public async Task<IActionResult> ProductDetails(int id)
        {
            // The header menu passes no id -> send the customer to the shop page
            // instead of returning 404.
            if (id <= 0)
            {
                return RedirectToAction(nameof(Shop));
            }

            var product = await _db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;
            ViewBag.Categories = await LoadCategoriesAsync();

            // Related products: same category, excluding the current one.
            ViewBag.RelatedProducts = await _db.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != product.ProductId)
                .OrderBy(p => p.ProductId)
                .Take(8)
                .ToListAsync();

            // The view file is named "product-details.cshtml" (contains a dash) so the
            // view name must be specified explicitly.
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

        /// <summary>
        /// Danh mục đang hoạt động, dùng cho sidebar và bộ lọc của trang chủ / shop.
        /// Nếu DB lưu trạng thái bằng giá trị khác "Active" thì lấy tất cả.
        /// </summary>
        private async Task<List<Category>> LoadCategoriesAsync()
        {
            var categories = await _db.Categories
                .AsNoTracking()
                .Where(c => c.Status == "Active" || c.Status == "active")
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (categories.Count == 0)
            {
                categories = await _db.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.CategoryName)
                    .ToListAsync();
            }

            return categories;
        }
    }
}
