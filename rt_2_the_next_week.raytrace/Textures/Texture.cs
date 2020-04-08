using System;
using System.Drawing;
using System.IO;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Textures
{
    class Texture
        : ITexture
    {
        byte[] data;
        int nx;
        int ny;

        public Texture(byte[] data, int nx, int ny)
        {
            this.data = data;
            this.nx = nx;
            this.ny = ny;
        }

        public static ITexture Create(Stream stream)
        {
            var bm = new Bitmap(stream);
            return Create(bm);
        }

        public static ITexture Create(Bitmap bm)
        {
            byte[] textur_image = new byte[bm.Width * bm.Height * 4];

            // TODO $$$ improve that nested loops

            for (int x = 0; x < bm.Width; ++x)
            {
                for (int y = 0; y < bm.Height; ++y)
                {
                    int i = (y * bm.Width + x) * 4;
                    Color c = bm.GetPixel(x, y);
                    textur_image[i + 0] = c.R;
                    textur_image[i + 1] = c.G;
                    textur_image[i + 2] = c.B;
                    textur_image[i + 3] = c.A;
                }
            }
            /*
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                textur_image = ms.ToArray();
            }
            */

            return Create(textur_image, bm.Width, bm.Height);
        }

        public static ITexture Create(byte[] data, int nx, int ny) => new Texture(data, nx, ny);

        public Vec3 Value(double u, double v, Vec3 p)
        {
            int i = Math.Max(0, Math.Min(nx - 1, (int)(u * nx)));
            int j = Math.Max(0, Math.Min(ny - 1, (int)((1 - v) * ny - 0.001)));
            double r = data[4 * i + 4 * nx * j] / 255.0;
            double g = data[4 * i + 4 * nx * j + 1] / 255.0;
            double b = data[4 * i + 4 * nx * j + 2] / 255.0;
            return Vec3.Create(r, g, b);
        }
    }
}
