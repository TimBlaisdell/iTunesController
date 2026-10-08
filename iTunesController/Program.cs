using System.IO;
using iTunesRatingsControl;

namespace iTunesController {
    internal static class Program {
        /// <summary>
        ///     The main entry point for the application.
        ///     Optional arguments: /statsfile=[path] or /config=[settings file with a "statsfile = [path]" line],
        ///     and /log=[path] to log what the overlay does with Apple Music, for troubleshooting.
        /// </summary>
        [STAThread] static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string statsfile = string.Empty;
            foreach (var arg in Environment.GetCommandLineArgs()) {
                if (arg.StartsWith("/statsfile=")) statsfile = arg.Substring(11).Trim();
                else if (arg.StartsWith("/log=")) AppleMusic.LogFile = arg.Substring(5).Trim();
                else if (arg.StartsWith("/config=") && File.Exists(arg.Substring(8).Trim())) {
                    var lines = File.ReadAllLines(arg.Substring(8).Trim());
                    var line = lines.FirstOrDefault(l => l.Trim().ToLower().StartsWith("statsfile"));
                    if (line != null) {
                        var fields = line.Split('=');
                        if (fields.Length > 1) statsfile = fields[1].Trim();
                    }
                }
            }
            Application.Run(new iTunesRatingControl(statsfile));
        }
    }
}
