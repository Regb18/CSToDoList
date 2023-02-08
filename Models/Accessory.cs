using System.ComponentModel.DataAnnotations;

namespace CSToDoList.Models
{
    public class Accessory
    {
        public int Id { get; set; }


        [Required]
        public string? AppUserId { get; set; }


        [Required]
        [Display(Name = "Accessory")]
        public string? Name { get; set; }



        // Navigation Properties
        public virtual AppUser? AppUser { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new HashSet<ToDoItem>();
    }
}
