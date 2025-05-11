
// Type: FruitNinja2.Leaderboards
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Mortar;
using System;

#nullable disable
namespace FruitNinja2
{
  internal class Leaderboards
  {
    private const int ENTRIES_TO_READ = 25;
    public const int LEADERBOARD_TYPE_CLASSIC = 0;
    public const int LEADERBOARD_TYPE_ZEN = 1;
    public const int LEADERBOARD_TYPE_ARCADE = 2;
    public const int LEADERBOARD_TYPE_TOTAL_SCORES = 3;
    public const int LEADERBOARD_TYPE_TOTAL_FRUIT = 4;
    public const int LEADERBOARD_RESULT_NO_PLAYER = 0;
    public const int LEADERBOARD_RESULT_TIMED_OUT = 1;
    public const int LEADERBOARD_RESULT_SUCCESS = 2;
    private static LeaderboardIdentity id;
    private static int gameMode;
    private static Leaderboards.ReadFinishedEventHandler notifier;
    private static LeaderboardReader leaderboardReader;
    private static Leaderboards.ReadMode mode = Leaderboards.ReadMode.None;

    public static bool StartRead(int leaderboard, Leaderboards.ReadFinishedEventHandler callback)
    {
      if (Leaderboards.mode == Leaderboards.ReadMode.Reading)
        return true;
      if (Game.isWP7TrialMode())
        return false;
            
      SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      if (signedInGamer == null)
      {
        Leaderboards.notifier = callback;
        Leaderboards.LeaderboardReadCallback((IAsyncResult) null);
        Leaderboards.notifier = (Leaderboards.ReadFinishedEventHandler) null;
        return false;
      }
      if (!signedInGamer.IsSignedInToLive)
      {
        Leaderboards.notifier = callback;
        Leaderboards.LeaderboardReadCallback((IAsyncResult) null);
        Leaderboards.notifier = (Leaderboards.ReadFinishedEventHandler) null;
        return false;
      }
      bool flag = false;
      Leaderboards.notifier = (Leaderboards.ReadFinishedEventHandler) null;
      Leaderboards.leaderboardReader = (LeaderboardReader) null;
      switch (leaderboard)
      {
        case 0:
          Leaderboards.gameMode = leaderboard;
          Leaderboards.notifier = callback;
          flag = true;
          Leaderboards.mode = Leaderboards.ReadMode.Reading;
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 0);
          LeaderboardReader.BeginRead(Leaderboards.id, (Gamer) signedInGamer, 25, new AsyncCallback(Leaderboards.LeaderboardReadCallback), (object) signedInGamer);
          break;
        case 1:
          Leaderboards.gameMode = leaderboard;
          Leaderboards.notifier = callback;
          flag = true;
          Leaderboards.mode = Leaderboards.ReadMode.Reading;
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 1);
          LeaderboardReader.BeginRead(Leaderboards.id, (Gamer) signedInGamer, 25, new AsyncCallback(Leaderboards.LeaderboardReadCallback), (object) signedInGamer);
          break;
        case 2:
          Leaderboards.gameMode = leaderboard;
          Leaderboards.notifier = callback;
          flag = true;
          Leaderboards.mode = Leaderboards.ReadMode.Reading;
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 2);
          LeaderboardReader.BeginRead(Leaderboards.id, (Gamer) signedInGamer, 25, new AsyncCallback(Leaderboards.LeaderboardReadCallback), (object) signedInGamer);
          break;
        case 3:
          Leaderboards.gameMode = leaderboard;
          Leaderboards.notifier = callback;
          flag = true;
          Leaderboards.mode = Leaderboards.ReadMode.Reading;
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 3);
          LeaderboardReader.BeginRead(Leaderboards.id, (Gamer) signedInGamer, 25, new AsyncCallback(Leaderboards.LeaderboardReadCallback), (object) signedInGamer);
          break;
        case 4:
          Leaderboards.gameMode = leaderboard;
          Leaderboards.notifier = callback;
          flag = true;
          Leaderboards.mode = Leaderboards.ReadMode.Reading;
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 4);
          LeaderboardReader.BeginRead(Leaderboards.id, (Gamer) signedInGamer, 25, new AsyncCallback(Leaderboards.LeaderboardReadCallback), (object) signedInGamer);
          break;
      }
      return flag;
    }

    public static void Write(int mode, long value)
    {
      if (Game.isWP7TrialMode())
        return;
      SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      if (signedInGamer == null || !signedInGamer.IsSignedInToLive)
        return;
      switch (mode)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
          Leaderboards.id = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, mode);
          LeaderboardWriter leaderboardWriter = signedInGamer.LeaderboardWriter;
          if (leaderboardWriter == null)
            break;
          LeaderboardEntry leaderboard = leaderboardWriter.GetLeaderboard(Leaderboards.id);
          if (leaderboard == null)
            break;
          leaderboard.Rating = value;
          leaderboard.Columns.SetValue("TimeStamp", DateTime.Now);
          leaderboard.Columns.SetValue("Outcome", LeaderboardOutcome.Win);
          break;
      }
    }

    private static void LeaderboardReadCallback(IAsyncResult result)
    {
      Leaderboards.leaderboardReader = (LeaderboardReader) null;
      Leaderboards.mode = Leaderboards.ReadMode.None;
      SignedInGamer signedInGamer = (SignedInGamer) null;
      if (result != null)
        signedInGamer = result.AsyncState as SignedInGamer;
      if (signedInGamer != null)
      {
        try
        {
          Leaderboards.leaderboardReader = LeaderboardReader.EndRead(result);
          if (Leaderboards.leaderboardReader != null && Leaderboards.leaderboardReader.Entries.Count > 0)
          {
            for (int index = 0; index < Leaderboards.leaderboardReader.Entries.Count; ++index)
            {
              LeaderboardEntry entry = Leaderboards.leaderboardReader.Entries[index];
              if (string.Compare(entry.Gamer.Gamertag, signedInGamer.Gamertag) == 0)
              {
                int valueInt32 = entry.Columns.GetValueInt32("BestScore");
                switch (Leaderboards.gameMode)
                {
                  case 0:
                    int currentModeHighscore1 = Game.GetCurrentModeHighscore(0);
                    if (currentModeHighscore1 > valueInt32)
                    {
                      Leaderboards.Write(Leaderboards.gameMode, (long) currentModeHighscore1);
                      Leaderboards.StartRead(Leaderboards.gameMode, Leaderboards.notifier);
                      return;
                    }
                    goto label_19;
                  case 1:
                    int currentModeHighscore2 = Game.GetCurrentModeHighscore(3);
                    if (currentModeHighscore2 > valueInt32)
                    {
                      Leaderboards.Write(Leaderboards.gameMode, (long) currentModeHighscore2);
                      Leaderboards.StartRead(Leaderboards.gameMode, Leaderboards.notifier);
                      return;
                    }
                    goto label_19;
                  case 2:
                    int currentModeHighscore3 = Game.GetCurrentModeHighscore(2);
                    if (currentModeHighscore3 > valueInt32)
                    {
                      Leaderboards.Write(Leaderboards.gameMode, (long) currentModeHighscore3);
                      Leaderboards.StartRead(Leaderboards.gameMode, Leaderboards.notifier);
                      return;
                    }
                    goto label_19;
                  case 3:
                    int num1 = DateTime.Now.DayOfYear / 7;
                    int num2 = DateTime.Now.Year - 2000;
                    int bestThisWeek = TheGame.settings.bestThisWeek;
                    int num3 = num2 << 24 | num1 << 16 | bestThisWeek;
                    if (num3 > valueInt32)
                    {
                      Leaderboards.Write(Leaderboards.gameMode, (long) num3);
                      Leaderboards.StartRead(Leaderboards.gameMode, Leaderboards.notifier);
                      return;
                    }
                    goto label_19;
                  case 4:
                    int tf = TheGame.settings.tf;
                    if (tf > valueInt32)
                    {
                      Leaderboards.Write(Leaderboards.gameMode, (long) tf);
                      Leaderboards.StartRead(Leaderboards.gameMode, Leaderboards.notifier);
                      return;
                    }
                    goto label_19;
                  default:
                    goto label_19;
                }
              }
            }
          }
label_19:
          if (Leaderboards.notifier != null)
            Leaderboards.notifier(2);
        }
        catch (Exception ex)
        {
          if (Leaderboards.notifier != null)
            Leaderboards.notifier(1);
        }
      }
      else if (Leaderboards.notifier != null)
        Leaderboards.notifier(0);
      Leaderboards.notifier = (Leaderboards.ReadFinishedEventHandler) null;
    }

    public static LeaderboardReader GetReader() => Leaderboards.leaderboardReader;

    public static void PageUp()
    {
    }

    public static void PageDown()
    {
    }

    public delegate void ReadFinishedEventHandler(int result);

    private enum ReadMode
    {
      None,
      Reading,
    }
  }
}
