
// Type: GameManager.SpeedControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortar;

#nullable disable
namespace GameManager
{
  public class SpeedControl : HUDControl3d
  {
    private static string[] backingDrumNames = new string[2]
    {
      "Combo-Blitz-Backing-Light",
      "Combo-Blitz-Backing"
    };
    public ushort m_pulseTime;
    public float m_pulseSpeed;
    public float m_onScreenTime;
    public Vector3 m_originalScale;
    public float m_lossTime;
    public float m_alpha;
    public float m_drumVol;
    private MortarSound m_backing;
    private int m_usingBackingSound;
    public PSPParticleEmitter m_emitter;
    private static bool first = true;
    private static bool firstFrame = true;

    public static float MIN_SCALE => 40f * Game.HUD_SCALE;

    public static float MAX_SCALE => 50f * Game.HUD_SCALE;

    public static float GAME_SCREEN_X_IN => 2f * Game.HUD_SCALE;

    public static float GAME_SCREEN_Y_IN => 2f * Game.HUD_SCALE;

    public static Vector3 IN_GAME_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + (double) SpeedControl.MIN_SCALE / 2.0) + SpeedControl.GAME_SCREEN_X_IN, (float) ((double) Game.SCREEN_HEIGHT / 2.0 - (double) SpeedControl.MIN_SCALE / 2.0) - SpeedControl.GAME_SCREEN_Y_IN, 0.0f);
      }
    }

    private float LINEAR_TRANSITION_TO(float val, float valTo, float movement)
    {
      if ((double) val > (double) valTo)
        return Math.MAX(valTo, val - movement);
      return (double) val >= (double) valTo ? valTo : Math.MIN(valTo, val + movement);
    }

    public SpeedControl()
    {
      this.m_texture = TextureManager.GetInstance().Load("extra/white");
      this.m_pulseTime = (ushort) 0;
      this.m_pulseSpeed = 0.0f;
      this.m_originalScale = this.m_scale = Vector3.Multiply(new Vector3((float) this.m_texture.GetWidth(), (float) this.m_texture.GetHeight() * 0.125f, 0.0f), Game.GAME_MODE_SCALE_FIX);
      this.m_onScreenTime = 0.0f;
      this.m_canBeTinted = false;
      this.m_lossTime = 1f;
      this.m_alpha = 0.0f;
      this.m_emitter = (PSPParticleEmitter) null;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT;
      this.m_drumVol = 0.0f;
    }

    public override void Reset()
    {
    }

    public override void Init()
    {
    }

    public override void Update(float dt)
    {
      if (SpeedControl.firstFrame)
      {
        if (Game.game_work.pause && (double) this.m_pulseSpeed > 0.0)
        {
          this.m_onScreenTime = 1f;
          this.m_alpha = Math.CLAMP(this.m_lossTime * 1.333f, 0.0f, 1f);
        }
        SpeedControl.firstFrame = false;
      }
      if (Game.game_work.pause)
        return;
      float valTo1 = 0.0f;
      float valTo2 = 1f;
      if ((double) this.m_pulseSpeed != 0.0)
      {
        if (Game.game_work.gameMode != Game.GAME_MODE.GM_ARCADE || Game.game_work.gameOver)
        {
          this.m_pulseSpeed = 0.0f;
        }
        else
        {
          float num = Math.CLAMP((float) (((double) WaveManager.GetInstance().GetComboBonusProgression(0) - 0.25) / 0.75), 0.0f, 1f);
          valTo2 = (float) (1.0 - (double) num * 0.20000000298023224);
          valTo1 = num * Math.CLAMP(this.m_alpha * 2f, 0.0f, 1f);
        }
        this.m_pulseTime += (ushort) ((double) Math.DEGREE_TO_IDX(180f) * (double) dt * (double) Math.CLAMP(this.m_pulseSpeed, 3f, 20f) * 0.25);
      }
      else
      {
        if (this.m_emitter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
          this.m_emitter = (PSPParticleEmitter) null;
        }
        this.m_lossTime = 0.0f;
      }
      this.m_alpha += (float) (((double) Math.CLAMP(this.m_lossTime * 1.333f, 0.0f, 1f) - (double) this.m_alpha) * 0.10000000149011612);
      this.m_alpha += (float) (((double) Math.CLAMP(this.m_lossTime * 1.333f, 0.0f, 1f) - (double) this.m_alpha) * 0.10000000149011612);
      if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
      {
        SoundManager.GetInstance().soundFadeOut = this.LINEAR_TRANSITION_TO(SoundManager.GetInstance().soundFadeOut, valTo2, dt);
        if (SpeedControl.first)
        {
          this.m_drumVol = valTo1;
          SpeedControl.first = false;
        }
        this.m_drumVol = this.LINEAR_TRANSITION_TO(this.m_drumVol, valTo1, dt / 2f);
      }
      else
      {
        this.m_drumVol = 0.0f;
        SoundManager.GetInstance().soundFadeOut = 1f;
      }
      if ((double) this.m_drumVol > 0.0)
      {
        if (this.m_backing == null)
        {
          this.m_usingBackingSound = 0;
          this.m_backing = new MortarSound();
          SoundManager.GetInstance().SFXPlay(SpeedControl.backingDrumNames[this.m_usingBackingSound], 0U, this.m_backing);
        }
        if (this.m_backing == null)
          return;
        if (this.m_backing.inst.State == Microsoft.Xna.Framework.Audio.SoundState.Stopped)
        {
          this.m_backing = new MortarSound();
          this.m_usingBackingSound ^= 1;
          SoundManager.GetInstance().SFXPlay(SpeedControl.backingDrumNames[this.m_usingBackingSound], 0U, this.m_backing);
        }
        this.m_backing.SetVolume(this.m_drumVol * 10f);
      }
      else
      {
        if (this.m_backing == null)
          return;
        SoundManager.GetInstance().Release(this.m_backing);
        this.m_backing = (MortarSound) null;
      }
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_SCORE;

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      if ((double) this.m_alpha <= 1.0 / 1000.0)
        return;
      GameVertex[] data = new GameVertex[6];
      this.m_color = new Color(123, 106, 0, (int) Math.CLAMP(this.m_alpha * 16f, 0.0f, (float) byte.MaxValue));
      for (int index = 0; index < 6; ++index)
      {
        data[index].color = this.m_color;
        data[index].X = data[index].Y = data[index].Z = data[index].nx = data[index].ny = 0.0f;
        data[index].nz = 1f;
        data[index].u = 0.5f;
        data[index].v = 0.5f;
        if (index / 2 != 1)
        {
          data[index].X = (float) ((index > 2 ? 1.0 : -1.0) * (double) Game.SCREEN_WIDTH / 2.0);
          data[index].Y = (float) (-(double) Game.SCREEN_WIDTH / 2.0);
        }
        if (index % 2 == 1)
          data[index].Y -= Game.SCREEN_WIDTH / 6f;
      }
      this.m_texture.Set();
      float num = (float) this.m_pulseTime / 65536f * (Game.SCREEN_WIDTH / 3f) - Game.SCREEN_HEIGHT / 2f;
      for (int index = 0; index < 4; ++index)
      {
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Translate(new Vector3(0.0f, num, -5550f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawPrimitives2((PrimitiveType) 1, data, 6, true, 4, 0);
        num += Game.SCREEN_WIDTH / 3f;
      }
      this.m_texture.UnSet();
    }

    public override void Skip()
    {
    }
  }
}
