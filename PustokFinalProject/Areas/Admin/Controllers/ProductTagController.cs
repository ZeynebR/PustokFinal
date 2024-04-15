using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Areas.Admin.ViewModels;
using PustokFinalProject.Data;
using PustokFinalProject.Models;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTagController : Controller
    {
        private readonly AppDbContext _context;

        public ProductTagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prTags = await _context.ProductTags.Include(x=>x.Product).Include(x=>x.Tag).ToListAsync();
            return View(prTags);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductTagVm productTagVm)
        {
            if (!ModelState.IsValid) return View(productTagVm);

            var exist = await _context.ProductTags.Include(x => x.Tag).AnyAsync(x => x.TagId == productTagVm.TagId);
            if (exist)
            {
                ModelState.AddModelError("", "Tag already exist");
                return View(productTagVm);
            }
            _context.ProductTags.Add((ProductTag)productTagVm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags.FindAsync(id);
            if (productTag == null)
            {
                return NotFound();
            }

            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(productTag);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTag productTag)
        {
            if (id != productTag.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(productTag);
            }

            try
            {
                _context.Update(productTag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTagExists(productTag.Id))
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

            var productTag = await _context.ProductTags
                .Include(pa => pa.Product)
                .Include(pa => pa.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTag = await _context.ProductTags.FindAsync(id);
            _context.ProductTags.Remove(productTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTagExists(int id)
        {
            return _context.ProductTags.Any(e => e.Id == id);
        }
    }
}

