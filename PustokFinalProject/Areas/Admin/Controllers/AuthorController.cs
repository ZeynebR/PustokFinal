using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Data;
using PustokFinalProject.Models;
using System.Threading.Tasks;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.ToListAsync();
            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            author.Name = author.Name.ToUpper().Trim();
            if (_context.Authors.Any(x => x.Name.ToUpper().Trim() == author.Name))
            {
                ModelState.AddModelError("", "Author already exists");
                return View();
            }
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(author.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
