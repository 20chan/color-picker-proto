using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace air_palette
{
    public static class Utility
    {
        static Bitmap pix = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        internal static Point GetCursorPos()
        {
            var pos = new Point();
            WinAPI.GetCursorPos(ref pos);
            return pos;
        }

        public static Color GetColorUnderMouse()
        {
            return GetColorAt(GetCursorPos());
        }

        public static Color GetColorAt(Point location)
        {
            using (var gdest = Graphics.FromImage(pix))
            using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hSrcDC = gsrc.GetHdc();
                var hDC = gdest.GetHdc();
                int retval = WinAPI.BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                gdest.ReleaseHdc();
                gsrc.ReleaseHdc();
            }

            return pix.GetPixel(0, 0);
        }

        public static void Magnify(Graphics g, int size, int scale)
        {
            var mid = GetCursorPos();
            var lefttop = new Point(mid.X - size / scale / 2, mid.Y - size / scale / 2);
            var _size = new Size(size / scale, size / scale);
            using (var bmp = new Bitmap(size, size))
            using (var gbmp = Graphics.FromImage(bmp))
            {
                gbmp.CopyFromScreen(lefttop, new Point(), _size);
                SetInvert(size / scale / 2, size / scale / 2, bmp, gbmp);
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.SmoothingMode = SmoothingMode.None;
                g.DrawImage(bmp, 0, 0, size * scale, size * scale);
            }

            void SetInvert(int x, int y, Bitmap from, Graphics to)
            {
                var p = from.GetPixel(x, y);
                var n = Color.FromArgb(p.ToArgb() ^ 0xffffff);
                to.FillRectangle(new SolidBrush(n), x, y, 1, 1);
            }
        }

        public static void SetColor(this Graphics g, Color color, int width, int height)
        {
            g.FillRectangle(new SolidBrush(color), 0, 0, width, height);
        }
    }
}
