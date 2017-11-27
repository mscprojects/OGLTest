using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    class Texture
    {
        private readonly int _textureID;

        public Texture(string path)
        {
            GL.GenTextures(1, out int textureID);
            _textureID = textureID;

            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            using (var bitmap = new Bitmap(path))
            {
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, 
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    bitmapData.Width, bitmapData.Height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

                bitmap.UnlockBits(bitmapData);
            }

            GL.TextureParameter(_textureID, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TextureParameter(_textureID, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

            GL.TextureParameter(_textureID, TextureParameterName.TextureWrapS, (int) All.ClampToEdge);
            GL.TextureParameter(_textureID, TextureParameterName.TextureWrapT, (int) All.ClampToEdge);

        }

        public void Bind(TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
        }
    }
}
