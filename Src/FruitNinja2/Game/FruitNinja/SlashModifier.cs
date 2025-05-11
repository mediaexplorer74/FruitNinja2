
// Type: GameManager.SlashModifier
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class SlashModifier : GameModifier
  {
    protected Color[] colours;
    protected int numColours;
    protected int slashType;
    protected float speed;
    protected string particles;
    protected string slashTexture;
    protected uint m_activePowersMask;
    protected bool m_hasBeenApplied;
    private static int referenced_slashMods;

    public SlashModifier()
    {
      this.m_activePowersMask = 0U;
      this.colours = (Color[]) null;
      this.numColours = 0;
      this.slashType = 0;
      this.speed = 1f;
      this.particles = (string) null;
      this.slashTexture = (string) null;
      this.m_hasBeenApplied = false;
    }

    public override void ResetSpecific()
    {
    }

    public override bool UpdateSpecific(float dt)
    {
      SlashEntity.ModPowerMask |= this.m_activePowersMask;
      return false;
    }

    public override void ApplyModifier(bool fromSave, float? length)
    {
      base.ApplyModifier(fromSave, length);
      if (this.colours == null || this.m_hasBeenApplied)
        return;
      this.m_hasBeenApplied = true;
      ++SlashModifier.referenced_slashMods;
      SlashEntity.SetModColors(this.colours, this.numColours, this.slashType, this.speed, this.particles, this.slashTexture);
    }

    public override void RemoveModifier()
    {
      if (!this.m_hasBeenApplied)
        return;
      --SlashModifier.referenced_slashMods;
      if (SlashModifier.referenced_slashMods > 0)
        return;
      ItemManager.GetInstance().SetEquippedItem(ItemType.ITEM_SLASH_MODIFIER, ItemManager.GetInstance().GetEquippedItem(ItemType.ITEM_SLASH_MODIFIER));
    }

    public override int GetType() => 1;

    public override void ParseSpecific(XElement parent)
    {
      XElement element1 = parent;
      if (element1 == null)
        return;
      element1.QueryFloatAttribute("speed", ref this.speed);
      this.slashType = SlashEntity.ParseSlashModColorType(element1.AttributeStr("type"));
      this.particles = element1.AttributeStr("particles");
      string str = element1.AttributeStr("texture");
      if (str != null)
        this.slashTexture = string.Format("textureswp7/{0}.tex", (object) str);
      for (XElement element2 = element1.FirstChildElement("slash_power"); element2 != null; element2 = element2.NextSiblingElement("slash_power"))
        this.m_activePowersMask |= SlashEntity.ParseSlashPowerMask(element2.AttributeStr("type"));
      for (XElement element3 = element1.FirstChildElement("colour"); element3 != null; element3 = element3.NextSiblingElement("colour"))
        ++this.numColours;
      if (this.numColours > 0)
      {
        this.colours = new Color[this.numColours];
        int index = 0;
        for (XElement element4 = element1.FirstChildElement("colour"); element4 != null; element4 = element4.NextSiblingElement("colour"))
        {
          StringFunctions.ParseColour(ref this.colours[index], element4.GetText());
          ++index;
        }
      }
      else
      {
        if (this.slashTexture == null && this.particles == null)
          return;
        this.numColours = 1;
        this.colours = new Color[1];
        this.colours[0] = Color.White;
      }
    }

    private void Duplicate(SlashModifier dest)
    {
      this.Duplicate((GameModifier) dest);
      dest.colours = new Color[this.colours.Length];
      for (int index = 0; index < this.colours.Length; ++index)
        dest.colours[index] = this.colours[index];
      dest.numColours = this.numColours;
      dest.slashType = this.slashType;
      dest.speed = this.speed;
      dest.particles = this.particles;
      dest.slashTexture = this.slashTexture;
      dest.m_activePowersMask = this.m_activePowersMask;
      dest.m_hasBeenApplied = this.m_hasBeenApplied;
    }

    public override GameModifier Clone()
    {
      SlashModifier slashModifier = new SlashModifier();
      slashModifier.isOriginal = false;
      return (GameModifier) slashModifier;
    }
  }
}
