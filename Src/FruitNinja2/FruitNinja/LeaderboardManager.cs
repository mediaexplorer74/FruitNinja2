
// Type: FruitNinja2.LeaderboardManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace FruitNinja2
{
  internal class LeaderboardManager
  {
    protected static LeaderboardManager m_instance;
    private FNHighscoreList[,] m_leaderboards;

    private LeaderboardManager()
    {
      this.m_leaderboards = new FNHighscoreList[4, 4];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
          this.m_leaderboards[index1, index2] = (FNHighscoreList) null;
      }
    }

    ~LeaderboardManager()
    {
    }

    public static LeaderboardManager GetInstance()
    {
      if (LeaderboardManager.m_instance == null)
        LeaderboardManager.m_instance = new LeaderboardManager();
      return LeaderboardManager.m_instance;
    }

    public void Init()
    {
    }

    public void Destroy()
    {
    }

    public FNHighscoreList GetLeaderboard(int mode, int type) => this.m_leaderboards[mode, type];

    public FNHighscoreList RefreshLeaderboard(int mode, int type) => (FNHighscoreList) null;

    public FNHighscoreList NextPage(int mode, int type) => (FNHighscoreList) null;

    public FNHighscoreList PreviousPage(int mode, int type) => (FNHighscoreList) null;

    public void ClearScores(int mode, int type)
    {
    }

    public enum LeaderboardType
    {
      LEADERBOARDTYPE_FRIENDS,
      LEADERBOARDTYPE_GLOBAL,
      LEADERBOARDTYPE_LOCAL,
      LEADERBOARDTYPE_WEEKLY,
      LEADERBOARDTYPE_MAX,
    }
  }
}
