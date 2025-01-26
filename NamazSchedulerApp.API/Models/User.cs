using System.ComponentModel.DataAnnotations;

namespace NamazSchedulerApp.API.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
