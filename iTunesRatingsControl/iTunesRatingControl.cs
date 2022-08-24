using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using iTunesControllerLib;
using iTunesLib;

// test
namespace iTunesRatingsControl {
    public sealed partial class iTunesRatingControl : Form {
        public iTunesRatingControl() {
            InitializeComponent();
            Opacity = 0.6;
            NewITunesAppClass();
            var rect = Screen.PrimaryScreen.Bounds;
            foreach (var screen in Screen.AllScreens) rect = Rectangle.Union(rect, screen.Bounds);
            _defaultLocation = Location = new Point(rect.Left, rect.Top - Height);
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            Bitmap bmp = new Bitmap(Width, Height);
            using (var gfx = Graphics.FromImage(bmp)) {
                gfx.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, Height);
            }
            BackgroundImage = bmp;
            FindItunes();
        }
        public iTunesRatingControl(string artfolder, string[] stringsToRemove, string hashFile, string statsfile, string tracklistfile) : this() {
            _artfolder = artfolder;
            _stringsToRemove = stringsToRemove;
            _hashes = HashCollection.Load(hashFile);
            _hashFile = hashFile;
            _statsfile = statsfile;
            _tracklistfile = tracklistfile;
            if (File.Exists(_statsfile)) {
                var lines = File.ReadAllLines(_statsfile);
                foreach (string line in lines) {
                    var vals = line.Split('=');
                    if (vals.Length != 2) continue;
                    switch (vals[0].Trim().ToLower()) {
                        case "1starcount":
                            _ratingCounters[0] = int.Parse(vals[1]);
                            break;
                        case "2starcount":
                            _ratingCounters[1] = int.Parse(vals[1]);
                            break;
                        case "3starcount":
                            _ratingCounters[2] = int.Parse(vals[1]);
                            break;
                        case "4starcount":
                            _ratingCounters[3] = int.Parse(vals[1]);
                            break;
                        case "5starcount":
                            _ratingCounters[4] = int.Parse(vals[1]);
                            break;
                    }
                }
            }
        }
        private void iTunesRatingControl_MouseEnter(object sender, EventArgs e) {
            _mousePointing = true;
            Opacity = 1;
            _mouseStars = 0;
            Invalidate();
        }
        private void iTunesRatingControl_MouseLeave(object sender, EventArgs e) {
            _mousePointing = false;
            Opacity = 0.6;
            Invalidate();
        }
        private void iTunesRatingControl_MouseMove(object sender, MouseEventArgs e) {
            int oldstars = _mouseStars;
            int w = Width / 5;
            if (e.Location.X < 5) _mouseStars = 0;
            else if (e.Location.X < w) _mouseStars = 1;
            else if (e.Location.X < w * 2) _mouseStars = 2;
            else if (e.Location.X < w * 3) _mouseStars = 3;
            else if (e.Location.X < w * 4) _mouseStars = 4;
            else _mouseStars = 5;
            if (_mouseStars != oldstars) Invalidate();
        }
        private void iTunesRatingControl_MouseUp(object sender, MouseEventArgs e) {
            if (!_mousePointing || e.Button != MouseButtons.Left) return;
            IITTrack track = null;
            try {
                track = _itunes.CurrentTrack as IITTrack;
            }
            catch {
                try {
                    NewITunesAppClass();
                }
                catch {
                    return;
                }
            }
            if (track != null) {
                track.Rating = _mouseStars * 20;
                _stars = _mouseStars;
                Invalidate();
            }
        }
        private void iTunesRatingControl_Shown(object sender, EventArgs e) {
            Location = _defaultLocation;
            timer.Start();
        }
        private void menuExitITunes_Click(object sender, EventArgs e) {
            _itunes.Quit();
            Close();
            Application.Exit();
        }
        private void menuExitITunesRatingControl_Click(object sender, EventArgs e) {
            Close();
            Application.Exit();
        }
        private void menuFindMissingTracks_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(_tracklistfile)) {
                MessageBox.Show("No track list file specified.");
                return;
            }
            try {
                var library = _itunes.LibraryPlaylist;
                var tracks = library.Tracks;
                new Thread(FindMissingTracksThread).Start(tracks);
            }
            catch {
                try {
                    NewITunesAppClass();
                }
                catch {
                    // do nothing.
                }
            }
        }
        private void timer_Tick(object sender, EventArgs e) {
            if ((DateTime.Now - _lastWindowRefresh).TotalSeconds > 5) {
                _lastWindowRefresh = DateTime.Now;
                new Thread(FindItunes).Start();
            }
            else {
                lock (_lockobj) {
                    var rect = new Rectangle();
                    GetWindowRect(_iTunesWinHandle, ref rect);
                    Location = rect.Location;
                    var nextwin = GetWindow(_iTunesWinHandle, GW_HWNDPREV);
                    SetWindowPos(Handle, nextwin.ToInt32(), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                }
            }
            if ((DateTime.Now - _lastTrackCheck).TotalSeconds > 1) {
                _lastTrackCheck = DateTime.Now;
                IITTrack track;
                int rating;
                try {
                    track = _itunes.CurrentTrack;
                    if (track != null) {
                        rating = track.Rating;
                        if (_lastTrackName == track.Name && _lastTrackAlbum == track.Album) return;
                        _lastTrackName = track.Name;
                        _lastTrackAlbum = track.Album;
                    }
                    else return;
                }
                catch {
                    try {
                        NewITunesAppClass();
                    }
                    catch {
                        // do nothing.
                    }
                    return;
                }
                try {
                    // if we get here, we know a track is playing and it's different than the last one we saw.
                    int stars = rating / 20;
                    if (stars > 0) {
                        ++_ratingCounters[stars - 1];
                        WriteStatsFile();
                    }
                    int newstars = (int)Math.Floor(rating / 20F);
                    if (newstars != _stars) {
                        _stars = newstars;
                        Invalidate();
                    }
                    if (_artfolder != null && track.Artwork.Count > 0) {
                        string artist = null;
                        try {
                            if (track is IITFileOrCDTrack) artist = ((IITFileOrCDTrack)track).AlbumArtist;
                        }
                        catch {
                            artist = null;
                        }
                        if (string.IsNullOrEmpty(artist)) artist = string.IsNullOrEmpty(track.Artist) ? "artist" : track.Artist;
                        string album = string.IsNullOrEmpty(track.Album) ? "album" : track.Album;
                        foreach (string s in _stringsToRemove) {
                            int i = album.IndexOf(s, StringComparison.OrdinalIgnoreCase);
                            if (i >= 0) album = album.Remove(i, s.Length).Trim();
                        }
                        if (track.Artwork.Count > 0) {
                            SaveArtworkFile(track.Artwork[1], artist + " - " + album);
                        }
                    }
                }
                catch (Exception ex) {
                    // do nothing.
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (_bmp == null) _bmp = new Bitmap(Width, Height);
            using (var gfx = Graphics.FromImage(_bmp)) {
                gfx.FillRectangle(_mousePointing ? new SolidBrush(Color.FromArgb(90, 90, 90)) : Brushes.Black, 0, 0, Width, Height);
                //gfx.DrawString("", Font, new SolidBrush(Color.FromArgb(100, 11, 11, 11)), new Point(0, 0));
                string stars = new string('', _stars);
                gfx.DrawString(stars, Font, new SolidBrush(Color.FromArgb(11, 11, 11)), new Point(0, 0));
                gfx.DrawString(stars, Font, new SolidBrush(Color.White), new Point(2, 2));
                if (_mousePointing) {
                    stars = new string('', _mouseStars);
                    gfx.DrawString(stars, Font, new SolidBrush(Color.Yellow), new Point(2, 2));
                }
                if (_trackListProcessing != null) {
                    gfx.DrawString(_trackListProcessing[0] + "/" + _trackListProcessing[1], new Font(FontFamily.GenericSansSerif, 20), Brushes.Red, new RectangleF(0, 0, Width, Height));
                }
            }
            e.Graphics.DrawImage(_bmp, 0, 0);
            //e.Graphics.DrawRectangle(Pens.Aqua, 0, 0, Width - 1, Height - 1);
            //e.Graphics.DrawString(lblStars.Text, lblStars.Font, new SolidBrush(Color.FromArgb(255 - _color.R, 255 - _color.G, 255 - _color.B)), lblStars.Location);
            //e.Graphics.DrawString(lblStars.Text, lblStars.Font, new SolidBrush(_color), new Point(lblStars.Left + 1, lblStars.Top + 1));
        }
        private void FindItunes() {
            lock (_lockobj) {
                try {
                    // Get the location and size of the iTunes window.
                    var processes = Process.GetProcessesByName("iTunes");
                    if (processes.Length == 0) {
                        this.AsyncInvokeIfRequired(() => Location = _defaultLocation);
                        _iTunesWinHandle = IntPtr.Zero;
                        return;
                    }
                    var process = processes[0];
                    var title = process.MainWindowTitle;
                    if (title.ToUpper() != "MINIPLAYER") {
                        this.AsyncInvokeIfRequired(() => Location = _defaultLocation);
                        _iTunesWinHandle = IntPtr.Zero;
                        return;
                    }
                    _iTunesWinHandle = process.MainWindowHandle;
                    var rect = new Rectangle();
                    GetWindowRect(_iTunesWinHandle, ref rect);
                    this.AsyncInvokeIfRequired(() => Location = rect.Location);
                    _lastWindowRefresh = DateTime.Now;
                }
                catch {
                    _iTunesWinHandle = IntPtr.Zero;
                }
            }
        }
        private void FindMissingTracksThread(object obj) {
            _trackListProcessing = new int[] { 0, 0 };
            var notFileKind = new List<IITTrack>();
            var noFile = new List<IITTrack>();
            var fileMissing = new List<IITTrack>();
            try {
                var tracks = (IITTrackCollection)obj;
                _trackListProcessing[1] = tracks.Count;
                for (int i = 0; i < tracks.Count; ++i) {
                    _trackListProcessing[0] = i + 1;
                    if (i % 10 == 0) Invalidate();
                    var track = tracks[i + 1];
                    string comment = track.Comment;
                    if (!string.IsNullOrEmpty(comment)) {
                        try {
                            if (comment.Contains("FLAG: NOT FILE KIND")) track.Comment = comment = comment.Replace("FLAG: NOT FILE KIND", "").Trim();
                            if (comment.Contains("FLAG: NO FILE")) track.Comment = comment = comment.Replace("FLAG: NO FILE", "").Trim();
                            if (comment.Contains("FLAG: FILE MISSING")) track.Comment = comment = comment.Replace("FLAG: FILE MISSING", "").Trim();
                        }
                        catch {
                            // don't do anything if I can't modify the comment.
                        }
                    }
                    else comment = "";
                    if (track.Kind != ITTrackKind.ITTrackKindFile) {
                        notFileKind.Add(track);
                        try {
                            track.Comment = "FLAG: NOT FILE KIND " + comment;
                        }
                        catch {
                            // don't do anything if I can't modify the comment.
                        }
                        continue;
                    }
                    var filetrack = (IITFileOrCDTrack)track;
                    if (string.IsNullOrEmpty(filetrack.Location)) {
                        noFile.Add(track);
                        try {
                            track.Comment = "FLAG: NO FILE " + comment;
                        }
                        catch {
                            // don't do anything if I can't modify the comment.
                        }
                    }
                    else if (!File.Exists(filetrack.Location)) {
                        fileMissing.Add(track);
                        try {
                            track.Comment = "FLAG: FILE MISSING " + comment;
                        }
                        catch {
                            // don't do anything if I can't modify the comment.
                        }
                    }
                    //var path = track.Location;
                }
                var sb = new StringBuilder();
                sb.Append("Not file kind: " + notFileKind.Count + Environment.NewLine);
                foreach (var track in notFileKind) {
                    sb.Append($"\"{track.Artist}\",\"{track.Album}\",\"{track.Name}\"{Environment.NewLine}");
                }
                sb.Append(Environment.NewLine + "File path empty: " + noFile.Count + Environment.NewLine);
                foreach (var track in noFile) {
                    sb.Append($"\"{track.Artist}\",\"{track.Album}\",\"{track.Name}\"{Environment.NewLine}");
                }
                sb.Append(Environment.NewLine + "File missing: " + fileMissing.Count + Environment.NewLine);
                foreach (var track in fileMissing) {
                    sb.Append($"\"{track.Artist}\",\"{track.Album}\",\"{track.Name}\"{Environment.NewLine}");
                }
                File.WriteAllText(_tracklistfile, sb.ToString());
                MessageBox.Show("Track list written to: " + Environment.NewLine + _tracklistfile);
            }
            catch (Exception ex) {
                MessageBox.Show("Exception thrown: " + ex.Message);
            }
            finally {
                _trackListProcessing = null;
            }
        }
        private void NewITunesAppClass() {
            _itunes = new iTunesAppClass();
            _itunes.OnQuittingEvent += () => {
                                           Close();
                                           Application.Exit();
                                       };
            _itunes.OnAboutToPromptUserToQuitEvent += () => {
                                                          Close();
                                                          Application.Exit();
                                                      };
        }
        private void SaveArtworkFile(IITArtwork art, string name) {
            try {
                string ext;
                switch (art.Format) {
                    case ITArtworkFormat.ITArtworkFormatJPEG:
                        ext = "jpg";
                        break;
                    case ITArtworkFormat.ITArtworkFormatPNG:
                        ext = "png";
                        break;
                    case ITArtworkFormat.ITArtworkFormatBMP:
                        ext = "bmp";
                        break;
                    default: return;
                }
                name = name.NormalizeFilename();
                string filepath = Path.Combine(_artfolder, name + ".");
                // don't write a jpg if there's already a png, no matter how different they look to CompareImages.
                if (!File.Exists(filepath + "jpg") && !File.Exists(filepath + "png") && !File.Exists(filepath + "bmp")) {
                    art.SaveArtworkToFile(filepath + ext);
                    int[] hash;
                    using (Bitmap bmp = new Bitmap(filepath + ext)) {
                        hash = bmp.GetHash(10);
                    }
                    bool found = false;
                    foreach (var h in _hashes) {
                        if (HashCollection.CompareHashes(hash, h.Hash) >= 97) {
                            found = true;
                            File.Delete(filepath + ext);
                        }
                    }
                    if (!found) {
                        _hashes.Add(new HashEntry { Filename = filepath + ext, Hash = hash });
                        _hashes.Write(_hashFile);
                    }
                }
                else {
                    string fname = File.Exists(filepath + "jpg") ? filepath + "jpg" : File.Exists(filepath + "png") ? filepath + "png" : filepath + "bmp";
                    int[] hash;
                    using (Bitmap bmp = new Bitmap(fname)) {
                        hash = bmp.GetHash(10);
                    }
                    if (!_hashes.Any(h => HashCollection.CompareHashes(h.Hash, hash) >= 97)) {
                        _hashes.Add(new HashEntry { Filename = fname, Hash = hash });
                        _hashes.Write(_hashFile);
                    }
                }
            }
            catch (Exception ex) {
                if (!ex.Message.Contains("track has been deleted")) MessageBox.Show("Exception in iTunesRatingControl: " + ex.Message);
            }
        }
        private void WriteStatsFile() {
            if (string.IsNullOrEmpty(_statsfile)) return;
            string text = $"1StarCount = {_ratingCounters[0]}{Environment.NewLine}" +
                          $"2StarCount = {_ratingCounters[1]}{Environment.NewLine}" +
                          $"3StarCount = {_ratingCounters[2]}{Environment.NewLine}" +
                          $"4StarCount = {_ratingCounters[3]}{Environment.NewLine}" +
                          $"5StarCount = {_ratingCounters[4]}{Environment.NewLine}";
            float total = _ratingCounters.Sum();
            text += $"1StarPct = {_ratingCounters[0] / total * 100}{Environment.NewLine}" +
                    $"2StarPct = {_ratingCounters[1] / total * 100}{Environment.NewLine}" +
                    $"3StarPct = {_ratingCounters[2] / total * 100}{Environment.NewLine}" +
                    $"4StarPct = {_ratingCounters[3] / total * 100}{Environment.NewLine}" +
                    $"5StarPct = {_ratingCounters[4] / total * 100}{Environment.NewLine}";
            File.WriteAllText(_statsfile, text);
        }
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")] private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        private readonly string _artfolder;
        private Bitmap _bmp;
        //private Color _color = Color.Black;
        private readonly Point _defaultLocation;
        private readonly HashCollection _hashes;
        private readonly string _hashFile;
        private iTunesAppClass _itunes;
        private IntPtr _iTunesWinHandle = IntPtr.Zero;
        private string _lastTrackAlbum;
        private DateTime _lastTrackCheck = DateTime.Now.AddDays(-1);
        private string _lastTrackName;
        private DateTime _lastWindowRefresh;
        private readonly object _lockobj = new object();
        private bool _mousePointing;
        private int _mouseStars = -1;
        private readonly int[] _ratingCounters = new int[5];
        //private readonly Random _rand = new Random();
        //private int _rgbChangeDir = 2;
        //private int _rgbChanging = 0;
        private int _stars;
        private readonly string _statsfile;
        /// <summary>
        ///     Strings to remove from album names when createing album art file.  For example (Deluxe version), etc.  Not case
        ///     sensitive.
        /// </summary>
        private readonly string[] _stringsToRemove;
        private readonly string _tracklistfile;
        private int[] _trackListProcessing;
        private const uint GW_HWNDNEXT = 2;
        private const uint GW_HWNDPREV = 3;
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
    }
}