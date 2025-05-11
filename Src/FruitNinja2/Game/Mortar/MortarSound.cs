
// Type: Mortar.MortarSound
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework.Audio;

#nullable disable
namespace Mortar
{
  public class MortarSound
  {
    public SoundEffectInstance inst;

    public void SetVolume(float v)
    {
      if (this.inst == null)
        return;
      this.inst.Volume = Math.CLAMP(v, 0.0f, 1f);
    }

    public void Stop(float v)
    {
      if (this.inst == null)
        return;
      this.inst.Stop();
    }

    public void Repeat()
    {
      if (this.inst == null || this.inst.State != SoundState.Stopped)
        return;
      this.inst.Play();
    }
  }
}
