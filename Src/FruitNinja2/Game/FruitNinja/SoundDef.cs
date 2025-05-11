
// Type: GameManager.SoundDef
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  public static class SoundDef
  {
    public static float DEFAULT_MUSIC_VOL => 1f;

    public static float DEFAULT_SFX_VOL => 1f;

    public static string SND_FAN_FARE => "equip-unlock";

    public static string SND_EQUIP_TICK(int tick)
    {
      if (tick == 0)
        return "equip-screen-move-3";
      return tick != 1 ? "equip-screen-move-1" : "equip-screen-move-2";
    }

    public static string SND_EQUIP_SLASH => "equip-new-sword";

    public static string SND_EQUIP_BACKGROUND => "equip-new-wallpaper";

    public static string SND_PAUSE => "Pause";

    public static string SND_UNPAUSE => "Unpause";

    public static string SND_MENU_BOMB => "menu-bomb";

    public static string SND_EXTRA_LIFE => "extra-life";

    public static string SND_BONUS_DRUM_ROLL => "Bonus-drum-roll";

    public static string SND_BOMB_EXPLODE => "Bomb-explode";

    public static string SND_BOMB_FUSE => "Bomb-Fuse";

    public static string SND_THROW_BOMB => "Throw-bomb";

    public static string SND_THROW_FRUIT => "Throw-fruit";

    public static string SND_GANK => "gank";

    public static string SND_CLEAN_SLICE_1 => "Clean-Slice-1";

    public static string SND_CLEAN_SLICE_2 => "Clean-Slice-2";

    public static string SND_CLEAN_SLICE_3 => "Clean-Slice-3";

    public static string SND_SPLATTER_LARGE_1 => "Splatter-Large-1";

    public static string SND_SPLATTER_LARGE_2 => "Splatter-Large-2";

    public static string SND_SPLATTER_MEDIUM_1 => "Splatter-Medium-1";

    public static string SND_SPLATTER_MEDIUM_2 => "Splatter-Medium-2";

    public static string SND_SPLATTER_SMALL_1 => "Splatter-Small-1";

    public static string SND_SPLATTER_SMALL_2 => "Splatter-Small-2";

    public static string SND_NEW_BEST => "New-best-score";

    public static string SND_COMBO => "Combo";

    public static string SND_TIME_UP => "time-up";

    public static string SND_TIME_TICK => "time-tick";

    public static string SND_TIME_TOCK => "time-tock";

    public static string SND_DRIP => Math.g_random.Rand32(2) == 0 ? "Pulp-drip-2" : "Pulp-drip-1";

    public static string SND_DANANANA_SCHWING => "Game-start";

    public static string SND_VISCERAL_IMPACT
    {
      get
      {
        if (Math.g_random.Rand32(3) == 0)
          return "Visceral-impact-1";
        return Math.g_random.Rand32(2) == 0 ? "Visceral-impact-3" : "Visceral-impact-2";
      }
    }

    public static string MENU_MUSIC_TRACK => "sound/at3music/music_menu.atrac3";

    public static string GAME_MUSIC_TRACK_1 => "sound/at3music/music_game_1.atrac3";

    public static string GAME_MUSIC_TRACK_2 => "sound/at3music/music_game_2.atrac3";

    public static string GAME_MUSIC_TRACK_3 => "sound/at3music/music_game_3.atrac3";
  }
}
