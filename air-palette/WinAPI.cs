using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace air_palette
{
    internal static class WinAPI
    {
        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        internal static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
    }
}
