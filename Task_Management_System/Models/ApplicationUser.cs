using System.ComponentModel.DataAnnotations;

namespace Task_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string fullName { get; set; }
    }
}
