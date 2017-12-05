using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    class Chunk
    {
        private const int CHUNK_SIZE = 30;
        private IBlock[,,] _blocks = new IBlock[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

        private Vector3 _chunkPosition;
        private RenderObject _renderObject;
        private TextureAtlas _textureAtlas;

        public Chunk(Vector3 position, TextureAtlas textureAtlas)
        {
            _textureAtlas = textureAtlas;
            _chunkPosition = position * CHUNK_SIZE;

            var random = new Random();
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        if (random.NextDouble() < 0.3)
                            _blocks[x, y, z] = new DirtBlock();
                        else if (random.NextDouble() < 1.0)
                            _blocks[x, y, z] = new StoneBrickBlock();
                        else
                            _blocks[x, y, z] = new AirBlock();
                    }
                }
            }

            createRenderObject();
        }

        bool shouldRenderBlock(int x, int y, int z)
        {
            if (!_blocks[x, y, z].render())
                return false;

            if (x == 0 || x == CHUNK_SIZE - 1 ||
                y == 0 || y == CHUNK_SIZE - 1 ||
                z == 0 || z == CHUNK_SIZE - 1)
                return true;

            if (!_blocks[x + 1, y, z].render())
                return true;
            if (!_blocks[x - 1, y, z].render())
                return true;

            if (!_blocks[x, y + 1, z].render())
                return true;
            if (!_blocks[x, y - 1, z].render())
                return true;

            if (!_blocks[x, y, z + 1].render())
                return true;
            if (!_blocks[x, y, z - 1].render())
                return true;

            return false;
        }

        List<Vertex> createCubeVertices(int x, int y, int z)
        {
            List<Vertex> cubeVertices = new List<Vertex>();

            IBlock block = _blocks[x, y, z];

            if (!shouldRenderBlock(x, y, z))
            {
                return cubeVertices;
            }

            BlockRenderer blockRenderer = new BlockRenderer(_textureAtlas, block);
            cubeVertices = blockRenderer.CreateMesh();

            for (int i = 0; i < cubeVertices.Count; i++)
            {
                Vertex cubeVertex = cubeVertices[i];
                cubeVertex._position += new Vector4(_chunkPosition);
                cubeVertex._position += new Vector4(x, y, z, 1.0f);
                cubeVertices[i] = cubeVertex;
            }

            return cubeVertices;
        }

        private void createRenderObject()
        {
            List<Vertex> chunkVertices = new List<Vertex>();

            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        var cubeVertices = createCubeVertices(x, y, z);
                        chunkVertices.AddRange(cubeVertices);
                    }
                }
            }

            _renderObject = new RenderObject(chunkVertices.ToArray());
        }

        public void Render()
        {
            _textureAtlas.Bind(TextureUnit.Texture0);
            _renderObject.Render();
        }
    }
}