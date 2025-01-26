namespace NamazSchedulerApp.API.Models
{
    public class PrayerTimesResponse
    {
        public string Date { get; set; }
        public Location Location { get; set; }
        public string Method { get; set; }
        public PrayerTimes PrayerTimes { get; set; }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
    }

    public class PrayerTimes
    {
        public string Fajr { get; set; }
        public string Dhuhr { get; set; }
        public string Asr { get; set; }
        public string Maghrib { get; set; }
        public string Isha { get; set; }
    }
}
