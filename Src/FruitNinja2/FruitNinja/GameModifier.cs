
// Type: FruitNinja2.GameModifier
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public abstract class GameModifier
  {
    protected float m_length;
    protected int m_fruitLength;
    protected float m_currentTime;
    protected bool isOriginal;
    protected float m_waitUntilTime;
    protected bool m_isWaiting;
    public PowerUp m_parent;

    public void Duplicate(GameModifier dest)
    {
      dest.m_length = this.m_length;
      dest.m_fruitLength = this.m_fruitLength;
      dest.m_currentTime = this.m_currentTime;
      dest.isOriginal = this.isOriginal;
      dest.m_waitUntilTime = this.m_waitUntilTime;
      dest.m_isWaiting = this.m_isWaiting;
      dest.m_parent = this.m_parent;
    }

    public GameModifier()
    {
      this.isOriginal = false;
      this.m_length = 0.0f;
      this.m_currentTime = 0.0f;
      this.m_waitUntilTime = -1f;
      this.m_isWaiting = false;
      this.m_parent = (PowerUp) null;
    }

    public void Reset()
    {
      this.m_currentTime = 0.0f;
      this.ResetSpecific();
    }

    public abstract void ResetSpecific();

    public virtual bool Update(float dt)
    {
      if (this.m_isWaiting)
      {
        if ((double) Game.game_work.saveData.timer > (double) this.m_waitUntilTime)
          return false;
        this.ApplyModifier(false, new float?());
        this.m_isWaiting = false;
      }
      if ((double) this.m_currentTime > 0.0)
      {
        this.m_currentTime -= dt;
        if ((double) this.m_currentTime <= 0.0)
          return true;
      }
      return this.UpdateSpecific(dt);
    }

    public abstract bool UpdateSpecific(float dt);

    public virtual void ApplyModifier(bool fromSave, float? length)
    {
      if ((double) this.m_currentTime > 0.0)
        this.m_currentTime += this.m_length;
      else
        this.m_currentTime = this.m_length;
      if (length.HasValue)
        this.m_currentTime = length.Value;
      uint hash1 = StringFunctions.StringHash("overtime");
      uint hash2 = StringFunctions.StringHash("freeze");
      float num = (float) ((double) Game.game_work.saveData.timer + (PowerUpManager.GetInstance().GetActiveSingle(hash1) == null ? 0.0 : 5.0) + (this.m_parent != null && (int) this.m_parent.GetHash() == (int) hash2 || PowerUpManager.GetInstance().GetActiveSingle(hash2) != null ? 50.0 : 0.0));
      if ((double) this.m_currentTime / (double) PowerUpManager.GetInstance().PrevPowerupDtModMultiply() <= (double) num)
        return;
      this.m_currentTime = Math.MAX((float) ((double) num * (double) PowerUpManager.GetInstance().PrevPowerupDtModMultiply() - 0.33300000429153442), 0.1f);
    }

    public virtual void RemoveModifier()
    {
    }

    public virtual int GetType() => -1;

    public void Parse(XElement parent)
    {
      this.isOriginal = true;
      parent.QueryFloatAttribute("length", ref this.m_length);
      parent.QueryFloatAttribute("waitUntilTime", ref this.m_waitUntilTime);
      if ((double) this.m_waitUntilTime > -1.0)
        this.m_isWaiting = true;
      this.ParseSpecific(parent);
    }

    public abstract void ParseSpecific(XElement parent);

    public abstract GameModifier Clone();

    public float GetCurrentTime() => this.m_currentTime;

    public float GetTotalTime() => this.m_length;

    public bool IsWaiting() => this.m_isWaiting;
  }
}
