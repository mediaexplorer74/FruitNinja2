
// Type: FruitNinja2.TutorialControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class TutorialControl : HUDControl3d
  {
    protected float m_fingerTime;
    protected Vector3 m_fingerPos;
    protected Texture m_ringTex;
    protected Color m_fingerColor;
    protected int m_fingerFrame;
    protected float m_fingerScale;

    public static float FINGER_TOUCH_ADD => 9.5f;

    public static float FINGER_WAIT_TIME => 10f;

    public static float FINGER_IN_TIME => 0.35f;

    public static float FINGER_START_WAIT_TIME => 0.25f;

    public static float FINGER_ANIMATE_TIME => 0.4f;

    public static float FINGER_SWIPE_TIME => 0.5f;

    public static float FINGER_END_WAIT_TIME => 0.75f;

    public static float FINGER_OUT_TIME => 0.5f;

    public static float FINGER_WAIT_TIME_TOTAL => -TutorialControl.FINGER_WAIT_TIME;

    public static float FINGER_IN_TIME_TOTAL => TutorialControl.FINGER_IN_TIME;

    public static float FINGER_START_WAIT_TIME_TOTAL
    {
      get => TutorialControl.FINGER_IN_TIME_TOTAL + TutorialControl.FINGER_START_WAIT_TIME;
    }

    public static float FINGER_ANIMATE_TIME_TOTAL
    {
      get => TutorialControl.FINGER_START_WAIT_TIME_TOTAL + TutorialControl.FINGER_ANIMATE_TIME;
    }

    public static float FINGER_SWIPE_TIME_TOTAL
    {
      get => TutorialControl.FINGER_ANIMATE_TIME_TOTAL + TutorialControl.FINGER_SWIPE_TIME;
    }

    public static float FINGER_END_WAIT_TIME_TOTAL
    {
      get => TutorialControl.FINGER_SWIPE_TIME_TOTAL + TutorialControl.FINGER_END_WAIT_TIME;
    }

    public static float FINGER_OUT_TIME_TOTAL
    {
      get => TutorialControl.FINGER_END_WAIT_TIME_TOTAL + TutorialControl.FINGER_OUT_TIME;
    }

    public static float FINGER_WAIT_TIME_PROGRESS(float t) => t / TutorialControl.FINGER_WAIT_TIME;

    public static float FINGER_IN_TIME_PROGRESS(float t) => t / TutorialControl.FINGER_IN_TIME;

    public static float FINGER_START_WAIT_TIME_PROGRESS(float t)
    {
      return (t - TutorialControl.FINGER_IN_TIME_TOTAL) / TutorialControl.FINGER_START_WAIT_TIME;
    }

    public static float FINGER_ANIMATE_TIME_PROGRESS(float t)
    {
      return (t - TutorialControl.FINGER_START_WAIT_TIME_TOTAL) / TutorialControl.FINGER_ANIMATE_TIME;
    }

    public static float FINGER_SWIPE_TIME_PROGRESS(float t)
    {
      return (t - TutorialControl.FINGER_ANIMATE_TIME_TOTAL) / TutorialControl.FINGER_SWIPE_TIME;
    }

    public static float FINGER_END_WAIT_TIME_PROGRESS(float t)
    {
      return (t - TutorialControl.FINGER_SWIPE_TIME_TOTAL) / TutorialControl.FINGER_END_WAIT_TIME;
    }

    public static float FINGER_OUT_TIME_PROGRESS(float t)
    {
      return (t - TutorialControl.FINGER_END_WAIT_TIME_TOTAL) / TutorialControl.FINGER_OUT_TIME;
    }

    public static float FINGER_TIME_TOTAL => TutorialControl.FINGER_OUT_TIME_TOTAL;

    public static float FINGER_DOWN_MOVE => 20f;

    public static Vector3 FINGER_START_POS => new Vector3(-0.5f, -0.075f, 0.0f);

    public static Vector3 FINGER_END_POS => new Vector3(0.5f, 0.075f, 0.0f);

    public static Vector3 FINGER_POS_DIFF => new Vector3(1f, 0.15f, 0.0f);

    public static Vector3 FINGER_POS(float t)
    {
      return Vector3.Add(TutorialControl.FINGER_START_POS, Vector3.Multiply(TutorialControl.FINGER_POS_DIFF, Math.CLAMP(TutorialControl.FINGER_SWIPE_TIME_PROGRESS(t), 0.0f, 1f)));
    }

    public TutorialControl()
    {
      this.m_texture = TextureManager.GetInstance().Load("textureswp7/swipe_fruit_begin.tex");
      this.m_ringTex = TextureManager.GetInstance().Load("textureswp7/press_indicate.tex");
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
    }

    public override void Init()
    {
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      this.Reset();
    }

    public override void Release()
    {
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      float num1 = (double) this.m_pos.X > 0.0 ? -1f : 1f;
      if ((double) this.m_fingerTime <= 0.0)
        return;
      if ((double) this.m_fingerTime > (double) TutorialControl.FINGER_START_WAIT_TIME_TOTAL
                && (double) this.m_fingerTime < (double) TutorialControl.FINGER_END_WAIT_TIME_TOTAL)
      {
        for (int index = 0; index < 4; ++index)
        {
          int num2 = (int) ((double) this.m_fingerTime * 2000.0) % 1000;
          float num3 = (float) index + (float) num2 / 1000f;

          int num4 = (int) Math.CLAMP((float) ((double) byte.MaxValue - (double) byte.MaxValue 
              * ((double) num3 - 3.0)), 0.0f, (float) byte.MaxValue);
          if ((double) this.m_fingerTime < (double) TutorialControl.FINGER_START_WAIT_TIME_TOTAL
                        + 0.25)
            num4 *= (int) (((double) this.m_fingerTime - 
                            (double) TutorialControl.FINGER_START_WAIT_TIME_TOTAL) / 0.25);
          else if ((double) this.m_fingerTime > (double) TutorialControl.FINGER_END_WAIT_TIME_TOTAL - 0.25)
            num4 *= (int) (1.0 - ((double) this.m_fingerTime - 
                            ((double) TutorialControl.FINGER_END_WAIT_TIME_TOTAL - 0.25)) / 0.25);
          float num5 = num3 * 2f;
          float num6 = num5 * num5;
          this.m_ringTex.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(Vector3.Multiply(Vector3.One, num6));
          MatrixManager.GetInstance().Translate(this.m_fingerPos);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(new Color((int) byte.MaxValue, (int) byte.MaxValue, 
              (int) byte.MaxValue, num4), 0.0f, 1f, 0.0f, 1f);
          this.m_ringTex.UnSet();
        }
      }
      this.m_texture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3(96f * num1, 96f, 1f));
      MatrixManager.GetInstance().Translate(Vector3.Subtract(this.m_fingerPos,
          Vector3.Multiply(new Vector3(-0.125f * num1, -13f / 32f, 0.0f), 96f)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(this.m_fingerColor, (float) this.m_fingerFrame * 0.5f, 
          (float) ((double) this.m_fingerFrame * 0.5 + 0.5), 0.0f, 1f);
      this.m_texture.UnSet();
    }

    public override void Update(float dt)
    {
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      this.m_fingerPos = new Vector3(-1000f, -1000f, -1000f);
      this.m_fingerColor = new Color(0, 0, 0, 0);
      if (this.CanShowTute())
      {
        if ((double) this.m_fingerTime < (double) TutorialControl.FINGER_TIME_TOTAL)
        {
          this.m_fingerTime += dt;
          this.m_fingerPos = TutorialControl.FINGER_POS(this.m_fingerTime);
          TutorialControl tutorialControl = this;
          tutorialControl.m_fingerPos = Vector3.Multiply(tutorialControl.m_fingerPos, this.m_fingerScale);
          if ((double) this.m_pos.X > 0.0)
            this.m_fingerPos.X *= -1f;
          this.m_fingerFrame = 1;
          if ((double) this.m_fingerTime > 0.0)
          {
            this.m_fingerColor = Color.White;
            if ((double) this.m_fingerTime < (double) TutorialControl.FINGER_IN_TIME_TOTAL)
            {
              this.m_fingerColor.A = (byte) ((double) byte.MaxValue 
                                * (double) TutorialControl.FINGER_IN_TIME_PROGRESS(this.m_fingerTime));
              this.m_fingerFrame = 0;
              this.m_fingerPos.Y += TutorialControl.FINGER_DOWN_MOVE;
            }
            else if ((double) this.m_fingerTime < (double) TutorialControl.FINGER_START_WAIT_TIME_TOTAL)
            {
              this.m_fingerFrame = 0;
              this.m_fingerPos.Y += TutorialControl.FINGER_DOWN_MOVE 
                                - TutorialControl.FINGER_START_WAIT_TIME_PROGRESS(this.m_fingerTime)
                                * TutorialControl.FINGER_DOWN_MOVE;
            }
            else if ((double) this.m_fingerTime >= 
                            (double) TutorialControl.FINGER_ANIMATE_TIME_TOTAL 
                            && (double) this.m_fingerTime >= (double) TutorialControl.FINGER_SWIPE_TIME_TOTAL && (double) this.m_fingerTime >= (double) TutorialControl.FINGER_END_WAIT_TIME_TOTAL)
            {
              if ((double) this.m_fingerTime < (double) TutorialControl.FINGER_OUT_TIME_TOTAL)
              {
                this.m_fingerColor.A = (byte) ((double) byte.MaxValue 
                                    - 254.0 * (double) TutorialControl.FINGER_OUT_TIME_PROGRESS(
                                        this.m_fingerTime));
                this.m_fingerFrame = 0;
                this.m_fingerPos.Y += TutorialControl.FINGER_DOWN_MOVE;
              }
              else
              {
                this.m_fingerFrame = 0;
                this.m_fingerPos.Y += TutorialControl.FINGER_DOWN_MOVE;
                this.m_fingerTime = TutorialControl.FINGER_WAIT_TIME_TOTAL;
              }
            }
          }
        }
      }
      else
        this.m_fingerTime = TutorialControl.FINGER_WAIT_TIME_TOTAL;
      TutorialControl tutorialControl1 = this;
      tutorialControl1.m_fingerPos = Vector3.Add(tutorialControl1.m_fingerPos, this.m_pos);
    }

    public override void Reset() => this.m_fingerTime = TutorialControl.FINGER_WAIT_TIME_TOTAL;

    public void ButtonPressedAtPos(MenuButton button)
    {
      if ((double) this.m_fingerTime >= 0.0)
        return;
      if (button != null)
      {
        this.m_pos = button.m_pos;
        this.m_fingerScale = button.m_originalScale.X;
        if ((double) this.m_fingerScale > 256.0)
          this.m_fingerScale *= 0.5f;
      }
      this.m_fingerTime += TutorialControl.FINGER_TOUCH_ADD;
      if ((double) this.m_fingerTime <= 0.0)
        return;
      this.m_fingerTime = 0.0f;
    }

    public void ResetTutePos() => this.ResetTutePos((MenuButton) null);

    public void ResetTutePos(MenuButton button)
    {
      if (button != null)
      {
        this.m_pos = button.m_pos;
        this.m_fingerScale = button.m_originalScale.X;
        if ((double) this.m_fingerScale > 256.0)
          this.m_fingerScale *= 0.5f;
      }
      this.m_fingerTime = TutorialControl.FINGER_WAIT_TIME_TOTAL;
    }

    public bool CanShowTute()
    {
      return (double) Math.Abs(Game.game_work.gameOverTransition) > 0.99000000953674316;
    }
  }
}
