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

        public Texture(int[] data, int width, int height)
        {
            GL.GenTextures(1, out int textureID);
            _textureID = textureID;

            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                            width, height, 0,
                            OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data);

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


        // Debug textures
        private static Texture _whiteTexture = null;
        public static Texture White()
        {
            if (_whiteTexture == null)
            {
                var white = unchecked((int) 0xFFFFFFFF);
                _whiteTexture = new Texture(new[] {white}, 1, 1);
            }
            return _whiteTexture;
        }

        private static Texture _redTexture = null;
        public static Texture Red()
        {
            if (_redTexture == null)
            {
                var red = unchecked((int)0xFFFF0000);
                _redTexture = new Texture(new[] { red }, 1, 1);
            }
            return _redTexture;
        }

    }
}
