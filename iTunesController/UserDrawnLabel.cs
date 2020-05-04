using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace iTunesController {
    public class OwnerDrawnLabel : Label {
        public OwnerDrawnLabel ( ) {
            OutlineWidth = 1.0f;
        }
        public bool OutlineText { get; set; }
        public float OutlineWidth { get; set; }
        public Color OutlineColor { get; set; }
        public bool DropShadow { get; set; }
        public Point DropShadowOffset { get; set; }
        protected override void OnPaint ( PaintEventArgs e ) {
            if (OutlineText) {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // prepare to draw text
                //StringFormat sf = new StringFormat();
                // draw the text to a path
                GraphicsPath path = new GraphicsPath();
                path.AddString(Text, Font.FontFamily, (int)Font.Style, e.Graphics.DpiY * Font.SizeInPoints / 72f, /*e.ClipRectangle*/ ClientRectangle.Location,
                               StringFormat.GenericTypographic);
                if (DropShadow) {
                    RectangleF offsetrect = ClientRectangle;
                    offsetrect.Offset(DropShadowOffset);
                    using (GraphicsPath offPath = new GraphicsPath()) {
                        offPath.AddString(Text, Font.FontFamily, (int)Font.Style, e.Graphics.DpiY * Font.SizeInPoints / 72f, offsetrect.Location,
                                          StringFormat.GenericTypographic);
                        using (Brush b = new SolidBrush(Color.FromArgb(100, 0, 0, 0))) {
                            e.Graphics.FillPath(b, offPath);
                        }
                    }
                }
                // draw the outline
                e.Graphics.DrawPath(new Pen(OutlineColor, OutlineWidth), path);
                // fill in the outline
                e.Graphics.FillPath(new SolidBrush(ForeColor), path);
                //e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), e.ClipRectangle);
            }
            else base.OnPaint(e);
        }
    }

}