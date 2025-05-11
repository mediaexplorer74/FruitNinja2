
// Type: FruitNinja2.PSPParticleSet
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace FruitNinja2
{
  public class PSPParticleSet
  {
    public PSPParticleTemplate template_idx;
    public ushort time_start;
    public ushort time_end;
    public byte number_start;
    public byte number_per_second;
    public ushort pad00;
    public Vector3 vel_min;
    public Vector3 vel_max;
  }
}
