using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Data;
using WebsiteShopping.Models;

namespace WebsiteShopping.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductId)
                .ToListAsync();

            return View(products);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            await FillCategoryList();
            return View(new Product());
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            await FillCategoryList();

            if (ModelState.IsValid)
            {
                product.Image = await SaveImage(image);
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await FillCategoryList();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? image)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            await FillCategoryList();

            if (ModelState.IsValid)
            {
                var oldProduct = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                if (oldProduct == null)
                {
                    return NotFound();
                }

                // Không chọn ảnh mới thì giữ nguyên ảnh cũ
                var newImage = await SaveImage(image);
                product.Image = string.IsNullOrEmpty(newImage) ? oldProduct.Image : newImage;

                _db.Update(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Sản phẩm đã có trong đơn hàng thì không cho xóa
            if (await _db.OrderDetails.AnyAsync(od => od.ProductId == id))
            {
                TempData["Error"] = "Sản phẩm đã có trong đơn hàng, không thể xóa!";
                return RedirectToAction(nameof(Index));
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Đổ danh mục vào ViewBag dùng cho thẻ <select>
        private async Task FillCategoryList()
        {
            ViewBag.CategoryList = await _db.Categories
                .OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .ToListAsync();
        }

        // Lưu ảnh tải lên vào wwwroot/images, trả về đường dẫn tương đối
        private async Task<string?> SaveImage(IFormFile? image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            var folder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }
    }
}
