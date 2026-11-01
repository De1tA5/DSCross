using System;
using System.Numerics;

namespace DemonShadeCrossMod.Core.Utils
{
    public static class DrawHelper
    {
        /// <summary>
        /// 将HSV转变为RGB
        /// </summary>
        /// <param name="h">色调，取值0-360</param>
        /// <param name="s">饱和度，取值0-1</param>
        /// <param name="v">明度，取值0-1</param>
        /// <returns></returns>
        public static Vector3 HSVToRGB(double h, double s, double v)
        {
            h = ((h % 360) + 360) % 360;
            s = Math.Clamp(s, 0, 1);
            v = Math.Clamp(v, 0, 1);

            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = v - c;

            double r, g, b;

            if (h < 60)
                (r, g, b) = (c, x, 0);
            else if (h < 120)
                (r, g, b) = (x, c, 0);
            else if (h < 180)
                (r, g, b) = (0, c, x);
            else if (h < 240)
                (r, g, b) = (0, x, c);
            else if (h < 300)
                (r, g, b) = (x, 0, c);
            else
                (r, g, b) = (c, 0, x);

            return new Vector3(
                (float)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255)
            );

        }
        public static Vector3 HSVToRGB(Vector3 HSV) => HSVToRGB(HSV.X, HSV.Y, HSV.Z);

        /// <summary>
        /// 将 RGB 颜色值转换为 HSV 颜色空间表示。
        /// </summary>
        /// <param name="r">红色分量，范围为 0 到 255。</param>
        /// <param name="g">绿色分量，范围为 0 到 255。</param>
        /// <param name="b">蓝色分量，范围为 0 到 255。</param>
        /// <returns>包含色相 (H)、饱和度 (S) 和明度 (V) 的Vector3</returns>
        public static Vector3 RGBToHSV(int r, int g, int b)
        {
            double rd = Math.Clamp(r, 0, 255) / 255.0;
            double gd = Math.Clamp(g, 0, 255) / 255.0;
            double bd = Math.Clamp(b, 0, 255) / 255.0;

            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double delta = max - min;

            double h = 0;
            double s = max == 0 ? 0 : delta / max;
            double v = max;

            if (delta != 0)
            {
                if (max == rd)
                    h = 60 * ((gd - bd) / delta % 6);
                else if (max == gd)
                    h = 60 * ((bd - rd) / delta + 2);
                else
                    h = 60 * ((rd - gd) / delta + 4);
            }

            if (h < 0)
                h += 360;

            return new Vector3((float)h, (float)s, (float)v);
        }
        public static Vector3 RGBToHSV(Vector3 RGB) => RGBToHSV((int)RGB.X, (int)RGB.Y, (int)RGB.Z);

    }
}
