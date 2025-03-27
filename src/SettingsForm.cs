/* Originally based on project by Frank McCown in 2010 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ScreenSaver.Properties;

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        private readonly FlipItSettings _settings;
        private readonly List<Location> _availableCities = new List<Location>();
        private readonly AutoCompleteStringCollection _cityAutoCompleteSource = new AutoCompleteStringCollection();

        public SettingsForm(FlipItSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            versionLabel.Text = $"Version {GetVersion()}";
        }

        private string GetVersion()
        {
            var version = typeof(SettingsForm).Assembly.GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private void SaveSettings()
        {
            _settings.Scale = scaleTrackBar.Value * 10;
            _settings.Save();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            display24hrRadioButton.Checked = _settings.Display24HrTime;
            display12hrRadioButton.Checked = !_settings.Display24HrTime;
            showDstIndicatorCheckBox.Checked = _settings.ShowDstIndicator;
            scaleTrackBar.Value = _settings.Scale / 10;

            screensListBox.Items.Clear();
            foreach (var screen in _settings.ScreenSettings)
            {
                screensListBox.Items.Add(screen);
            }
            screensListBox.SelectedIndex = 0;
        }

        private void DisplayScreenDetails()
        {
            var screenSettings = GetCurrentScreenSettings();
            selectedScreenNameLabel.Text = screenSettings.Description;

            switch (screenSettings.DisplayType)
            {
                case DisplayType.None:
                    displayNothingRadioButton.Checked = true;
                    break;
                case DisplayType.CurrentTime:
                    displayCurrentTimeRadioButton.Checked = true;
                    break;
                case DisplayType.WorldTime:
                    displayWorldTimesRadioButton.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            worldTimesListView.Items.Clear();
            foreach (var location in screenSettings.Locations)
            {
                AddLocationToListView(location);
            }
            CheckWorldTimesControls();
        }

        private ScreenSetting GetCurrentScreenSettings()
        {
            return (ScreenSetting)screensListBox.SelectedItem;
        }

        private void CheckWorldTimesControls()
        {
            var allowLocationEditing = displayWorldTimesRadioButton.Checked;
            worldTimesListView.Enabled
