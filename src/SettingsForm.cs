using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace FlipIt
{
    public partial class SettingsForm : Form
    {
        private FlipItSettings settings;
        private List<TimeZoneInfo> timeZones;

        public SettingsForm(FlipItSettings currentSettings)
        {
            InitializeComponent();
            settings = new FlipItSettings
            {
                Use24HourFormat = currentSettings.Use24HourFormat,
                TimeZoneId = currentSettings.TimeZoneId,
                ShowCityName = currentSettings.ShowCityName
            };
            
            LoadTimeZones();
            PopulateControls();
        }

        private void LoadTimeZones()
        {
            try
            {
                timeZones = TimeZoneInfo.GetSystemTimeZones().OrderBy(tz => tz.DisplayName).ToList();
            }
            catch (Exception)
            {
                timeZones = new List<TimeZoneInfo>();
            }
        }

        private void PopulateControls()
        {
            use24HourFormatCheckBox.Checked = settings.Use24HourFormat;
            showCityNameCheckBox.Checked = settings.ShowCityName;
            
            timeZoneComboBox.Items.Clear();
            timeZoneComboBox.Items.Add("(Local Time)");
            
            foreach (var tz in timeZones)
            {
                timeZoneComboBox.Items.Add(tz.DisplayName);
            }
            
            // Select current time zone
            if (string.IsNullOrEmpty(settings.TimeZoneId))
            {
                timeZoneComboBox.SelectedIndex = 0;
            }
            else
            {
                var index = timeZones.FindIndex(tz => tz.Id == settings.TimeZoneId);
                timeZoneComboBox.SelectedIndex = index >= 0 ? index + 1 : 0;
            }
        }

        public FlipItSettings GetSettings()
        {
            return settings;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            settings.Use24HourFormat = use24HourFormatCheckBox.Checked;
            settings.ShowCityName = showCityNameCheckBox.Checked;
            
            // Get selected time zone
            if (timeZoneComboBox.SelectedIndex == 0)
            {
                settings.TimeZoneId = string.Empty;
            }
            else if (timeZoneComboBox.SelectedIndex > 0)
            {
                int index = timeZoneComboBox.SelectedIndex - 1;
                if (index >= 0 && index < timeZones.Count)
                {
                    settings.TimeZoneId = timeZones[index].Id;
                }
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
