using System.ComponentModel.DataAnnotations;

namespace CSToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "To Do Item")]
        public string? Name { get; set; }


        [Required]
        public string? AppUserId { get; set; }


        [Required]
        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }


        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }


        [Required]
        public bool Completed { get; set; }


        // Navigation Properties
        public virtual AppUser? AppUser { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; } = new HashSet<Accessory>();
    
    }
}
