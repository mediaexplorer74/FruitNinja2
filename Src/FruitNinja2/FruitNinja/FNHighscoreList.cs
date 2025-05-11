
// Type: FruitNinja2.FNHighscoreList
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace FruitNinja2
{
  internal class FNHighscoreList
  {
    private bool m_reorderRanks;
    private bool m_allowDuplicateUsers;

    private bool AddScore(string user, int score, int rank) => false;

    public void ClearScores(int mode, int type)
    {
    }

    public bool GetHighscoreForUser(
      string user,
      FNHighscore userScore,
      FNHighscore before,
      FNHighscore after)
    {
      return false;
    }

    public void PrepareForDataRetrieval()
    {
    }

    public void SetReorderRanks(bool reorderRanks) => this.m_reorderRanks = reorderRanks;

    public bool GetReorderRanks() => this.m_reorderRanks;

    public void SetAllowDuplicateUsers(bool allowDuplicates)
    {
      this.m_allowDuplicateUsers = allowDuplicates;
    }

    public bool AllowDuplicateUsers() => this.m_allowDuplicateUsers;
  }
}
