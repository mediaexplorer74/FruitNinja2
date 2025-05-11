
// Type: FruitNinja2.DEFAULT_WAVE_INFO
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace FruitNinja2
{
  public class DEFAULT_WAVE_INFO
  {
    public int waveChance;
    public float waveChanceRegrowth;
    public float criticalChance;
    public float dt;
    public float dtInc;
    public float dtSpInc;
    public float beforeDelay;
    public float beforeDelayInc;
    public float nextDelay;
    public float nextDelayInc;
    public float nextDelaySpInc;
    public bool waitForEntities;
    public float speedLoss;
    public int overideProbabilty;
    public int[] players = new int[Game.MAX_PLAYERS + 1];

    public void Reset()
    {
      this.speedLoss = 0.0f;
      this.players[0] = 0;
      this.players[1] = -1;
      this.waveChance = 10;
      this.waveChanceRegrowth = 0.25f;
      this.criticalChance = 1f;
      this.dt = 1f;
      this.dtInc = 0.0f;
      this.dtSpInc = 0.0f;
      this.nextDelay = 0.0f;
      this.nextDelayInc = 0.0f;
      this.nextDelaySpInc = 0.0f;
      this.beforeDelay = 2f;
      this.beforeDelayInc = 0.0f;
      this.overideProbabilty = 100;
      this.waitForEntities = true;
    }

    public DEFAULT_WAVE_INFO() => this.Reset();
  }
}
