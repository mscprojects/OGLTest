using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
  public class Chunk
  {
    public const int CHUNK_SIZE = 16;
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
            else if (random.NextDouble() < 0.6)
              _blocks[x, y, z] = new StoneBrickBlock();
            else
              _blocks[x, y, z] = new AirBlock();
          }
        }
      }

      CreateRenderObject();
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

    IBlock GetBlock(int x, int y, int z)
    {
      if (x < 0 || x >= CHUNK_SIZE ||
          y < 0 || y >= CHUNK_SIZE ||
          z < 0 || z >= CHUNK_SIZE)
        return new AirBlock();

      return _blocks[x, y, z];
    }

    List<Vertex> CreateCubeVertices(int x, int y, int z)
    {
      var block = GetBlock(x, y, z);

      if (!block.render())
      {
        return new List<Vertex>();
      }

      var cubeMesh = new CubeMesh
      {
        TextureCoordinates = _textureAtlas.GetTextureCoordinates(block.texture()),
        Position = _chunkPosition + new Vector3(x, y, z)
      };

      cubeMesh.IncludeFaces.Top = !GetBlock(x, y + 1, z).render();
      cubeMesh.IncludeFaces.Bottom = !GetBlock(x, y - 1, z).render();

      cubeMesh.IncludeFaces.Front = !GetBlock(x, y, z - 1).render();
      cubeMesh.IncludeFaces.Back = !GetBlock(x, y, z + 1).render();

      cubeMesh.IncludeFaces.Right = !GetBlock(x + 1, y, z).render();
      cubeMesh.IncludeFaces.Left = !GetBlock(x - 1, y, z).render();

      return cubeMesh.CreateMesh();
    }

    private void CreateRenderObject()
    {
      List<Vertex> chunkVertices = new List<Vertex>();

      for (int x = 0; x < CHUNK_SIZE; x++)
      {
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
          for (int z = 0; z < CHUNK_SIZE; z++)
          {
            var cubeVertices = CreateCubeVertices(x, y, z);
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

    public IBlock BlockAtPosition(Vector3 pos)
    {
      Vector3 blockOffset = pos - _chunkPosition;

      return _blocks[(int)blockOffset.X, (int)blockOffset.Y, (int)blockOffset.Z];
    }

    public void DestroyBlock(Vector3 pos)
    {
      Vector3 blockOffset = pos - _chunkPosition;
      _blocks[(int)blockOffset.X, (int)blockOffset.Y, (int)blockOffset.Z] = new AirBlock();
      CreateRenderObject();
    }
  }
}