
// Type: Mortar.Fps
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public static class Fps
  {
    private const float OneSecond = 1000f;
    private static long time;
    private static int ticks;
    private static int rate;
    private static int min;
    private static int max;

    static Fps() => Fps.Reset();

    public static void Reset()
    {
      Fps.time = 0L;
      Fps.ticks = 0;
      Fps.rate = 0;
      Fps.min = 1000000;
      Fps.max = -1000000;
    }

    public static void Update(GameTime gameTime)
    {
      Fps.time += gameTime.ElapsedGameTime.Ticks;
      if (Fps.time > 10000000L)
      {
        Fps.rate = Fps.ticks;
        Fps.time = 0L;
        Fps.ticks = 0;
        if (Fps.rate < Fps.min)
          Fps.min = Fps.rate;
        if (Fps.rate <= Fps.max)
          return;
        Fps.max = Fps.rate;
      }
      else
        ++Fps.ticks;
    }

    public static int FrameRate => Fps.rate;

    public static int FrameRateMin => Fps.min;

    public static int FrameRateMax => Fps.max;
  }
}
