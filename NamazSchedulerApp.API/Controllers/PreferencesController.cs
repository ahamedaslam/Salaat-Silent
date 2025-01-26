using Microsoft.AspNetCore.Mvc;
using NamazSchedulerApp.API.Context;
using NamazSchedulerApp.API.Models;

namespace NamazSchedulerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PreferencesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetPreferences(Guid userId)
        {
            var preferences = _context.Preferences.FirstOrDefault(p => p.UserId == userId);
            if (preferences == null)
                return NotFound("Preferences not found for the user.");

            return Ok(preferences);
        }

        [HttpPost]
        public async Task<IActionResult> SavePreferences([FromBody] UserPreferences preferences)
        {
            if (preferences == null)
                return BadRequest("Invalid data.");

            var existing = _context.Preferences.FirstOrDefault(p => p.UserId == preferences.UserId);

            if (existing != null)
            {
                existing.Method = preferences.Method;
                existing.SilentMode = preferences.SilentMode;
                existing.Latitude = preferences.Latitude;
                existing.Longitude = preferences.Longitude;
                _context.Preferences.Update(existing);
            }
            else
            {
                preferences.PreferenceId = Guid.NewGuid();
                _context.Preferences.Add(preferences);
            }

            await _context.SaveChangesAsync();
            return Ok("Preferences saved successfully.");
        }
    }
}
