
// Type: FruitNinja2.TimeControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class TimeControl : HUDControl3d
  {
    private float m_time;
    private string m_text;
    private float m_countingDownFrom;
    private float m_timeImageFrame;
    private string m_stopTimeText;
    private float[] zen_times = new float[5]
    {
      180f,
      120f,
      90f,
      60f,
      30f
    };
    private static float normalX;
    private static bool beenInUpdateBefore = false;
    private static bool tickTock = true;

    public static int MAX_MULTIPLYERS => 8;

    public TimeControl()
    {
      this.m_texture = (Texture) null;
      this.m_countingDownFrom = -1f;
      this.m_scale = new Vector3(0.0f, 18f, 0.0f);
      this.m_pos = new Vector3((float) (((double) Game.SCREEN_WIDTH - (double) this.m_scale.X) / 2.0 - 5.0), (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y) / 2.0 - 5.0), 0.0f);
      this.m_selfCleanUp = false;
      this.m_stopTimeText = "";
      this.Reset();
    }

    public override void Reset()
    {
      this.m_time = Math.MAX(0.0f, this.m_countingDownFrom);
      this.m_stopTimeText = "";
      if (Game.IsMultiplayer())
        this.m_time = this.zen_times[(Game.game_work.saveData.GetTotal(StringFunctions.StringHash("vs_option_zen")) - 1) % this.zen_times.Length] + 0.9f;
      else if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
      {
        this.m_time = 60.9f;
        if ((double) Game.game_work.saveData.timer == 0.0 && (double) Game.game_work.gameOverTransition <= 0.0)
          Game.game_work.saveData.timer = this.m_time;
      }
      this.m_timeImageFrame = 0.0f;
      this.m_color = Color.White;
    }

    public float GetCountDown()
    {
      return Game.game_work.gameMode != Game.GAME_MODE.GM_ARCADE ? this.m_countingDownFrom : 60.9f;
    }

    public void AddTime(float time) => this.m_time += time;

    public void SetTime(float time) => this.m_time = time;

    public float GetTime() => this.m_time;

    public override void Release() => this.m_texture = (Texture) null;

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      if (!TimeControl.beenInUpdateBefore)
      {
        TimeControl.beenInUpdateBefore = true;
        TimeControl.normalX = this.m_pos.X;
      }
      if (Game.game_work.gameMode == Game.GAME_MODE.GM_ZEN || Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
      {
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
        this.m_pos.X = TimeControl.normalX;
        if (!Game.game_work.pause && !Game.game_work.gameOver)
        {
          if ((double) this.m_countingDownFrom > 0.0)
          {
            int b = (int) m_color.B;
            float num = 0.0f;
            if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
              num = PowerUpManager.GetInstance().GetStopClockAmount();
            if ((double) num > 0.0)
            {
              this.m_color = new Color((int) byte.MaxValue, 100, 100, (int) byte.MaxValue);
              this.m_stopTimeText = string.Format("+{0}", (object) ((int) num + 1));
            }
            else
            {
              this.m_stopTimeText = "";
              if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                this.m_time -= dt * PowerUpManager.GetInstance().SlowClock();
              else
                this.m_time -= dt;
              if ((double) this.m_time < 0.5)
              {
                Game.GameOver();
                Fruit.s_consecutiveCount = 0;
                Fruit.s_consecutiveType = -1;
                this.m_time = 0.0f;
                this.m_color = new Color((int) byte.MaxValue, 100, 100, (int) byte.MaxValue);
                SoundManager.GetInstance().SFXPlay(SoundDef.SND_TIME_UP);
              }
              else if ((double) this.m_time < 3.0)
                this.m_color = (int) ((double) this.m_time * 8.0) % 2 != 0 
                                    ? new Color((int) byte.MaxValue, 100, 100, (int) byte.MaxValue) 
                                    : Color.White;

              else if ((double) this.m_time < 6.0)
                this.m_color = (int) ((double) this.m_time * 4.0) % 2 != 0 
                                    ? new Color((int) byte.MaxValue, 100, 100, (int) byte.MaxValue) 
                                    : Color.White;

              else if ((double) this.m_time < 11.0)
                this.m_color = (int) ((double) this.m_time * 2.0) % 2 != 0 
                                    ? new Color((int) byte.MaxValue, 100, 100, (int) byte.MaxValue) 
                                    : Color.White;

              if ((double) this.m_time > 0.0 && (double) this.m_time < 11.0 
                                && b != (int) this.m_color.B)
              {
                TimeControl.tickTock = !TimeControl.tickTock;
                SoundManager.GetInstance().SFXPlay(TimeControl.tickTock
                    ? SoundDef.SND_TIME_TICK 
                    : SoundDef.SND_TIME_TOCK);
              }
              this.m_timeImageFrame = (float) ((int) ((double) this.m_countingDownFrom
                                - (double) this.m_time) % 6) + 0.5f;
            }
          }
          else
          {
            this.m_time += dt;
            this.m_timeImageFrame = (float) ((int) this.m_time % 6) + 0.5f;
          }
        }
        Game.game_work.saveData.timer = this.m_time;
        int num1 = (int) this.m_time % 60;
        this.m_text = string.Format("{0}:{2}{1}", (object) (int) ((double) this.m_time / 60.0), (object) num1, num1 < 10 ? (object) "0" : (object) "");
        this.m_pos.Y = (float) (((double) Game.SCREEN_HEIGHT + 2.0 * (double) this.m_scale.Y) / 2.0 - 2.0 * (double) this.m_scale.Y * (1.0 - (double) Math.Abs(Game.game_work.gameOverTransition)));
        if (!Game.IsMultiplayer())
          return;
        this.m_pos.Y *= -1f;
        this.m_pos.Y += 5f;
      }
      else
      {
        this.m_drawOrder = ~HUD.HUD_ORDER.HUD_ORDER_NORMAL;
        Game.game_work.saveData.timer = -1f;
      }
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      if (((double) Math.Abs(Game.game_work.gameOverTransition) >= 1.0 || Game.game_work.gameMode != Game.GAME_MODE.GM_ZEN) && Game.game_work.gameMode != Game.GAME_MODE.GM_ARCADE)
        return;
      Game.game_work.pNumberFont.DrawString(this.m_text, this.m_pos.X - this.m_scale.X * 0.6f, this.m_pos.Y, 0.0f, HUDControl.TintColor(this.m_color, tintChannels), 32f, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);
      if (this.m_stopTimeText.Length > 0)
        Game.game_work.pNumberFont.DrawString(this.m_stopTimeText, this.m_pos.X - this.m_scale.X * 0.6f, this.m_pos.Y - 32f, 0.0f, HUDControl.TintColor(Color.Green, tintChannels), 24f, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);
      if (this.m_texture == null)
        return;
      float u0 = (float) ((int) this.m_timeImageFrame % 4) / 4f;
      float v0 = (float) ((int) this.m_timeImageFrame / 4) / 2f;
      this.m_texture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(this.m_scale);
      MatrixManager.GetInstance().Translate(this.m_pos);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(HUDControl.TintWhite(tintChannels), u0, u0 + 0.25f, v0, v0 + 0.5f);
      this.m_texture.UnSet();
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_TIMER;

    public override void Skip()
    {
      this.m_time = Game.game_work.saveData.timer;
      this.m_timeImageFrame = 0.0f;
    }

    public void CountDown(float time) => this.m_countingDownFrom = time;
  }
}
