
// Type: FruitNinja2.PowerUp
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class PowerUp
  {
    protected List<GameModifier> m_modifiers = new List<GameModifier>();
    protected uint m_nameHash;
    protected string m_name;
    protected string m_nameUpperCase;
    protected bool m_isSingle;
    protected bool m_auto;
    protected PurchaseInfo m_purchaseInfo;
    protected bool m_isClone;
    protected float m_currentTime;
    protected float m_totalTime;
    protected Color m_colour;
    protected float m_onscreenAmt;
    protected Texture m_barTexture;
    protected Texture m_popupTexture;
    protected ScreenEffect m_screenEffect;
    protected Vector3 m_pos;
    protected int m_deferedPoints;
    public float m_barX;

    public int GetDeferedPoints() => this.m_deferedPoints;

    public int AddDeferedPoints(int points)
    {
      if (this.m_deferedPoints < 0)
        this.m_deferedPoints = 0;
      this.m_deferedPoints += points;
      return 0;
    }

    public void SetDeferedPoints(int points)
    {
      foreach (GameModifier modifier in this.m_modifiers)
      {
        if (modifier.GetType() == 2)
          ((ScoreModifier) modifier).DeferPoints(points);
      }
    }

    public void DrawBar()
    {
      if ((double) this.m_onscreenAmt <= 0.0 || this.m_barTexture == null)
        return;
      PowerUpManager.GetInstance().PrevPowerupDtModMultiply();
      float num1 = this.m_barX + (float) this.m_barTexture.GetWidth() * 0.0f;
      float num2 = 1f - this.m_onscreenAmt;
      float num3 = num2 * num2;
      MatrixManager.GetInstance().SetMatrix(Matrix.Identity);
      MatrixManager.GetInstance().Scale(new Vector3((float) this.m_barTexture.GetWidth(), (float) this.m_barTexture.GetHeight(), 0.0f));
      MatrixManager.GetInstance().Translate(new Vector3(num1, (float) ((double) Game.SCREEN_HEIGHT / 2.0 + (double) this.m_barTexture.GetHeight() * ((double) num3 - 0.5) + 1.0), 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      this.m_barTexture.Set();
      Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
      this.m_barTexture.UnSet();
    }

    public Color GetColour() => this.m_colour;

    public bool IsClone() => this.m_isClone;

    public bool IsSingle() => this.m_isSingle;

    public bool IsTimed() => (double) this.m_totalTime > 0.0;

    public bool IsAuto() => this.m_auto;

    public float GetCurrentTimeProgress()
    {
      return (double) this.m_totalTime <= 0.0 ? 0.0f : this.m_currentTime / this.m_totalTime;
    }

    public float GetCurrentTime() => this.m_currentTime;

    public float GetTotalTime() => this.m_totalTime;

    public void SetCurrentTime(float val) => this.m_currentTime = val;

    public void SetTotalTime(float val)
    {
      this.m_totalTime = val;
      this.m_onscreenAmt = Math.MIN((float) (((double) this.m_totalTime - (double) this.m_currentTime) * 4.0), 1f);
      if (this.m_screenEffect == null)
        return;
      this.m_screenEffect.Update(this.m_totalTime - this.m_currentTime, this.m_currentTime, this.m_totalTime);
    }

    public void AddModifier(GameModifier mod)
    {
      mod.m_parent = this;
      this.m_modifiers.Add(mod);
    }

    public void Activate(bool isNew, bool fromSave) => this.Activate(isNew, fromSave, Vector3.Zero);

    public void Activate(bool isNew, bool fromSave, Vector3 pos)
    {
      this.Activate(isNew, fromSave, pos, new float?());
    }

    public void Activate(bool isNew, bool fromSave, Vector3 pos, float? length)
    {
      if (!fromSave && this.m_popupTexture != null)
      {
        MissControl free = MissControl.GetFree();
        free.MakeDisappear(pos, 0, this.m_popupTexture);
        free.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (!fromSave && this.m_purchaseInfo != null)
        Game.AddCoins(-this.m_purchaseInfo.GetCost());
      foreach (GameModifier modifier in this.m_modifiers)
      {
        if (!modifier.IsWaiting())
          modifier.ApplyModifier(fromSave, length);
      }
      if (this.m_screenEffect == null || !isNew)
        return;
      this.m_screenEffect.Activate();
    }

    public void Deactivate() => this.Deactivate(false);

    public void Deactivate(bool clearAll)
    {
      foreach (GameModifier modifier in this.m_modifiers)
      {
        if (!modifier.IsWaiting() || clearAll)
          modifier.RemoveModifier();
      }
      this.m_modifiers.Clear();
      if (this.m_screenEffect == null)
        return;
      this.m_screenEffect.Deactivate();
      Delete.SAFE_DELETE<ScreenEffect>(ref this.m_screenEffect);
    }

    public uint GetHash() => this.m_nameHash;

    public string GetName() => this.m_name;

    public string GetNameUpperCase() => this.m_nameUpperCase;

    public void Parse(XElement parent)
    {
      this.m_name = parent.AttributeStr("name");
      this.m_nameHash = StringFunctions.StringHash(this.m_name);
      this.m_nameUpperCase = this.m_name.ToUpper();
      this.m_isSingle = StringFunctions.CompareWords(parent.AttributeStr("single"), "true");
      this.m_auto = StringFunctions.CompareWords(parent.AttributeStr("automatic"), "true");
      StringFunctions.ParseColour(ref this.m_colour, parent.AttributeStr("colour"));
      if (parent.AttributeStr("localise") == null)
      {
        this.m_barTexture = StringFunctions.LoadTexture(parent.AttributeStr("bar"));
      }
      else
      {
        string texture = parent.AttributeStr("bar") + ".tex";
        this.m_barTexture = TextureManager.GetInstance().Load(texture, true);
      }
      string str = parent.AttributeStr("popup");
      this.m_popupTexture = str == null ? (Texture) null : TextureManager.GetInstance().Load(str + ".tex", true);
      this.m_totalTime = 0.0f;
      for (XElement xelement = parent.FirstChildElement(); xelement != null; xelement = xelement.NextSiblingElement())
      {
        if ((XName) "purchase_info" == xelement.Name)
        {
          this.m_purchaseInfo = new PurchaseInfo();
          this.m_purchaseInfo.Parse(xelement);
          this.m_isSingle = true;
        }
        else if ((XName) "effect" == xelement.Name)
        {
          if (this.m_screenEffect == null)
          {
            this.m_screenEffect = new ScreenEffect();
            this.m_screenEffect.m_parent = this;
            this.m_screenEffect.Parse(xelement);
          }
        }
        else
        {
          GameModifier gameModifier = (GameModifier) null;
          if ((XName) "wave_mod" == xelement.Name)
            gameModifier = (GameModifier) new WaveModifier();
          if ((XName) "slash_mod" == xelement.Name)
            gameModifier = (GameModifier) new SlashModifier();
          if ((XName) "time_mod" == xelement.Name)
            gameModifier = (GameModifier) new TimeModifier();
          if ((XName) "score_mod" == xelement.Name)
            gameModifier = (GameModifier) new ScoreModifier();
          if (gameModifier != null)
          {
            gameModifier.Parse(xelement);
            if ((double) gameModifier.GetTotalTime() > (double) this.m_totalTime)
              this.m_totalTime = gameModifier.GetTotalTime();
            this.m_modifiers.Add(gameModifier);
          }
        }
      }
    }

    public PowerUp()
    {
      this.m_screenEffect = (ScreenEffect) null;
      this.m_isSingle = false;
      this.m_purchaseInfo = (PurchaseInfo) null;
      this.m_isClone = false;
      this.m_currentTime = 0.0f;
      this.m_totalTime = 0.0f;
      this.m_colour = Color.White;
      this.m_onscreenAmt = 0.0f;
      this.m_barTexture = (Texture) null;
      this.m_popupTexture = (Texture) null;
      this.m_deferedPoints = -1;
      this.m_barX = 0.0f;
      this.m_auto = false;
    }

    public PowerUp(PowerUp copyFrom)
    {
      this.m_barX = 0.0f;
      this.m_nameHash = copyFrom.GetHash();
      this.m_name = copyFrom.GetName();
      this.m_nameUpperCase = copyFrom.GetNameUpperCase();
      this.m_purchaseInfo = (PurchaseInfo) null;
      this.m_colour = copyFrom.GetColour();
      this.m_isSingle = copyFrom.IsSingle();
      this.m_auto = copyFrom.IsAuto();
      this.m_isClone = true;
      this.m_currentTime = 0.0f;
      this.m_totalTime = copyFrom.GetTotalTime();
      this.m_onscreenAmt = 0.0f;
      this.m_deferedPoints = -1;
      this.m_screenEffect = (ScreenEffect) null;
      if (copyFrom.m_screenEffect != null)
      {
        this.m_screenEffect = new ScreenEffect();
        copyFrom.m_screenEffect.Duplicate(this.m_screenEffect);
        this.m_screenEffect.m_parent = this;
      }
      this.m_barTexture = copyFrom.m_barTexture;
      this.m_popupTexture = copyFrom.m_popupTexture;
    }

    public int Purchaseable() => this.m_purchaseInfo == null ? 0 : this.m_purchaseInfo.GetCost();

    public PurchaseInfo GetPurchasableInfo() => this.m_purchaseInfo;

    public void Release()
    {
      this.m_modifiers.Clear();
      Delete.SAFE_DELETE<PurchaseInfo>(ref this.m_purchaseInfo);
      Delete.SAFE_DELETE<ScreenEffect>(ref this.m_screenEffect);
    }

    public virtual bool Update(float dt)
    {
      int num = 0;
      this.m_currentTime = 0.0f;
      int index = 0;
      while (index < this.m_modifiers.Count)
      {
        GameModifier modifier = this.m_modifiers[index];
        if (modifier.Update(dt))
        {
          modifier.RemoveModifier();
          this.m_modifiers.RemoveAt(index);
        }
        else
        {
          if ((double) modifier.GetCurrentTime() > (double) this.m_currentTime)
          {
            this.m_currentTime = modifier.GetCurrentTime();
            if ((double) this.m_currentTime > (double) this.m_totalTime)
              this.m_totalTime = this.m_currentTime;
          }
          ++num;
          ++index;
        }
      }
      if (this.m_screenEffect != null)
        this.m_screenEffect.Update(dt, this.m_currentTime, this.m_totalTime);
      if ((double) this.m_currentTime > 0.0)
        this.m_onscreenAmt = Math.MIN(this.m_onscreenAmt + dt * 4f, 1f);
      if (this.m_purchaseInfo != null)
        return this.m_purchaseInfo.CurrentGames < 0 && num == 0;
      if (num == 0)
      {
        if ((double) this.m_onscreenAmt <= 0.0)
          return true;
        this.m_onscreenAmt = Math.MAX(this.m_onscreenAmt - dt * 12f, 0.0f);
      }
      return false;
    }

    public PowerUp Clone()
    {
      PowerUp powerUp = new PowerUp(this);
      foreach (GameModifier modifier in this.m_modifiers)
      {
        GameModifier mod = modifier.Clone();
        powerUp.AddModifier(mod);
      }
      if (this.m_purchaseInfo != null)
        powerUp.m_purchaseInfo = this.m_purchaseInfo.Duplicate();
      return powerUp;
    }

    public List<GameModifier> GetFirstMod() => this.m_modifiers;

    public bool IsSpecial() => this.IsTimed() && !this.IsAuto() && this.Purchaseable() == 0;

    public float GetLongestMod()
    {
      float longestMod = -1f;
      foreach (GameModifier modifier in this.m_modifiers)
      {
        if ((double) modifier.GetTotalTime() > (double) longestMod)
          longestMod = modifier.GetTotalTime();
      }
      return longestMod;
    }
  }
}
