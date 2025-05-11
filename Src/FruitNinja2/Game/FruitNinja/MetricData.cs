
// Type: GameManager.MetricData
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  public class MetricData
  {
    public int highScore;
    public int totalTotals;
    public SliceTotal[] totals = ArrayInit.CreateFilledArray<SliceTotal>(5);
    public int AchievementsUnlocked;
    public uint[] Achievements = new uint[20];
  }
}
