namespace ScreenSaver.Properties {
    internal sealed partial class Settings : System.Configuration.ApplicationSettingsBase {
        private static Settings defaultInstance = 
            (Settings)System.Configuration.ApplicationSettingsBase.Synchronized(new Settings());

        public static Settings Default {
            get { return defaultInstance; }
        }
    }
}
