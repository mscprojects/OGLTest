using System.Collections.Generic;
using OGLTest.DebugUtilites;
using OGLTest.DebugUtilities;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Graphics;

namespace OGLTest
{
  public class LightController
  {
    private CollisionController _collisionController;
    private List<Light> _lights = new List<Light>();

    public LightController(CollisionController collisionController)
    {
      _collisionController = collisionController;
    }

    public void AddLight(Light l)
    {
      _lights.Add(l);
    }

    public Color4 GetLightAt(Vector3 position)
    {
      // Cast a ray from each light to the position
      foreach (var light in _lights)
      {
        var maxDistance = (position - light.Position).Length;

        var ray = new Ray(position, light.Position - position);

        DebugDrawManager.AddElement(new DebugDrawLine(ray, maxDistance), 10000);

        if (!_collisionController.CollidesWithWorld(ray, maxDistance, out var hit))
        {
          return light.Color;
        }
      }
      return new Color4(100, 100, 100, 255);
    }

    public void Update()
    {
      var newPosition = _lights[0].Position;
      newPosition.X = Utils.Sin(GameState.GlobalTime());
      newPosition.Z = Utils.Cos(GameState.GlobalTime());
      _lights[0].Position = newPosition;
    }
  }
}