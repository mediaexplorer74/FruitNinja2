
// Type: GameManager.ConstructionScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class ConstructionScreen : HUDControl3d
  {
    protected float m_time;
    protected Vector3 m_originalScale;
    protected MenuButton m_quitButton;
    protected static Texture s_boardTexture;
    protected static Texture s_boardTexture2;
    protected static Texture m_creditsTexture;
    protected static Texture m_senseiTexture;
    public DojoScreen m_dojoScreen;
    private int m_state;
    private int m_mode;

    public static int SENSEI_CENTRE_X => 395;

    public static int SENSEI_CENTRE_Y => 104;

    public static int CREDITS_CENTRE_X => 190;

    public static int CREDITS_CENTRE_Y => 256;

    public static int ABOUT_CENTRE_X => 190;

    public static int ABOUT_CENTRE_Y => 97;

    public static float ABOUT_SCREEN_HEIGHT => 320f;

    public static string VERSION_TITLE => "VERSION:";

    public ConstructionScreen(DojoScreen dojo, int mode)
    {
      this.m_dojoScreen = dojo;
      this.m_mode = mode;
      this.m_texture = mode == 0 ? ConstructionScreen.s_boardTexture : ConstructionScreen.s_boardTexture2;
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
      ConstructionScreen.s_boardTexture = TextureManager.GetInstance().Load("textureswp7/swag_soon.tex");
      ConstructionScreen.s_boardTexture2 = TextureManager.GetInstance().Load("textureswp7/arcade_soon.tex");
    }

    public static void UnLoadContent()
    {
      ConstructionScreen.s_boardTexture = (Texture) null;
      ConstructionScreen.m_creditsTexture = (Texture) null;
      ConstructionScreen.m_senseiTexture = (Texture) null;
      ConstructionScreen.s_boardTexture2 = (Texture) null;
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
      if (Game2.isWP7TrialMode())
      {
        Game2.ShowBuyMessageBox();
        if (this.m_mode == 0)
        {
          this.m_dojoScreen.Reset();
        }
        else
        {
          GameModeScreen control = new GameModeScreen(false);
          control.Init();
          Game2.game_work.hud.AddControl((HUDControl) control);
        }
        this.m_terminate = true;
      }
      else
      {
        switch (this.m_state)
        {
          case 0:
            this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
            if ((double) this.m_time <= 0.99900001287460327)
              break;
            this.m_time = 1f;
            this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game2.SCREEN_WIDTH / 2.0), (float) ((double) ConstructionScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
            this.m_quitButton.Init();
            Game2.game_work.hud.AddControl((HUDControl) this.m_quitButton);
            Game2.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
            MenuButton quitButton = this.m_quitButton;
            quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
            Entity entity = this.m_quitButton.m_entity;
            entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
            this.m_state = 1;
            break;
          case 2:
            this.m_time *= 0.75f;
            if ((double) this.m_time >= 1.0 / 1000.0)
              break;
            if (this.m_mode == 0)
            {
              this.m_dojoScreen.Reset();
            }
            else
            {
              GameModeScreen control = new GameModeScreen(false);
              control.Init();
              Game2.game_work.hud.AddControl((HUDControl) control);
            }
            this.m_terminate = true;
            break;
        }
      }
    }

    public override void Draw(float[] tintChannels)
    {
      if (this.m_texture != null)
      {
        this.m_texture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3(480f, 320f, 1f));
        float num1 = (float) ((double) ConstructionScreen.ABOUT_SCREEN_HEIGHT / 2.0 + (double) this.m_texture.GetHeight() * 0.5 + 40.0);
        float num2 = 0.0f;
        float num3 = num1 - (num1 - num2) * this.m_time;
        MatrixManager.GetInstance().Translate(new Vector3(0.0f, num3, 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_texture.UnSet();
      }
      if (ConstructionScreen.m_creditsTexture != null)
      {
        ConstructionScreen.m_creditsTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3((float) ((double) ConstructionScreen.m_creditsTexture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) ConstructionScreen.m_creditsTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 1f));
        float num4 = (float) (-((double) Game2.SCREEN_HEIGHT / 2.0) - (double) ConstructionScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game2.GAME_MODE_SCALE_FIX);
        float num5 = (float) -((double) Game2.SCREEN_HEIGHT / 2.0) + (float) (320 - ConstructionScreen.CREDITS_CENTRE_Y);
        float num6 = num4 - (num4 - num5) * this.m_time;
        MatrixManager.GetInstance().Translate(new Vector3((float) ConstructionScreen.CREDITS_CENTRE_X - Game2.SCREEN_WIDTH / 2f, num6, 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        ConstructionScreen.m_creditsTexture.UnSet();
      }
      if (ConstructionScreen.m_senseiTexture == null)
        return;
      ConstructionScreen.m_senseiTexture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3((float) ((double) ConstructionScreen.m_senseiTexture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) ConstructionScreen.m_senseiTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 1f));
      float num7 = (float) ((double) ConstructionScreen.m_senseiTexture.GetWidth() * 0.5 + (double) Game2.SCREEN_WIDTH / 2.0);
      float num8 = (float) ConstructionScreen.SENSEI_CENTRE_X - Game2.SCREEN_WIDTH / 2f;
      float num9 = num7 - (num7 - num8) * this.m_time;
      MatrixManager.GetInstance().Translate(new Vector3(num9, ConstructionScreen.ABOUT_SCREEN_HEIGHT / 2f - (float) ConstructionScreen.SENSEI_CENTRE_Y, 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(Color.White);
      ConstructionScreen.m_senseiTexture.UnSet();
    }

    public enum AS
    {
      AS_IN,
      AS_WAIT,
      AS_OUT,
    }
  }
}
