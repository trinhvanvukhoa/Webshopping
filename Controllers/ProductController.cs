using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebsiteShopping.Data;
using WebsiteShopping.Models;

namespace WebsiteShopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private const string CartSessionKey = "Cart";

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await LoadCategoriesAsync();
            ViewBag.Products = await _db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
            return View("index");
        }

        public async Task<IActionResult> Shop(int? categoryId, string? search)
        {
            IQueryable<Product> query = _db.Products.AsNoTracking().Include(p => p.Category);

            if (categoryId is > 0)
            {
                var catId = categoryId.Value;
                query = query.Where(p => p.CategoryId == catId);
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

        public async Task<IActionResult> ProductDetails(int id)
        {
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

            ViewBag.RelatedProducts = await _db.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != product.ProductId)
                .OrderBy(p => p.ProductId)
                .Take(8)
                .ToListAsync();
            return View("product-details");
        }

        public IActionResult Cart()
        {
            var cart = GetCart();
            ViewBag.CartCount = GetCartItemCount();
            return View(cart);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại");
            }

            var cart = GetCart();

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Image = product.Image,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            SaveCart(cart);

            // Nếu là AJAX request, trả về JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, cartCount = GetCartItemCount(), cartTotal = cart.Sum(c => c.Total) });
            }
            return Redirect(GetSafeRedirectUrl(nameof(Shop)));
        }

        // POST: Product/UpdateCart - Cập nhật số lượng (form duy nhất cho toàn bộ giỏ)
        [HttpPost]
        public IActionResult UpdateCart()
        {
            var cart = GetCart();

            // Đọc tất cả các giá trị quantity_X từ form
            foreach (var key in Request.Form.Keys)
            {
                if (key.ToString().StartsWith("quantity_"))
                {
                    int productId = int.Parse(key.ToString().Replace("quantity_", ""));
                    int quantity = int.Parse(Request.Form[key].ToString());

                    var item = cart.FirstOrDefault(c => c.ProductId == productId);
                    if (item != null)
                    {
                        if (quantity > 0)
                        {
                            item.Quantity = quantity;
                        }
                        else
                        {
                            cart.Remove(item);
                        }
                    }
                }
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        // POST: Product/RemoveFromCart - Xóa sản phẩm khỏi giỏ
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            // Nếu đến từ header dropdown, trở về trang trước đó
            var referer = Request.Headers.Referer.ToString();
            if (!string.IsNullOrWhiteSpace(referer) && referer.Contains("Cart", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(Cart));

            return RedirectToAction(nameof(Cart));
        }

        // POST: Product/ClearCart - Xóa toàn bộ giỏ hàng
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction(nameof(Cart));
        }

        // GET: Product/Checkout - Hiển thị form checkout
        public IActionResult Checkout()
        {
            var cart = GetCart();

            // Nếu giỏ hàng trống, chuyển về trang giỏ hàng
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Cart));
            }

            ViewBag.CartItems = cart;
            ViewBag.CartTotal = cart.Sum(c => c.Total);

            return View();
        }

        // POST: Product/Checkout - Xử lý đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = GetCart();

            // Nếu giỏ hàng trống, chuyển về trang giỏ hàng
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Cart));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CartItems = cart;
                ViewBag.CartTotal = cart.Sum(c => c.Total);
                return View(model);
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                CustomerName = model.CustomerName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Note = model.Note,
                OrderDate = DateTime.Now,
                Status = "Chờ xử lý"
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Tạo chi tiết đơn hàng từ giỏ hàng
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = (int)item.Price
                };

                _db.OrderDetails.Add(orderDetail);
            }

            await _db.SaveChangesAsync();

            // Xóa giỏ hàng sau khi đặt hàng thành công
            HttpContext.Session.Remove(CartSessionKey);

            // Chuyển đến trang xác nhận đơn hàng
            return RedirectToAction(nameof(OrderConfirmation), new { orderId = order.OrderId });
        }

        // GET: Product/OrderConfirmation/5 - Trang xác nhận đơn hàng
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
        }

        private string GetSafeRedirectUrl(string fallbackAction)
        {
            var referer = Request.Headers.Referer.ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                return referer;
            }

            var fallbackUrl = Url.Action(fallbackAction, "Product");
            return !string.IsNullOrWhiteSpace(fallbackUrl) ? fallbackUrl : "/Product/Shop";
        }

        private int GetCartItemCount()
        {
            return GetCart().Sum(c => c.Quantity);
        }
        
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
