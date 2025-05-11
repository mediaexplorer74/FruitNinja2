
// Type: FruitNinja2.WaveModifier
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class WaveModifier : GameModifier
  {
    protected List<PROBABILITY_OVERIDE> m_overides = new List<PROBABILITY_OVERIDE>();
    protected float m_bombMultiplyer;
    protected float m_bombScale;
    protected float m_fruitMultiplyer;
    protected float m_powerUpDtMod;
    protected int m_overideWave;
    protected float m_criticalChanceMod;

    public static int MAGIC_MAXIE => 10000;

    public WaveModifier()
    {
      this.m_fruitMultiplyer = 1f;
      this.m_bombMultiplyer = 1f;
      this.m_powerUpDtMod = 1f;
      this.m_bombScale = 1f;
      this.m_criticalChanceMod = 1f;
      this.m_overideWave = WaveModifier.MAGIC_MAXIE;
    }

    public override bool UpdateSpecific(float dt)
    {
      double num1 = (double) WaveManager.GetInstance().FruitMultiplyer(this.m_fruitMultiplyer);
      double num2 = (double) WaveManager.GetInstance().BombMultiplyer(this.m_bombMultiplyer);
      double num3 = (double) WaveManager.GetInstance().BombScale(this.m_bombScale);
      double num4 = (double) WaveManager.GetInstance().CriticalChanceMod(this.m_criticalChanceMod);
      PowerUpManager.GetInstance().PowerupDtModMultiply(this.m_powerUpDtMod);
      return false;
    }

    public override void ResetSpecific()
    {
    }

    public override void ParseSpecific(XElement parent)
    {
      parent.QueryFloatAttribute("fruitMultiplyer", ref this.m_fruitMultiplyer);
      parent.QueryFloatAttribute("bombMultiplyer", ref this.m_bombMultiplyer);
      parent.QueryFloatAttribute("bombScale", ref this.m_bombScale);
      parent.QueryFloatAttribute("criticalChance", ref this.m_criticalChanceMod);
      parent.QueryFloatAttribute("powerUpDtMod", ref this.m_powerUpDtMod);
      parent.QueryIntAttribute("waveOveride", ref this.m_overideWave);
      for (XElement element = parent.FirstChildElement("OverideProbability"); element != null; element = parent.NextSiblingElement("OverideProbability"))
      {
        PROBABILITY_OVERIDE probabilityOveride = new PROBABILITY_OVERIDE();
        probabilityOveride.numWaves = 0;
        probabilityOveride.Parse(element);
        this.m_overides.Add(probabilityOveride);
      }
    }

    public override int GetType() => 3;

    private void Duplicate(WaveModifier dest)
    {
      this.Duplicate((GameModifier) dest);
      dest.m_overides = this.m_overides;
      dest.m_bombMultiplyer = this.m_bombMultiplyer;
      dest.m_bombScale = this.m_bombScale;
      dest.m_fruitMultiplyer = this.m_fruitMultiplyer;
      dest.m_powerUpDtMod = this.m_powerUpDtMod;
      dest.m_overideWave = this.m_overideWave;
      dest.m_criticalChanceMod = this.m_criticalChanceMod;
    }

    public override GameModifier Clone()
    {
      WaveModifier dest = new WaveModifier();
      this.Duplicate(dest);
      dest.isOriginal = false;
      return (GameModifier) dest;
    }

    public override void ApplyModifier(bool fromSave, float? length)
    {
      base.ApplyModifier(fromSave, length);
      if (this.m_overideWave < WaveModifier.MAGIC_MAXIE && !fromSave && WaveManager.GetInstance().WaveCount() > this.m_overideWave)
        WaveManager.GetInstance().SetCurrentWave(this.m_overideWave, -1f);
      List<PROBABILITY_OVERIDE> currentOverideList = WaveManager.GetInstance().GetCurrentOverideList();
      foreach (PROBABILITY_OVERIDE probabilityOveride in currentOverideList)
        probabilityOveride.SelectType();
      currentOverideList.InsertRange(0, (IEnumerable<PROBABILITY_OVERIDE>) this.m_overides);
      this.m_overides.Clear();
    }

    public override void RemoveModifier()
    {
      if (WaveManager.GetInstance().WaveCount() >= 0 || WaveManager.GetInstance().WaveCount() < this.m_overideWave)
        return;
      WaveManager.GetInstance().SetCurrentWave(5, 1f);
    }
  }
}
