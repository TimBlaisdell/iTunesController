using System;
using System.IO;
using System.Windows.Forms;

namespace iTunesRatingsControl {
    static class Program {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread] static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string artfolder = null;
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args) {
                if (arg.StartsWith("/artfolder=")) artfolder = arg.Substring(11).Trim();
            }
            if (artfolder != null && !Directory.Exists(artfolder)) {
                Directory.CreateDirectory(artfolder);
            }
            Application.Run(new iTunesRatingControl(artfolder));
        }
    }
}