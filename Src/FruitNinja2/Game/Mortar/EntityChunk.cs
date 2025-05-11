
// Type: Mortar.EntityChunk
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public class EntityChunk
  {
    public int size;
    public string name;
    public uint name_hash;
    public string type;
    public uint type_hash;
    public Vector3 scale;
    public Vector3 rotate;
    public Vector3 translate;
    public Vector3 volume_min;
    public Vector3 volume_max;
  }
}
