
// Type: GameManager.GameModeScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace GameManager
{
  public class GameModeScreen : HUDControl3d
  {
    protected float m_time;
    protected Vector3 m_originalScale;
    protected MenuButton m_quitButton;
    protected float m_comingSoonRespawnTime1;
    protected float m_comingSoonRespawnTime2;
    protected MenuButton m_comingSoon1;
    protected MenuButton m_comingSoon2;
    protected float m_infoBoardTime;
    protected bool m_isMultiplayer;
    protected HUD.HUD_ORDER m_backOrder;
    public GameModeScreen.GMS m_state;
    private static Texture s_classicTexture = (Texture) null;
    private static Texture s_zenTexture = (Texture) null;
    private static Texture s_commingSoonTexture = (Texture) null;
    private static Texture s_titleTexture = (Texture) null;
    private static Texture s_senseiTexture = (Texture) null;
    private static Texture s_modeselectTexture = (Texture) null;
    private static Texture s_zenBoardTexture = (Texture) null;
    private bool RESPOAWWEN;
    private static Vector3 Updatescale;
    private static bool firstRender = false;
    private static GameVertex[] top_tri = ArrayInit.CreateFilledArray<GameVertex>(3);
    private static GameVertex[] btm_tri = ArrayInit.CreateFilledArray<GameVertex>(3);

    public GameModeScreen()
      : this(false)
    {
    }

    public static Vector3 CLASSIC_MODE_POS => new Vector3(-70f, 71f, 0.0f);

    public static Vector3 ZEN_MODE_POS => new Vector3(88f, 48f, 0.0f);

    public static Vector3 INTENSE_MODE_POS => new Vector3(19f, -76f, 0.0f);

    public static Vector3 COMING_SOON_1_POS => new Vector3(12f, -72f, 0.0f);

    public static Vector3 COMING_SOON_2_POS => new Vector3(90f, -75f, 0.0f);

    public static Vector3 ZEN_BOARD_POS_START => new Vector3(314f, 14f, 10f);

    public static Vector3 ZEN_BOARD_POS_FINISH => new Vector3(194f, 29f, 10f);

    public static Vector3 MODE_SELECT_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0 + 125.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 30.0), 0.0f);
      }
    }

    public static Vector3 TITLE_POS
    {
      get
      {
        return new Vector3((float) ((double) Game2.SCREEN_WIDTH / 2.0 - 58.0), (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - 23.0), 0.0f);
      }
    }

    public static Vector3 SENSEI_SELECT_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0 + 52.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 128.0), 0.0f);
      }
    }

    public static void LoadContent()
    {
      GameModeScreen.s_titleTexture = TextureManager.GetInstance().Load("textureswp7/sml_title.tex");
      GameModeScreen.s_senseiTexture = TextureManager.GetInstance().Load("textureswp7/mode_sensei.tex");
      GameModeScreen.s_modeselectTexture = TextureManager.GetInstance().Load("mode_select.tex", true);
      GameModeScreen.s_classicTexture = TextureManager.GetInstance().Load("classic.tex", true);
      GameModeScreen.s_zenTexture = TextureManager.GetInstance().Load("mode_2.tex", true);
      GameModeScreen.s_commingSoonTexture = TextureManager.GetInstance().Load("arcade_mode.tex", true);
      GameModeScreen.s_zenBoardTexture = TextureManager.GetInstance().Load("zen_sign.tex", true);
    }

    public static void UnLoadContent()
    {
      GameModeScreen.s_classicTexture = (Texture) null;
      GameModeScreen.s_zenTexture = (Texture) null;
      GameModeScreen.s_commingSoonTexture = (Texture) null;
      GameModeScreen.s_titleTexture = (Texture) null;
      GameModeScreen.s_senseiTexture = (Texture) null;
      GameModeScreen.s_modeselectTexture = (Texture) null;
      GameModeScreen.s_zenBoardTexture = (Texture) null;
    }

    public GameModeScreen(bool isMultiplayer)
    {
      this.m_isMultiplayer = isMultiplayer;
      this.m_texture = (Texture) null;
      this.m_infoBoardTime = -2.5f;
      this.m_selfCleanUp = false;
      this.m_quitButton = (MenuButton) null;
      this.m_comingSoon1 = (MenuButton) null;
      this.m_comingSoon2 = (MenuButton) null;
      this.m_state = GameModeScreen.GMS.GMS_IN;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
      this.m_backOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_time = 0.0f;
      this.RESPOAWWEN = false;
    }

    public override void Init() => this.Reset();

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_time = 0.0f;
    }

    public override void Update(float dt)
    {
      if (ActorManager.GetInstance().GetNumEntities(0) == 0U && this.RESPOAWWEN)
      {
        this.RESPOAWWEN = false;
        this.m_time = 0.0f;
        this.m_state = GameModeScreen.GMS.GMS_IN;
                Mortar.Game1.instance.DoUpsell(false);
      }
      float time = this.m_time;
      switch (this.m_state)
      {
        case GameModeScreen.GMS.GMS_IN:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.15000000596046448);
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_state = GameModeScreen.GMS.GMS_WAIT;
          this.m_time = 1f;
          this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) ((double) Game2.SCREEN_WIDTH / 2.0 - 45.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 50.0), 0.0f), new MenuButton.MenuCallback(this.QuitCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
          this.m_quitButton.Init();
          this.m_quitButton.m_triggerOnBackPress = true;
          Game2.game_work.hud.AddControl((HUDControl) this.m_quitButton);
          MenuButton quitButton = this.m_quitButton;
          quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.75f);
          Entity entity1 = this.m_quitButton.m_entity;
          entity1.m_cur_scale = Vector3.Multiply(entity1.m_cur_scale, 0.75f);
          MenuButton menuButton1 = new MenuButton(GameModeScreen.s_classicTexture, GameModeScreen.CLASSIC_MODE_POS, new MenuButton.MenuCallback(this.ClassicModeCallback), Fruit.FruitType("watermelon"));
          Game2.game_work.tutorialControl.ResetTutePos(menuButton1);
          menuButton1.Init();
          Game2.game_work.hud.AddControl((HUDControl) menuButton1);
          MenuButton menuButton2 = menuButton1;
          menuButton2.m_originalScale = Vector3.Multiply(menuButton2.m_originalScale, 0.9f);
          Entity entity2 = menuButton1.m_entity;
          entity2.m_cur_scale = Vector3.Multiply(entity2.m_cur_scale, 0.95f);
          GameModeScreen.Updatescale = Vector3.Multiply(menuButton1.m_originalScale, 0.85f);
          MenuButton control1 = new MenuButton(GameModeScreen.s_zenTexture, GameModeScreen.ZEN_MODE_POS, new MenuButton.MenuCallback(this.ZenModeCallback), Fruit.FruitType("apple_red"));
          control1.m_isTrialLockable = true;
          control1.Init();
          control1.m_originalScale = GameModeScreen.Updatescale;
          Entity entity3 = control1.m_entity;
          entity3.m_cur_scale = Vector3.Multiply(entity3.m_cur_scale, 0.9f);
          Game2.game_work.hud.AddControl((HUDControl) control1);
          MenuButton control2 = new MenuButton(GameModeScreen.s_commingSoonTexture, GameModeScreen.COMING_SOON_1_POS, new MenuButton.MenuCallback(this.CommingsSoonCallback), Fruit.FruitType("banana"));
          control2.m_isTrialLockable = true;
          control2.Init();
          control2.m_originalScale = GameModeScreen.Updatescale;
          Entity entity4 = control2.m_entity;
          entity4.m_cur_scale = Vector3.Multiply(entity4.m_cur_scale, 0.75f);
          Game2.game_work.hud.AddControl((HUDControl) control2);
          this.m_comingSoonRespawnTime1 = -1f;
          this.m_comingSoonRespawnTime2 = -1f;
          break;
        case GameModeScreen.GMS.GMS_WAIT:
          this.m_infoBoardTime += Mortar.Math.CLAMP((float) ((1.0 - (double) this.m_infoBoardTime) * 0.25), -0.1f, 0.1f);
          if ((double) this.m_comingSoonRespawnTime1 > 0.0)
          {
            this.m_comingSoonRespawnTime1 -= dt;
            if ((double) this.m_comingSoonRespawnTime1 <= 0.0)
              this.m_comingSoonRespawnTime1 = -1f;
          }
          if ((double) this.m_comingSoonRespawnTime1 < 0.0)
          {
            MenuButton comingSoon1 = this.m_comingSoon1;
          }
          if ((double) this.m_comingSoonRespawnTime2 <= 0.0)
            break;
          this.m_comingSoonRespawnTime2 -= dt;
          if ((double) this.m_comingSoonRespawnTime2 > 0.0)
            break;
          this.m_comingSoonRespawnTime2 = -1f;
          break;
        case GameModeScreen.GMS.GMS_CLASSIC:
        case GameModeScreen.GMS.GMS_CASINO:
        case GameModeScreen.GMS.GMS_ARCADE:
        case GameModeScreen.GMS.GMS_ZEN:
          this.m_time *= 0.85f;
          this.m_infoBoardTime = this.m_time;
          if ((double) Game2.game_work.gameOverTransition < -0.89999997615814209)
          {
            Game2.game_work.levelStartCoins = Game2.game_work.coins;
            WaveManager.GetInstance().Reset(true);
            Game2.game_work.gameOver = true;
          }
          Game2.game_work.gameOverTransition *= 0.75f;
          if ((double) Mortar.Math.Abs(Game2.game_work.gameOverTransition) >= 1.0 / 1000.0)
            break;
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_DANANANA_SCHWING);
          Game2.game_work.gameOverTransition = 0.0f;
          Game2.game_work.gameOver = false;
          this.m_terminate = true;
          Game2.game_work.mainScreen.m_state = MainScreen.MS.MS_GAME;
          break;
        case GameModeScreen.GMS.GMS_OUT:
          this.m_time *= 0.75f;
          this.m_infoBoardTime = this.m_time;
          if ((double) time > 0.25 && (double) this.m_time <= 0.25)
            Game2.game_work.mainScreen.m_state = MainScreen.MS.MS_RETURN;
          if ((double) this.m_time >= 1.0 / 1000.0)
            break;
          this.m_terminate = true;
          break;
      }
    }

    public void DeletedMenuButton(HUDControl control)
    {
      if (control == this.m_comingSoon1)
      {
        this.m_comingSoon1 = (MenuButton) null;
        this.m_comingSoonRespawnTime1 = 0.85f;
      }
      if (control != this.m_comingSoon2)
        return;
      this.m_comingSoon2 = (MenuButton) null;
      this.m_comingSoonRespawnTime2 = 0.85f;
    }

    public override void Draw(float[] tintchannnnel)
    {
      if (!GameModeScreen.firstRender)
      {
        GameModeScreen.firstRender = true;
        Color color = new Color(0, 0, 0, 128);
        for (int index = 0; index < 3; ++index)
        {
          GameModeScreen.top_tri[index].X = GameModeScreen.top_tri[index].Y = GameModeScreen.top_tri[index].Z = GameModeScreen.top_tri[index].nx = GameModeScreen.top_tri[index].ny = 0.0f;
          GameModeScreen.top_tri[index].nz = 1f;
          GameModeScreen.top_tri[index].color = color;
          GameModeScreen.top_tri[index].u = 0.5f;
          GameModeScreen.top_tri[index].v = 0.5f;
          GameModeScreen.btm_tri[index].X = GameModeScreen.btm_tri[index].Y = GameModeScreen.btm_tri[index].Z = GameModeScreen.btm_tri[index].nx = GameModeScreen.btm_tri[index].ny = 0.0f;
          GameModeScreen.btm_tri[index].nz = 1f;
          GameModeScreen.btm_tri[index].color = color;
          GameModeScreen.btm_tri[index].u = 0.5f;
          GameModeScreen.btm_tri[index].v = 0.5f;
        }
        GameModeScreen.top_tri[1].Y = GameModeScreen.top_tri[2].Y = 82f;
        GameModeScreen.top_tri[2].X = -656f;
        GameModeScreen.btm_tri[1].Y = GameModeScreen.btm_tri[2].Y = -82f;
        GameModeScreen.btm_tri[2].X = 656f;
      }
      MatrixManager.GetInstance().Reset();

      MatrixManager.GetInstance().Scale(Vector3.Multiply(new Vector3((float) 
          (GameModeScreen.s_senseiTexture.GetWidth() + 1U),
          (float) (GameModeScreen.s_senseiTexture.GetHeight() + 1U), 0.0f), 
          Game2.GAME_MODE_SCALE_FIX));

      MatrixManager.GetInstance().Translate(
          Vector3.Subtract(GameModeScreen.SENSEI_SELECT_POS, 
          Vector3.Multiply(Vector3.Multiply(Vector3.UnitX, 
          (float) GameModeScreen.s_senseiTexture.GetWidth()), 1f - this.m_time)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      GameModeScreen.s_senseiTexture.Set();
      Mesh.DrawQuad(Color.White);
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Translate(new Vector3(Game2.SCREEN_WIDTH / 2f, 
          (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - 48.0 * (double) this.m_time), 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawTriList(GameModeScreen.top_tri, 3, true);
      GameModeScreen.s_senseiTexture.UnSet();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(Vector3.Multiply(new Vector3((float)
          (GameModeScreen.s_titleTexture.GetWidth() + 1U), 
          (float) (GameModeScreen.s_titleTexture.GetHeight() + 1U), 0.0f), Game2.GAME_MODE_SCALE_FIX));
      MatrixManager.GetInstance().Translate(Vector3.Add(
          GameModeScreen.TITLE_POS, Vector3.Multiply(Vector3.Multiply(Vector3.UnitY, 48f), 1f - this.m_time)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      GameModeScreen.s_titleTexture.Set();
      Mesh.DrawQuad(Color.White);
      GameModeScreen.s_titleTexture.UnSet();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Translate(new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 55.0 * (double) this.m_time), 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      GameModeScreen.s_senseiTexture.Set();
      Mesh.DrawTriList(GameModeScreen.btm_tri, 3, true);
      GameModeScreen.s_senseiTexture.UnSet();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(Vector3.Multiply(new Vector3((float) (GameModeScreen.s_modeselectTexture.GetWidth() + 1U), (float) (GameModeScreen.s_modeselectTexture.GetHeight() + 1U), 0.0f), Game2.GAME_MODE_SCALE_FIX));
      MatrixManager.GetInstance().Translate(Vector3.Subtract(GameModeScreen.MODE_SELECT_POS, Vector3.Multiply(Vector3.Multiply(Vector3.UnitY, 55f), 1f - this.m_time)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      GameModeScreen.s_modeselectTexture.Set();
      Mesh.DrawQuad(Color.White);
      GameModeScreen.s_modeselectTexture.UnSet();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(Vector3.Multiply(new Vector3((float) (GameModeScreen.s_zenBoardTexture.GetWidth() + 1U), (float) (GameModeScreen.s_zenBoardTexture.GetHeight() + 1U), 0.0f), Game2.GAME_MODE_SCALE_FIX));
      MatrixManager.GetInstance().Translate(Vector3.Add(GameModeScreen.ZEN_BOARD_POS_START, Vector3.Multiply(Vector3.Subtract(GameModeScreen.ZEN_BOARD_POS_FINISH, GameModeScreen.ZEN_BOARD_POS_START), this.m_infoBoardTime)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      GameModeScreen.s_zenBoardTexture.Set();
      Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
      GameModeScreen.s_zenBoardTexture.UnSet();
    }

    public void QuitCallback()
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      this.m_state = GameModeScreen.GMS.GMS_OUT;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(5f) + 5f, -Mortar.Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public void ClassicModeCallback()
    {
      this.m_state = GameModeScreen.GMS.GMS_CLASSIC;
      Game2.game_work.gameMode = Game2.GAME_MODE.GM_CLASSIC;
    }

    public void CasinoModeCallback()
    {
      this.m_state = GameModeScreen.GMS.GMS_CASINO;
      Game2.game_work.gameMode = Game2.GAME_MODE.GM_CASINO;
    }

    public void ZenModeCallback()
    {
      if (Game2.isWP7TrialMode())
      {
        Game2.ClearMenuItems();
        this.RESPOAWWEN = true;
      }
      else
      {
        this.m_state = GameModeScreen.GMS.GMS_ZEN;
        Game2.game_work.gameMode = Game2.GAME_MODE.GM_ZEN;
      }
    }

    public void CommingsSoonCallback()
    {
      if (Game2.isWP7TrialMode())
      {
        Game2.ClearMenuItems();
        this.RESPOAWWEN = true;
      }
      else
      {
        this.m_state = GameModeScreen.GMS.GMS_ARCADE;
        Game2.game_work.gameMode = Game2.GAME_MODE.GM_ARCADE;
      }
    }

    public bool IsMultiplayer() => throw new MissingMethodException();

    public enum GMS
    {
      GMS_IN,
      GMS_WAIT,
      GMS_CLASSIC,
      GMS_CASINO,
      GMS_ARCADE,
      GMS_ZEN,
      GMS_OUT,
    }
  }
}
