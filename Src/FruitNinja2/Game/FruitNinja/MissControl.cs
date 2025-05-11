
// Type: GameManager.MissControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace GameManager
{
  public class MissControl : HUDControl3d
  {
    public const int SHAKE_TIME = 30;
    public const int MAX_RANGE = 8;
    public const float DISAPPEAR_TIME = 1.66f;
    public const float FADE_OUT_TIME = 0.0f;
    public const float DISAPPEAR_WAVES = 2.25f;
    public const float DISAPPEAR_Y = 0.0f;
    public const float DISAPPEAR_X = 3f;
    public const float DISAPPEAR_SCALE = 48f;
    public const int MAX_COMBO = 10;
    public const int REVOLUTIONS = 6;
    public const float MIDDLE_SCALE = 0.65f;
    public int m_id;
    public bool m_registered;
    public short m_shakeTimer;
    public float m_fadeAway;
    public bool m_isCritical;
    public bool m_isCombo;
    public int m_fruitsInCombo;
    private bool m_playSound;
    public static int s_numCriticals;
    public static float s_dtMod = 1f;
    public float m_dtMod;
    public static int s_refCount = 0;
    private static WeakReference s_sharedStore;
    private MissControl.TextureStore m_store;
    public static MissControl[] pool;
    public static int poolCount;
    public static int curentFree;

    private Texture s_cross
    {
      get => this.m_store.s_cross;
      set => this.m_store.s_cross = value;
    }

    private Texture s_critical
    {
      get => this.m_store.s_critical;
      set => this.m_store.s_critical = value;
    }

    private Texture s_ultraRare
    {
      get => this.m_store.s_ultraRare;
      set => this.m_store.s_ultraRare = value;
    }

    private Texture[] s_combo
    {
      get => this.m_store.s_combo;
      set => this.m_store.s_combo = value;
    }

    public MissControl()
    {
      if (MissControl.s_sharedStore != null)
        this.m_store = MissControl.s_sharedStore.Target as MissControl.TextureStore;
      if (this.m_store == null)
      {
        this.m_store = new MissControl.TextureStore();
        MissControl.s_sharedStore = new WeakReference((object) this.m_store);
        this.s_cross = TextureManager.GetInstance().Load("textureswp7/hud_cross.tex");
        this.s_critical = TextureManager.GetInstance().Load("critical.tex", true);
        this.s_ultraRare = TextureManager.GetInstance().Load("ultra_rare_plus_50.tex", true);
        for (int index = 1; index < 11; ++index)
        {
          if (index < 3)
          {
            this.s_combo[index - 1] = (Texture) null;
          }
          else
          {
            string texture = string.Format("combo_{0}.tex", (object) index);
            this.s_combo[index - 1] = TextureManager.GetInstance().Load(texture, true);
          }
        }
      }
      ++MissControl.s_refCount;
      this.Init();
      this.m_update = false;
    }

    public override void Init()
    {
      this.m_update = true;
      this.m_playSound = true;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
      this.m_id = 0;
      this.m_isCritical = false;
      this.m_rotation = 0.0f;
      this.m_id = 0;
      this.m_texture = this.s_cross;
      this.m_update = true;
      this.m_fadeAway = 0.0f;
      this.m_terminate = false;
      this.m_isCombo = false;
      this.m_dtMod = 1f;
      this.m_fadeAway = 0.0f;
      this.m_fruitsInCombo = 0;
      this.m_scale = new Vector3((float) (this.m_texture.GetWidth() / 2U + 1U), (float) (this.m_texture.GetWidth() / 2U + 1U), 0.0f);
      this.Reset();
    }

    public override void Release() => this.m_texture = (Texture) null;

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      Vector3 zero = Vector3.Zero;
      float v1 = 1f;
      if (this.m_shakeTimer > (short) 0)
      {
        int num1 = Mortar.Math.g_random.Rand32(8) - 4;
        int num2 = Mortar.Math.g_random.Rand32(8) - 4;
        zero = new Vector3((float) num1, (float) num2, 0.0f);
        --this.m_shakeTimer;
      }
      if ((double) this.m_fadeAway > 0.0)
      {
        if ((double) this.m_fadeAway > 1.6599999666213989)
          return;
        float idx = (float) ((double) this.m_fadeAway / 1.6599999666213989 * 360.0 * 6.0 * 182.0);
        v1 = Mortar.Math.Abs(Mortar.Math.SinIdx((ushort) idx));
        if ((double) idx > (double) Mortar.Math.DEGREE_TO_IDX(90f) && (double) idx < 376740.0)
          v1 = (double) idx < (double) Mortar.Math.DEGREE_TO_IDX(180f) || (double) idx > 360360.0 ? Mortar.Math.MAX(v1, 0.65f) : 0.65f;
      }
      else if (Game.FailureEnabled() && !Game.IsMultiplayer())
        zero.Y += (float) ((double) Mortar.Math.Abs(Game.game_work.gameOverTransition) * ((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_pos.Y) * 3.0);
      else
        zero.Y += (float) (1.0 * ((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_pos.Y) * 3.0);
      if (this.m_texture == null)
        return;
      this.m_texture.Set();
      MatrixManager.instance.Reset();
      MatrixManager.instance.Scale(Vector3.Multiply(this.m_scale, v1));
      if ((double) this.m_rotation != 0.0)
        MatrixManager.instance.RotZ(MathHelper.ToRadians(this.m_rotation));
      MatrixManager.instance.TranslateGlobal(Vector3.Add(this.m_pos, zero));
      MatrixManager.instance.UploadCurrentMatrices();
      Mesh.DrawQuad(this.m_color, this.m_isCritical ? 0.0f : (this.m_registered ? 0.5f : 0.0f), this.m_isCritical ? 1f : (this.m_registered ? 1f : 0.5f), this.m_isCritical ? 0.0f : 0.25f, this.m_isCritical ? 1f : 0.75f);
      this.m_texture.UnSet();
    }

    public override void Update(float dt)
    {
      int currentMissCount = (int) Game.game_work.currentMissCount;
      if (!this.m_registered && currentMissCount >= this.m_id + 1)
      {
        this.m_color.A = byte.MaxValue;
        this.m_shakeTimer = (short) 30;
        this.m_registered = true;
      }
      float num1 = dt;
      if (this.m_isCritical)
      {
        ++MissControl.s_numCriticals;
        Vector2 vector2_1 = Vector2.Zero;
        for (int index = 0; index < MissControl.poolCount; ++index)
        {
          MissControl missControl = MissControl.pool[index];
          if (missControl.m_update && missControl != this)
          {
            Vector2 vector2_2 = new Vector2(missControl.m_pos.X - this.m_pos.X,
                missControl.m_pos.Y - this.m_pos.Y);
            float ss = vector2_2.LengthSquared();
            if ((double) ss < 4900.0)
            {
              float num2 = 1f;
              if ((double) ss > 0.0)
              {
                num2 = Mortar.Math.Sqrt(ss);
              }
              else
              {
                ushort idx = (ushort) Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(359f));
                vector2_2 = new Vector2(Mortar.Math.SinIdx(idx), Mortar.Math.CosIdx(idx));
              }
              vector2_1 = Vector2.Subtract(vector2_1, Vector2.Multiply(Vector2.Multiply(Vector2.Multiply(Vector2.Divide(vector2_2, num2), 70f - num2), num1), 15f));
            }
          }
        }
        this.m_pos.X += vector2_1.X;
        this.m_pos.Y += vector2_1.Y;
        dt *= MissControl.s_dtMod * this.m_dtMod;
      }
      if ((double) this.m_fadeAway > 0.0)
      {
        if (Game.game_work.pause)
          return;
        float fadeAway = this.m_fadeAway;
        this.m_fadeAway -= dt;
        this.m_pos.Z = 0.0f;
        if ((double) fadeAway >= 1.6599999666213989 && (double) this.m_fadeAway < 1.6599999666213989 && this.m_isCritical && this.m_playSound)
        {
          string filename = !this.m_isCombo ? SoundDef.SND_NEW_BEST : string.Format("combo-{0}", (object) Mortar.Math.CLAMP(this.m_fruitsInCombo - 2, 1, 8));
          SoundManager.GetInstance().SFXPlay(filename);
        }
        if ((double) this.m_fadeAway > 0.0)
          return;
        this.m_deleteCall((HUDControl) this);
        this.m_update = false;
      }
      else
      {
        if (!this.m_registered || currentMissCount > this.m_id)
          return;
        this.m_color.A = byte.MaxValue;
        this.m_shakeTimer = (short) 30;
        this.m_registered = false;
      }
    }

    public override void Reset()
    {
      this.m_color = Color.White;
      this.m_color.A = byte.MaxValue;
      this.m_shakeTimer = (short) 0;
      this.m_registered = false;
      if ((double) this.m_fadeAway <= 0.0)
        return;
      this.m_color.A = (byte) 0;
      this.m_update = false;
    }

    public static void PreUpdate(float dt)
    {
      MissControl.s_dtMod = (float) (0.5 + (double) MissControl.s_numCriticals * 1.0);
      MissControl.s_numCriticals = 0;
    }

    public void MakeDisappear(Vector3 pos) => this.MakeDisappear(pos, 0);

    public void MakeDisappear(Vector3 pos, int forPlayer)
    {
      this.MakeDisappear(pos, forPlayer, (Texture) null);
    }

    public void MakeDisappear(Vector3 pos, int forPlayer, Texture texture)
    {
      this.Init();
      this.m_pos = pos;
      this.m_color.A = byte.MaxValue;
      if (texture != null)
      {
        this.m_playSound = false;
        this.m_texture = texture;
        this.m_isCritical = true;
        this.m_fadeAway = 1.81f;
        this.m_id = (int) Game.MAX_FRUIT_MISSES;
        this.m_shakeTimer = (short) 0;
        this.m_registered = true;
        this.m_scale = new Vector3((float) (this.m_texture.GetWidth() + 1U), 
            (float) (this.m_texture.GetHeight() + 1U), 0.0f);
        this.SetPlayer(forPlayer);
      }
      else
      {
        this.m_fadeAway = 1.66f;
        this.m_id = (int) Game.MAX_FRUIT_MISSES;
        this.m_shakeTimer = forPlayer > 0 ? (short) 30 : (short) 0;
        this.m_registered = true;
        this.m_scale = Vector3.Multiply(Vector3.One, 48f);
        this.SetPlayer(forPlayer);
        this.m_scale = Vector3.Multiply(Vector3.One, 48f);
        this.m_pos.X = Mortar.Math.CLAMP(this.m_pos.X,
            (float) (-((double) Game.SCREEN_WIDTH / 2.0) + (double) this.m_scale.X * 0.5), 
            (float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) this.m_scale.X * 0.5));
        this.m_pos.Y = Mortar.Math.CLAMP(this.m_pos.Y, (float) 
            (-((double) Game.SCREEN_HEIGHT / 2.0) + (double) this.m_scale.Y * 0.5),
            (float) ((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_scale.Y * 0.5));
      }
    }

    public void MakeCritical(Vector3 pos) => this.MakeCritical(pos, 0);

    public void MakeCritical(Vector3 pos, int forPlayer)
    {
      this.Init();
      this.m_texture = this.s_critical;
      this.m_isCritical = true;
      this.m_pos = pos;
      this.m_fadeAway = 1.81f;
      this.m_color.A = byte.MaxValue;
      this.m_id = (int) Game.MAX_FRUIT_MISSES;
      this.m_shakeTimer = (short) 0;
      this.m_registered = true;
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_ITALIAN || Game.game_work.language == StringTableUtils.Language.LANGUAGE_FRENCH || Game.game_work.language == StringTableUtils.Language.LANGUAGE_GERMAN || Game.game_work.language == StringTableUtils.Language.LANGUAGE_SPANISH)
        this.m_scale = new Vector3((float) (this.m_texture.GetWidth() * 2U), (float) (this.m_texture.GetHeight() * 2U), 0.0f);
      else
        this.m_scale = new Vector3((float) (this.m_texture.GetWidth() + 20U), (float) (this.m_texture.GetHeight() + 20U), 0.0f);
      MissControl missControl1 = this;
      missControl1.m_scale = Vector3.Multiply(missControl1.m_scale, 0.5f);
      if ((double) this.m_pos.X + (double) this.m_scale.X > (double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = Game.SCREEN_WIDTH * 0.5f - this.m_scale.X;
      if ((double) this.m_pos.Y + (double) this.m_scale.Y > (double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = Game.SCREEN_HEIGHT * 0.5f - this.m_scale.Y;
      if ((double) this.m_pos.X - (double) this.m_scale.X < -(double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = (float) (-(double) Game.SCREEN_WIDTH * 0.5) + this.m_scale.X;
      if ((double) this.m_pos.Y - (double) this.m_scale.Y < -(double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = (float) (-(double) Game.SCREEN_HEIGHT * 0.5) + this.m_scale.Y;
      MissControl missControl2 = this;
      missControl2.m_scale = Vector3.Add(missControl2.m_scale, this.m_scale);
      this.SetPlayer(forPlayer);
    }

    public void MakeRare(Vector3 pos)
    {
      this.Init();
      this.m_texture = this.s_ultraRare;
      this.m_isCritical = true;
      this.m_pos = pos;
      this.m_fadeAway = 1.81f;
      this.m_color.A = byte.MaxValue;
      this.m_id = (int) Game.MAX_FRUIT_MISSES;
      this.m_shakeTimer = (short) 0;
      this.m_registered = true;
      this.m_scale = new Vector3((float) (this.m_texture.GetWidth() + 1U), (float) (this.m_texture.GetHeight() + 1U), 0.0f);
      this.m_dtMod = 0.5f;
      MissControl missControl1 = this;
      missControl1.m_scale = Vector3.Multiply(missControl1.m_scale, 0.5f);
      if ((double) this.m_pos.X + (double) this.m_scale.X > (double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = Game.SCREEN_WIDTH * 0.5f - this.m_scale.X;
      if ((double) this.m_pos.Y + (double) this.m_scale.Y > (double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = Game.SCREEN_HEIGHT * 0.5f - this.m_scale.Y;
      if ((double) this.m_pos.X - (double) this.m_scale.X < -(double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = (float) (-(double) Game.SCREEN_WIDTH * 0.5) + this.m_scale.X;
      if ((double) this.m_pos.Y - (double) this.m_scale.Y < -(double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = (float) (-(double) Game.SCREEN_HEIGHT * 0.5) + this.m_scale.Y;
      MissControl missControl2 = this;
      missControl2.m_scale = Vector3.Add(missControl2.m_scale, this.m_scale);
    }

    public void MakeCombo(Vector3 pos, int combo) => this.MakeCombo(pos, combo, 0);

    public void MakeCombo(Vector3 pos, int combo, int forPlayer)
    {
      this.Init();
      this.m_texture = this.s_combo[Mortar.Math.CLAMP(combo - 1, 0, 9)];
      this.m_isCritical = true;
      this.m_isCombo = true;
      this.m_fruitsInCombo = combo;
      if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
        this.m_fruitsInCombo = (int) ((double) WaveManager.GetInstance().GetSpeed() + 0.64999997615814209);
      this.m_pos = pos;
      this.m_fadeAway = 1.81f;
      this.m_color.A = byte.MaxValue;
      this.m_id = (int) Game.MAX_FRUIT_MISSES;
      this.m_shakeTimer = (short) 0;
      this.m_registered = true;
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_ITALIAN || Game.game_work.language == StringTableUtils.Language.LANGUAGE_FRENCH || Game.game_work.language == StringTableUtils.Language.LANGUAGE_GERMAN || Game.game_work.language == StringTableUtils.Language.LANGUAGE_SPANISH)
        this.m_scale = new Vector3((float) (this.m_texture.GetWidth() * 2U), (float) (this.m_texture.GetHeight() * 2U), 0.0f);
      else
        this.m_scale = new Vector3((float) (this.m_texture.GetWidth() + 1U), (float) (this.m_texture.GetHeight() + 1U), 0.0f);
      MissControl missControl1 = this;
      missControl1.m_scale = Vector3.Multiply(missControl1.m_scale, 0.5f);
      if ((double) this.m_pos.X + (double) this.m_scale.X > (double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = Game.SCREEN_WIDTH * 0.5f - this.m_scale.X;
      if ((double) this.m_pos.Y + (double) this.m_scale.Y > (double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = Game.SCREEN_HEIGHT * 0.5f - this.m_scale.Y;
      if ((double) this.m_pos.X - (double) this.m_scale.X < -(double) Game.SCREEN_WIDTH * 0.5)
        this.m_pos.X = (float) (-(double) Game.SCREEN_WIDTH * 0.5) + this.m_scale.X;
      if ((double) this.m_pos.Y - (double) this.m_scale.Y < -(double) Game.SCREEN_HEIGHT * 0.5)
        this.m_pos.Y = (float) (-(double) Game.SCREEN_HEIGHT * 0.5) + this.m_scale.Y;
      MissControl missControl2 = this;
      missControl2.m_scale = Vector3.Add(missControl2.m_scale, this.m_scale);
      this.SetPlayer(forPlayer);
    }

    public void SetPlayer(int player)
    {
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_MISS;

    public override void Skip()
    {
      if ((int) Game.game_work.currentMissCount < this.m_id + 1)
        return;
      this.m_color.A = byte.MaxValue;
      this.m_shakeTimer = (short) 0;
      this.m_registered = true;
    }

    public static void CreatePool(int num, HUD hud)
    {
      MissControl.pool = ArrayInit.CreateFilledArray<MissControl>(num);
      MissControl.poolCount = num;
      MissControl.curentFree = 0;
      foreach (MissControl control in MissControl.pool)
      {
        hud.AddControl((HUDControl) control);
        control.m_selfCleanUp = true;
      }
    }

    public static void CleanPool() => MissControl.poolCount = 0;

    public static MissControl GetFree()
    {
      for (int index = 0; MissControl.pool[MissControl.curentFree].m_update && index < MissControl.poolCount; ++index)
        MissControl.curentFree = (MissControl.curentFree + 1) % MissControl.poolCount;
      return MissControl.pool[MissControl.curentFree];
    }

    public class TextureStore
    {
      public Texture s_cross;
      public Texture s_critical;
      public Texture s_ultraRare;
      public Texture[] s_combo = new Texture[10];
    }
  }
}
