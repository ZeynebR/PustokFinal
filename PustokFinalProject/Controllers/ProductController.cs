using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Data;
using PustokFinalProject.Models;

namespace PustokFinalProject.Controllers
{
    public class ProductController : Controller
    {
		private readonly AppDbContext _context;

		public ProductController(AppDbContext context)
		{
			_context = context;
		}

		public  async Task<IActionResult> Index()
        {

			var products = await _context.Products.
                Include(x=>x.Category).
                Include(x=>x.ProductAuthors).
                Include(x=>x.ProductTags).
                Include(x=>x.ProductImages).
                OrderByDescending(x => x.Id).Take(4).ToListAsync();
			return View(products);
        }

        public  async Task<ActionResult> Detail()
        {
            var product = await _context.Products
              .Include(x => x.Category)
              .Include(x => x.ProductImages)
              .FirstOrDefaultAsync();


            return View(product);
        }

        //public async Task<IActionResult> Detail(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var product = await _context.Products
        //       .Include(x => x.Category)
        //       .Include(x => x.ProductImages)
        //       .FirstOrDefaultAsync(x => x.Id == id);

          
        //    return View(product);
        //}
    }
}
