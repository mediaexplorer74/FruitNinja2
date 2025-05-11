
// Type: GameManager.PSPEmitterTemplate
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class PSPEmitterTemplate
  {
    public string name;
    public uint hash;
    public float life;
    public byte size_start;
    public byte size_end;
    public byte pad00;
    public byte particle_set_num;
    public List<PSPParticleSet> sets = new List<PSPParticleSet>();

    public bool Ends()
    {
      for (int index = 0; index < (int) this.particle_set_num; ++index)
      {
        if (this.sets[index].time_end <= (ushort) 0 && this.sets[index].number_per_second > (byte) 0)
          return false;
      }
      return true;
    }
  }
}
