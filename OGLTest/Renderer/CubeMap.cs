using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGLTest.Utilities;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest.Renderer
{
  class CubeMap
  {
    private readonly int _textureID;

    public CubeMap(List<string> faces)
    {
      GL.GenTextures(1, out int textureID);
      _textureID = textureID;

      GL.BindTexture(TextureTarget.TextureCubeMap, _textureID);
      for (int i = 0; i < 6; i++)
      {
        var imageDataLoader = new ImageDataLoader(faces[i]);

        var textureTarget = TextureTarget.TextureCubeMapPositiveX + i;
        GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba,
          imageDataLoader.Width, imageDataLoader.Height, 0,
          PixelFormat.Bgra, PixelType.UnsignedByte, imageDataLoader.Data);
      }

      GL.TextureParameter(_textureID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
      GL.TextureParameter(_textureID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

      GL.TextureParameter(_textureID, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
      GL.TextureParameter(_textureID, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
    }

    public void Bind(TextureUnit textureUnit)
    {
      GL.ActiveTexture(textureUnit);
      GL.BindTexture(TextureTarget.TextureCubeMap, _textureID);
    }

  }
}
