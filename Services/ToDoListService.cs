using CSToDoList.Data;
using CSToDoList.Models;
using CSToDoList.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSToDoList.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly ApplicationDbContext _context;

        // constructor is the same name as the class
        public ToDoListService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddToDoToAccessAsync(IEnumerable<int> accessoryIds, int toDoItemId)
        {
            try
            {
                ToDoItem? toDoItem = await _context.ToDoItem
                                                   .Include(c => c.Accessories) // Eager Load
                                                   .FirstOrDefaultAsync(c => c.Id == toDoItemId);

                foreach (int accesoryId in accessoryIds)
                {
                    Accessory? accessory = await _context.Accessories.FindAsync(accesoryId);

                    if (toDoItem != null && accessory != null)
                    {
                        // Can use add because we're working with objects
                        toDoItem.Accessories.Add(accessory);
                    }
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveAllToDoAccessoriesAsync(int toDoItemId)
        {
            try
            {

                ToDoItem? toDoItem = await _context.ToDoItem
                                                 .Include(c => c.Accessories)
                                                 .FirstOrDefaultAsync(c => c.Id == toDoItemId);


                toDoItem!.Accessories.Clear();
                //
                _context.Update(toDoItem);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    

        public async Task<bool> IsToDoInAccess(int accessoryId, int toDoItemId)
        {
            try
            {
                ToDoItem? toDoItem = await _context.ToDoItem
                                                 .Include(c => c.Accessories) // Eager Load
                                                 .FirstOrDefaultAsync(c => c.Id == toDoItemId);

                bool isInCategory = toDoItem!.Accessories.Select(c => c.Id).Contains(accessoryId);

                return isInCategory;

            }
            catch (Exception)
            {

                throw;
            }
        }
    

        public Task<IEnumerable<Accessory>> GetAppUserCategoriesAsync(string appUserId)
        {
            throw new NotImplementedException();
        }


        public Task RemoveAllCategoryContactsAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task AddCategoryToContactsAsync(IEnumerable<int> contacts, int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task AddContactToCategoryAsync(int accessoryId, int toDoItemId)
        {
            throw new NotImplementedException();
        }
    }
}
