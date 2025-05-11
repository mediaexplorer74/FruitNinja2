
// Type: FruitNinja2.TimeModifier
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class TimeModifier : GameModifier
  {
    protected float m_dt;
    protected float m_dtTransitionTime;
    protected float m_currentDt;
    protected bool m_stopClock;
    protected float m_clockSpeed;
    protected float m_addToClock;
    protected int m_addClockWait;

    public override void ResetSpecific() => this.m_currentDt = 0.0f;

    public TimeModifier()
    {
      this.m_addToClock = 0.0f;
      this.m_addClockWait = 0;
      this.m_dt = 1f;
      this.m_stopClock = false;
      this.m_currentDt = 1f;
      this.m_dt = 1f;
      this.m_dtTransitionTime = 0.0f;
      this.m_clockSpeed = 1f;
    }

    public override bool UpdateSpecific(float dt)
    {
      if (this.m_addClockWait > 0)
      {
        --this.m_addClockWait;
        if (this.m_addClockWait <= 0)
        {
          Game.game_work.timeControl.AddTime(this.m_addToClock);
          return true;
        }
      }
      if (this.m_stopClock)
        PowerUpManager.GetInstance().StopClock(this.m_currentTime);
      if ((double) this.m_clockSpeed != 1.0)
      {
        double num = (double) PowerUpManager.GetInstance().SlowClock(this.m_clockSpeed);
      }
      if ((double) this.m_dtTransitionTime > 0.0)
      {
        if ((double) this.m_currentTime <= (double) this.m_dtTransitionTime && (double) this.m_length > 0.0)
          this.m_currentDt = (double) this.m_dt <= 1.0 ? Math.MAX(this.m_currentDt, (float) (1.0 + ((double) this.m_dt - 1.0) * ((double) this.m_currentTime / (double) this.m_dtTransitionTime))) : Math.MIN(this.m_currentDt, (float) (1.0 + ((double) this.m_dt - 1.0) * ((double) this.m_currentTime / (double) this.m_dtTransitionTime)));
        else if ((double) this.m_currentDt < (double) this.m_dt)
        {
          this.m_currentDt += dt / this.m_dtTransitionTime;
          if ((double) this.m_currentDt > (double) this.m_dt)
            this.m_currentDt = this.m_dt;
        }
        else if ((double) this.m_currentDt > (double) this.m_dt)
        {
          this.m_currentDt -= dt / this.m_dtTransitionTime;
          if ((double) this.m_currentDt < (double) this.m_dt)
            this.m_currentDt = this.m_dt;
        }
      }
      else
        this.m_currentDt = this.m_dt;
      PowerUpManager.GetInstance().ApplyDtMod(this.m_currentDt);
      return false;
    }

    public override int GetType() => 0;

    public override void ParseSpecific(XElement parent)
    {
      this.m_stopClock = StringFunctions.CompareWords(parent.AttributeStr("stopClock"), "true");
      parent.QueryFloatAttribute("slowClock", ref this.m_clockSpeed);
      parent.QueryFloatAttribute("addClock", ref this.m_addToClock);
      if ((double) this.m_addToClock != 0.0)
        this.m_addClockWait = 1;
      this.m_currentDt = 1f;
      this.m_dt = 1f;
      this.m_dtTransitionTime = 0.0f;
      XElement element = parent.FirstChildElement("dt_speed");
      if (element == null)
        return;
      element.QueryFloatAttribute("transitionTime", ref this.m_dtTransitionTime);
      element.QueryFloatAttribute("dt", ref this.m_dt);
    }

    private void Duplicate(TimeModifier dest)
    {
      this.Duplicate((GameModifier) dest);
      dest.m_dt = this.m_dt;
      dest.m_dtTransitionTime = this.m_dtTransitionTime;
      dest.m_currentDt = this.m_currentDt;
      dest.m_stopClock = this.m_stopClock;
      dest.m_clockSpeed = this.m_clockSpeed;
      dest.m_addToClock = this.m_addToClock;
      dest.m_addClockWait = this.m_addClockWait;
    }

    public override GameModifier Clone()
    {
      TimeModifier dest = new TimeModifier();
      this.Duplicate(dest);
      dest.isOriginal = false;
      return (GameModifier) dest;
    }

    public bool DoesStopClock()
    {
      return this.m_stopClock || this.m_addClockWait != 0 || (double) this.m_clockSpeed == 0.0;
    }
  }
}
