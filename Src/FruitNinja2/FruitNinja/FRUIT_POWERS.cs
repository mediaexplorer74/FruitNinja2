
// Type: FruitNinja2.FRUIT_POWERS
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class FRUIT_POWERS
  {
    public FRUIT_POWER[] powerUps;
    public int numPowerUpTypes;

    public uint RandomPower()
    {
      int num = Math.g_random.Rand32(this.powerUps[this.numPowerUpTypes - 1].totalChance);
      for (int index = 0; index < this.numPowerUpTypes; ++index)
      {
        if (num < this.powerUps[index].totalChance)
          return this.powerUps[index].powerHash;
      }
      return this.powerUps[0].powerHash;
    }

    public bool AnyActivePowers()
    {
      for (int index = 0; index < this.numPowerUpTypes; ++index)
      {
        if (PowerUpManager.GetInstance().GetActiveSingle(this.powerUps[index].powerHash) != null)
          return true;
      }
      return false;
    }

    public FRUIT_POWERS() => this.numPowerUpTypes = 0;
  }
}
