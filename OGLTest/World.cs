using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    class World
    {
        private List<Chunk> _chunks = new List<Chunk>();
        private RenderObject _blockRenderObject;
        private TextureAtlas _textureAtlas;

        public World()
        {
            _textureAtlas = new TextureAtlas(@"textures\blocks\");

            _chunks.Add(new Chunk(Vector3.Zero, _textureAtlas));

            _chunks.Add(new Chunk(Vector3.Zero + Vector3.UnitX, _textureAtlas));
            _chunks.Add(new Chunk(Vector3.Zero + Vector3.UnitY, _textureAtlas));
            _chunks.Add(new Chunk(Vector3.Zero + Vector3.UnitZ, _textureAtlas));

            _chunks.Add(new Chunk(Vector3.Zero - Vector3.UnitX, _textureAtlas));
            _chunks.Add(new Chunk(Vector3.Zero - Vector3.UnitY, _textureAtlas));
            _chunks.Add(new Chunk(Vector3.Zero - Vector3.UnitZ, _textureAtlas));
        }

        public void Render(ShaderProgram program)
        {
            foreach (var chunk in _chunks)
            {
                chunk.Render();
            }
        }
    }
}
