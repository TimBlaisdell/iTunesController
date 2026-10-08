using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using Windows.Media.Control;

namespace iTunesController {
    /// <summary>
    ///     The track Apple Music reports as current (playing or paused) through the Windows media controls.
    /// </summary>
    public sealed record NowPlaying(string Title, string Artist, string Album, bool IsPlaying) {
        public bool IsSameTrack(NowPlaying? other) {
            return other != null && other.Title == Title && other.Artist == Artist && other.Album == Album;
        }
        public override string ToString() => $"{Title} / {Artist} / {Album}";
    }

    /// <summary>
    ///     Talks to the Apple Music app for Windows. Apple Music has no automation API like iTunes did, so:
    ///     - the current track comes from the Windows media controls (SMTC),
    ///     - the rating is read from the rating slider in the current track's row of the main window's song list
    ///       (via UI Automation; works while the main window is minimized),
    ///     - the rating is set by focusing that slider and sending arrow keys. Setting the slider's value through
    ///       UI Automation only changes what's displayed; Apple Music only saves ratings that come from real input.
    ///     All methods except FindMiniPlayerWindow may be slow and should be called off the UI thread.
    /// </summary>
    public sealed class AppleMusic {
        public async Task<NowPlaying?> GetNowPlayingAsync() {
            _sessionManager ??= await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var session = _sessionManager.GetSessions().FirstOrDefault(s => s.SourceAppUserModelId.StartsWith(AppUserModelIdPrefix, StringComparison.OrdinalIgnoreCase));
            if (session == null) return null;
            var props = await session.TryGetMediaPropertiesAsync();
            if (props == null || string.IsNullOrEmpty(props.Title)) return null;
            // Apple Music puts "Artist — Album" in the artist field and leaves the album empty.
            string artist = props.Artist ?? string.Empty;
            string album = props.AlbumTitle ?? string.Empty;
            int i = artist.IndexOf(ArtistAlbumSeparator, StringComparison.Ordinal);
            if (i >= 0) {
                if (string.IsNullOrEmpty(album)) album = artist.Substring(i + ArtistAlbumSeparator.Length).Trim();
                artist = artist.Substring(0, i).Trim();
            }
            bool playing = session.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            return new NowPlaying(props.Title, artist, album, playing);
        }
        /// <summary>
        ///     Returns the handle of the visible MiniPlayer window, or IntPtr.Zero if there isn't one.
        /// </summary>
        public static IntPtr FindMiniPlayerWindow() {
            return FindAppleMusicWindow(MiniPlayerTitle);
        }
        /// <summary>
        ///     Returns the track's rating (0-5), or null if its row can't be found in the main window's current song list.
        /// </summary>
        public int? ReadRating(NowPlaying track) {
            var slider = FindRatingSlider(track);
            if (slider == null) return null;
            return (int)Math.Round(((RangeValuePattern)slider.GetCurrentPattern(RangeValuePattern.Pattern)).Current.Value);
        }
        /// <summary>
        ///     Sets the track's rating (0-5) the way a user would: focus its rating control and press arrow keys. If the main
        ///     window is minimized, it's restored off-screen for the moment it takes and then minimized again.
        ///     Returns the rating afterwards, or null if the row couldn't be found or focused.
        /// </summary>
        public int? SetRating(NowPlaying track, int stars) {
            stars = Math.Clamp(stars, 0, 5);
            var slider = FindRatingSlider(track, userInitiated: true);
            if (slider == null || _mainWindow == null) return null;
            var range = (RangeValuePattern)slider.GetCurrentPattern(RangeValuePattern.Pattern);
            int current = (int)Math.Round(range.Current.Value);
            if (current == stars) return current;
            Log($"SetRating {current} -> {stars}: {track}");
            var hwnd = new IntPtr(_mainWindow.Current.NativeWindowHandle);
            var foreground = GetForegroundWindow();
            var placement = WINDOWPLACEMENT.Create();
            GetWindowPlacement(hwnd, ref placement);
            bool wasMinimized = placement.showCmd == SW_SHOWMINIMIZED;
            try {
                if (wasMinimized) {
                    // Restore it somewhere nobody can see it.
                    var offscreen = placement;
                    int w = placement.rcNormalPosition.Right - placement.rcNormalPosition.Left;
                    int h = placement.rcNormalPosition.Bottom - placement.rcNormalPosition.Top;
                    var virtualScreen = SystemInformation.VirtualScreen;
                    offscreen.rcNormalPosition = new RECT { Left = virtualScreen.Left - w - 200, Top = virtualScreen.Top, Right = virtualScreen.Left - 200, Bottom = virtualScreen.Top + h };
                    offscreen.showCmd = SW_SHOWNORMAL;
                    offscreen.flags = 0;
                    int disable = 1;
                    DwmSetWindowAttribute(hwnd, DWMWA_TRANSITIONS_FORCEDISABLED, ref disable, sizeof(int));
                    SetWindowPlacement(hwnd, ref offscreen);
                    Thread.Sleep(150);
                }
                slider.SetFocus();
                if (!WaitForFocus(slider)) return null;
                byte key = stars > current ? VK_RIGHT : VK_LEFT;
                for (int i = 0; i < Math.Abs(stars - current); ++i) {
                    keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                    keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
                    Thread.Sleep(30);
                }
                Thread.Sleep(100);
                return (int)Math.Round(range.Current.Value);
            }
            finally {
                if (wasMinimized) {
                    SetWindowPlacement(hwnd, ref placement);
                    int enable = 0;
                    DwmSetWindowAttribute(hwnd, DWMWA_TRANSITIONS_FORCEDISABLED, ref enable, sizeof(int));
                }
                else if (foreground != IntPtr.Zero && foreground != hwnd) {
                    // Put the main window back behind whatever was in front before.
                    SetWindowPos(hwnd, foreground, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                }
                if (foreground != IntPtr.Zero && foreground != hwnd) RestoreForeground(foreground);
            }
        }
        private static string DescribeRow(AutomationElement row) {
            try {
                return row.Current.Name;
            }
            catch (ElementNotAvailableException) {
                return string.Empty;
            }
        }
        private static IntPtr FindAppleMusicWindow(string title) {
            var pids = Process.GetProcessesByName(ProcessName).Select(p => (uint)p.Id).ToHashSet();
            if (pids.Count == 0) return IntPtr.Zero;
            IntPtr found = IntPtr.Zero;
            var sb = new StringBuilder(256);
            EnumWindows((h, _) => {
                GetWindowThreadProcessId(h, out uint pid);
                if (!pids.Contains(pid) || !IsWindowVisible(h)) return true;
                sb.Clear();
                GetWindowText(h, sb, sb.Capacity);
                if (sb.ToString() != title) return true;
                found = h;
                return false;
            }, IntPtr.Zero);
            return found;
        }
        private AutomationElement? FindRatingSlider(NowPlaying track, bool userInitiated = false) {
            var row = FindRow(track, userInitiated);
            return row?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Slider));
        }
        /// <summary>
        ///     Finds the track's row in the main window's song list. Rows scrolled out of view are only placeholders, so
        ///     this first checks the last row found and the rows that exist, then pages through the list.
        /// </summary>
        private AutomationElement? FindRow(NowPlaying track, bool userInitiated) {
            if (_lastRow != null && RowMatches(DescribeRow(_lastRow), track)) return _lastRow;
            Log($"FindRow start: {track}");
            _lastRow = null;
            var list = FindSongList();
            if (list == null) {
                Log("FindRow: no song list");
                return null;
            }
            var row = FindRealizedRow(list, track);
            if (row == null && list.TryGetCurrentPattern(ScrollPattern.Pattern, out object sp)) row = ScrollToRow(list, (ScrollPattern)sp, track, userInitiated);
            Log($"FindRow end: {(row == null ? "not found" : "found")}");
            _lastRow = row;
            return row;
        }
        /// <summary>
        ///     Pages through the song list looking for the track's row. Scrolling the list makes Apple Music activate its
        ///     main window (even when minimized), which would steal the keyboard from whatever the user is typing in. So
        ///     this only runs once there's been no typing for a moment, gives up as soon as a key is pressed, and hands
        ///     the foreground back after every step. An interrupted search resumes where it stopped on the next call for
        ///     the same track. The exception is when the user clicked a star (userInitiated), since setting the rating
        ///     activates Apple Music anyway. Returns null if it didn't find the row or gave up.
        /// </summary>
        private AutomationElement? ScrollToRow(AutomationElement list, ScrollPattern scroll, NowPlaying track, bool userInitiated) {
            double view = scroll.Current.VerticalViewSize;
            if (!scroll.Current.VerticallyScrollable || view <= 0) return null;
            int lastKey = _lastKeyboardTick;
            if (!userInitiated && Environment.TickCount - lastKey < IdleMillisecondsBeforeScrolling) {
                Log("FindRow: user is typing; not scrolling yet");
                return null;
            }
            if (!track.IsSameTrack(_scanTrack)) {
                _scanTrack = track;
                _scanResumePercent = 0;
            }
            // Scan from where the last attempt stopped to the bottom, then from the top back to there.
            double step = Math.Max(1, view * 0.9);
            double start = _scanResumePercent;
            var stops = new List<double>();
            for (double pct = start; pct < 100 + step; pct += step) stops.Add(Math.Min(100, pct));
            for (double pct = 0; pct < start; pct += step) stops.Add(pct);
            var foreground = GetForegroundWindow();
            foreach (double pct in stops) {
                if (!userInitiated && _lastKeyboardTick != lastKey) {
                    Log($"FindRow: user typed; stopped scrolling at {pct:F1}%");
                    _scanResumePercent = pct;
                    return null;
                }
                Log($"FindRow: scroll to {pct:F1}%");
                scroll.SetScrollPercent(ScrollPattern.NoScroll, pct);
                if (foreground != IntPtr.Zero && GetForegroundWindow() != foreground) {
                    Log("FindRow: Apple Music took the foreground; giving it back");
                    RestoreForeground(foreground);
                }
                var row = FindRealizedRow(list, track);
                if (row != null) {
                    _scanTrack = null;
                    return row;
                }
            }
            _scanResumePercent = 0;
            return null;
        }
        /// <summary>
        ///     Called (from any thread) whenever a key is pressed anywhere, so scrolling can wait until the user stops typing.
        /// </summary>
        public static void NoteKeyboardInput() {
            _lastKeyboardTick = Environment.TickCount;
        }
        /// <summary>
        ///     Appends a timestamped line, along with the current foreground window, to LogFile (if set).
        /// </summary>
        public static void Log(string message) {
            if (string.IsNullOrEmpty(LogFile)) return;
            var fg = GetForegroundWindow();
            var sb = new StringBuilder(256);
            GetWindowText(fg, sb, sb.Capacity);
            GetWindowThreadProcessId(fg, out uint pid);
            string process = "?";
            try {
                process = Process.GetProcessById((int)pid).ProcessName;
            }
            catch {
                // it's gone.
            }
            lock (LogLock) {
                File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} {message}   [foreground: {process} '{sb}']{Environment.NewLine}");
            }
        }
        public static string? LogFile;
        private static readonly object LogLock = new();
        private static AutomationElement? FindRealizedRow(AutomationElement list, NowPlaying track) {
            var rows = list.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            foreach (AutomationElement row in rows) {
                if (RowMatches(DescribeRow(row), track)) return row;
            }
            return null;
        }
        private AutomationElement? FindSongList() {
            try {
                if (_mainWindow == null || _mainWindow.Current.ProcessId == 0) _mainWindow = null;
            }
            catch (ElementNotAvailableException) {
                _mainWindow = null;
            }
            if (_mainWindow == null) {
                var hwnd = FindAppleMusicWindow(MainWindowTitle);
                if (hwnd == IntPtr.Zero) return null;
                _mainWindow = AutomationElement.FromHandle(hwnd);
            }
            return _mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, SongListAutomationId));
        }
        private static void RestoreForeground(IntPtr hwnd) {
            if (SetForegroundWindow(hwnd)) return;
            try {
                AutomationElement.FromHandle(hwnd).SetFocus();
            }
            catch {
                // Not worth failing over; the user just has to click where they were.
            }
        }
        /// <summary>
        ///     A row's name is its visible columns run together, e.g. "Find Me 4:06 Kings of Leon WALLS three stars 7".
        /// </summary>
        private static bool RowMatches(string rowName, NowPlaying track) {
            if (!rowName.StartsWith(track.Title + " ", StringComparison.Ordinal)) return false;
            string rest = rowName.Substring(track.Title.Length);
            if (!string.IsNullOrEmpty(track.Artist) && !rest.Contains(track.Artist, StringComparison.Ordinal)) return false;
            if (!string.IsNullOrEmpty(track.Album) && !rest.Contains(track.Album, StringComparison.Ordinal)) return false;
            return true;
        }
        private static bool WaitForFocus(AutomationElement element) {
            for (int i = 0; i < 10; ++i) {
                try {
                    if (Automation.Compare(AutomationElement.FocusedElement, element)) return true;
                }
                catch {
                    // focus is changing; try again.
                }
                Thread.Sleep(50);
            }
            return false;
        }
        [DllImport("dwmapi.dll")] private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);
        [DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT placement);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int max);
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint pid);
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extraInfo);
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern bool SetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT placement);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hwnd, IntPtr insertAfter, int x, int y, int cx, int cy, uint flags);
        private delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
        private static volatile int _lastKeyboardTick = Environment.TickCount - IdleMillisecondsBeforeScrolling;
        private AutomationElement? _lastRow;
        private AutomationElement? _mainWindow;
        /// <summary>
        ///     Where an interrupted scroll search for _scanTrack should pick up again.
        /// </summary>
        private double _scanResumePercent;
        private NowPlaying? _scanTrack;
        private GlobalSystemMediaTransportControlsSessionManager? _sessionManager;
        private const string AppUserModelIdPrefix = "AppleInc.AppleMusic";
        private const string ArtistAlbumSeparator = " — ";
        private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
        /// <summary>
        ///     How long there must be no typing before the song list may be scrolled.
        /// </summary>
        private const int IdleMillisecondsBeforeScrolling = 1500;
        private const uint KEYEVENTF_EXTENDEDKEY = 1;
        private const uint KEYEVENTF_KEYUP = 2;
        private const string MainWindowTitle = "Apple Music";
        private const string MiniPlayerTitle = "MiniPlayer";
        private const string ProcessName = "AppleMusic";
        private const string SongListAutomationId = "DataGridListView";
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWNORMAL = 1;
        private const uint SWP_NOACTIVATE = 0x10;
        private const uint SWP_NOMOVE = 2;
        private const uint SWP_NOSIZE = 1;
        private const byte VK_LEFT = 0x25;
        private const byte VK_RIGHT = 0x27;
        [StructLayout(LayoutKind.Sequential)] public struct RECT {
            public int Left, Top, Right, Bottom;
        }
        [StructLayout(LayoutKind.Sequential)] private struct WINDOWPLACEMENT {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public RECT rcNormalPosition;
            public static WINDOWPLACEMENT Create() => new() { length = Marshal.SizeOf<WINDOWPLACEMENT>() };
        }
    }
}
