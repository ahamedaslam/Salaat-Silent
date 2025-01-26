using Microsoft.AspNetCore.Mvc;
using NamazSchedulerApp.API.Context;
using NamazSchedulerApp.API.Models;
using Newtonsoft.Json;

namespace NamazSchedulerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrayerTimesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PrayerTimesController> _logger;

        public PrayerTimesController(AppDbContext context, ILogger<PrayerTimesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrayerTimes([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] string method = "2")
        {
            if (latitude == 0 || longitude == 0)
                return BadRequest("Latitude and Longitude are required.");

            var apiUrl = $"http://api.aladhan.com/v1/timings?latitude={latitude}&longitude={longitude}&method={method}";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Failed to fetch prayer times. StatusCode: {response.StatusCode}");
                        return StatusCode((int)response.StatusCode, "Failed to fetch prayer times.");
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var prayerTimesResponse = JsonConvert.DeserializeObject<PrayerTimesResponse>(jsonResponse);

                    // Null checks for deserialized data
                    if (prayerTimesResponse == null)
                    {
                        _logger.LogError("Failed to deserialize the prayer times response.");
                        return StatusCode(500, "Error processing the API response.");
                    }

                    if (prayerTimesResponse.Location == null || prayerTimesResponse.PrayerTimes == null)
                    {
                        _logger.LogError("Incomplete data received from the external API.");
                        return StatusCode(500, "Incomplete data received from the external API.");
                    }

                    // Save data to database
                    var location = new Location
                    {
                        Latitude = prayerTimesResponse.Location.Latitude,
                        Longitude = prayerTimesResponse.Location.Longitude,
                        Timezone = prayerTimesResponse.Location.Timezone
                    };

                    var prayerTimes = new PrayerTimes
                    {
                        Fajr = prayerTimesResponse.PrayerTimes.Fajr,
                        Dhuhr = prayerTimesResponse.PrayerTimes.Dhuhr,
                        Asr = prayerTimesResponse.PrayerTimes.Asr,
                        Maghrib = prayerTimesResponse.PrayerTimes.Maghrib,
                        Isha = prayerTimesResponse.PrayerTimes.Isha
                    };

                    _context.Locations.Add(location);
                    _context.Prayers.Add(prayerTimes);
                    await _context.SaveChangesAsync();

                    return Ok(prayerTimesResponse);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the external API.");
                return StatusCode(500, "Error while fetching prayer times from the external API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
