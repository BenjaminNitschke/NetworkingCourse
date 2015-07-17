using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    public class Texture
    {
        public Texture(string imageFilename)
	    {
            int textureHandle;
            GL.GenTextures(1, out textureHandle);
            Handle = textureHandle;
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            using (var bitmap = new Bitmap(imageFilename))
            {
                Width = bitmap.Width;
                Height = bitmap.Height;
                var hasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    hasAlpha
                    ? System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    : System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    hasAlpha ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb, Width, Height, 0,
                    hasAlpha ? PixelFormat.Bgra : PixelFormat.Bgr, PixelType.UnsignedByte,
                    bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
        }

        public int Handle { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}