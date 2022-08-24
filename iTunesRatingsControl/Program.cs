using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string hashfile = null;
            string statsfile = null;
            string tracklistfile = null;
            string[] linesToRemove = new string[0];
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args) {
                if (arg.StartsWith("/artfolder=")) artfolder = arg.Substring(11).Trim();
                else if (arg.StartsWith("/config=") && File.Exists(arg.Substring(8).Trim())) {
                    var lines = File.ReadAllLines(arg.Substring(8).Trim());
                    var line = lines.FirstOrDefault(l => l.Trim().ToLower().StartsWith("artfolder"));
                    if (line != null) {
                        var fields = line.Split('=');
                        if (fields.Length > 1) artfolder = fields[1].Trim();
                    }
                    line = lines.FirstOrDefault(l => l.Trim().ToLower().StartsWith("hashfile"));
                    if (line != null) {
                        var fields = line.Split('=');
                        if (fields.Length > 1) hashfile = fields[1].Trim();
                    }
                    line = lines.FirstOrDefault(l => l.Trim().ToLower().StartsWith("statsfile"));
                    if (line != null) {
                        var fields = line.Split('=');
                        if (fields.Length > 1) statsfile = fields[1].Trim();
                    }
                    line = lines.FirstOrDefault(l => l.Trim().ToLower().StartsWith("tracklistfile"));
                    if (line != null) {
                        var fields = line.Split('=');
                        if (fields.Length > 1) tracklistfile = fields[1].Trim();
                    }
                    int i = Array.FindIndex(lines, l => l.ToLower().Trim().StartsWith("stringstoremove"));
                    if (i >= 0) {
                        var list = new List<string>();
                        ++i;
                        while (i < lines.Length) {
                            line = lines[i].Trim();
                            if (line.ToLower() == "end") break;
                            list.Add(line);
                            ++i;
                        }
                        linesToRemove = list.ToArray();
                    }
                }
            }
            if (artfolder != null && !Directory.Exists(artfolder)) {
                Directory.CreateDirectory(artfolder);
            }
            Application.Run(new iTunesRatingControl(artfolder, linesToRemove, hashfile ?? "hashfile.txt", statsfile, tracklistfile));
        }
    }
}