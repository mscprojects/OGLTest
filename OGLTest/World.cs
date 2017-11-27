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
        private List<IBlock> _blocks = new List<IBlock>();
        private RenderObject _blockRenderObject;
        private TextureManager _textureManager = new TextureManager();

        public World()
        {
            _blockRenderObject = new RenderObject(BlockVertices.Vertices.ToArray());

            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (random.NextDouble() < 0.5)
                        _blocks.Add(new DirtBlock(i, 0, j));
                    else 
                        _blocks.Add(new SandBlock(i, 0, j));
                }
            }
        }

        public void Render(ShaderProgram program)
        {
            foreach (var block in _blocks)
            {
                Texture blockTexture = _textureManager.getTexture(block.texture());
                blockTexture.Bind(TextureUnit.Texture0);

                Matrix4 modelMatrix = Matrix4.Identity;
                modelMatrix *= Matrix4.CreateTranslation(block.position());
                program.setMat4("model", modelMatrix);

                _blockRenderObject.Render();
            }
        }
    }
}
