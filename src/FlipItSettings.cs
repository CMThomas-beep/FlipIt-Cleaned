using System;
using System.Configuration;

namespace FlipIt
{
    public class FlipItSettings
    {
        private const string USE_24_HOUR_FORMAT_KEY = "Use24HourFormat";
        private const string TIME_ZONE_ID_KEY = "TimeZoneId";
        private const string SHOW_CITY_NAME_KEY = "ShowCityName";

        public bool Use24HourFormat { get; set; }
        public string TimeZoneId { get; set; }
        public bool ShowCityName { get; set; }

        public FlipItSettings()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                
                Use24HourFormat = GetBoolSetting(appSettings[USE_24_HOUR_FORMAT_KEY], false);
                TimeZoneId = appSettings[TIME_ZONE_ID_KEY] ?? string.Empty;
                ShowCityName = GetBoolSetting(appSettings[SHOW_CITY_NAME_KEY], true);
            }
            catch (Exception)
            {
                // Use default settings if loading fails
                Use24HourFormat = false;
                TimeZoneId = string.Empty;
                ShowCityName = true;
            }
        }

        public void Save()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;
                
                UpdateOrAddSetting(settings, USE_24_HOUR_FORMAT_KEY, Use24HourFormat.ToString());
                UpdateOrAddSetting(settings, TIME_ZONE_ID_KEY, TimeZoneId);
                UpdateOrAddSetting(settings, SHOW_CITY_NAME_KEY, ShowCityName.ToString());
                
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception)
            {
                // Log error or show message if saving fails
            }
        }

        private bool GetBoolSetting(string value, bool defaultValue)
        {
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        private void UpdateOrAddSetting(KeyValueConfigurationCollection settings, string key, string value)
        {
            if (settings[key] != null)
            {
                settings[key].Value = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }
    }
}
