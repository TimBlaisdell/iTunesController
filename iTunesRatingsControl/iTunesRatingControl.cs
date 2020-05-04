using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesRatingsControl {
    public sealed partial class iTunesRatingControl : Form {
        public iTunesRatingControl(string artfolder) : this() {
            _artfolder = artfolder;
        }
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
            if ((DateTime.Now - _lastTrackCheck).TotalSeconds > 2) {
                _lastTrackCheck = DateTime.Now;
                IITTrack track;
                int rating = -1;
                try {
                    track = _itunes.CurrentTrack;
                    if (track != null) rating = track.Rating;
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
                    if (track != null && rating >= 0) {
                        int newstars = (int) Math.Floor(rating / 20F);
                        if (newstars != _stars) {
                            _stars = newstars;
                            Invalidate();
                        }
                        if (_artfolder != null && track.Artwork.Count > 0) {
                            string artist = null;
                            try {
                                if (track is IITFileOrCDTrack) artist = ((IITFileOrCDTrack) track).AlbumArtist;
                            }
                            catch {
                                artist = null;
                            }
                            if (string.IsNullOrEmpty(artist)) artist = string.IsNullOrEmpty(track.Artist) ? "artist" : track.Artist;
                            string album = string.IsNullOrEmpty(track.Album) ? "album" : track.Album;
                            if (track.Artwork.Count == 1) {
                                SaveArtworkFile(track.Artwork[1], artist + " - " + album);
                            }
                            else {
                                for (int i = 1; i <= track.Artwork.Count; ++i) {
                                    SaveArtworkFile(track.Artwork[i], artist + " - " + album + "." + i);
                                }
                            }
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
                name = name.Replace(':', '_').Replace('?', '_').Replace('\\', '_').Replace('/', '_').Replace('"', '_').Replace('*', '_').Replace('<', '_').Replace('>', '_');
                string filepath = Path.Combine(_artfolder, name + ".");
                if (!File.Exists(filepath + "jpg") && !File.Exists(filepath + "png") && !File.Exists(filepath + "bmp")) art.SaveArtworkToFile(filepath + ext);
            }
            catch {
                // do nothing.
            }
        }
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")] private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        private string _artfolder;
        private Bitmap _bmp;
        //private Color _color = Color.Black;
        private readonly Point _defaultLocation;
        private iTunesAppClass _itunes;
        private IntPtr _iTunesWinHandle = IntPtr.Zero;
        private DateTime _lastTrackCheck = DateTime.Now.AddDays(-1);
        private DateTime _lastWindowRefresh;
        private readonly object _lockobj = new object();
        private bool _mousePointing;
        private int _mouseStars = -1;
        //private readonly Random _rand = new Random();
        //private int _rgbChangeDir = 2;
        //private int _rgbChanging = 0;
        private int _stars;
        private const uint GW_HWNDNEXT = 2;
        private const uint GW_HWNDPREV = 3;
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
    }
}