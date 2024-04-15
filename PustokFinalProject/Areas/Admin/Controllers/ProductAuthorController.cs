using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Areas.Admin.ViewModels;
using PustokFinalProject.Data;
using PustokFinalProject.Models;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductAuthorController : Controller
    {
        private readonly AppDbContext _context;

        public ProductAuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prAuthors = await _context.ProductAuthors.Include(x=>x.Product).Include(x=>x.Author).ToListAsync();
            return View(prAuthors);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductAuthorVm productAuthorVm)
        {
            if (!ModelState.IsValid) return View(productAuthorVm);

            var exist = await _context.ProductAuthors.Include(x => x.Author).AnyAsync(x => x.AuthorId == productAuthorVm.AuthorId);
            if (exist)
            {
                ModelState.AddModelError("", "Size already exist");
                return View(productAuthorVm);
            }
            _context.ProductAuthors.Add((ProductAuthor)productAuthorVm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAuthor = await _context.ProductAuthors.FindAsync(id);
            if (productAuthor == null)
            {
                return NotFound();
            }

            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(productAuthor);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductAuthor productAuthor)
        {
            if (id != productAuthor.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = await _context.Authors.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(productAuthor);
            }

            try
            {
                _context.Update(productAuthor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductAuthorExists(productAuthor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

   
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAuthor = await _context.ProductAuthors
                .Include(pa => pa.Product)
                .Include(pa => pa.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAuthor == null)
            {
                return NotFound();
            }

            return View(productAuthor);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAuthor = await _context.ProductAuthors.FindAsync(id);
            _context.ProductAuthors.Remove(productAuthor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAuthorExists(int id)
        {
            return _context.ProductAuthors.Any(e => e.Id == id);
        }
    }
}
    

