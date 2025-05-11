
// Type: FruitNinja2.SoundEffect
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class SoundEffect
  {
    public string file;
    public float timeStart;
    public float timeEnd;
    public MortarSound sfx;

    public SoundEffect Duplicate()
    {
      return new SoundEffect()
      {
        file = this.file,
        timeStart = this.timeStart,
        timeEnd = this.timeEnd,
        sfx = this.sfx
      };
    }

    public SoundEffect()
    {
      this.sfx = (MortarSound) null;
      this.file = "";
      this.timeStart = 1f;
      this.timeEnd = -1f;
    }

    ~SoundEffect()
    {
      if (this.sfx == null)
        return;
      SoundManager.GetInstance().Release(this.sfx);
      Delete.SAFE_DELETE<MortarSound>(ref this.sfx);
    }

    public void Parse(XElement parent)
    {
      string str = parent.AttributeStr("name");
      if (str != null)
        this.file = str;
      parent.QueryFloatAttribute("timeStart", ref this.timeStart);
      parent.QueryFloatAttribute("timeEnd", ref this.timeEnd);
    }
  }
}
