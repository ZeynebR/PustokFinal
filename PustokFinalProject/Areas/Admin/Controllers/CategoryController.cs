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

        public async Task <IActionResult> Index()
        {
            var categories = await _context.Categories.Include(x=>x.Products).ToListAsync();   
            return View(categories);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentCategories = _context.Categories.ToList(); 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
               
                var parentCategory = category.ParentId != null ?
                    await _context.Categories.FindAsync(category.ParentId) :
                    null;

                category.ParentCategories = parentCategory;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

       
            ViewBag.ParentCategories = _context.Categories.ToList(); 
            return View(category);
        }
    }
}
    



