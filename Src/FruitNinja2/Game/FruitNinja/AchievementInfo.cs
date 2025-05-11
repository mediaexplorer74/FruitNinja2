
// Type: GameManager.AchievementInfo
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  public class AchievementInfo
  {
    public string name;
    public string id;
    public uint idHash;
    public Texture texture;
    public string description;
    public int total;
    public int score;
    public bool isGameOver;
    public AchievementUnlockType type;
    public uint modeMask;
    public SpecificOrder specificOrder;

    public AchievementInfo()
    {
      this.texture = (Texture) null;
      this.specificOrder = (SpecificOrder) null;
      this.total = 0;
      this.score = 0;
      this.type = AchievementUnlockType.UNLOCK_TYPE_MAX;
    }
  }
}
