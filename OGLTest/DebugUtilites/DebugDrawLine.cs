using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGLTest.Engine;
using OpenTK;
using OpenTK.Graphics;

namespace OGLTest.DebugUtilites
{
  class DebugDrawLine : IDebugElement
  {
    private Vector3 _point1, _point2;
    private RenderObject _renderObject;

    public Color4 Color { get; set; } = Color4.White;

    public DebugDrawLine(Vector3 point1, Vector3 point2)
    {
      _point1 = point1;
      _point2 = point2;
      CreateMesh();
    }

    public DebugDrawLine(Ray r, float distance) : this(r.Start, r.Start + r.Direction * distance) {}

    private void CreateMesh()
    {
      var vertices = new Vertex[2];

      vertices[0] = new Vertex(_point1, Vector3.Zero, Color4.Red, Vector2.Zero);
      vertices[1] = new Vertex(_point2, Vector3.Zero, Color4.Red, Vector2.One);

      _renderObject = new RenderObject(vertices);
    }

    public void Render()
    {
      _renderObject.RenderLines();
    }
  }
}
