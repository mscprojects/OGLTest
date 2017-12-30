using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest.Renderer
{
  class Skybox
  {
    private CubeMap _cubeMap;
    private RenderObject _renderObject;

    public Skybox()
    {
      var faces = new List<string>
      {
        @"textures/skybox/right.jpg",
        @"textures/skybox/left.jpg",
        @"textures/skybox/top.jpg",
        @"textures/skybox/bottom.jpg",
        @"textures/skybox/back.jpg",
        @"textures/skybox/front.jpg"
      };
      _cubeMap = new CubeMap(faces);
      var cubeMesh = new CubeMesh();
      cubeMesh.Scale = new Vector3(50.0f);
      cubeMesh.Position = new Vector3(-25.0f);
      var cubeVertices = cubeMesh.CreateMesh();
      _renderObject = new RenderObject(cubeVertices.ToArray());
    }

    public void Render()
    {
      _cubeMap.Bind(TextureUnit.Texture0);
      _renderObject.Render();
    }
  }
}
