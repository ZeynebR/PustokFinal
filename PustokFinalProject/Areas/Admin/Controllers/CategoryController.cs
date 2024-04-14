using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Data;
using PustokFinalProject.Models;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(x => x.Products.Where(x => !x.IsDeleted))
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return View(categories);
        }


        [HttpGet]
        public IActionResult Create()
        {
           
            ViewBag.ParentCategories = _context.Categories.Where(x => !x.IsDeleted).ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
              
                if (!Request.Form.ContainsKey("isSubcategory"))
                {
                    category.ParentId = null;
                }
                else
                {
                    
                    var parentCategory = category.ParentId != null ?
                        await _context.Categories.FindAsync(category.ParentId) :
                        null;

                    category.ParentCategories = parentCategory;
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentCategories = _context.Categories.Where(x => !x.IsDeleted).ToList();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            _context.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

           
            ViewBag.ParentCategories = _context.Categories.Where(x => !x.IsDeleted).ToList();
            ViewBag.SelectedParentId = category.ParentId;

            return View(category);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var parentCategory = category.ParentId != null ?
                        await _context.Categories.FindAsync(category.ParentId) :
                        null;

                    category.ParentCategories = parentCategory;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }


    }
}
    



