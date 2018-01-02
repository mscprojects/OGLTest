using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGLTest.DebugUtilities
{
  class DebugDrawManager
  {
    private static List<DebugDrawManagerEntry> _debugElements = new List<DebugDrawManagerEntry>();

    public static void AddElement(IDebugElement element, long time)
    {
      _debugElements.Add(new DebugDrawManagerEntry
      {
        Element = element,
        ExpireTime = GameState.GlobalTime() + time
      });
    }

    public static void Update()
    {
      _debugElements.RemoveAll(e => e.ExpireTime < GameState.GlobalTime());
    }

    public static void Render()
    {
      foreach (var debugElement in _debugElements)
      {
        debugElement.Element.Render();
      }
    }
  }
}
