
// Type: FruitNinja2.PowerUpManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  internal class PowerUpManager
  {
    private const int BAR_WIDTH = 110;
    private Dictionary<uint, PowerUp> m_powerups = new Dictionary<uint, PowerUp>();
    private List<PowerUp> m_activePowerUps = new List<PowerUp>();
    private Dictionary<uint, PowerUp> m_activeSinglePowerUps = new Dictionary<uint, PowerUp>();
    private Dictionary<uint, ScreenEffect> m_screenEffects = new Dictionary<uint, ScreenEffect>();
    private List<ScreenEffect> m_activeScreenEffects = new List<ScreenEffect>();
    private List<PowerUp> m_purchasable = new List<PowerUp>();
    private PowerUp m_currentTimedPower;
    private float m_dtMod;
    private float m_totalStopClockTime;
    private float m_clockSpeed;
    private float m_powerUpDtMod;
    private float m_previousPowerUpDtMod;
    private int m_scoreGainMultiply;
    private int m_scoreGainAdd;
    private int m_scoreLossMultiply;
    private int m_scoreLossAdd;
    private float m_currentTimedPowerProgress;
    private static PowerUpManager instance = new PowerUpManager();
    private static Texture s_bar = (Texture) null;

    private void ActivatePurchase(PowerUp power)
    {
      power.Deactivate(true);
      PowerUp powerUp;
      if (this.m_powerups.TryGetValue(power.GetHash(), out powerUp))
      {
        foreach (GameModifier gameModifier in powerUp.GetFirstMod())
        {
          GameModifier mod = gameModifier.Clone();
          power.AddModifier(mod);
        }
        foreach (GameModifier gameModifier in powerUp.GetFirstMod())
        {
          if (!gameModifier.IsWaiting())
            gameModifier.ApplyModifier(false, new float?());
        }
      }
      --power.GetPurchasableInfo().CurrentGames;
    }

    public PowerUpManager()
    {
      this.m_powerUpDtMod = 1f;
      this.m_previousPowerUpDtMod = this.m_powerUpDtMod;
    }

    public static PowerUpManager GetInstance() => PowerUpManager.instance;

    public void SetDefaults()
    {
      this.m_powerUpDtMod = 1f;
      this.m_dtMod = 1f;
      this.m_totalStopClockTime = 0.0f;
      this.m_clockSpeed = 1f;
      SlashEntity.ModPowerMask = 0U;
      this.m_currentTimedPowerProgress = 0.0f;
      this.m_currentTimedPower = (PowerUp) null;
      this.ClearScoreMultipliers();
      for (int index = 0; index < 3; ++index)
      {
        Game.game_work.hud.m_backTint[index] = 1f;
        Game.game_work.hud.m_tint[index] = 1f;
      }
    }

    public void Load()
    {
      PowerUpManager.s_bar = TextureManager.GetInstance().Load("textureswp7/hud_power_meter.tex");
      XDocument element1 = MortarXml.Load("xmlwp7/powerUpList.xml");
      this.m_powerups.Clear();
      this.m_purchasable.Clear();
      if (element1 == null)
        throw new Exception("No powerup data");
      if (false)
        return;
      XElement element2 = element1.FirstChildElement("powerInfoFile");
      for (XElement xelement = element2.FirstChildElement("power"); xelement != null; xelement = xelement.NextSiblingElement("power"))
      {
        PowerUp powerUp = new PowerUp();
        powerUp.Parse(xelement);
        this.m_powerups[powerUp.GetHash()] = powerUp;
        if (powerUp.Purchaseable() != 0)
          this.m_purchasable.Add(powerUp);
      }
      for (XElement xelement = element2.FirstChildElement("effect"); xelement != null; xelement = xelement.NextSiblingElement("effect"))
      {
        if (xelement.Attribute((XName) "name") != null)
        {
          ScreenEffect screenEffect = new ScreenEffect();
          screenEffect.Parse(xelement);
          this.m_screenEffects[screenEffect.GetNameHash()] = screenEffect;
        }
      }
    }

    public void Reset() => this.Reset(false);

    public void Reset(bool newGame)
    {
      this.m_powerUpDtMod = 1f;
      this.m_previousPowerUpDtMod = this.m_powerUpDtMod;
      this.m_dtMod = 1f;
      this.m_totalStopClockTime = 0.0f;
      this.m_clockSpeed = 1f;
      SlashEntity.ModPowerMask = 0U;
      this.m_currentTimedPowerProgress = 0.0f;
      this.m_currentTimedPower = (PowerUp) null;
      this.ClearScoreMultipliers();
      for (int index = 0; index < 3; ++index)
      {
        Game.game_work.hud.m_backTint[index] = 1f;
        Game.game_work.hud.m_tint[index] = 1f;
      }
      if (newGame)
        Game.game_work.timeControl.Reset();
      int index1 = 0;
      while (index1 < this.m_activePowerUps.Count)
      {
        PowerUp activePowerUp = this.m_activePowerUps[index1];
        if (activePowerUp.Purchaseable() != 0)
        {
          activePowerUp.Deactivate(true);
          if (newGame)
          {
            this.ActivatePurchase(activePowerUp);
            ++index1;
            continue;
          }
          if (activePowerUp.GetPurchasableInfo().CurrentGames > 0)
          {
            ++index1;
            continue;
          }
        }
        this.m_activeSinglePowerUps.Remove(activePowerUp.GetHash());
        activePowerUp.Deactivate();
        activePowerUp.Release();
        this.m_activePowerUps.RemoveAt(index1);
      }
      foreach (KeyValuePair<uint, PowerUp> powerup in this.m_powerups)
      {
        if (powerup.Value.IsAuto())
          this.ActivatePower(powerup.Key);
      }
    }

    public void ClearTimedPowers()
    {
      this.m_currentTimedPowerProgress = 0.0f;
      this.m_currentTimedPower = (PowerUp) null;
      int index = 0;
      while (index < this.m_activePowerUps.Count)
      {
        PowerUp activePowerUp = this.m_activePowerUps[index];
        if (activePowerUp.Purchaseable() != 0 || !activePowerUp.IsTimed())
        {
          ++index;
        }
        else
        {
          this.m_activeSinglePowerUps.Remove(activePowerUp.GetHash());
          activePowerUp.Deactivate();
          activePowerUp.Release();
          this.m_activePowerUps.RemoveAt(index);
        }
      }
    }

    public void Update(float dt)
    {
      float num1 = dt * this.m_previousPowerUpDtMod;
      this.m_powerUpDtMod = 1f;
      this.m_dtMod = 1f;
      this.m_totalStopClockTime = 0.0f;
      this.m_clockSpeed = 1f;
      SlashEntity.ModPowerMask = 0U;
      this.m_currentTimedPowerProgress = 0.0f;
      this.m_currentTimedPower = (PowerUp) null;
      this.ClearScoreMultipliers();
      for (int index = 0; index < 3; ++index)
      {
        Game.game_work.hud.m_backTint[index] = 1f;
        Game.game_work.hud.m_tint[index] = 1f;
      }
      int num2 = 0;
      int index1 = 0;
      while (index1 < this.m_activePowerUps.Count)
      {
        PowerUp activePowerUp = this.m_activePowerUps[index1];
        if (activePowerUp.Update(activePowerUp.Purchaseable() == 0 ? num1 : dt))
        {
          this.m_activeSinglePowerUps.Remove(activePowerUp.GetHash());
          activePowerUp.Deactivate();
          activePowerUp.Release();
          this.m_activePowerUps.RemoveAt(index1);
        }
        else
        {
          float currentTimeProgress = activePowerUp.GetCurrentTimeProgress();
          if ((double) currentTimeProgress > (double) this.m_currentTimedPowerProgress)
          {
            if (activePowerUp.Purchaseable() != 0)
            {
              if ((double) this.m_currentTimedPowerProgress < 1.0 / 1000.0)
                this.m_currentTimedPowerProgress = 1f / 1000f;
            }
            else
            {
              this.m_currentTimedPower = activePowerUp;
              this.m_currentTimedPowerProgress = currentTimeProgress;
            }
          }
          float num3 = -55f * (float) (this.GetNumActiveTimedPowers() - 1) + (float) (110 * num2);
          activePowerUp.m_barX += (float) (((double) num3 - (double) activePowerUp.m_barX) * 0.20000000298023224);
          if (activePowerUp.IsSpecial())
            ++num2;
          ++index1;
        }
      }
      int index2 = 0;
      while (index2 < this.m_activeScreenEffects.Count)
      {
        ScreenEffect activeScreenEffect = this.m_activeScreenEffects[index2];
        activeScreenEffect.Update(dt, 0.0f, 0.0f);
        if ((double) activeScreenEffect.m_time <= 0.0)
        {
          activeScreenEffect.Deactivate();
          this.m_activeScreenEffects.RemoveAt(index2);
        }
        else
          ++index2;
      }
      this.m_previousPowerUpDtMod = this.m_powerUpDtMod;
    }

    public void Draw()
    {
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
        activePowerUp.DrawBar();
    }

    public PowerUp ActivatePower(uint hash) => this.ActivatePower(hash, Vector3.Zero);

    public PowerUp ActivatePower(uint hash, Vector3 pos)
    {
      return this.ActivatePower(hash, pos, new float?());
    }

    public PowerUp ActivatePower(uint hash, Vector3 pos, float? fromSave)
    {
      PowerUp powerUp1;
      if (!this.m_powerups.TryGetValue(hash, out powerUp1))
        return (PowerUp) null;
      PowerUp powerUp2 = powerUp1;
      if (this.m_activeSinglePowerUps.TryGetValue(hash, out powerUp1))
      {
        float longestMod = powerUp1.GetLongestMod();
        powerUp1.Activate(false, fromSave.HasValue, pos, new float?(longestMod));
        return powerUp1;
      }
      PowerUp powerUp3 = powerUp2.Clone();
      this.m_activePowerUps.Add(powerUp3);
      if (this.GetNumActiveTimedPowers() != 0 && !powerUp3.IsAuto() && powerUp3.Purchaseable() == 0 && !fromSave.HasValue)
      {
        float num = powerUp3.GetLongestMod();
        foreach (PowerUp activePowerUp in this.m_activePowerUps)
        {
          if (activePowerUp.IsSpecial())
          {
            float longestMod = activePowerUp.GetLongestMod();
            if ((double) longestMod < (double) num)
              num = longestMod;
          }
        }
        foreach (PowerUp activePowerUp in this.m_activePowerUps)
        {
          if (activePowerUp.IsSpecial() && activePowerUp != powerUp3)
            activePowerUp.Activate(false, false, Vector3.Zero, new float?(num));
        }
        powerUp3.Activate(true, false, pos, new float?(num));
      }
      else
        powerUp3.Activate(true, fromSave.HasValue, pos, fromSave);
      powerUp3.m_barX = 55f * (float) this.GetNumActiveTimedPowers();
      if (powerUp3.IsSingle())
        this.m_activeSinglePowerUps[hash] = powerUp3;
      return powerUp3;
    }

    public float GetDtMod() => this.m_dtMod;

    public void StopClock(float time) => this.m_totalStopClockTime += time;

    public float SlowClock() => this.SlowClock(1f);

    public float SlowClock(float val) => this.m_clockSpeed *= val;

    public float GetStopClockAmount() => this.m_totalStopClockTime;

    public float GetCurrentTimedPowerProgress() => this.m_currentTimedPowerProgress;

    public void ApplyDtMod(float mod) => this.m_dtMod *= mod;

    public int GetScoreGainMultiplier() => this.m_scoreGainMultiply * this.m_scoreGainAdd;

    public int GetScoreLossMultiplier() => this.m_scoreLossMultiply * this.m_scoreLossAdd;

    public void AddToScoreGainAdd(int multiplier) => this.m_scoreGainAdd += multiplier;

    public void AddToScoreLossAdd(int multiplier) => this.m_scoreLossAdd += multiplier;

    public void AddToScoreGainMultiply(int multiplier) => this.m_scoreGainMultiply *= multiplier;

    public void AddToScoreLossMultiply(int multiplier) => this.m_scoreLossMultiply *= multiplier;

    public void ClearScoreMultipliers()
    {
      this.m_scoreGainAdd = 1;
      this.m_scoreLossAdd = 1;
      this.m_scoreGainMultiply = 1;
      this.m_scoreLossMultiply = 1;
    }

    public void Release()
    {
      PowerUpManager.s_bar = (Texture) null;
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
        activePowerUp.Release();
      foreach (KeyValuePair<uint, PowerUp> powerup in this.m_powerups)
        powerup.Value.Release();
      foreach (KeyValuePair<uint, ScreenEffect> screenEffect in this.m_screenEffects)
        screenEffect.Value.Deactivate();
      this.m_screenEffects.Clear();
      this.m_activeScreenEffects.Clear();
    }

    public float PrevPowerupDtModMultiply() => this.m_previousPowerUpDtMod;

    public void PowerupDtModMultiply(float value) => this.m_powerUpDtMod *= value;

    public PowerUp GetActiveSingle(uint hash)
    {
      PowerUp powerUp = (PowerUp) null;
      return this.m_activeSinglePowerUps.TryGetValue(hash, out powerUp) ? powerUp : (PowerUp) null;
    }

    public void SaveActivePowerUps(XElement root)
    {
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
      {
        XElement content = new XElement((XName) "power");
        content.SetAttributeValue((XName) "name", (object) activePowerUp.GetName());
        content.SetAttributeValue((XName) "time", (object) activePowerUp.GetCurrentTime());
        content.SetAttributeValue((XName) "totalTime", (object) activePowerUp.GetTotalTime());
        if (activePowerUp.GetDeferedPoints() >= 0)
          content.SetAttributeValue((XName) "deferedPoints", (object) activePowerUp.GetDeferedPoints());
        root.Add((object) content);
      }
    }

    public void LoadActivePowerUps(XElement root, int gameMode)
    {
      for (XElement element = root.FirstChildElement("power"); element != null; element = element.NextSiblingElement("power"))
      {
        float val = 0.0f;
        element.QueryFloatAttribute("time", ref val);
        uint num = StringFunctions.StringHash(element.AttributeStr("name"));
        PowerUp powerUp1;
        if (this.m_powerups.TryGetValue(num, out powerUp1))
        {
          PowerUp powerUp2 = powerUp1;
          if (!powerUp2.IsSpecial() && !powerUp2.IsAuto() || gameMode == 2)
          {
            PowerUp powerUp3 = this.ActivatePower(num, Vector3.Zero, new float?(val));
            powerUp3.SetCurrentTime(val);
            element.QueryFloatAttribute("totalTime", ref val);
            powerUp3.SetTotalTime(val);
            int score = -1;
            element.QueryIntAttribute("deferedPoints", ref score);
            if (score >= 0)
              Game.AddToCurrentScore(score);
          }
        }
      }
    }

    public int GetNumActiveTimedPowers()
    {
      int activeTimedPowers = 0;
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
      {
        if (activePowerUp.IsSpecial())
          ++activeTimedPowers;
      }
      return activeTimedPowers;
    }

    public float GetActiveProgression() => this.GetActiveProgression(0.0f);

    public float GetActiveProgression(float plusTime)
    {
      PowerUp powerUp = (PowerUp) null;
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
      {
        if (activePowerUp.IsSpecial())
          powerUp = activePowerUp;
      }
      if (powerUp == null)
        return 2f;
      return (double) plusTime > 0.0 && (double) powerUp.GetTotalTime() > 0.0 ? (powerUp.GetCurrentTime() - plusTime) / powerUp.GetTotalTime() : powerUp.GetCurrentTimeProgress();
    }

    public bool ActivateScreenEffect(uint hash)
    {
      ScreenEffect screenEffect;
      if (!this.m_screenEffects.TryGetValue(hash, out screenEffect))
        return false;
      ScreenEffect dest = new ScreenEffect();
      screenEffect.Duplicate(dest);
      dest.Activate();
      this.m_activeScreenEffects.Add(dest);
      return true;
    }

    public bool SetAppropriateScoreCallback()
    {
      foreach (PowerUp activePowerUp in this.m_activePowerUps)
      {
        foreach (GameModifier gameModifier in activePowerUp.GetFirstMod())
        {
          if (gameModifier.GetType() == 2)
          {
            ScoreModifier scoreModifier = (ScoreModifier) gameModifier;
            if (scoreModifier.DoesDeferPoint())
            {
              Game.SetScoreDelegate(new Game.ScoreDelegate(scoreModifier.DeferPoints));
              return true;
            }
          }
        }
      }
      Game.SetScoreDelegate();
      return false;
    }
  }
}
