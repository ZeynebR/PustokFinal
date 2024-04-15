
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Data;
using PustokFinalProject.Extensions;
using PustokFinalProject.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                                               .Include(x => x.ProductImages)
                                               .Include(x => x.Category)
                                               .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var selectList = new SelectList(categories, "Id", "Name");
            ViewBag.Categories = selectList;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            if (_context.Products.Any(p => p.Name == product.Name))
            {
                ModelState.AddModelError("", "Product already exists");
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            product.ProductImages = new List<ProductImage>();

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    if (!file.CheckFileSize(2))
                    {
                        ModelState.AddModelError("Files", "Files cannot be more than 2mb");
                        ViewBag.Categories = await _context.Categories.ToListAsync();
                        return View(product);
                    }

                    if (!file.CheckFileType("image"))
                    {
                        ModelState.AddModelError("Files", "Files must be image type!");
                        ViewBag.Categories = await _context.Categories.ToListAsync();
                        return View(product);
                    }

                    var filename = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");
                    var additionalProductImages = CreateProduct(filename, false, false, product);
                    product.ProductImages.Add(additionalProductImages);
                }
            }

            if (!product.MainFile.CheckFileSize(2) || !product.MainFile.CheckFileType("image"))
            {
                ModelState.AddModelError("MainFile", "Main file must be an image and cannot be more than 2mb");
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            var mainFileName = await product.MainFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");
            var mainProductImageCreate = CreateProduct(mainFileName, false, true, product);
            product.ProductImages.Add(mainProductImageCreate);

            if (!product.HoverFile.CheckFileSize(2) || !product.HoverFile.CheckFileType("image"))
            {
                ModelState.AddModelError("HoverFile", "Hover file must be an image and cannot be more than 2mb");
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            var hoverFileName = await product.HoverFile.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");
            var hoverProductImageCreate = CreateProduct(hoverFileName, true, false, product);
            product.ProductImages.Add(hoverProductImageCreate);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ProductImage CreateProduct(string url, bool isHover, bool isMain, Product product)
        {
            return new ProductImage
            {
                Url = url,
                IsHovered = isHover,
                IsMainCover = isMain,
                Product = product
            };
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var product = await _context.Products.Include(x => x.Category)
                                                 .Include(x => x.ProductImages)
                                                 .Include(x => x.ProductTags)
                                                 .ThenInclude(x => x.Tag).
                                                 Include(x =>x.ProductAuthors).
                                                 ThenInclude(x=>x.Author)
                                                 .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

           
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == product.CategoryId 
            }).ToList();

            return View(product);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SellingPrice,DiscountedPrice,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Fetch categories for dropdown list
                var categories = await _context.Categories.ToListAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = c.Id == product.CategoryId 
                }).ToList();

                return View(product);
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
