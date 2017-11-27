using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGLTest
{
    class TextureManager
    {
        private Dictionary<string, Texture> _textures= new Dictionary<string, Texture>();

        public Texture getTexture(string texturePath)
        {
            Texture result;
            if (!_textures.TryGetValue(texturePath, out result))
            {
                result = new Texture(texturePath);
                _textures[texturePath] = result;
            }
            return result;
        }
    }
}
