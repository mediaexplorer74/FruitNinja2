
// Type: GameManager.CreditsScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mortar;

#nullable disable
namespace GameManager
{
  public class CreditsScreen : HUDControl3d
  {
    protected float m_time;
    protected Vector3 m_originalScale;
    protected MenuButton m_quitButton;
    protected static Texture s_boardTexture;
    protected static Texture m_creditsTexture;
    protected static Texture m_senseiTexture;
    public DojoScreen m_dojoScreen;
    private int m_state;
    private static float sx = 38f;
    private static float sy = 80f;

    public static int SENSEI_CENTRE_X => 424;

    public static int SENSEI_CENTRE_Y => 104;

    public static int CREDITS_CENTRE_X => 180;

    public static int CREDITS_CENTRE_Y => 160;

    public static int ABOUT_CENTRE_X => 190;

    public static int ABOUT_CENTRE_Y => 97;

    public static float ABOUT_SCREEN_HEIGHT => 320f;

    public static string VERSION_TITLE => "VERSION:";

    public CreditsScreen(DojoScreen dojo)
    {
      this.m_dojoScreen = dojo;
      this.m_texture = CreditsScreen.s_boardTexture;
      this.m_selfCleanUp = false;
      this.m_quitButton = (MenuButton) null;
      this.m_state = 0;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_time = 0.0f;
    }

    public void QuitGameCallback()
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      this.m_state = 2;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public static void LoadContent()
    {
      CreditsScreen.m_creditsTexture = TextureManager.GetInstance().Load("credits_screen.tex", true);
      CreditsScreen.m_senseiTexture = TextureManager.GetInstance().Load("textureswp7/sensei.tex");
    }

    public static void UnLoadContent()
    {
      CreditsScreen.s_boardTexture = (Texture) null;
      CreditsScreen.m_creditsTexture = (Texture) null;
      CreditsScreen.m_senseiTexture = (Texture) null;
    }

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_time = 0.0f;
    }

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      switch (this.m_state)
      {
        case 0:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_time = 1f;

          this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game2.SCREEN_WIDTH / 2.0), 
              (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), 
              Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);

          this.m_quitButton.Init();
          this.m_quitButton.m_triggerOnBackPress = true;
          Game2.game_work.hud.AddControl((HUDControl) this.m_quitButton);
          Game2.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
          MenuButton quitButton = this.m_quitButton;
          quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
          Entity entity = this.m_quitButton.m_entity;
          entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
          this.m_state = 1;
          break;
        case 1:
          GamePadState state = GamePad.GetState((PlayerIndex) 0);
          GamePadButtons buttons = state.Buttons;
          if (buttons.Back != ButtonState.Pressed)
            break;
          this.QuitGameCallback();
          break;
        case 2:
          this.m_time *= 0.75f;
          if ((double) this.m_time >= 1.0 / 1000.0)
            break;
          this.m_dojoScreen.Reset();
          this.m_terminate = true;
          break;
      }
    }

    public override void Draw(float[] tintChannels)
    {
      if (this.m_texture != null)
      {
        this.m_texture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3((float) ((double) this.m_texture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0),
            (float) ((double) this.m_texture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 1f));

        float num1 = (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 + (double) this.m_texture.GetHeight() * 0.5);
        float num2 = (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - (double) CreditsScreen.ABOUT_CENTRE_Y - 60.0);
        float num3 = num1 - (num1 - num2) * this.m_time;
        MatrixManager.GetInstance().Translate(new Vector3((float) CreditsScreen.ABOUT_CENTRE_X - Game2.SCREEN_WIDTH / 2f, num3, 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_texture.UnSet();
      }
      if (CreditsScreen.m_creditsTexture != null)
      {
        CreditsScreen.m_creditsTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3((float) (((double) CreditsScreen.m_creditsTexture.GetWidth() - (double) CreditsScreen.sx) * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) (((double) CreditsScreen.m_creditsTexture.GetHeight() - (double) CreditsScreen.sy) * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 1f));
        float num4 = (float) (-((double) Game2.SCREEN_HEIGHT / 2.0) - (double) CreditsScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game2.GAME_MODE_SCALE_FIX);
        float num5 = (float) -((double) Game2.SCREEN_HEIGHT / 2.0) + (float) (320 - CreditsScreen.CREDITS_CENTRE_Y);
        float num6 = num4 - (num4 - num5) * this.m_time;
        MatrixManager.GetInstance().Translate(new Vector3((float) CreditsScreen.CREDITS_CENTRE_X - Game2.SCREEN_WIDTH / 2f, num6, 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        CreditsScreen.m_creditsTexture.UnSet();
      }
      if (CreditsScreen.m_senseiTexture == null)
        return;
      CreditsScreen.m_senseiTexture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3((float) ((double) CreditsScreen.m_senseiTexture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) CreditsScreen.m_senseiTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 1f));
      float num7 = (float) ((double) CreditsScreen.m_senseiTexture.GetWidth() * 0.5 + (double) Game2.SCREEN_WIDTH / 2.0);
      float num8 = (float) CreditsScreen.SENSEI_CENTRE_X - Game2.SCREEN_WIDTH / 2f;
      float num9 = num7 - (num7 - num8) * this.m_time;
      MatrixManager.GetInstance().Translate(new Vector3(num9, CreditsScreen.ABOUT_SCREEN_HEIGHT / 2f - (float) CreditsScreen.SENSEI_CENTRE_Y, 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(Color.White);
      CreditsScreen.m_senseiTexture.UnSet();
    }

    public enum AS
    {
      AS_IN,
      AS_WAIT,
      AS_OUT,
    }
  }
}
