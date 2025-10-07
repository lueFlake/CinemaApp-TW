namespace CinemaApp.API.Settings
{
    public class CinemaSettings
    {
        public static string SectionName = "CinemaSettings";

        public OpenHoursSettings OperatingHours { get; set; } = new();
    }
}
