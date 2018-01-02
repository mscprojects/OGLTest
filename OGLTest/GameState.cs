using System.Diagnostics;

namespace OGLTest
{
  public class GameState
  {
    private static readonly Stopwatch _stopwatch = new Stopwatch();

    public static void Initialize()
    {
      _stopwatch.Start();
    }

    public static long GlobalTime()
    {
      return _stopwatch.ElapsedMilliseconds;
    }
  }
}