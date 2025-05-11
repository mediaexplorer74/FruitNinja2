
// Type: GameManager.Initialise
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  internal class Initialise
  {
    private static string[] modeNames = new string[5]
    {
      "CLASSIC",
      "CASINO",
      "ARCADE",
      "ZEN",
      "ALL"
    };
    private static uint[] names = new uint[4]
    {
      StringFunctions.StringHash("CLASSIC"),
      StringFunctions.StringHash("CASINO"),
      StringFunctions.StringHash("ARCADE"),
      StringFunctions.StringHash("ZEN")
    };

    public static string GetModeName(Game.GAME_MODE mode) => Initialise.modeNames[(int) mode];

    public static Game.GAME_MODE ParseGameMode(uint hash)
    {
      for (int gameMode = 0; gameMode < Initialise.names.Length; ++gameMode)
      {
        if ((int) Initialise.names[gameMode] == (int) hash)
          return (Game.GAME_MODE) gameMode;
      }
      return Game.GAME_MODE.GM_MAX;
    }

    public static uint GetModeBitMask(Game.GAME_MODE mode)
    {
      return mode == Game.GAME_MODE.GM_MAX ? uint.MaxValue : 1U << (int) (mode & (Game.GAME_MODE) 31);
    }
  }
}
