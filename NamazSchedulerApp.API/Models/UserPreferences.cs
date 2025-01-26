using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NamazSchedulerApp.API.Models
{
    public class UserPreferences
    {
        [Key]
        public Guid PreferenceId { get; set; }

        [Required]
        [ForeignKey(nameof(User))] // Explicitly specify the foreign key

        public Guid UserId { get; set; }

        public string Method { get; set; } = "MWL";  // Default: MWL
        public bool SilentMode { get; set; } = true;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
