
// Type: GameManager.ScreenEffect
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class ScreenEffect
  {
    private List<Emmiter> m_emmiters = new List<Emmiter>();
    private List<EffectImage> m_effectImages = new List<EffectImage>();
    private List<ScreenTint> m_screenTints = new List<ScreenTint>();
    private List<SoundEffect> m_sounds = new List<SoundEffect>();
    private string m_name;
    private uint m_nameHash;
    public PowerUp m_parent;
    public float m_time;
    public float m_totalTime;

    public void Duplicate(ScreenEffect dest)
    {
      foreach (Emmiter emmiter in this.m_emmiters)
        dest.m_emmiters.Add(emmiter.Duplicate());
      foreach (EffectImage effectImage in this.m_effectImages)
        dest.m_effectImages.Add(effectImage.Duplicate());
      foreach (ScreenTint screenTint in this.m_screenTints)
        dest.m_screenTints.Add(screenTint.Duplicate());
      foreach (SoundEffect sound in this.m_sounds)
        dest.m_sounds.Add(sound.Duplicate());
      dest.m_name = this.m_name;
      dest.m_nameHash = this.m_nameHash;
      dest.m_parent = this.m_parent;
      dest.m_time = this.m_time;
      dest.m_totalTime = this.m_totalTime;
    }

    public ScreenEffect()
    {
      this.m_parent = (PowerUp) null;
      this.m_time = 0.0f;
      this.m_totalTime = 0.0f;
    }

    public void Deactivate()
    {
      foreach (Emmiter emmiter in this.m_emmiters)
      {
        if (emmiter.emmiter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(emmiter.emmiter);
          emmiter.emmiter = (PSPParticleEmitter) null;
        }
      }
      foreach (EffectImage effectImage in this.m_effectImages)
      {
        if (effectImage.control != null)
        {
          if (effectImage.deferer)
          {
            if ((double) this.m_parent.GetCurrentTimeProgress() <= 0.0099999997764825821)
              ((ScoreMultiplyerBoard) effectImage.control).m_finalScore = this.m_parent.GetDeferedPoints() * 2;
            else
              ((ScoreMultiplyerBoard) effectImage.control).m_finalScore = 0;
            ((ScoreMultiplyerBoard) effectImage.control).m_moveFrom = effectImage.control.m_pos;
            ((ScoreMultiplyerBoard) effectImage.control).m_power = (PowerUp) null;
          }
          else
            effectImage.control.m_terminate = true;
        }
      }
      this.m_effectImages.Clear();
      this.m_screenTints.Clear();
      this.m_emmiters.Clear();
      this.m_sounds.Clear();
    }

    public void Update(float dt, float timeLeft, float totalTime)
    {
      if ((double) this.m_time > 0.0)
      {
        timeLeft = this.m_time;
        this.m_time -= dt;
        totalTime = this.m_totalTime;
      }
      foreach (EffectImage effectImage in this.m_effectImages)
      {
        float max = effectImage.timeStart * totalTime;
        float min = effectImage.timeEnd * totalTime;
        if (Game2.game_work.hud != null && !effectImage.added)
        {
          effectImage.added = true;
          Game2.game_work.hud.AddControl((HUDControl) effectImage.control);
        }
        bool flag = true;
        if ((double) effectImage.transitionTime > 0.0)
        {
          if ((double) timeLeft <= (double) max)
          {
            if ((double) timeLeft > (double) min + (double) effectImage.transitionTime)
            {
              effectImage.transitionAmount = Mortar.Math.MIN(1f, effectImage.transitionAmount + dt / effectImage.transitionTime);
            }
            else
            {
              flag = false;
              effectImage.transitionAmount = Mortar.Math.CLAMP((timeLeft - min) / effectImage.transitionTime, 0.0f, 1f);
            }
          }
          else
            effectImage.transitionAmount = 0.0f;
        }
        else
          effectImage.transitionAmount = Mortar.Math.BETWEEN(timeLeft, min, max) ? 1f : 0.0f;

        float num1 = 1f - effectImage.transitionAmount;
        float num2 = num1 * num1;

        effectImage.control.m_pos = Vector3.Add(effectImage.pos,
            Vector3.Multiply(flag ? effectImage.transitionMoveIn : effectImage.transitionMoveOut,
            num2));
        if (((int) effectImage.transitionMask & 1) != 0)
          effectImage.control.m_scale = Vector3.Multiply(effectImage.scale, 1f - num2);
        else
          effectImage.control.m_scale = effectImage.scale;

        if (((int) effectImage.transitionMask & 2) != 0)
          effectImage.control.m_color.A = (byte) ((double) effectImage.colour.A 
                        * (double) effectImage.transitionAmount);

        effectImage.pulse += (ushort) ((double) Mortar.Math.DEGREE_TO_IDX(180f)
                    * (double) dt * (double) effectImage.pulseSpeed);
        float num3 = Mortar.Math.SinIdx(effectImage.pulse);

        float num4 = (double) num3 <= 0.0 
                    ? num3 * effectImage.pulseScaleNegative
                    : num3 * effectImage.pulseScalePositive;
        HUDControl3d control = effectImage.control;
        control.m_scale = Vector3.Multiply(control.m_scale, num4 + 1f);
      }
      foreach (ScreenTint screenTint in this.m_screenTints)
      {
        float num5 = screenTint.timeStart * totalTime;
        float num6 = screenTint.timeEnd * totalTime;
        screenTint.transitionAmount = (double) screenTint.transitionTime <= 0.0 ? 1f : ((double) timeLeft > (double) num5 ? 0.0f : ((double) timeLeft <= (double) num6 + (double) screenTint.transitionTime ? Mortar.Math.CLAMP((timeLeft - num6) / screenTint.transitionTime, 0.0f, 1f) : Mortar.Math.MIN(1f, screenTint.transitionAmount + dt / screenTint.transitionTime)));
        if (Game2.game_work.hud != null)
        {
          for (int index = 0; index < 3; ++index)
          {
            Game2.game_work.hud.m_backTint[index] *= Mortar.Math.CLAMP((float) (1.0 + ((double) screenTint.backTint[index] - 1.0) * (double) screenTint.transitionAmount), 0.0f, 1f);
            Game2.game_work.hud.m_backTint[index] *= Mortar.Math.CLAMP((float) (1.0 + ((double) screenTint.backTint[index] - 1.0) * (double) screenTint.transitionAmount), 0.0f, 1f);
            Game2.game_work.hud.m_tint[index] *= Mortar.Math.CLAMP((float) (1.0 + ((double) screenTint.hudTint[index] - 1.0) * (double) screenTint.transitionAmount), 0.0f, 1f);
            Game2.game_work.hud.m_tint[index] *= Mortar.Math.CLAMP((float) (1.0 + ((double) screenTint.hudTint[index] - 1.0) * (double) screenTint.transitionAmount), 0.0f, 1f);
          }
        }
      }
      foreach (Emmiter emmiter in this.m_emmiters)
      {
        if (emmiter.emmiter != null && (double) timeLeft < 0.800000011920929)
          emmiter.emmiter.rateScale = 0.0f;
      }
      int index1 = 0;
      while (index1 < this.m_sounds.Count)
      {
        SoundEffect sound = this.m_sounds[index1];
        float num7 = sound.timeStart * totalTime;
        float num8 = sound.timeEnd * totalTime;
        if ((double) timeLeft <= (double) num7)
        {
          if ((double) num8 >= 0.0)
          {
            sound.timeStart = 100f;
            if (sound.sfx == null)
              sound.sfx = SoundManager.GetInstance().SFXPlay(sound.file);
            if ((double) timeLeft < (double) num8)
            {
              this.m_sounds.RemoveAt(index1);
              continue;
            }
          }
          else
          {
            SoundManager.GetInstance().SFXPlay(sound.file);
            this.m_sounds.RemoveAt(index1);
            continue;
          }
        }
        ++index1;
      }
    }

    public void Activate()
    {
      foreach (Emmiter emmiter in this.m_emmiters)
      {
        if (PSPParticleManager.GetInstance().EmitterExists(emmiter.hash))
        {
          emmiter.emmiter = PSPParticleManager.GetInstance().AddEmitter(emmiter.hash, (Action<PSPParticleEmitter>) null);
          if (emmiter.emmiter != null)
          {
            emmiter.emmiter.pos = emmiter.pos;
            if (emmiter.emmiter.tmplt.Ends())
              emmiter.emmiter = (PSPParticleEmitter) null;
          }
        }
      }
      foreach (EffectImage effectImage in this.m_effectImages)
      {
        if (effectImage.deferer)
          effectImage.control = (HUDControl3d) new ScoreMultiplyerBoard()
          {
            m_power = this.m_parent
          };
        else
          effectImage.control = new HUDControl3d();
        effectImage.control.m_texture = effectImage.texture;
        effectImage.control.m_pos = Vector3.Add(effectImage.pos, effectImage.transitionMoveIn);
        effectImage.control.m_color = effectImage.colour;
        effectImage.control.m_drawOrder = effectImage.drawOrder;
        if (((int) effectImage.transitionMask & 1) != 0)
          effectImage.control.m_scale = Vector3.Zero;
        else
          effectImage.control.m_scale = effectImage.scale;
        if (((int) effectImage.transitionMask & 2) != 0)
          effectImage.control.m_color.A = (byte) 0;
        if (Game2.game_work.hud != null)
          Game2.game_work.hud.AddControl((HUDControl) effectImage.control);
        else
          effectImage.added = false;
      }
    }

    public void Parse(XElement parent)
    {
      this.m_name = parent.AttributeStr("name") ?? "void";
      this.m_nameHash = StringFunctions.StringHash(this.m_name);
      parent.QueryFloatAttribute("length", ref this.m_totalTime);
      if ((double) this.m_totalTime > 0.0)
        this.m_time = this.m_totalTime;
      for (XElement xelement = parent.FirstChildElement(); xelement != null; xelement = xelement.NextSiblingElement())
      {
        string str = xelement.AttributeStr("hardware");
        switch (str)
        {
          case "fast":
            if (!Game2.IsFastHardware())
              break;
            goto default;
          default:
            if (str == null || !(str == "slow") || !Game2.IsFastHardware())
            {
              if (xelement.Name == (XName) "image")
              {
                EffectImage effectImage = new EffectImage();
                effectImage.Parse(xelement);
                this.m_effectImages.Add(effectImage);
                break;
              }
              if (xelement.Name == (XName) "emmiter")
              {
                Emmiter emmiter = new Emmiter();
                emmiter.Parse(xelement);
                this.m_emmiters.Add(emmiter);
                break;
              }
              if (xelement.Name == (XName) "tint")
              {
                ScreenTint screenTint = new ScreenTint();
                screenTint.Parse(xelement);
                this.m_screenTints.Add(screenTint);
                break;
              }
              if (xelement.Name == (XName) "sound")
              {
                SoundEffect soundEffect = new SoundEffect();
                soundEffect.Parse(xelement);
                this.m_sounds.Add(soundEffect);
                break;
              }
              break;
            }
            break;
        }
      }
    }

    public uint GetNameHash() => this.m_nameHash;

    public string GetName() => this.m_name;
  }
}
