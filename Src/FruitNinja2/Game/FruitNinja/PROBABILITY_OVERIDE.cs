
// Type: GameManager.PROBABILITY_OVERIDE
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class PROBABILITY_OVERIDE
  {
    public int percentageChance;
    public int perWave;
    public int spawnedThisWave;
    public List<string> typeNames = new List<string>();
    public int[] types = new int[WaveManager.MAX_PROBABILITY_OVERIDE_TYPES];
    public int typeCount;
    public float canSpawnWithPowerup;
    public int waveCount;
    public int numWaves;

    public PROBABILITY_OVERIDE()
    {
      this.waveCount = -1000000;
      this.numWaves = -1;
      this.percentageChance = 0;
      this.perWave = 0;
      this.spawnedThisWave = 0;
      this.typeCount = 0;
      for (int index = 0; index < WaveManager.MAX_PROBABILITY_OVERIDE_TYPES; ++index)
        this.types[index] = -1;
      this.canSpawnWithPowerup = 0.0f;
    }

    public void Parse(XElement element)
    {
      element.QueryIntAttribute("percentageChance", ref this.percentageChance);
      element.QueryIntAttribute("waveCount", ref this.waveCount);
      this.typeCount = StringFunctions.SplitWords(element.AttributeStr("types"), ref this.typeNames);
      element.QueryIntAttribute("perWave", ref this.perWave);
      element.QueryFloatAttribute("disableWhenPowered", ref this.canSpawnWithPowerup);
      element.QueryIntAttribute("numWaves", ref this.numWaves);
    }

    public void SelectType()
    {
      for (int index = 0; index < this.typeCount; ++index)
      {
        uint num1 = StringFunctions.StringHash(this.typeNames[index]);
        uint[] numArray = new uint[2]
        {
          StringFunctions.StringHash("bomb"),
          StringFunctions.StringHash("Bomb")
        };
        uint num2 = StringFunctions.StringHash("1fruit");
        this.types[index] = (int) num1 == (int) numArray[0] || (int) num1 == (int) numArray[1] ? -2 : ((int) num1 != (int) num2 ? Fruit.FruitType(this.typeNames[index]) : Fruit.RandomFruit(false));
      }
    }

    public virtual int GetType() => this.types[Math.g_random.Rand32(this.typeCount)];
  }
}
