//----------------------------------------------------------------namespace ScreenSaver.Properties {
    internal class Resources {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;

        internal Resources() { }

        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (resourceMan == null) {
                    resourceMan = new System.Resources.ResourceManager("ScreenSaver.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        internal static System.Globalization.CultureInfo Culture {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

        internal static byte[] HelveticaLTStd_BoldCond {
            get { return (byte[])ResourceManager.GetObject("HelveticaLTStd_BoldCond", resourceCulture); }
        }

        internal static byte[] HelveticaLTStd_Cond {
            get { return (byte[])ResourceManager.GetObject("HelveticaLTStd_Cond", resourceCulture); }
        }

        internal static System.Drawing.Bitmap screen {
            get { return (System.Drawing.Bitmap)ResourceManager.GetObject("screen", resourceCulture); }
        }

        internal static string TimeZoneCities {
            get { return ResourceManager.GetString("TimeZoneCities", resourceCulture); }
        }
    }
}

