
// Type: FruitNinja2.PauseScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Mortar;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace FruitNinja2
{
  public class PauseScreen : HUDControl3d
  {
    public static bool show_leaderboards;
    public static bool show_leaderboards_init;
    protected float m_time;
    protected Vector3 m_originalScale;
    protected Vector3 m_pauseButtonScale;
    protected MenuButton m_pauseButton;
    protected MenuButton m_quitButton;
    protected MenuButton m_resumeButton;
    protected MenuButton m_retryButton;
    protected MenuButton m_leaderboardsButton;
    protected MenuButton m_achivementsButton;
    protected float m_enabled;
    protected Texture m_pauseTexture;
    protected Texture m_playTexture;
    protected float m_repauseTime;
    private PauseScreen.PS m_state;
    private static Texture s_flashTexture;

    public static float BEGINNING_WAIT => 0.15f;

    public static int PAUSE_TITLE_Y => 30;

    public static int RESUME_TITLE_Y => 145;

    public static int RETRY_TITLE_Y => 190;

    public static int QUIT_TITLE_Y => (int) byte.MaxValue;

    public static int TOP_BUTTONS_Y => 120;

    public PauseScreen()
    {
      this.m_texture = (Texture) null;
      this.m_texture = TextureManager.GetInstance().Load("pause_title.tex", true);
      this.m_repauseTime = 0.0f;
      this.m_originalScale = this.m_scale = new Vector3((float) this.m_texture.GetWidth(), (float) this.m_texture.GetHeight(), 1f);
      this.m_state = PauseScreen.PS.PS_DISABLED;
      this.m_selfCleanUp = false;
      this.m_time = 0.0f;
      this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT - (double) this.m_scale.Y) * 0.5), 0.0f);
      this.m_terminate = false;
      this.m_resumeButton = (MenuButton) null;
      this.m_quitButton = (MenuButton) null;
      this.m_leaderboardsButton = (MenuButton) null;
      this.m_achivementsButton = (MenuButton) null;
      this.m_pauseButton = (MenuButton) null;
      this.m_time = 0.0f;
      this.m_enabled = 1f;
      this.m_pauseButtonScale = Vector3.One;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
    }

    public void ContinueGameCallback()
    {
      if (this.m_state != PauseScreen.PS.PS_PAUSED)
        return;
      this.m_state = PauseScreen.PS.PS_UNPAUSE;
    }

    public void PauseGameCallback()
    {
      if ((double) this.m_enabled != 0.0)
        return;
      if (this.m_state == PauseScreen.PS.PS_DISABLED)
      {
        this.m_state = PauseScreen.PS.PS_IN;
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_PAUSE);
        GameTask.PauseGame();
      }
      else
      {
        if (this.m_state != PauseScreen.PS.PS_PAUSED)
          return;
        this.m_repauseTime = 2f;
        this.m_pauseButton.m_enabled = false;
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_UNPAUSE);
        this.m_state = PauseScreen.PS.PS_UNPAUSE;
      }
    }

    private void ExitGameCallback(IAsyncResult result)
    {
      int? nullable = default;//Guide.EndShowMessageBox(result);
      if (!nullable.HasValue || !nullable.HasValue || nullable.Value != 1)
        return;
      Game.game_work.saveData.ClearTotals();
      Game.game_work.saveData.ClearCombo();
      this.m_state = PauseScreen.PS.PS_QUIT;
      Game.game_work.loadedSaveState = false;
    }

    public void QuitGameCallback()
    {
      try
      {
        if (this.m_state != PauseScreen.PS.PS_PAUSED)
          return;
        string str1 = TheGame.instance.stringTable.GetString(707);
        string str2 = TheGame.instance.stringTable.GetString(702);
        string title = TheGame.instance.stringTable.GetString(705);
        string text = TheGame.instance.stringTable.GetString(738);
        string[] buttons = new string[2]{ str2, str1 };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (Guide.IsVisible)
        //  return;
        //Guide.BeginShowMessageBox(title, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.ExitGameCallback), (object) null);
      }
      catch
      {
        if (this.m_state != PauseScreen.PS.PS_PAUSED)
          return;
        Game.game_work.saveData.ClearTotals();
        Game.game_work.saveData.ClearCombo();
        this.m_state = PauseScreen.PS.PS_QUIT;
        Game.game_work.loadedSaveState = false;
      }
    }

    public void QuitGameCallbackForLeaderboards()
    {
      PopOverControl.Instance.In((PopOverControl.WhenIn) (() =>
      {
        LeaderboardsScreen control = new LeaderboardsScreen();
        control.Init();
        Game.game_work.hud.AddControl((HUDControl) control);
      }));
    }

    public void PlayerReview()
    {
    }

    public void RetryGameCallback()
    {
      if (this.m_state != PauseScreen.PS.PS_PAUSED)
        return;
      Game.game_work.saveData.ClearTotals();
      Game.game_work.saveData.ClearCombo();
      this.m_state = PauseScreen.PS.PS_RETRY;
      Game.game_work.loadedSaveState = false;
    }

    public override void Draw(float[] tintChannels)
    {
      if ((double) this.m_time <= 0.0)
        return;
      base.Draw(tintChannels);
    }

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_playTexture = (Texture) null;
      this.m_pauseTexture = (Texture) null;
    }

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      if (this.m_pauseButton == null)
      {
        this.m_pauseButton = new MenuButton("pause_button.tex", new Vector3(Game.SCREEN_WIDTH / 2f, (float) (-(double) Game.SCREEN_HEIGHT / 2.0), 0.0f), new MenuButton.MenuCallback(this.PauseGameCallback));
        this.m_pauseButton.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_pauseButton);
        this.m_pauseButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        this.m_pauseButtonScale = this.m_pauseButton.m_originalScale = Vector3.Multiply(Vector3.Multiply(Vector3.One, 64f), Game.HUD_SCALE);
        this.m_pauseButton.m_outerBound = 500f;
        this.m_pauseButton.m_triggerOnBackPress = true;
        this.m_pauseTexture = this.m_pauseButton.m_texture;
        this.m_playTexture = TextureManager.GetInstance().Load("textureswp7/play_button.tex");
      }
      if (this.m_quitButton == null)
      {
        this.m_quitButton = new MenuButton("quit_title.tex", new Vector3(0.0f, Game.SCREEN_HEIGHT, 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), -1, Vector3.Zero, true);
        this.m_quitButton.Init();
        this.m_quitButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        MenuButton quitButton = this.m_quitButton;
        quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, Game.GAME_MODE_SCALE_FIX);
        Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
      }
      if (this.m_retryButton == null)
      {
        this.m_retryButton = new MenuButton("retry_button.tex", new Vector3(0.0f, Game.SCREEN_HEIGHT, 0.0f), new MenuButton.MenuCallback(this.RetryGameCallback));
        this.m_retryButton.Init();
        this.m_retryButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        MenuButton retryButton = this.m_retryButton;
        retryButton.m_originalScale = Vector3.Multiply(retryButton.m_originalScale, Game.GAME_MODE_SCALE_FIX);
        Game.game_work.hud.AddControl((HUDControl) this.m_retryButton);
      }
      switch (this.m_state)
      {
        case PauseScreen.PS.PS_DISABLED:
          this.m_time *= 0.75f;
          if ((double) this.m_time < 0.0099999997764825821)
            this.m_time = 0.0f;
          if ((double) this.m_repauseTime > 0.0)
          {
            this.m_repauseTime -= dt;
            if ((double) this.m_repauseTime <= 0.0)
            {
              this.m_pauseButton.m_enabled = true;
              break;
            }
            break;
          }
          this.m_pauseButton.m_enabled = true;
          break;
        case PauseScreen.PS.PS_TO_MENU:
          this.m_enabled = 0.0f;
          this.m_time = 1f;
          if (Game.BombFlashFull())
          {
            this.m_time = 0.0f;
            this.m_enabled = 1f;
            PowerUpManager.GetInstance().Reset();
            this.m_state = PauseScreen.PS.PS_DISABLED;
            break;
          }
          break;
        case PauseScreen.PS.PS_IN:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.25);
          Game.game_work.pause = true;
          if ((double) this.m_time > 0.99900001287460327)
          {
            this.m_time = 1f;
            this.m_state = PauseScreen.PS.PS_PAUSED;
            break;
          }
          break;
        case PauseScreen.PS.PS_PAUSED:
          this.m_pauseButton.m_enabled = true;
          Game.game_work.pause = true;
          GamePadState state = GamePad.GetState((PlayerIndex) 0);
          GamePadButtons buttons = state.Buttons;
          if (buttons.Back == ButtonState.Pressed)
          {
            this.ContinueGameCallback();
            break;
          }
          break;
        case PauseScreen.PS.PS_UNPAUSE:
        case PauseScreen.PS.PS_RETRY:
          this.m_time *= 0.75f;
          if ((double) this.m_time < 1.0 / 1000.0)
          {
            this.m_time = 0.0f;
            if (this.m_state == PauseScreen.PS.PS_QUIT)
            {
              Game.QuitToMenu();
              Game.HitMenuBomb(this.m_quitButton.m_pos);
              this.m_enabled = 0.0f;
              this.m_time = 1f;
              this.m_state = PauseScreen.PS.PS_TO_MENU;
              GameTask.SaveCurrentData();
            }
            else if (this.m_state == PauseScreen.PS.PS_RETRY)
            {
              GameTask.SaveCurrentData();
              this.m_enabled = 0.0f;
              this.m_time = 0.0f;
              this.m_state = PauseScreen.PS.PS_DISABLED;
              this.m_repauseTime = 2f;
              GameTask.RetryLevel();
            }
            else
            {
              this.m_repauseTime = 2f;
              this.m_state = PauseScreen.PS.PS_DISABLED;
            }
            GameTask.UnpauseGame();
            break;
          }
          Game.game_work.pause = true;
          break;
        case PauseScreen.PS.PS_QUIT:
          this.m_time *= 0.5f;
          goto case PauseScreen.PS.PS_UNPAUSE;
      }
      if (this.IsEnabled())
      {
        this.m_enabled *= 0.75f;
        if ((double) this.m_enabled < 1.0 / 1000.0)
          this.m_enabled = 0.0f;
      }
      else
        this.m_enabled += (float) ((1.0 - (double) this.m_enabled) * 0.25);
      float time = this.m_time;
      float enabled = this.m_enabled;
      if (this.m_state == PauseScreen.PS.PS_QUIT)
      {
        this.m_enabled = 0.0f;
        this.m_time = 1f;
      }
      if ((double) this.m_time > 0.5)
      {
        this.m_pauseButton.m_texture = this.m_playTexture;
        this.m_pauseButton.m_rotation = 0.0f;
      }
      else
      {
        this.m_pauseButton.m_texture = this.m_pauseTexture;
        this.m_pauseButton.m_rotation = Game.IsMultiplayer() ? 90f : 0.0f;
      }
      this.m_pos.Y = (float) ((double) Game.SCREEN_HEIGHT * 0.5 + (double) this.m_scale.Y - ((double) Game.SCREEN_HEIGHT * 0.5 - (double) PauseScreen.PAUSE_TITLE_Y) * (double) this.m_time);
      if (this.m_resumeButton != null)
        this.m_resumeButton.m_pos.Y = (float) ((double) Game.SCREEN_HEIGHT * 1.5 - (double) PauseScreen.RESUME_TITLE_Y - (double) Game.SCREEN_HEIGHT * (double) this.m_time);
      if (this.m_retryButton != null)
      {
        this.m_retryButton.m_pos.Y = -20f;
        this.m_retryButton.m_pos.X = (float) (300.0 + -250.0 * (double) this.m_time);
      }
      if (this.m_quitButton != null)
      {
        this.m_quitButton.m_pos.Y = (float) -((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_quitButton.m_originalScale.Y * 0.5 - 5.0 + (1.0 - (double) this.m_time) * ((double) this.m_quitButton.m_originalScale.Y + 10.0));
        this.m_quitButton.m_pos.X = (float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) this.m_quitButton.m_originalScale.X * 0.5);
        this.m_quitButton.SetActive((double) this.m_time > 0.0099999997764825821);
      }
      if (this.m_leaderboardsButton != null)
      {
        this.m_leaderboardsButton.m_pos.Y = (float) -((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_quitButton.m_originalScale.Y * 0.5 - 5.0 + (1.0 - (double) this.m_time) * ((double) this.m_quitButton.m_originalScale.Y + 10.0));
        this.m_leaderboardsButton.m_pos.X = (float) (-(double) this.m_quitButton.m_originalScale.X * 0.5 - 20.0);
        this.m_leaderboardsButton.SetActive((double) this.m_time > 0.0099999997764825821);
      }
      if (this.m_achivementsButton != null)
      {
        this.m_achivementsButton.m_pos.Y = (float) -((double) Game.SCREEN_HEIGHT / 2.0 - (double) this.m_quitButton.m_originalScale.Y * 0.5 - 5.0 + (1.0 - (double) this.m_time) * ((double) this.m_quitButton.m_originalScale.Y + 10.0));
        this.m_achivementsButton.m_pos.X = (float) (-(double) this.m_quitButton.m_originalScale.X * 0.5 + 80.0);
        this.m_achivementsButton.SetActive((double) this.m_time > 0.0099999997764825821);
      }
      this.m_pauseButton.m_pos.Y = (float) (-(double) Game.SCREEN_HEIGHT / 2.0 + (double) this.m_pauseButton.m_originalScale.Y * 0.5 - 5.0);
      if (Game.IsMultiplayer())
      {
        this.m_pauseButton.m_pos.X = 0.0f;
        this.m_pauseButton.m_pos.Y -= Mortar.Math.Abs(this.m_enabled) * (this.m_pauseButton.m_originalScale.X + 10f);
      }
      else
        this.m_pauseButton.m_pos.X = (float) -((double) Game.SCREEN_WIDTH / 2.0 - (double) this.m_pauseButton.m_originalScale.X * 0.5 + 4.0 + (double) Mortar.Math.Abs(this.m_enabled) * ((double) this.m_pauseButton.m_originalScale.X + 10.0));
      MenuButton pauseButton = this.m_pauseButton;
      pauseButton.m_pos = Vector3.Add(pauseButton.m_pos, Vector3.Multiply(
          Vector3.Subtract(new Vector3(-50f, -20f, 0.0f), this.m_pauseButton.m_pos), this.m_time));
      this.m_pauseButton.m_originalScale = Vector3.Multiply(this.m_pauseButtonScale,
          (float) (0.75 + (double) this.m_time * 1.25));
      this.m_pauseButton.SetActive((double) Mortar.Math.Abs(
          Game.game_work.gameOverTransition) < 0.99000000953674316);
      this.m_time = time;
      this.m_enabled = enabled;
    }

    public override void PreDraw(float[] tintChannels)
    {
      if ((double) this.m_time <= 0.0)
        return;
      if (PauseScreen.s_flashTexture == null)
        PauseScreen.s_flashTexture = TextureManager.GetInstance().Load("textureswp7/flash.tex");
      float num = this.m_time * 10000f;
      PauseScreen.s_flashTexture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3(num, num, 1f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(new Color(0.0f, 0.0f, 0.0f, Mortar.Math.CLAMP(this.m_time * 1000f, 0.0f, 128f) / 256f));
      PauseScreen.s_flashTexture.UnSet();
    }

    public void SkipTo()
    {
      this.m_state = PauseScreen.PS.PS_PAUSED;
      this.m_time = 1f;
    }

    public bool IsEnabled()
    {
      return (double) Mortar.Math.Abs(Game.game_work.gameOverTransition) < 1.0 / 1000.0 && (double) Game.game_work.hitBombTime <= 0.0 && !Game.game_work.gameOver;
    }

    public float GetTime() => this.m_state != PauseScreen.PS.PS_QUIT ? this.m_time : 1f;

    public enum PS
    {
      PS_DISABLED,
      PS_TO_MENU,
      PS_IN,
      PS_PAUSED,
      PS_UNPAUSE,
      PS_RETRY,
      PS_QUIT,
      PS_GAME,
    }
  }
}
