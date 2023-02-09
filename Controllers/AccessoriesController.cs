using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSToDoList.Data;
using CSToDoList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CSToDoList.Controllers
{
    [Authorize]
    public class AccessoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccessoriesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Accessories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accessories.Include(a => a.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accessories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accessories == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessories
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessory == null)
            {
                return NotFound();
            }

            return View(accessory);
        }

        // GET: Accessories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accessories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserId,Name")] Accessory accessory)
        {
            ModelState.Remove("AppUserId");
            string? userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                accessory.AppUserId = _userManager.GetUserId(User);

                _context.Add(accessory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accessory);
        }

        // GET: Accessories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accessories == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessories.FindAsync(id);
            if (accessory == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", accessory.AppUserId);
            return View(accessory);
        }

        // POST: Accessories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,Name")] Accessory accessory)
        {
            if (id != accessory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accessory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessoryExists(accessory.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", accessory.AppUserId);
            return View(accessory);
        }

        // GET: Accessories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accessories == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessories
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessory == null)
            {
                return NotFound();
            }

            return View(accessory);
        }

        // POST: Accessories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accessories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accessories'  is null.");
            }
            var accessory = await _context.Accessories.FindAsync(id);
            if (accessory != null)
            {
                _context.Accessories.Remove(accessory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessoryExists(int id)
        {
          return (_context.Accessories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
