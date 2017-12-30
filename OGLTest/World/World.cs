using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
  public class World
  {
    private Dictionary<Vector3, Chunk> _chunkDictionary = new Dictionary<Vector3, Chunk>();

    private TextureAtlas _textureAtlas;

    public World()
    {
      _textureAtlas = new TextureAtlas(@"textures\blocks\", 
        new List<string>() {"dirt.png", "sand.png", "stonebrick.png", "hardened_clay_stained_green.png", "hardened_clay_stained_cyan.png"});

      AddChunk(Vector3.Zero);

      AddChunk(Vector3.Zero + Vector3.UnitX);
      AddChunk(Vector3.Zero + Vector3.UnitY);
      AddChunk(Vector3.Zero + Vector3.UnitZ);
      
      AddChunk(Vector3.Zero - Vector3.UnitX);
      AddChunk(Vector3.Zero - Vector3.UnitY);
      AddChunk(Vector3.Zero - Vector3.UnitZ);

    }

    public void Render()
    {
      foreach (var chunk in _chunkDictionary)
      {
        chunk.Value.Render();
      }
    }

    public Chunk ChunkAtPosition(Vector3 pos)
    {
      Vector3 chunkPos = new Vector3(
        Utils.FloorF(pos.X / Chunk.CHUNK_SIZE),
        Utils.FloorF(pos.Y / Chunk.CHUNK_SIZE),
        Utils.FloorF(pos.Z / Chunk.CHUNK_SIZE));

      if (!_chunkDictionary.ContainsKey(chunkPos))
        return null;

      return _chunkDictionary[chunkPos];
    }

    public IBlock BlockAtPosition(Vector3 pos)
    {
      var chunk = ChunkAtPosition(pos);

      if (chunk == null)
        return new AirBlock();

      return chunk.BlockAtPosition(pos);
    }

    private void AddChunk(Vector3 pos)
    {
      _chunkDictionary[pos] = new Chunk(pos, _textureAtlas);
    }

    public void DestroyBlock(Vector3 pos)
    {
      var chunk = ChunkAtPosition(pos);

      if (chunk == null)
        return;

      chunk.DestroyBlock(pos);
    }
  }
}
