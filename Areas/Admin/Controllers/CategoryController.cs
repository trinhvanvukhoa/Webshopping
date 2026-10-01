using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Data;
using WebsiteShopping.Models;

namespace WebsiteShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.OrderBy(c => c.CategoryId).ToListAsync();
            return View(categories);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View(new Category { Status = "Hiển thị" });
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Danh mục còn sản phẩm thì không cho xóa
            if (await _db.Products.AnyAsync(p => p.CategoryId == id))
            {
                TempData["Error"] = "This category contains products and cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
