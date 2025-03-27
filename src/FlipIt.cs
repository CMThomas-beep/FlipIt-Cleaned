using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlipIt
{
    public partial class FlipIt : Form
    {
        private bool previewMode = false;
        private DateTime currentTime;
        private FlipItSettings settings;
        private Dictionary<string, string> timeZoneCities = new Dictionary<string, string>();

        public FlipIt(bool preview)
        {
            InitializeComponent();
            previewMode = preview;
            settings = new FlipItSettings();
            LoadTimeZoneCities();
            UpdateTimeDisplay();
        }

        private void LoadTimeZoneCities()
        {
            // Load time zone cities from resource file
            var citiesResource = Properties.Resources.TimeZoneCities;
            var lines = citiesResource.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
            {
                if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line)) continue;
                
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    timeZoneCities[parts[0]] = parts[1];
                }
            }
        }

        private void UpdateTimeDisplay()
        {
            currentTime = DateTime.Now;
            
            if (!string.IsNullOrEmpty(settings.TimeZoneId))
            {
                try
                {
                    var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(settings.TimeZoneId);
                    currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                }
                catch (Exception)
                {
                    // Fallback to local time if time zone conversion fails
                    currentTime = DateTime.Now;
                }
            }
            
            DisplayTime();
        }

        private void DisplayTime()
        {
            timeLabel.Text = currentTime.ToString(settings.Use24HourFormat ? "HH:mm" : "hh:mm");
            secondsLabel.Text = currentTime.ToString("ss");
            ampmLabel.Visible = !settings.Use24HourFormat;
            ampmLabel.Text = currentTime.ToString("tt");
            cityLabel.Visible = settings.ShowCityName;
            
            if (settings.ShowCityName && !string.IsNullOrEmpty(settings.TimeZoneId))
            {
                cityLabel.Text = GetCityName(settings.TimeZoneId);
            }
        }

        private string GetCityName(string timeZoneId)
        {
            if (timeZoneCities.TryGetValue(timeZoneId, out string cities))
            {
                var cityList = cities.Split(',');
                return cityList[0];
            }
            return timeZoneId;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void FlipIt_MouseMove(object sender, MouseEventArgs e)
        {
            if (previewMode) return;
            
            // Show cursor on mouse move
            Cursor.Show();
            
            // Reset cursor timer
            cursorTimer.Stop();
            cursorTimer.Start();
        }

        private void cursorTimer_Tick(object sender, EventArgs e)
        {
            // Hide cursor after timer elapses
            Cursor.Hide();
            cursorTimer.Stop();
        }

        private void FlipIt_KeyDown(object sender, KeyEventArgs e)
        {
            // Exit on Escape key
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
            
            // Toggle 24-hour format on F1
            if (e.KeyCode == Keys.F1)
            {
                settings.Use24HourFormat = !settings.Use24HourFormat;
                settings.Save();
                UpdateTimeDisplay();
            }
            
            // Toggle city display on F2
            if (e.KeyCode == Keys.F2)
            {
                settings.ShowCityName = !settings.ShowCityName;
                settings.Save();
                UpdateTimeDisplay();
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    settings = settingsForm.GetSettings();
                    settings.Save();
                    UpdateTimeDisplay();
                }
            }
        }
    }
}
