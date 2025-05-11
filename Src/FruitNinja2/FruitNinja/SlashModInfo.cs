
// Type: FruitNinja2.SlashModInfo
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class SlashModInfo : ItemInfo
  {
    private Color[] colours;
    private int numColors;
    private int slashType;
    private float speed;
    private string particles;
    private string slashTexture;

    public override void SetEquipped()
    {
      SlashEntity.SetModColors(this.colours, this.numColors, this.slashType, this.speed, this.particles, this.slashTexture);
    }

    public override void Parse(XElement el)
    {
      base.Parse(el);
      this.numColors = 0;
      this.colours = (Color[]) null;
      this.slashType = 0;
      this.ParseSlashModInfo(el.FirstChildElement("slashModInfo"));
      if (this.colours != null)
        return;
      this.numColors = 1;
      this.colours = new Color[this.numColors];
      this.colours[0] = this.colour;
    }

    public void ParseSlashModInfo(XElement slashModInfo)
    {
      if (slashModInfo == null)
        return;
      slashModInfo.QueryFloatAttribute("speed", ref this.speed);
      this.slashType = SlashEntity.ParseSlashModColorType(slashModInfo.AttributeStr("type"));
      this.particles = slashModInfo.AttributeStr("particles");
      string str = slashModInfo.AttributeStr("texture");
      if (str != null)
        this.slashTexture = string.Format("textureswp7/{0}.tex", (object) str);
      for (XElement element = slashModInfo.FirstChildElement("colour"); element != null; element = element.NextSiblingElement("colour"))
        ++this.numColors;
      if (this.numColors <= 0)
        return;
      this.colours = new Color[this.numColors];
      int index = 0;
      for (XElement element = slashModInfo.FirstChildElement("colour"); element != null; element = element.NextSiblingElement("colour"))
      {
        StringFunctions.ParseColour(ref this.colours[index], element.Value);
        ++index;
      }
    }

    public SlashModInfo()
    {
      this.colours = (Color[]) null;
      this.numColors = 0;
      this.slashType = 0;
      this.speed = 1f;
      this.particles = (string) null;
      this.slashTexture = (string) null;
    }
  }
}
