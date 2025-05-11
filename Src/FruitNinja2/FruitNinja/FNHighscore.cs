
// Type: FruitNinja2.FNHighscore
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace FruitNinja2
{
  internal class FNHighscore
  {
    private string user;
    private string displayName;
    private uint userHash;
    private uint rank;
    private int score;

    private FNHighscore()
    {
      this.user = (string) null;
      this.displayName = (string) null;
      this.userHash = 0U;
      this.score = 0;
      this.rank = 0U;
    }

    private FNHighscore(string name, uint hash, int a_score, int a_rank, string display)
    {
      this.user = name;
      this.displayName = display;
      this.userHash = hash;
      this.score = a_score;
      this.rank = (uint) a_rank;
    }

    private bool IsCurrentUser() => false;

    public static bool operator >(FNHighscore b1, FNHighscore b2) => b2.score < b1.score;

    public static bool operator <(FNHighscore b1, FNHighscore b2) => b2.score > b1.score;
  }
}
