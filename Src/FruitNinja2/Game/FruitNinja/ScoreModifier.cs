
// Type: GameManager.ScoreModifier
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class ScoreModifier : GameModifier
  {
    protected int m_gainAdd;
    protected int m_gainMultiply;
    protected int m_lossAdd;
    protected int m_lossMultiply;
    protected int m_count;
    protected bool m_deferPoints;
    protected int m_deferedPoints;

    private void Duplicate(ScoreModifier dest)
    {
      this.Duplicate((GameModifier) dest);
      dest.m_gainAdd = this.m_gainAdd;
      dest.m_gainMultiply = this.m_gainMultiply;
      dest.m_lossAdd = this.m_lossAdd;
      dest.m_lossMultiply = this.m_lossMultiply;
      dest.m_count = this.m_count;
      dest.m_deferPoints = this.m_deferPoints;
      dest.m_deferedPoints = this.m_deferedPoints;
    }

    public ScoreModifier()
    {
      this.m_gainAdd = 0;
      this.m_gainMultiply = 1;
      this.m_lossAdd = 0;
      this.m_lossMultiply = 1;
      this.m_count = 0;
      this.m_deferPoints = false;
      this.m_deferedPoints = 0;
    }

    private int AddScoreNomal(int score) => score;

    public override void ApplyModifier(bool fromSave, float? length)
    {
      base.ApplyModifier(fromSave, length);
      if (this.m_deferPoints)
      {
        this.m_parent.AddDeferedPoints(0);
        Game.SetScoreDelegate(new Game.ScoreDelegate(this.DeferPoints));
      }
      ++this.m_count;
    }

    public override void RemoveModifier()
    {
      if (!this.m_deferPoints)
        return;
      Game.SetScoreDelegate();
    }

    public override bool UpdateSpecific(float dt)
    {
      if (!this.m_deferPoints)
      {
        PowerUpManager.GetInstance().AddToScoreGainAdd(this.m_gainAdd * this.m_count);
        PowerUpManager.GetInstance().AddToScoreLossAdd(this.m_lossAdd * this.m_count);
        for (int index = 0; index < this.m_count; ++index)
        {
          PowerUpManager.GetInstance().AddToScoreGainMultiply(this.m_gainMultiply);
          PowerUpManager.GetInstance().AddToScoreLossMultiply(this.m_lossMultiply);
        }
      }
      return false;
    }

    public override void ResetSpecific()
    {
      this.m_gainAdd = 0;
      this.m_gainMultiply = 1;
      this.m_lossAdd = 0;
      this.m_lossMultiply = 1;
    }

    public override void ParseSpecific(XElement parent)
    {
      XElement element = parent.FirstChildElement("multiplier");
      this.ResetSpecific();
      if (element == null)
        return;
      element.QueryIntAttribute("gainAdd", ref this.m_gainAdd);
      element.QueryIntAttribute("gainMultiply", ref this.m_gainMultiply);
      element.QueryIntAttribute("lossAdd", ref this.m_lossAdd);
      element.QueryIntAttribute("lossMultiply", ref this.m_lossMultiply);
      this.m_deferPoints = StringFunctions.CompareWords(element.AttributeStr("deferPoints"), "true");
    }

    public override int GetType() => 2;

    public override GameModifier Clone()
    {
      ScoreModifier dest = new ScoreModifier();
      this.Duplicate(dest);
      dest.isOriginal = false;
      return (GameModifier) dest;
    }

    public int DeferPoints(int points)
    {
      this.m_parent.AddDeferedPoints(points);
      this.m_deferedPoints += points;
      return 0;
    }

    public bool DoesDeferPoint() => this.m_deferPoints;
  }
}
