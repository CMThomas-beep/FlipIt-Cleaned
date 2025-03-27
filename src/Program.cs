using System;
using System.Windows.Forms;

namespace FlipIt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            bool previewMode = false;
            if (args.Length > 0 && args[0].ToLower() == "/s")
            {
                previewMode = true;
            }
            
            Application.Run(new FlipIt(previewMode));
        }
    }
}
