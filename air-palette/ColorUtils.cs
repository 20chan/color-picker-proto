using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace air_palette
{
    public static class ColorUtils
    {
        public static string ToHexString(this Color color)
            => ColorTranslator.ToHtml(color);

        public static string ToHLSString(this Color color)
        {
            var (h, l, s) = color.ToHSL();
            return $"h: {h}, l: {l}, s: {s}";
        }

        public static string ToCMYKString(this Color color)
        {
            var (c, m, y, k) = color.ToCMYK();
            return $"c: {c}, m: {m}, y: {y}, k: {k}";
        }

        public static (byte r, byte g, byte b) GetRGB(this Color color)
            => (color.R, color.G, color.B);

        public static Color FromCMYK(float c, float m, float y, float k)
            => Color.FromArgb(
                Convert.ToInt32(255 * (1 - c) * (1 - k)),
                Convert.ToInt32(255 * (1 - m) * (1 - k)),
                Convert.ToInt32(255 * (1 - y) * (1 - k)));

        public static (float c, float m, float y, float k) ToCMYK(this Color rgb)
        {
            float rf = rgb.R / 255f;
            float gf = rgb.G / 255f;
            float bf = rgb.B / 255f;

            float k = ClampCMYK(1 - Max(Max(rf, gf), bf));
            float c = ClampCMYK((1 - rf - k) / (1 - k));
            float m = ClampCMYK((1 - gf - k) / (1 - k));
            float y = ClampCMYK((1 - bf - k) / (1 - k));

            return (c, m, y, k);
        }

        private static float ClampCMYK(float value)
            => value < 0 || float.IsNaN(value) ? 0 : value;

        public static Color FromHSL(int h, float s, float l)
        {
            if (l == 0)
                return Color.FromArgb(0, 0, 0);

            float p2 = l <= 0.5 ? l * (l + s) : l + s - l * s;
            float p1 = 2 * l - p2;
            int r = ClampHue(p1, p2, h + 120);
            int g = ClampHue(p1, p2, h);
            int b = ClampHue(p1, p2, h - 120);

            return Color.FromArgb(r, g, b);
        }

        private static int ClampHue(float q1, float q2, float hue)
        {
            float res = -1;
            if (hue > 360) hue -= 360;
            if (hue < 0) hue += 360;
            if (hue < 60) res= q1 + (q2 - q1) * hue / 60;
            if (hue < 180) res= q2;
            if (hue < 240) res= q1 + (q2 - q1) * (240 - hue) / 60;
            res = q1;

            return (int)(res * 255f);
        }

        public static (int h, float s, float l) ToHSL(this Color rgb)
        {
            float r = rgb.R / 255f;
            float g = rgb.G / 255f;
            float b = rgb.B / 255f;

            float max = Max(r, Max(g, b));
            float min = Min(r, Min(g, b));

            float diff = max - min;
            float l = (max + min) / 2f;

            if (diff < 0.0001f)
                return (0, 0, l);

            float s = l <= 0.5 ? diff / (max + min) : diff / (2 - max - min);
            float hue = -1;
            if (r == max)
                hue = (g - b) / 6 / diff;
            else if (g == max)
                hue = 1f / 3 + (b - r) / 6 / diff;
            else
                hue = 2f / 3 + (r - g) / 6 / diff;

            if (hue < 0) hue += 1;
            if (hue > 1) hue -= 1;
            int h = (int)hue * 360;

            return (h, s, l);
        }
    }
}
