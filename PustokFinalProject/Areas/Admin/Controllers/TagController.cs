using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokFinalProject.Data;
using PustokFinalProject.Models;

namespace PustokFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {

       
      private readonly AppDbContext _context;
            public TagController(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Index()
            {
                var tags = await _context.Tags.ToListAsync();   
                return View(tags);
            }

            public IActionResult Create()
            {
                return View();
            }


            [HttpPost]
            public async Task<IActionResult> Create(Tag tag)
            {
                tag.Name = tag.Name.ToUpper().Trim();
                if (_context.Tags.Any(x => x.Name.ToUpper().Trim() == tag.Name))
                {
                    ModelState.AddModelError("", "Tag already exist");
                    return View();
                }
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.Id))
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

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag );
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }

    }
    }

