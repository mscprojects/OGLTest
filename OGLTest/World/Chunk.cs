using System;
using System.Collections.Generic;
using OGLTest.Engine;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
  public class Chunk
  {
    private readonly LightController _lightController;

    public const int CHUNK_SIZE = 8;
    private IBlock[,,] _blocks = new IBlock[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

    private WorldPosition _chunkPosition;
    private RenderObject _renderObject;
    private TextureAtlas _textureAtlas;

    public Chunk(WorldPosition position, TextureAtlas textureAtlas, LightController lightController)
    {
      _textureAtlas = textureAtlas;
      _lightController = lightController;
      _chunkPosition = position;

      var random = new Random();
      for (int x = 0; x < CHUNK_SIZE; x++)
      {
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
          for (int z = 0; z < CHUNK_SIZE; z++)
          {
            if (random.NextDouble() < 0.1)
              _blocks[x, y, z] = new DirtBlock();
            else if (random.NextDouble() < 0.2)
              _blocks[x, y, z] = new StoneBrickBlock();
            else
              _blocks[x, y, z] = new AirBlock();
          }
        }
      }
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
        Position = (_chunkPosition + new WorldPosition(x, y, z)).AsVector3()
      };

      // Only Render faces when they are visible
      cubeMesh.IncludeFaces.Top = !GetBlock(x, y + 1, z).render();
      cubeMesh.IncludeFaces.Bottom = !GetBlock(x, y - 1, z).render();
      cubeMesh.IncludeFaces.Front = !GetBlock(x, y, z - 1).render();
      cubeMesh.IncludeFaces.Back = !GetBlock(x, y, z + 1).render();
      cubeMesh.IncludeFaces.Right = !GetBlock(x + 1, y, z).render();
      cubeMesh.IncludeFaces.Left = !GetBlock(x - 1, y, z).render();

      var vertices = cubeMesh.CreateMesh();
      return vertices;
    }

    public void CreateRenderObject()
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

    public IBlock BlockAtPosition(WorldPosition pos)
    {
      var blockOffset = pos - _chunkPosition;
      return _blocks[blockOffset.X, blockOffset.Y, blockOffset.Z];
    }

    public void DestroyBlock(WorldPosition pos)
    {
      var blockOffset = pos - _chunkPosition;
      _blocks[blockOffset.X, blockOffset.Y, blockOffset.Z] = new AirBlock();
      CreateRenderObject();
    }
  }
}