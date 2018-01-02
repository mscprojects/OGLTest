using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OGLTest.Engine;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
  public class World
  {
    private Dictionary<WorldPosition, Chunk> _chunkDictionary = new Dictionary<WorldPosition, Chunk>();
    private LightController _lightController;
    private TextureAtlas _textureAtlas;

    public World()
    {
      _textureAtlas = new TextureAtlas(@"textures\blocks\", 
        new List<string>() {"dirt.png", "sand.png", "stonebrick.png", "hardened_clay_stained_green.png", "hardened_clay_stained_cyan.png"});

      _lightController = new LightController(new CollisionController(this));
      _lightController.AddLight(new Light
      {
        Position = new Vector3(0, 10, 50),
        Color = Color4.White
      });

      AddChunk(new WorldPosition(Vector3.Zero));

      //AddChunk(Vector3.Zero + Vector3.UnitX);
      //AddChunk(Vector3.Zero + Vector3.UnitY);
      //AddChunk(Vector3.Zero + Vector3.UnitZ);

      //AddChunk(Vector3.Zero - Vector3.UnitX);
      //AddChunk(Vector3.Zero - Vector3.UnitY);
      //AddChunk(Vector3.Zero - Vector3.UnitZ);
    }

    public void Render()
    {
      foreach (var chunk in _chunkDictionary)
      {
        chunk.Value.Render();
      }
    }

    public Chunk ChunkAtPosition(WorldPosition pos)
    {
      pos.X = pos.X / Chunk.CHUNK_SIZE;
      pos.Y = pos.Y / Chunk.CHUNK_SIZE;
      pos.Z = pos.Z / Chunk.CHUNK_SIZE;

      if (!_chunkDictionary.ContainsKey(pos))
        return null;

      return _chunkDictionary[pos];
    }

    public IBlock BlockAtPosition(WorldPosition pos)
    {
      var chunk = ChunkAtPosition(pos);

      if (chunk == null)
        return new AirBlock();

      return chunk.BlockAtPosition(pos);
    }

    private void AddChunk(WorldPosition pos)
    {
      var chunk = new Chunk(pos, _textureAtlas, _lightController);
      _chunkDictionary[pos] = chunk;
      chunk.CreateRenderObject();
    }

    public void DestroyBlock(WorldPosition pos)
    {
      var chunk = ChunkAtPosition(pos);

      if (chunk == null)
        return;

      chunk.DestroyBlock(pos);
    }

    public void Update(float time)
    {
      _lightController.Update();
    }
  }
}
