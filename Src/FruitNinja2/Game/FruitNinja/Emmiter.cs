
// Type: GameManager.Emmiter
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class Emmiter
  {
    public uint hash;
    public PSPParticleEmitter emmiter;
    public Vector3 pos;

    public Emmiter Duplicate()
    {
      return new Emmiter()
      {
        hash = this.hash,
        emmiter = this.emmiter,
        pos = this.pos
      };
    }

    public Emmiter()
    {
      this.pos = Vector3.Zero;
      this.hash = 0U;
      this.emmiter = (PSPParticleEmitter) null;
    }

    public void Parse(XElement parent)
    {
      this.pos = Save.ParseVector(parent.AttributeStr("pos"));
      if (parent.Attribute((XName) "particle") == null)
        return;
      this.hash = StringFunctions.StringHash(parent.AttributeStr("particle"));
    }
  }
}
