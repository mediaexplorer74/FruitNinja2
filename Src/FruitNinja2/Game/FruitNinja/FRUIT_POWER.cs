
// Type: GameManager.FRUIT_POWER
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace GameManager
{
  public class FRUIT_POWER
  {
    public uint powerHash;
    public int chance;
    public int totalChance;

    public FRUIT_POWER()
    {
      this.totalChance = 0;
      this.chance = 100;
      this.powerHash = 0U;
    }
  }
}
