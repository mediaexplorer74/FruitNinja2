
// Type: FruitNinja2.BonusAwardHud
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class BonusAwardHud
  {
    public string text;
    public int points;
    public int multiplyer;
    public int count;
    public int visiblePoints;
    public Color colour;
    public float numberScale;
    public Color numberColour;
    public Texture texture;

    public BonusAwardHud()
    {
      this.points = 0;
      this.multiplyer = 1;
      this.text = "";
      this.colour = Color.White;
      this.visiblePoints = 0;
      this.texture = (Texture) null;
    }
  }
}
