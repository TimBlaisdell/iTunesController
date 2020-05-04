using System;
using System.Drawing;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesController {
   public sealed partial class DuplicateTrack : Form {
      public DuplicateTrack() {
         InitializeComponent();
         ResizingRegionSize = 10;
      }
      public DuplicateTrack(Image background, IITTrack curtrack, IITTrack othertrack) : this() {
         Bitmap bmp = new Bitmap(background);
         Color c = Color.FromArgb(128, 0, 0, 0);
         using (var gfx = Graphics.FromImage(bmp)) {
            gfx.FillRectangle(new SolidBrush(c), 0, 0, bmp.Width, bmp.Height);
         }
         BackgroundImage = bmp;
         c = Color.FromArgb(128, 255, 255, 255);
         bmp = new Bitmap(background.Width / 2, background.Height);
         using (var gfx = Graphics.FromImage(bmp)) {
            gfx.DrawImage(background, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, background.Width / 2, background.Height), GraphicsUnit.Pixel);
            gfx.FillRectangle(new SolidBrush(c), 0, 0, bmp.Width, bmp.Height);
         }
         splitContainer.Panel1.BackgroundImageLayout = ImageLayout.Stretch;
         splitContainer.Panel1.BackgroundImage = bmp;
         bmp = new Bitmap(background.Width / 2, background.Height);
         using (var gfx = Graphics.FromImage(bmp)) {
            gfx.DrawImage(background,
                          new Rectangle(0, 0, bmp.Width, bmp.Height),
                          new Rectangle(background.Width / 2, 0, background.Width - background.Width / 2, background.Height),
                          GraphicsUnit.Pixel);
            gfx.FillRectangle(new SolidBrush(c), 0, 0, bmp.Width, bmp.Height);
         }
         splitContainer.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
         splitContainer.Panel2.BackgroundImage = bmp;
         if (splitContainer.Panel1.Size.Width != splitContainer.Panel2.Size.Width) splitContainer.SplitterDistance += (splitContainer.Panel2.Size.Width - splitContainer.Panel1.Width) / 2;
         lblCurrentLabel.Text = curtrack.Name;
         lblOtherNameLabel.Text = othertrack.Name;
      }
      public int ResizingRegionSize { get; set; }
      private void border_MouseDown(object sender, MouseEventArgs e) {
         if (_inBorderRegion == BorderRegion.None) _isWindowMoving = true;
         else _isWindowResizing = true;
         _lastMousePos = MousePosition;
      }
      private void border_MouseEnter(object sender, EventArgs e) {
         border_MouseMove(null, null);
      }
      private void border_MouseLeave(object sender, EventArgs e) {
         Cursor = Cursors.Default;
         _inBorderRegion = BorderRegion.None;
      }
      private void border_MouseMove(object sender, MouseEventArgs e) {
         var screenNewpos = MousePosition;
         var newpos = PointToClient(screenNewpos);
         Point change = new Point(screenNewpos.X - _lastMousePos.X, screenNewpos.Y - _lastMousePos.Y);
         if (_isWindowMoving) {
            Location = new Point(Location.X + change.X, Location.Y + change.Y);
         }
         else if (_isWindowResizing) {
            Point poschange = new Point(_inBorderRegion.HasFlag(BorderRegion.Left) ? change.X : 0, _inBorderRegion.HasFlag(BorderRegion.Top) ? change.Y : 0);
            Size sizechange = new Size(_inBorderRegion.HasFlag(BorderRegion.Left) || _inBorderRegion.HasFlag(BorderRegion.Right) ? change.X : 0,
                                       _inBorderRegion.HasFlag(BorderRegion.Top) || _inBorderRegion.HasFlag(BorderRegion.Bottom) ? change.Y : 0);
            if (_inBorderRegion.HasFlag(BorderRegion.Left)) sizechange.Width *= -1;
            if (_inBorderRegion.HasFlag(BorderRegion.Top)) sizechange.Height *= -1;
            Location = new Point(Left + poschange.X, Top + poschange.Y);
            Size = new Size(Width + sizechange.Width, Height + sizechange.Height);
            OnPaint(new PaintEventArgs(CreateGraphics(), ClientRectangle));
         }
         else {
            _inBorderRegion = BorderRegion.None;
            if (newpos.X >= Width - ResizingRegionSize) _inBorderRegion |= BorderRegion.Right;
            else if (newpos.X <= ResizingRegionSize) _inBorderRegion |= BorderRegion.Left;
            if (newpos.Y >= Height - ResizingRegionSize) _inBorderRegion |= BorderRegion.Bottom;
            else if (newpos.Y <= ResizingRegionSize) _inBorderRegion |= BorderRegion.Top;
            SetCursor();
         }
         _lastMousePos = screenNewpos;
      }
      private void border_MouseUp(object sender, MouseEventArgs e) {
         _isWindowMoving = false;
         _isWindowResizing = false;
      }
      private void SetCursor() {
         switch (_inBorderRegion) {
            case BorderRegion.None:
               Cursor = Cursors.Default;
               break;
            case BorderRegion.Top:
            case BorderRegion.Bottom:
               Cursor = Cursors.SizeNS;
               break;
            case BorderRegion.Left:
            case BorderRegion.Right:
               Cursor = Cursors.SizeWE;
               break;
            case BorderRegion.Left | BorderRegion.Top:
            case BorderRegion.Right | BorderRegion.Bottom:
               Cursor = Cursors.SizeNWSE;
               break;
            case BorderRegion.Left | BorderRegion.Bottom:
            case BorderRegion.Right | BorderRegion.Top:
               Cursor = Cursors.SizeNESW;
               break;
         }
      }
      private BorderRegion _inBorderRegion = BorderRegion.None;
      private bool _isWindowMoving;
      private bool _isWindowResizing;
      private Point _lastMousePos;
      [Flags] private enum BorderRegion {
         None = 0,
         Top = 0x0001,
         Bottom = 0x0010,
         Left = 0x0100,
         Right = 0x1000
      }
   }
}