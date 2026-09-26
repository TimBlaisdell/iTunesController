using iTunesRatingsControl;

namespace iTunesController {
    public sealed partial class LowerWindow : Form {
        public LowerWindow() {
            InitializeComponent();
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            Bitmap bmp = new Bitmap(Width, Height);
            using (var gfx = Graphics.FromImage(bmp)) {
                gfx.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, Height);
            }
            BackgroundImage = bmp;
        }
        private void timer_Tick(object sender, EventArgs e) {
            if (iTunesWinHandle != IntPtr.Zero) {
                iTunesRatingControl.GetWindowRect(iTunesWinHandle, out Rectangle rect);
                int wid = rect.Right - 2 * rect.Left;
                int hgt = rect.Bottom - 2 * rect.Top;
                Location = new Point(rect.Left, rect.Top + wid + 126);
                Size = new Size(19, hgt - (wid + 126));
                var nextwin = iTunesRatingControl.GetWindow(iTunesWinHandle, GW_HWNDPREV);
                iTunesRatingControl.SetWindowPos(Handle, nextwin.ToInt32(), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }
        }
        public IntPtr iTunesWinHandle = IntPtr.Zero;
        private const uint GW_HWNDPREV = 3;
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
    }
}