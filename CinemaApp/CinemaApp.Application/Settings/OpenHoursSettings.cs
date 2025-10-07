namespace CinemaApp.API.Settings
{
    public class OpenHoursSettings
    {
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string TimeZone { get; set; } = "UTC";
    }
}
