using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using iTunesController.Properties;
using iTunesLib;

namespace iTunesController {
    public sealed partial class AlbumArtForm : Form {
        public AlbumArtForm() {
            InitializeComponent();
            _starsButtons = new[] {btnStars1, btnStars2, btnStars3, btnStars4, btnStars5};
            for (int i = 0; i < 5; ++i) _starsButtons[i].Tag = i;
            NewITunesAppClass();
            timerUIUpdater.Start();
            BackColor = Color.White;
            //TransparencyKey = Color.White;
            Location = Settings.Default.WindowLocation;
        }
        private void AlbumArtForm_Load(object sender, EventArgs e) {
        }
        private void AlbumArtForm_Shown(object sender, EventArgs e) {
            lblTrackName.Focus();
        }
        private void btnStars_Click(object sender, EventArgs e) {
            int i = (int) ((Button) sender).Tag;
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
            if (track == null) return;
            _curRating = (i + 1) * 20;
            track.Rating = _curRating;
            SetStars(_curRating, lblTrackName.ForeColor);
            lblTrackName.Focus();
        }
        private void btnStars_MouseEnter(object sender, EventArgs e) {
            _inStarButton = true;
            int i = (int) ((Button) sender).Tag;
            SetStars(20 * (i + 1), Color.Yellow);
        }
        private void btnStars_MouseLeave(object sender, EventArgs e) {
            _inStarButton = false;
            SetStars(_curRating, lblTrackName.ForeColor);
        }
        private void btnStars1_MouseMove(object sender, MouseEventArgs e) {
        }
        private void menuitemExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }
        private void timerUIUpdater_Tick(object sender, EventArgs e) {
            if (!_inStarButton) lblTrackName.Focus();
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
            if (track == null) return;
            try {
                switch (_timerOp) {
                    case TimerOperation.LookForDuplicates:
                        _timerOp = TimerOperation.CheckForNewTrack;
                        iTunesApp itunes = new iTunesApp();
                        IITLibraryPlaylist library = itunes.LibraryPlaylist;
                        int i = library.Tracks.Count;
                        var curname = track.Name.ToLower();
                        var curartist = track.Artist.ToLower();
                        var curfile = track.Kind == ITTrackKind.ITTrackKindFile ? ((dynamic) track).Location : "";
                        foreach (IITTrack t in library.Tracks) {
                            var tname = t.Name.ToLower();
                            var tartist = t.Artist.ToLower();
                            var tfile = t.Kind == ITTrackKind.ITTrackKindFile ? ((dynamic) t).Location : "";
                            if ((tname == curname || tname.Contains(curname) || curname.Contains(tname)) && (tartist == curartist || tartist.Contains(curartist) || curartist.Contains(tartist)) &&
                                curfile != tfile) {
                                var dupform = new DuplicateTrack(BackgroundImage, track, t);
                                dupform.ShowDialog();
                            }
                        }
                        break;
                    default:
                        // Get the location and size of the iTunes window.
                        var processes = Process.GetProcessesByName("iTunes");
                        if (processes.Length == 0) return;
                        var process = processes[0];
                        var title = process.MainWindowTitle;
                        if (title.ToUpper() != "MINIPLAYER") {
                            Visible = false;
                            return;
                        }
                        Visible = true;
                        var winhandle = process.MainWindowHandle;
                        var rect = new Rectangle();
                        GetWindowRect(winhandle, ref rect);
                        Point location = rect.Location;
                        Size size = new Size(rect.Width - rect.X, rect.Height - rect.Y);
                        Location = new Point(location.X, location.Y + size.Height);
                        // Set the size of my window to sit underneath the iTunes window.
                        Size = new Size(size.Width, Size.Height);
                        // Adjust the font size as needed to get the track name to fit properly.
                        float stringwidth;
                        using (var gfx = lblTrackName.CreateGraphics()) {
                            stringwidth = gfx.MeasureString(lblTrackName.Text, lblTrackName.Font).Width;
                        }
                        if (stringwidth + lblTrackName.Left > Width) lblTrackName.Font = new Font(lblTrackName.Font.FontFamily, lblTrackName.Font.Size - 0.1F);
                        else if (lblTrackName.Font.Size < 21.75 && stringwidth + lblTrackName.Left + 5 < Width)
                            lblTrackName.Font = new Font(lblTrackName.Font.FontFamily, lblTrackName.Font.Size + 0.1F);
                        // Get the currently playing track.
                        if (!string.IsNullOrEmpty(_curTrackPath) && string.Compare(_curTrackPath, track.Artist + "/" + track.Album + "/" + track.Name, StringComparison.OrdinalIgnoreCase) == 0)
                            return;
                        //****
                        //**** If we get here, there was a track change since the last timer event.
                        //****
                        _timerOp = TimerOperation.LookForDuplicates;
                        // Set the current track path.
                        _curTrackPath = track.Artist + "/" + track.Album + "/" + track.Name;
                        // Update the track info
                        lblTrackName.Text = track.Name;
                        lblArtist.Text = track.Artist;
                        lblAlbum.Text = track.Album;
                        lblTrackName.Visible = lblArtist.Visible = lblAlbum.Visible = true;
                        SetStars(track.Rating, lblTrackName.ForeColor);
                        _curRating = track.Rating;
                        // Get the artwork.
                        IITArtworkCollection artcollection = track.Artwork;
                        if (artcollection == null || artcollection.Count < 1) return;
                        IITArtwork art = artcollection[1];
                        // Save the artwork to a temporary file.
                        string filename;
                        switch (art.Format) {
                            case ITArtworkFormat.ITArtworkFormatJPEG:
                                filename = "temp.jpg";
                                break;
                            case ITArtworkFormat.ITArtworkFormatPNG:
                                filename = "temp.png";
                                break;
                            default:
                                filename = "temp.bmp";
                                break;
                        }
                        filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + filename;
                        art.SaveArtworkToFile(filename);
                        Color darkest = Color.Black, lightest = Color.White;
                        int avgR, avgG, avgB;
                        using (Image img = Image.FromFile(filename)) {
                            using (var img2 = new Bitmap(img, new Size(10, 10))) {
                                int darkestindex = darkest.R + darkest.G + darkest.B;
                                int lightestindex = lightest.R + lightest.G + lightest.B;
                                int totR = 0, totG = 0, totB = 0;
                                for (int y = 0; y < 10; ++y) {
                                    for (int x = 0; x < 10; ++x) {
                                        var color = img2.GetPixel(x, y);
                                        int index = color.R + color.G + color.B;
                                        totR += color.R;
                                        totG += color.G;
                                        totB += color.B;
                                        if (index < darkestindex) {
                                            darkestindex = index;
                                            darkest = color;
                                        }
                                        if (index > lightestindex) {
                                            lightestindex = index;
                                            lightest = color;
                                        }
                                    }
                                }
                                avgR = (int) Math.Round(totR / 100F);
                                avgG = (int) Math.Round(totG / 100F);
                                avgB = (int) Math.Round(totB / 100F);
                            }
                            using (var img2 = new Bitmap(img, new Size(50, 50))) {
                                for (int x = 0; x < 50; ++x) {
                                    int totR = 0, totG = 0, totB = 0;
                                    for (int y = 0; y < 50; ++y) {
                                        var color = img2.GetPixel(x, y);
                                        totR += color.R;
                                        totG += color.G;
                                        totB += color.B;
                                    }
                                    Color c = Color.FromArgb(160, (int) Math.Round(totR / 50F), (int) Math.Round(totG / 50F), (int) Math.Round(totB / 50F));
                                    for (int y = 0; y < 50; ++y) img2.SetPixel(x, y, c);
                                }
                                BackgroundImage = new Bitmap(img2);
                            }
                        }
                        Color forecolor = Color.FromArgb(Math.Min(Math.Max(0, 255 - avgR), 255), Math.Min(Math.Max(0, 255 - avgG), 255), Math.Min(Math.Max(0, 255 - avgB), 255));
                        Color backcolor = Color.FromArgb(Math.Min(Math.Max(0, avgR), 255), Math.Min(Math.Max(0, avgG), 255), Math.Min(Math.Max(0, avgB), 255));
                        if (ColorDist(forecolor, backcolor) < 210)
                            forecolor = ColorDist(backcolor, lightest) < ColorDist(backcolor, darkest) ? darkest : lightest;
                        //lblTrackName.OutlineColor = lightest;
                        lblTrackName.ForeColor = lblAlbum.ForeColor = lblArtist.ForeColor = forecolor;
                        foreach (var btn in _starsButtons) btn.ForeColor = forecolor;
                        BackColor = backcolor;
                        break;
                }
            }
            catch {
                // do nothing.
            }
        }
        private void NewITunesAppClass() {
            _itunes = new iTunesAppClass();
            _itunes.OnQuittingEvent += () => {
                                           _itunes = null;
                                           Application.Exit();
                                       };
            _itunes.OnAboutToPromptUserToQuitEvent += () => {
                                                          _itunes = null;
                                                          Application.Exit();
                                                      };
        }
        private void SetStars(int rating, Color hilite) {
            foreach (var btn in _starsButtons) {
                btn.Text = "";
                btn.ForeColor = hilite;
            }
            for (int i = 0; i < rating / 20; ++i) _starsButtons[i].Text = "";
        }
        private static int ColorDist(Color c1, Color c2) {
            return Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
        }
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        private int _curRating;
        private string _curTrackPath;
        private bool _inStarButton;
        private iTunesAppClass _itunes;
        private readonly Button[] _starsButtons;
        private TimerOperation _timerOp = TimerOperation.CheckForNewTrack;
        private enum TimerOperation {
            CheckForNewTrack,
            LookForDuplicates
        }
    }
}