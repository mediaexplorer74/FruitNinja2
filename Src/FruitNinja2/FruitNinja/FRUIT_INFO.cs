
// Type: FruitNinja2.FRUIT_INFO
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class FRUIT_INFO
  {
    public string fruitName;
    public string fruitSingular;
    public string fruitPlural;
    public string fruitNameTotal;
    public string fruitNamePointTotal;
    public string modelName;
    public Color splatColor;
    public float scale;
    public float collision;
    public float hitInfluence;
    public uint[] hash = new uint[6];
    public bool onSide;
    public int factCount;
    public FruitFact[] facts;
    public string factTextureName;
    public Color factColor;
    public bool onlySprinkle;
    public Texture icon;
    public Texture zenIcon;
    public int chance;
    public int totalChance;
    public int totalChanceOfCritical;
    public int score;
    public bool canBeCritical;
    public bool hasSplatSeeds;
    public ImpactSound[] impactSounds;
    public int numImpactSounds;
    public int coinsMin;
    public int coinsMax;
    public FRUIT_POWERS powers;

    public FRUIT_INFO()
    {
      this.powers = (FRUIT_POWERS) null;
      this.hasSplatSeeds = false;
      this.numImpactSounds = 0;
      this.impactSounds = (ImpactSound[]) null;
      this.hitInfluence = 0.75f;
      this.coinsMin = 0;
      this.coinsMax = 0;
      this.score = 1;
      this.chance = 0;
      this.totalChance = 0;
      this.totalChanceOfCritical = 0;
      this.icon = (Texture) null;
      this.zenIcon = (Texture) null;
      this.onlySprinkle = false;
      this.onSide = false;
      this.factCount = 0;
      this.facts = (FruitFact[]) null;
      this.canBeCritical = true;
    }
  }
}
