
// Type: FruitNinja2.ScreenTint
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class ScreenTint
  {
    public float transitionAmount;
    public float transitionTime;
    public float timeStart;
    public float timeEnd;
    public float[] backTint = new float[3];
    public float[] hudTint = new float[3];

    public ScreenTint Duplicate()
    {
      ScreenTint screenTint = new ScreenTint();
      Array.Copy((Array) this.backTint, (Array) screenTint.backTint, this.backTint.Length);
      Array.Copy((Array) this.hudTint, (Array) screenTint.hudTint, this.hudTint.Length);
      screenTint.transitionAmount = this.transitionAmount;
      screenTint.transitionTime = this.transitionTime;
      screenTint.timeStart = this.timeStart;
      screenTint.timeEnd = this.timeEnd;
      return screenTint;
    }

    public ScreenTint()
    {
      for (int index = 0; index < 3; ++index)
      {
        this.backTint[index] = 0.0f;
        this.hudTint[index] = 0.0f;
      }
      this.transitionAmount = 0.0f;
      this.transitionTime = 0.0f;
      this.timeStart = 1f;
      this.timeEnd = 0.0f;
    }

    public void Parse(XElement parent)
    {
      parent.QueryFloatAttribute("timeStart", ref this.timeStart);
      parent.QueryFloatAttribute("timeEnd", ref this.timeEnd);
      StringFunctions.ParseFloats(parent.AttributeStr("tint"), this.backTint, 3);
      for (int index = 0; index < 3; ++index)
        this.hudTint[index] = this.backTint[index];
      StringFunctions.ParseFloats(parent.AttributeStr("backTint"), this.backTint, 3);
      StringFunctions.ParseFloats(parent.AttributeStr("hudTint"), this.hudTint, 3);
      parent.QueryFloatAttribute("transitionTime", ref this.transitionTime);
    }
  }
}
