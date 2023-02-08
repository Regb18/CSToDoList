using CSToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace CSToDoList.Services.Interfaces
{
    public interface IToDoListService
    {
        // no return on these two Tasks
        public Task AddContactToCategoryAsync(int accessoryId, int toDoItemId);
        public Task AddToDoToAccessAsync(IEnumerable<int> accessoryIds, int toDoItemId);

        // Returns the IEnumerable<Category>, appUserId is a string because that's what microsoft makes for IdentityUser
        public Task<IEnumerable<Accessory>> GetAppUserCategoriesAsync(string appUserId);

        public Task<bool> IsToDoInAccess(int accessoryId, int toDoItemId);

        public Task RemoveAllToDoAccessoriesAsync(int toDoItemId);

        // Category Stuff - Testing
        public Task AddCategoryToContactsAsync(IEnumerable<int> contacts, int categoryId);

        public Task RemoveAllCategoryContactsAsync(int categoryId);
    }
}
