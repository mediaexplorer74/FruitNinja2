
// Type: GameManager.ItemInfo
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class ItemInfo
  {
    public string name;
    public uint nameHash;
    public int cost;
    public ItemType type;
    public string shopTitle;
    public string shopDescription;
    public string unlockDescription;
    public string unlockTotal;
    public int unlockCountDownFrom;
    public string textureName;
    public Color colour;
    public Color titleColor;
    public bool hasBeenSeen;

    public bool IsLocked() => this.cost > 0;

    public ItemInfo()
    {
      this.hasBeenSeen = true;
      this.name = (string) null;
      this.shopTitle = (string) null;
      this.shopDescription = (string) null;
      this.textureName = (string) null;
      this.unlockDescription = (string) null;
      this.cost = 0;
      this.type = ItemType.ITEM_NONE;
      this.colour = Color.White;
      this.unlockTotal = (string) null;
      this.unlockCountDownFrom = 0;
    }

    public virtual void SetEquipped()
    {
    }

    public virtual void Parse(XElement el)
    {
      XElement element = el.FirstChildElement("requirements");
      if (element != null)
      {
        this.cost = 1;
        element.QueryIntAttribute("coins", ref this.cost);
        this.unlockDescription = element.AttributeStr("description");
        if (this.unlockDescription == null)
          this.unlockDescription = element.Value;
        element.QueryIntAttribute("countDownFrom", ref this.unlockCountDownFrom);
        this.unlockTotal = element.AttributeStr("total");
      }
      this.name = el.AttributeStr("name");
      this.nameHash = StringFunctions.StringHash(this.name);
      this.shopTitle = el.AttributeStr("title");
      XElement xelement = el.FirstChildElement("description");
      if (xelement != null)
        this.shopDescription = xelement.Value;
      this.textureName = el.AttributeStr("texture");
      StringFunctions.ParseColour(ref this.colour, el.AttributeStr("colour"));
      this.titleColor = this.colour;
      StringFunctions.ParseColour(ref this.titleColor, el.AttributeStr("titleolour"));
    }
  }
}
