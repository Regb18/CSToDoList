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
using CSToDoList.Services;
using CSToDoList.Services.Interfaces;

namespace CSToDoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToDoListService _toDoListService;

        public ToDoItemsController(ApplicationDbContext context, UserManager<AppUser> userManager, IToDoListService toDoListService)
        {
            _context = context;
            _userManager = userManager;
            _toDoListService = toDoListService;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User)!;

            List<ToDoItem> toDoItems = new List<ToDoItem>();

            toDoItems = await _context.ToDoItem
                         .Where(c => c.AppUserId == userId)
                         .Include(c => c.Accessories)
                         .ToListAsync();

            return View(toDoItems);
        }

        // GET: ToDoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            string userId = _userManager.GetUserId(User)!;
            
            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItem
                .Where(c => c.AppUserId == userId)
                .Include(t => t.Accessories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public async Task<IActionResult> Create()
        {
            string? userId = _userManager.GetUserId(User);
            IEnumerable<Accessory> accessoryList = await _context.Accessories
                                                     .Where(c => c.AppUserId == userId)
                                                     .ToListAsync();


            ViewData["AccessoryList"] = new MultiSelectList(accessoryList, "Id", "Name");
            return View();
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AppUserId,DateCreated,DueDate,Completed")] ToDoItem toDoItem, IEnumerable<int> selected)
        {
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {
                toDoItem.AppUserId = _userManager.GetUserId(User);
                toDoItem.DateCreated = DateTime.UtcNow;

                if (toDoItem.DueDate != null)
                {
                    toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate.Value, DateTimeKind.Utc);
                }

                _context.Add(toDoItem);
                await _context.SaveChangesAsync();

                // TODO: Add Service call
                await _toDoListService.AddToDoToAccessAsync(selected, toDoItem.Id);


                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", toDoItem.AppUserId);
            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            string? userId = _userManager.GetUserId(User);

            var toDoItem = await _context.ToDoItem
                                         .Include(c => c.Accessories)
                                         .FirstOrDefaultAsync(c => c.Id ==id);

            IEnumerable<Accessory> accessoriesList = await _context.Accessories.Where(c => c.AppUserId == userId).ToListAsync();

            IEnumerable<int> currentAccessories = toDoItem!.Accessories.Select(c => c.Id);

            ViewData["AccessoryList"] = new MultiSelectList(accessoriesList, "Id", "Name", currentAccessories);

            if (toDoItem == null)
            {
                return NotFound();
            }
           
            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AppUserId,DateCreated,DueDate,Completed")] ToDoItem toDoItem, IEnumerable<int> selected)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDoItem.DateCreated = DateTime.SpecifyKind(toDoItem.DateCreated, DateTimeKind.Utc);

                    if (toDoItem.DueDate != null)
                    {
                        toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate.Value, DateTimeKind.Utc);
                    }

                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();

                    if (selected != null)
                    {
                        await _toDoListService.RemoveAllToDoAccessoriesAsync(toDoItem.Id);

                        await _toDoListService.AddToDoToAccessAsync(selected, toDoItem.Id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
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
           
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string userId = _userManager.GetUserId(User)!;

            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItem
                .Where(c=>c.AppUserId == userId)
                .Include(t => t.Accessories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToDoItem'  is null.");
            }
            var toDoItem = await _context.ToDoItem.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItem.Remove(toDoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
          return (_context.ToDoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
