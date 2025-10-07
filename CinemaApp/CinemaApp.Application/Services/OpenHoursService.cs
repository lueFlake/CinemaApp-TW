using CinemaApp.API.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services
{
    public class OpenHoursService
    {
        private readonly OpenHoursSettings _settings;
        private readonly TimeZoneInfo _timeZone;

        public TimeSpan OpeningTime => _settings.OpeningTime;
        public TimeSpan ClosingTime => _settings.ClosingTime;
        public TimeZoneInfo TimeZone => _timeZone;

        public OpenHoursService(IOptions<CinemaSettings> cinemaSettings)
        {
            _settings = cinemaSettings.Value.OperatingHours;

            try
            {
                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_settings.TimeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                _timeZone = TimeZoneInfo.Utc;
            }
        }

        public bool IsWithinOpenHours(DateTime dateTime)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, _timeZone);
            var timeOfDay = localTime.TimeOfDay;

            return timeOfDay >= _settings.OpeningTime && timeOfDay <= _settings.ClosingTime;
        }
    }
}
