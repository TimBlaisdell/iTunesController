using System.IO;
using System.Runtime.InteropServices;
using iTunesController;
using iTunesControllerLib;

namespace iTunesRatingsControl {
    /// <summary>
    ///     A transparent row of stars laid over Apple Music's MiniPlayer, showing the current track's rating. Hover to
    ///     pick a rating and click to set it.
    /// </summary>
    public sealed partial class iTunesRatingControl : Form {
        public iTunesRatingControl() : this(string.Empty) {
        }
        public iTunesRatingControl(string statsfile) {
            _statsfile = statsfile;
            InitializeComponent();
            Opacity = 0.6;
            var rect = Screen.AllScreens.Aggregate(Rectangle.Empty, (r, s) => r.IsEmpty ? s.Bounds : Rectangle.Union(r, s.Bounds));
            _defaultLocation = Location = new Point(rect.Left, rect.Top - Height);
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            Bitmap bmp = new Bitmap(Width, Height);
            using (var gfx = Graphics.FromImage(bmp)) {
                gfx.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, Height);
            }
            BackgroundImage = bmp;
            ReadStatsFile();
        }
        protected override CreateParams CreateParams {
            get {
                // Don't take focus when clicked, so setting a rating can hand focus back to whatever had it.
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
                return cp;
            }
        }
        protected override bool ShowWithoutActivation => true;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            // Get told about keystrokes in any app (only that one happened, not which key), so the song list is only
            // scrolled when the user isn't typing. See AppleMusic.ScrollToRow.
            var device = new RAWINPUTDEVICE { usUsagePage = HID_USAGE_PAGE_GENERIC, usUsage = HID_USAGE_GENERIC_KEYBOARD, dwFlags = RIDEV_INPUTSINK, hwndTarget = Handle };
            RegisterRawInputDevices(new[] { device }, 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
        }
        protected override void WndProc(ref Message m) {
            if (m.Msg == WM_INPUT) AppleMusic.NoteKeyboardInput();
            base.WndProc(ref m);
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
            if (!_mousePointing || e.Button != MouseButtons.Left || _track == null) return;
            _pendingRating = _mouseStars;
            _stars = _mouseStars;
            Invalidate();
            _wake.Release();
        }
        private void iTunesRatingControl_Shown(object sender, EventArgs e) {
            Location = _defaultLocation;
            timer.Start();
            _worker = Task.Run(() => WorkerLoop(_cts.Token));
        }
        private void menuExitITunesRatingControl_Click(object sender, EventArgs e) {
            Close();
            Application.Exit();
        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            _cts.Cancel();
            base.OnFormClosing(e);
        }
        /// <summary>
        ///     Keeps the stars on the MiniPlayer's album art, just above it in the z-order.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e) {
            var mini = AppleMusic.FindMiniPlayerWindow();
            if (mini == IntPtr.Zero || IsIconic(mini) || DwmGetWindowAttribute(mini, DWMWA_EXTENDED_FRAME_BOUNDS, out AppleMusic.RECT frame, Marshal.SizeOf<AppleMusic.RECT>()) != 0) {
                if (Location != _defaultLocation) Location = _defaultLocation;
                return;
            }
            var location = new Point(frame.Left + OffsetFromCorner, frame.Top + OffsetFromCorner);
            if (Location != location) Location = location;
            if ((GetWindowLong(mini, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0) {
                SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
            else {
                if ((GetWindowLong(Handle, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0) SetWindowPos(Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                var above = GetWindow(mini, GW_HWNDPREV);
                if (above != Handle) SetWindowPos(Handle, above == IntPtr.Zero ? HWND_TOP : above, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
        }
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (_bmp == null) _bmp = new Bitmap(Width, Height);
            using (var gfx = Graphics.FromImage(_bmp)) {
                gfx.FillRectangle(_mousePointing ? new SolidBrush(Color.FromArgb(90, 90, 90)) : Brushes.Black, 0, 0, Width, Height);
                if (_stars > 0) {
                    string stars = new string(StarGlyph, _stars);
                    gfx.DrawString(stars, Font, new SolidBrush(Color.FromArgb(11, 11, 11)), new Point(0, 0));
                    gfx.DrawString(stars, Font, new SolidBrush(Color.White), new Point(2, 2));
                }
                if (_mousePointing) {
                    string stars = new string(StarGlyph, _mouseStars);
                    gfx.DrawString(stars, Font, new SolidBrush(Color.Yellow), new Point(2, 2));
                }
            }
            e.Graphics.DrawImage(_bmp, 0, 0);
        }
        private void ReadStatsFile() {
            if (!File.Exists(_statsfile)) return;
            foreach (string line in File.ReadAllLines(_statsfile)) {
                var vals = line.Split('=');
                if (vals.Length != 2) continue;
                string key = vals[0].Trim().ToLower();
                if (key.Length == 10 && key.EndsWith("starcount") && char.IsDigit(key[0]) && int.TryParse(vals[1], out int count)) {
                    int stars = key[0] - '0';
                    if (stars >= 1 && stars <= 5) _ratingCounters[stars - 1] = count;
                }
            }
        }
        private void ShowStars(int stars) {
            this.AsyncInvokeIfRequired(() => {
                                           if (_stars == stars) return;
                                           _stars = stars;
                                           Invalidate();
                                       });
        }
        /// <summary>
        ///     Watches Apple Music on a background thread: notices track changes, reads the rating, and sets it when a star
        ///     is clicked. UI Automation calls into another process can take a while, so none of this runs on the UI thread.
        /// </summary>
        private async Task WorkerLoop(CancellationToken ct) {
            var music = new AppleMusic();
            DateTime lastRead = DateTime.MinValue;
            int lastStars = -1;
            bool counted = false;
            while (!ct.IsCancellationRequested) {
                try {
                    var track = await music.GetNowPlayingAsync();
                    if (track == null) {
                        _track = null;
                        ShowStars(-1);
                    }
                    else if (_pendingRating is int wanted && track.IsSameTrack(_track)) {
                        _pendingRating = null;
                        ShowStars(lastStars = music.SetRating(track, wanted) ?? -1);
                        lastRead = DateTime.Now;
                    }
                    else if (!track.IsSameTrack(_track)) {
                        AppleMusic.Log($"Track changed: {track}");
                        _pendingRating = null;
                        _track = track;
                        counted = false;
                        ShowStars(lastStars = music.ReadRating(track) ?? -1);
                        lastRead = DateTime.Now;
                    }
                    else if ((DateTime.Now - lastRead).TotalSeconds >= (lastStars < 0 ? RetrySeconds : RereadSeconds)) {
                        // Pick up changes made in Apple Music itself, or a row that's since come into the list.
                        // While the rating is unknown, retry often: the row may not have been found because the user was busy.
                        ShowStars(lastStars = music.ReadRating(track) ?? -1);
                        lastRead = DateTime.Now;
                    }
                    if (!counted && lastStars > 0 && _track != null) {
                        // Count each track once, whenever its rating first becomes known.
                        counted = true;
                        ++_ratingCounters[lastStars - 1];
                        WriteStatsFile();
                    }
                }
                catch (Exception ex) when (!ct.IsCancellationRequested) {
                    System.Diagnostics.Debug.WriteLine("iTunesRatingControl: " + ex);
                }
                try {
                    await _wake.WaitAsync(PollMilliseconds, ct);
                }
                catch (OperationCanceledException) {
                    break;
                }
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
        [DllImport("dwmapi.dll")] private static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out AppleMusic.RECT rect, int size);
        [DllImport("user32.dll")] public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hwnd, int index);
        [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle rect);
        [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] devices, uint count, uint size);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hwnd, IntPtr insertAfter, int x, int y, int cx, int cy, uint flags);
        private Bitmap? _bmp;
        private readonly CancellationTokenSource _cts = new();
        private readonly Point _defaultLocation;
        private bool _mousePointing;
        private int _mouseStars = -1;
        private volatile object? _pendingRating;
        private readonly int[] _ratingCounters = new int[5];
        /// <summary>
        ///     The current track's rating, or -1 if it isn't known (nothing playing, or its row isn't in the main window's list).
        /// </summary>
        private int _stars = -1;
        private readonly string _statsfile;
        private volatile NowPlaying? _track;
        private readonly SemaphoreSlim _wake = new(0);
        private Task? _worker;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        private const int GWL_EXSTYLE = -20;
        private const ushort HID_USAGE_GENERIC_KEYBOARD = 6;
        private const ushort HID_USAGE_PAGE_GENERIC = 1;
        private const uint RIDEV_INPUTSINK = 0x100;
        private const int WM_INPUT = 0xFF;
        private const uint GW_HWNDPREV = 3;
        private static readonly IntPtr HWND_NOTOPMOST = new(-2);
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;
        private static readonly IntPtr HWND_TOPMOST = new(-1);
        private const int OffsetFromCorner = 8;
        private const int PollMilliseconds = 500;
        private const int RereadSeconds = 5;
        private const int RetrySeconds = 1;
        private const char StarGlyph = ''; // a star in Wingdings
        private const uint SWP_NOACTIVATE = 0x10;
        private const uint SWP_NOMOVE = 2;
        private const uint SWP_NOSIZE = 1;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_TOPMOST = 0x8;
        [StructLayout(LayoutKind.Sequential)] private struct RAWINPUTDEVICE {
            public ushort usUsagePage;
            public ushort usUsage;
            public uint dwFlags;
            public IntPtr hwndTarget;
        }
    }
}
