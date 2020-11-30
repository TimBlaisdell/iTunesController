using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
// ReSharper disable UnusedMember.Global

namespace iTunesControllerLib {
    public static class Extensions {
        public static void AsyncInvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action) {
            try {
                if (obj.InvokeRequired) {
                    var args = new object[0];
                    obj.BeginInvoke(action, args);
                }
                else {
                    action();
                }
            }
            catch {
                // do nothing.
            }
        }
        public static int[] GetHash(this Bitmap bmp, int scalesize, string savefilename = null) {
            int[] hash;
            using (var bmpscaled = new Bitmap(scalesize, scalesize)) {
                using (var gfx = Graphics.FromImage(bmpscaled)) {
                    gfx.DrawImage(bmp, new Rectangle(0, 0, scalesize, scalesize), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                }
                if (savefilename != null) {
                    bmpscaled.Save(savefilename + ".scaled.png");
                }
                //int[] aa = new int[scalesize * scalesize];
                int[] ra = new int[scalesize * scalesize];
                int[] ga = new int[scalesize * scalesize];
                int[] ba = new int[scalesize * scalesize];
                bmpscaled.IterateOver((x, y, a, r, g, b, index, itres) => {
                                          //aa[index] = a;
                                          ra[index] = r;
                                          ga[index] = g;
                                          ba[index] = b;
                                      });
                hash = new int[scalesize * scalesize * 3];
                //Array.Copy(aa, 0, hash, 0, scalesize * scalesize);
                Array.Copy(ra, 0, hash, 0, scalesize * scalesize);
                Array.Copy(ga, 0, hash, scalesize * scalesize, scalesize * scalesize);
                Array.Copy(ba, 0, hash, scalesize * scalesize * 2, scalesize * scalesize);
            }
            return hash;
        }
        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action) {
            try {
                if (obj.InvokeRequired) {
                    var args = new object[0];
                    obj.Invoke(action, args);
                }
                else {
                    action();
                }
            }
            catch {
                // do nothing.
            }
        }
        /// <summary>
        ///     Iterate over the bitmap, calling the specified function for each pixel.  Function must be defined as
        ///     void Func(int x, int y, byte a, byte r, byte g, byte b, int index, IterationResult itres)
        /// </summary>
        public static void IterateOver(this Bitmap bmp, Action<int, int, byte, byte, byte, byte, int, IterationResult> p) {
            bmp.IterateOver(new Rectangle(0, 0, bmp.Width, bmp.Height), p);
        }
        /// <summary>
        ///     Iterate over an area of the bitmap, calling the specified function for each pixel.  Function must be defined as
        ///     void Func(int x, int y, byte a, byte r, byte g, byte b, int index, IterationResult itres)
        /// </summary>
        public static void IterateOver(this Bitmap bmp, Rectangle rect, Action<int, int, byte, byte, byte, byte, int, IterationResult> p) {
            // Lock the bitmap's bits.
            var bmprect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            rect = Rectangle.Intersect(rect, bmprect);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * rect.Height;
            //if (_rgbValues == null || _rgbValues.Length < bytes) {
            //    _rgbValues = new byte[bytes];
            //}
            byte[] rgbValues = new byte[bytes];
            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            //int count = 0;
            int stride = bmpData.Stride;
            bool changedSomething = false;
            var itres = new IterationResult();
            rect = Rectangle.Intersect(rect, bmprect);
            int i = 0;
            for (int y = 0; y < rect.Height; y++) {
                for (int x = 0; x < rect.Width; x++) {
                    byte b = rgbValues[(y * stride) + (x * 4)];
                    byte g = rgbValues[(y * stride) + (x * 4) + 1];
                    byte r = rgbValues[(y * stride) + (x * 4) + 2];
                    byte a = rgbValues[(y * stride) + (x * 4) + 3];
                    p(x + rect.Left, y + rect.Top, a, r, g, b, i++, itres.Reset());
                    if (itres.Color != null) {
                        changedSomething = true;
                        var color = itres.Color.Value;
                        rgbValues[(y * stride) + (x * 4)] = color.B;
                        rgbValues[(y * stride) + (x * 4) + 1] = color.G;
                        rgbValues[(y * stride) + (x * 4) + 2] = color.R;
                        rgbValues[(y * stride) + (x * 4) + 3] = color.A;
                    }
                    if (itres.Abort) break;
                }
                if (itres.Abort) break;
            }
            if (changedSomething) Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
        }
        public static string NormalizeFilename(this string s) {
            return s.Replace(':', '_').Replace('?', '_').Replace('\\', '_').Replace('/', '_').Replace('"', '_').Replace('*', '_').Replace('<', '_').Replace('>', '_');
        }
        public class IterationResult {
            public IterationResult Reset() {
                Color = null;
                Abort = false;
                return this;
            }
            public bool Abort;
            public Color? Color;
        }
    }
}