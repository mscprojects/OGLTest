using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    class BlockRenderer
    {
        private float[][] _blockPoints =
        {
            // bottom points
            new float[] {0, 0, 0},
            new float[] {1, 0, 0},
            new float[] {1, 0, 1},
            new float[] {0, 0, 1},

            // top points
            new float[] {0, 1, 0},
            new float[] {1, 1, 0},
            new float[] {1, 1, 1},
            new float[] {0, 1, 1}
        };

        private TextureAtlas _textureAtlas;
        private IBlock _block;

        public BlockRenderer(TextureAtlas textureAtlas, IBlock block)
        {
            _textureAtlas = textureAtlas;
            _block = block;
        }

        public List<Vertex> CreateMesh()
        {
            List<Vertex> vertices = new List<Vertex>();

            // Bottom
            vertices.AddRange(CreateFace(new[] { 0, 1, 2, 3 }, _textureAtlas.GetTextureCoordinates(_block.texture())));
            // Top
            vertices.AddRange(CreateFace(new[] { 4, 7, 6, 5 }, _textureAtlas.GetTextureCoordinates(_block.texture())));
            
            // Sides
            vertices.AddRange(CreateFace(new[] { 0, 4, 5, 1 }, _textureAtlas.GetTextureCoordinates(_block.texture())));
            vertices.AddRange(CreateFace(new[] { 1, 5, 6, 2 }, _textureAtlas.GetTextureCoordinates(_block.texture())));
            vertices.AddRange(CreateFace(new[] { 2, 6, 7, 3 }, _textureAtlas.GetTextureCoordinates(_block.texture())));
            vertices.AddRange(CreateFace(new[] { 3, 7, 4, 0 }, _textureAtlas.GetTextureCoordinates(_block.texture())));

            return vertices;
        }

        private List<Vertex> CreateFace(int[] positionHandles, TextureAtlasCoordinates textureCoordinates)
        {
            List<Vertex> ret = new List<Vertex>
            {
                CreateVertex(_blockPoints[positionHandles[0]], textureCoordinates.uLeft, textureCoordinates.vBottom),
                CreateVertex(_blockPoints[positionHandles[2]], textureCoordinates.uRight, textureCoordinates.vTop),
                CreateVertex(_blockPoints[positionHandles[1]], textureCoordinates.uLeft, textureCoordinates.vTop),

                CreateVertex(_blockPoints[positionHandles[0]], textureCoordinates.uLeft, textureCoordinates.vBottom),
                CreateVertex(_blockPoints[positionHandles[3]], textureCoordinates.uRight, textureCoordinates.vBottom),
                CreateVertex(_blockPoints[positionHandles[2]], textureCoordinates.uRight, textureCoordinates.vTop)
            };

            return ret;
        }

        private Vertex CreateVertex(float[] position, float u, float v)
        {
            return new Vertex(new Vector4(position[0], position[1], position[2], 1.0f),
                Color4.White, new Vector2(u, v));
        }
    }
}
