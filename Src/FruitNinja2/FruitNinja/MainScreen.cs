
// Type: FruitNinja2.MainScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mortar;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace FruitNinja2
{
  public class MainScreen : HUDControl3d
  {
    protected float m_time;
    protected Vector3 m_originalScale;
    protected Mortar.Texture s_newGameTex;
    protected Mortar.Texture s_aboutTex;
    protected Mortar.Texture s_moreGamesTex;
    protected Mortar.Texture s_leaderboardsTex;
    protected MenuButton m_achivementsButton;
    protected MenuButton m_gamerProfileButton;
    protected MenuButton m_helpOptionsButton;
    protected MenuButton m_leaderboardsButton;
    protected MenuButton m_marketplaceButton;
    protected MenuButton m_purchaseButton;
    protected Texture2D m_live;
    protected MenuButton m_newGame;
    protected MenuButton m_dojoButton;
    protected MenuButton m_moreGames;
    protected MenuButton m_multiplayerGame;
    protected MenuButton m_sound;
    protected MenuButton m_music;
    protected Mortar.Texture m_leaderBoardComingSoonTexture;
    protected Vector3 m_comingSoonPos;
    protected Mortar.Texture[] m_musicTextures = new Mortar.Texture[2];
    protected Mortar.Texture[] m_soundTextures = new Mortar.Texture[2];
    protected Mortar.Texture m_fruitTex;
    protected Mortar.Texture m_ninjaTex;
    protected Mortar.Model m_backing;
    protected Mortar.Texture m_tuteTex;
    protected Vector3 m_tutePos;
    protected float m_tuteTime;
    protected Vector3 m_fruitPos;
    protected Vector3 m_ninjaPos;
    protected float m_ninjaGrav;
    public MainScreen.MS m_state;
    public float m_transitionWait;
    private static float tute = 1f;
    private static int exitCode;
    private GamePadState prev;
    private GamePadState curr;
    private static float liveX;
    private static float liveY;
    private static bool madeTris = false;
    private static GameVertex[] top_tri = new GameVertex[3];

    public static float BEGINNING_WAIT => 0.15f;

    public static float BEGINNING_SCALE => 2f;

    public static float NORMAL_SCALE => 1f;

    public static float SLIDE_IN_TIME => 0.2f;

    public static float POP_SINE => (float) Mortar.Math.DEGREE_TO_IDX(110f);

    public static float TOP_BUTTONS_Y => (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 24.5);

    public static Vector3 TUTE_DIRECTION => new Vector3(-120f, -17f, 0.0f);

    public static Vector3 TUTE_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + 65.0), (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 134.0), 0.0f);
      }
    }

    public static float BOMB_FLASH_FULL => 1.55f;

    public static float OVERALL_BUTTON_SCALE => 1f;

    public static Vector3 NEW_GAME_POS
    {
      get => new Vector3((float) (236.0 - (double) Game.SCREEN_WIDTH / 2.0), -66f, 0.0f);
    }

    public static Vector3 DOJO_POS
    {
      get => new Vector3((float) (69.0 - (double) Game.SCREEN_WIDTH / 2.0), -87f, 0.0f);
    }

    public static Vector3 LEADERBOARD_BUTTON_POS
    {
      get => new Vector3((float) (388.0 - (double) Game.SCREEN_WIDTH / 2.0), 7f, 0.0f);
    }

    public static Vector3 MORE_GAMES_POS
    {
      get => new Vector3((float) (422.0 - (double) Game.SCREEN_WIDTH / 2.0), -106f, 0.0f);
    }

    public MainScreen()
    {
      this.m_fruitTex = TextureManager.GetInstance().Load("textureswp7/fruit_text.tex");
      this.m_ninjaTex = TextureManager.GetInstance().Load("textureswp7/ninja_text.tex");
      this.m_tuteTex = TextureManager.GetInstance().Load("slice_fruit.tex", true);
      this.m_backing = null;
      this.s_newGameTex = TextureManager.GetInstance().Load("newgame.tex", true);
      this.s_aboutTex = TextureManager.GetInstance().Load("textureswp7/dojo_icon.tex");
      this.m_moreGames = (MenuButton) null;
      this.m_originalScale = this.m_scale = new Vector3(480f, 138f, 1f);
      this.m_state = MainScreen.MS.MS_IN;
      this.m_selfCleanUp = false;
      this.m_time = 0.0f;
      this.m_sound = (MenuButton) null;
      this.m_music = (MenuButton) null;
      this.m_newGame = (MenuButton) null;
      this.m_multiplayerGame = (MenuButton) null;
      this.m_dojoButton = (MenuButton) null;
      this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT - (double) this.m_scale.Y) * 0.5), 0.0f);
      this.m_tuteTime = 1f;
      this.m_ninjaPos.Y = Game.SCREEN_HEIGHT / 2f + (float) (this.m_ninjaTex.GetHeight() / 2U);
      this.m_ninjaGrav = 0.0f;
      this.m_transitionWait = 0.0f;
      this.m_terminate = false;
      for (int index = 0; index < 2; ++index)
      {
        this.m_soundTextures[index] = null;
        this.m_musicTextures[index] = null;
      }
    }

    public void NewGameCallback()
    {
      this.m_state = MainScreen.MS.MS_OUT;
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_DANANANA_SCHWING);
      if (this.m_newGame == null)
        return;
      Mortar.Math.g_random.Seed((uint) ((double) int.MinValue * (double) this.m_newGame.m_rotation));
    }

    public void LeaderboardsCallback()
    {
      //RnD
      if (1==0)//(Gamer.SignedInGamers[(PlayerIndex) 0] == null || !TheGame.logInSucceeded)
      {
        string[] buttons = new string[1]
        {
          TheGame.instance.stringTable.GetString(703)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
      }
      else
      {
        LeaderboardsScreen.SetStartLeaderboard(0);
        this.m_state = MainScreen.MS.MS_OPENFEINT;
        this.m_time = 1f;
        Game.game_work.tutorialControl.ResetTutePos();
        Game.ClearMenuItems();
      }
    }

    public void MoreGamesCallback() => this.m_state = MainScreen.MS.MS_IN;

    public void AboutCallback()
    {
      this.m_state = MainScreen.MS.MS_DOJO;
      this.m_time = 1f;
      Game.game_work.tutorialControl.ResetTutePos();
    }

    private void NotConnected(IAsyncResult result)
    {
    }

    public void MarketplaceCallback()
    {
      try
      {
        SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
        if (signedInGamer == null)
        {
          string[] buttons = new string[1]
          {
            TheGame.instance.stringTable.GetString(703)
          };
          //while (Guide.IsVisible)
          //  Thread.Sleep(32);
          //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
        }
        //else if (!Guide.IsVisible && signedInGamer.Privileges.AllowPurchaseContent && signedInGamer.IsSignedInToLive && TheGame.logInSucceeded)
        //{
            //if (Game.isWP7TrialMode())
            //{
            //    int num = Guide.IsVisible ? 1 : 0;
            //    //Guide.ShowMarketplace((PlayerIndex) 0);
            //}
            //else
            //{
                //new MarketplaceDetailTask()
                //{
                //    ContentType = ((MarketplaceContentType)1)
                //}.Show();
            //}
        //}
        else
        {
          string[] buttons = new string[1]
          {
            TheGame.instance.stringTable.GetString(703)
          };
          //while (Guide.IsVisible)
          //  Thread.Sleep(32);
          //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), 
          //    TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0,
          //    MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
        }
      }
      catch
      {
        string[] buttons = new string[1]
        {
          TheGame.instance.stringTable.GetString(703)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), 
        //    TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0,
        //    MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
      }
    }

    public void ShowCardCallback()
    {
      SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      if (signedInGamer == null)
      {
        string[] buttons = new string[1]
        {
          TheGame.instance.stringTable.GetString(703)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
      }
      else
      {
        bool flag = true;//signedInGamer.Privileges.AllowProfileViewing != GamerPrivilegeSetting.Blocked && signedInGamer.Privileges.AllowUserCreatedContent != GamerPrivilegeSetting.Blocked;
        //if (Guide.IsVisible || !flag)
        //  return;
        //Guide.ShowGamerCard((PlayerIndex) 0, (Gamer) signedInGamer);
      }
    }

    public void AchievementCallback()
    {
      if (1 == 0)//(Gamer.SignedInGamers[(PlayerIndex) 0] == null || !TheGame.logInSucceeded)
      {
        string[] buttons = new string[1]
        {
          TheGame.instance.stringTable.GetString(703)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(845), TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
      }
      else
      {
        this.m_state = MainScreen.MS.MS_ACHIEVEMENTS;
        this.m_time = 1f;
        Game.game_work.tutorialControl.ResetTutePos();
        Game.ClearMenuItems();
      }
    }

    public void BlankCallback()
    {
    }

    public void GameModeCallback()
    {
      this.m_state = MainScreen.MS.MS_GAME_MODE;
      this.m_time = 1f;
      Game.game_work.tutorialControl.ResetTutePos();
      if (this.m_newGame == null)
        return;
      Mortar.Math.g_random.Seed((uint) ((double) int.MinValue * (double) this.m_newGame.m_rotation));
    }

    private static void MuteCallback(IAsyncResult result)
    {
      //int? nullable = Guide.EndShowMessageBox(result);
      //if (!nullable.HasValue)
      //  return;
      //Guide.EndShowMessageBox(result);
      //if (!nullable.HasValue || nullable.Value != 0)
      //  return;
      Game.game_work.musicEnabled = !Game.game_work.musicEnabled;
      try
      {
        MediaPlayer.IsMuted = !Game.game_work.musicEnabled;
      }
      catch
      {
      }
    }

    public void MusicCallback()
    {
      if (Game.game_work.musicEnabled && SoundManager.GetInstance().CustomMusic)
      {
        string[] buttons = new string[2]
        {
          TheGame.instance.stringTable.GetString(702),
          TheGame.instance.stringTable.GetString(707)
        };
        try
        {
          //if (Guide.IsVisible)
          //  Thread.Sleep(32);
          //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(890), TheGame.instance.stringTable.GetString(891), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(MainScreen.MuteCallback), (object) null);
        }
        catch
        {
        }
      }
      else
      {
        Game.game_work.musicEnabled = !Game.game_work.musicEnabled;
        try
        {
          MediaPlayer.IsMuted = !Game.game_work.musicEnabled;
        }
        catch
        {
        }
      }
    }

    public void SoundCallback()
    {
      Game.game_work.soundEnabled = !Game.game_work.soundEnabled;
      SoundManager.GetInstance().SetSFXVolume(Game.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
    }

    public void MultiplayerGameModeCallback()
    {
    }

    public void ClickedCallback() => throw new MissingMethodException();

    public override void Reset()
    {
    }

    public override void Release()
    {
      if (this.m_dojoButton != null)
        this.m_dojoButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(
            HUDControl.DefaultDeleteCallback);
      this.m_texture = null;
      this.m_backing = null;
      for (int index = 0; index < 2; ++index)
      {
        this.m_soundTextures[index] = null;
        this.m_musicTextures[index] = null;
      }
      this.s_newGameTex = null;
      this.s_aboutTex = null;
      this.s_moreGamesTex = null;
      this.s_leaderboardsTex = null;
    }

    public override void Init() => this.Reset();

    public void HelpAndOptionsCallback()
    {
      this.m_state = MainScreen.MS.MS_ABOUT;
      this.m_time = 1f;
      Game.game_work.tutorialControl.ResetTutePos();
      Game.ClearMenuItems();
    }

    private string YES => TheGame.instance.stringTable.GetString(707);

    private string NO => TheGame.instance.stringTable.GetString(702);

    private void ExitGameCallback(IAsyncResult result)
    {
      //int? nullable = Guide.EndShowMessageBox(result);
      //if (!nullable.HasValue || !nullable.HasValue || nullable.Value != 1)
      //  return;
      //if (Game.isWP7TrialMode())
      //{
      //  TheGame.instance.DoUpsell(true);
      //}
      //else
      {
        try
        {
          MediaPlayer.IsMuted = false;
        }
        catch
        {
        }
        TheGame.exitingGame = true;
        TheGame.instance.Exit();
      }
    }

    public override void Update(float dt)
    {
      float num1 = -Game.game_work.gameOverTransition;
      Vector3 scale = new Vector3(128f, 40f, 1f);
      if (this.m_achivementsButton == null)
      {
        this.m_achivementsButton = new MenuButton("menu_achieve.tex", new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.AchievementCallback), -1, scale, true);
        this.m_achivementsButton.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_achivementsButton);
        this.m_achivementsButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (this.m_live == null)
        this.m_live = TextureManager.GetInstance().Load("textureswp7/xboxliveonwindowsphone_4cko_s.tex").intex;
      if (this.m_gamerProfileButton == null)
      {
        SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
        if (signedInGamer != null && signedInGamer.IsSignedInToLive && TheGame.picture != null && TheGame.logInSucceeded)
        {
          Mortar.Texture texture = new Mortar.Texture();
          texture.intex = TheGame.picture;
          texture.w = (uint) TheGame.picture.Width;
          texture.h = (uint) TheGame.picture.Height;
          texture.hasAlpha = false;
          this.m_gamerProfileButton = new MenuButton(texture, new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.ShowCardCallback), -1, new Vector3((float) texture.w, (float) texture.h, 1f));
          this.m_gamerProfileButton.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_gamerProfileButton);
          this.m_gamerProfileButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
      }
      if (this.m_helpOptionsButton == null)
      {
        this.m_helpOptionsButton = new MenuButton("menu_help.tex", new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.HelpAndOptionsCallback), -1, scale, true);
        this.m_helpOptionsButton.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_helpOptionsButton);
        this.m_helpOptionsButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (this.m_leaderboardsButton == null)
      {
        this.m_leaderboardsButton = new MenuButton("menu_leader.tex", new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.LeaderboardsCallback), -1, scale, true);
        this.m_leaderboardsButton.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_leaderboardsButton);
        this.m_leaderboardsButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (Game.isWP7TrialMode())
      {
        if (this.m_purchaseButton == null)
        {
          this.m_purchaseButton = new MenuButton("menu_purchase.tex", new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.MarketplaceCallback), -1, scale, true);
          this.m_purchaseButton.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_purchaseButton);
          this.m_purchaseButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
      }
      else if (this.m_marketplaceButton == null)
      {
        this.m_marketplaceButton = new MenuButton("menu_market.tex", new Vector3(-1000f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.MarketplaceCallback), -1, scale, true);
        this.m_marketplaceButton.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_marketplaceButton);
        this.m_marketplaceButton.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (this.m_sound == null)
      {
        this.m_sound = new MenuButton("sound.tex", new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 24.0), MainScreen.TOP_BUTTONS_Y, 0.0f), new MenuButton.MenuCallback(this.SoundCallback), -1, new Vector3(32f, 32f, 1f), false);
        this.m_sound.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_sound);
        this.m_soundTextures[0] = this.m_sound.m_texture;
        this.m_soundTextures[1] = TextureManager.GetInstance().Load("textureswp7/sound_cross.tex");
        this.m_sound.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      if (this.m_music == null)
      {
        this.m_music = new MenuButton("music.tex", new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 24.0 - 40.0), MainScreen.TOP_BUTTONS_Y, 0.0f), new MenuButton.MenuCallback(this.MusicCallback), -1, new Vector3(32f, 32f, 1f), false);
        this.m_music.Init();
        Game.game_work.hud.AddControl((HUDControl) this.m_music);
        this.m_musicTextures[0] = this.m_music.m_texture;
        this.m_musicTextures[1] = TextureManager.GetInstance().Load("textureswp7/music_cross.tex");
        this.m_music.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
      this.m_music.m_texture = this.m_musicTextures[Game.game_work.musicEnabled ? 0 : 1];
      this.m_sound.m_texture = this.m_soundTextures[Game.game_work.soundEnabled ? 0 : 1];
      switch (this.m_state)
      {
        case MainScreen.MS.MS_IN:
          Game.game_work.gameMode = Game.GAME_MODE.GM_CLASSIC;
          this.m_moreGames = (MenuButton) null;
          if ((double) this.m_transitionWait > 0.0 || (double) Game.game_work.hitBombTime > (double) MainScreen.BOMB_FLASH_FULL - 0.10000000149011612)
          {
            this.m_transitionWait -= dt;
            Game.game_work.gameOverTransition += (float) ((-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
            if ((double) Game.game_work.gameOverTransition < 0.0)
              Game.game_work.gameOverTransition = 0.0f;
          }
          else
          {
            this.m_time += dt;
            if ((double) Game.game_work.gameOverTransition < -0.99900001287460327)
              Game.game_work.gameOverTransition = -1f;
            else
              Game.game_work.gameOverTransition += (float) (
                                (-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
          }
          if ((double) this.m_time > (double) MainScreen.BEGINNING_WAIT 
                        && (double) Game.game_work.gameOverTransition < 0.0)
          {
            this.m_state = MainScreen.MS.MS_WAIT;
            this.m_newGame = new MenuButton(this.s_newGameTex, MainScreen.NEW_GAME_POS, 
                new MenuButton.MenuCallback(this.GameModeCallback), 3);
            this.m_newGame.Init();
            Game.game_work.hud.AddControl((HUDControl) this.m_newGame);

            this.m_newGame.m_originalScale = Vector3.Multiply(
                Vector3.Multiply(
                    new Vector3((float) (this.m_newGame.m_texture.GetWidth() + 1U),
                    (float) (this.m_newGame.m_texture.GetHeight() + 1U), 1f), 
                    Game.GAME_MODE_SCALE_FIX), MainScreen.OVERALL_BUTTON_SCALE);
            Entity entity1 = this.m_newGame.m_entity;
            entity1.m_cur_scale = Vector3.Multiply(entity1.m_cur_scale, 
                MainScreen.OVERALL_BUTTON_SCALE);
            this.m_newGame.m_overallScratchScale = 0.5f;
            Game.game_work.tutorialControl.ResetTutePos(this.m_newGame);
            this.m_dojoButton = new MenuButton(this.s_aboutTex, MainScreen.DOJO_POS, 
                new MenuButton.MenuCallback(this.AboutCallback), Fruit.FruitType("mango"));

            this.m_dojoButton.Init();
            this.m_dojoButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
            this.m_dojoButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.ButtonDeleted);
            if (this.m_dojoButton.m_entity != null)
            {
              Entity entity2 = this.m_dojoButton.m_entity;
              entity2.m_cur_scale = Vector3.Multiply(entity2.m_cur_scale, 0.9f * MainScreen.OVERALL_BUTTON_SCALE);
            }
            MenuButton dojoButton = this.m_dojoButton;
            dojoButton.m_originalScale = Vector3.Multiply(dojoButton.m_originalScale, 1.05f
                * MainScreen.OVERALL_BUTTON_SCALE);

            this.m_dojoButton.m_clearOthers = true;
            Game.game_work.hud.AddControl((HUDControl) this.m_dojoButton);
            break;
          }
          break;
        case MainScreen.MS.MS_WAIT:
          if (/*!Guide.IsVisible &&*/ TheGame.instance.BackButtonWasPressed)
          {
            string[] buttons = new string[2]
            {
              this.NO,
              this.YES
            };
            //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(705), 
            //    TheGame.instance.stringTable.GetString(738), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.ExitGameCallback), (object) null);
            return;
          }
          if (this.m_dojoButton != null)
            this.m_dojoButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
          MenuButton moreGames = this.m_moreGames;
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          if ((double) Game.game_work.gameOverTransition < -0.99900001287460327)
          {
            Game.game_work.gameOverTransition = -1f;
            break;
          }
          Game.game_work.gameOverTransition += (float) ((-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
          break;
        case MainScreen.MS.MS_OUT:
          if ((double) num1 > 0.99900001287460327)
          {
            Game.game_work.levelStartCoins = Game.game_work.coins;
            WaveManager.GetInstance().Reset();
            Game.game_work.gameOver = true;
          }
          Game.game_work.gameOverTransition *= 0.75f;
          if ((double) Mortar.Math.Abs(Game.game_work.gameOverTransition) < 1.0 / 1000.0)
          {
            Game.game_work.gameOverTransition = 0.0f;
            Game.game_work.gameOver = false;
            this.m_state = MainScreen.MS.MS_GAME;
          }
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
        case MainScreen.MS.MS_ABOUT:
        case MainScreen.MS.MS_DOJO:
          int numEntities1 = (int) ActorManager.GetInstance().GetNumEntities(0);
          this.m_time *= 0.75f;
          if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
          {
            this.m_time = 0.0f;
            HUDControl control = this.m_state != MainScreen.MS.MS_ABOUT ? (HUDControl) new DojoScreen() : (HUDControl) new AboutScreen(this);
            control.Init();
            Game.game_work.hud.AddControl(control);
          }
          num1 = this.m_time;
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
        case MainScreen.MS.MS_RETURN:
          if ((double) this.m_time > 0.99900001287460327)
          {
            num1 = 1f;
            this.m_time += dt;
            if ((double) this.m_time > 1.5)
            {
              this.m_time = MainScreen.BEGINNING_WAIT;
              this.m_transitionWait = 0.0f;
              this.m_state = MainScreen.MS.MS_IN;
            }
          }
          else
          {
            this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
            num1 = this.m_time;
          }
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
        case MainScreen.MS.MS_OPENFEINT:
        case MainScreen.MS.MS_OPENFEINT_GAMES:
          int numEntities2 = (int) ActorManager.GetInstance().GetNumEntities(0);
          this.m_time *= 0.75f;
          if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
          {
            this.m_time = 0.0f;
            LeaderboardsScreen control = new LeaderboardsScreen();
            control.Init();
            Game.game_work.hud.AddControl((HUDControl) control);
          }
          num1 = this.m_time;
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
        case MainScreen.MS.MS_GAME_MODE:
        case MainScreen.MS.MS_GAME_MODE_MP:
          float time = this.m_time;
          this.m_time *= 0.85f;
          if ((double) time > 0.25 && (double) this.m_time <= 0.25)
          {
            this.m_time = 0.0f;
            GameModeScreen control = new GameModeScreen(false);
            control.Init();
            Game.game_work.hud.AddControl((HUDControl) control);
          }
          num1 = this.m_time;
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
        case MainScreen.MS.MS_ACHIEVEMENTS:
          int numEntities3 = (int) ActorManager.GetInstance().GetNumEntities(0);
          this.m_time *= 0.75f;
          if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
          {
            this.m_time = 0.0f;
            AchievementsScreen control = new AchievementsScreen();
            control.Init();
            Game.game_work.hud.AddControl((HUDControl) control);
          }
          num1 = this.m_time;
          this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
          break;
      }
      if (this.m_state != MainScreen.MS.MS_WAIT && this.m_state != MainScreen.MS.MS_IN && this.m_newGame != null && (double) this.m_newGame.m_scale.X < 45.0 * (double) MainScreen.OVERALL_BUTTON_SCALE)
        this.m_newGame = (MenuButton) null;
      if (this.m_state != MainScreen.MS.MS_WAIT || this.m_moreGames != null && this.m_moreGames.m_entity == null && (double) this.m_moreGames.m_scale.X < 45.0)
        this.m_moreGames = (MenuButton) null;
      dt = Mortar.Math.MIN(dt, 0.04f);
      this.m_fruitPos.X = (float) (-(double) Game.SCREEN_WIDTH / 4.0);
      this.m_fruitPos.Y = this.m_pos.Y + 18f;
      this.m_fruitPos.Z = this.m_ninjaPos.Z = 0.0f;
      this.m_ninjaPos.X = this.m_fruitPos.X + 180f;
      this.m_ninjaGrav -= dt * 55f;
      this.m_ninjaPos.Y += (float) ((double) this.m_ninjaGrav * (double) dt * 15.0);
      this.m_tutePos = this.m_fruitPos;
      if ((double) dt > 0.0)
        MainScreen.tute = 1f;
      if ((double) this.m_ninjaPos.Y < (double) this.m_fruitPos.Y - 15.0)
      {
        this.m_ninjaPos.Y = this.m_fruitPos.Y - 15f;
        this.m_ninjaGrav *= -0.25f;
        if ((double) Mortar.Math.Abs(this.m_ninjaGrav) < 5.0 && (double) num1 > 0.99000000953674316 && (double) dt > 0.0)
          MainScreen.tute = 0.0f;
      }
      this.m_tuteTime += (float) (((double) MainScreen.tute - (double) this.m_tuteTime) * 0.25);
      this.m_tutePos = Vector3.Add(MainScreen.TUTE_POS, 
          Vector3.Multiply(Vector3.Multiply(MainScreen.TUTE_DIRECTION, this.m_tuteTime), 2f));

      if (this.m_sound != null && this.m_music != null)
      {
        this.m_sound.m_pos.X = -180f;
        this.m_music.m_pos.X = -140f;
        this.m_sound.SetActive(true);
        this.m_music.SetActive(true);
        this.m_sound.m_pos.Y = AboutScreen.AboutScreenTime;
        this.m_music.m_pos.Y = AboutScreen.AboutScreenTime;
      }
      float num2 = (float) (340.0 + (double) num1 * -170.0);
      float num3 = 30f;
      float num4 = -50f;
      if ((double) GameTask.GetPauseAmount() != 0.0)
        return;
      if (this.m_gamerProfileButton != null)
      {
        SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
        if (signedInGamer != null && signedInGamer.IsSignedInToLive && TheGame.picture != null && TheGame.logInSucceeded)
        {
          this.m_gamerProfileButton.m_pos.Y = 116f;
          this.m_gamerProfileButton.m_pos.X = num2;
        }
      }
      if (this.m_live != null)
      {
        MainScreen.liveY = 12f;
        MainScreen.liveX = (float) (130.0 + (double) num2 * 2.0);
      }
      if (this.m_achivementsButton != null)
      {
        if (Game.isWP7TrialMode())
        {
          this.m_achivementsButton.m_pos.Y = -12333f;
          this.m_achivementsButton.m_pos.X = -12333f;
        }
        else
        {
          this.m_achivementsButton.m_pos.Y = num3;
          this.m_achivementsButton.m_pos.X = num2;
          num3 += num4;
        }
      }
      if (this.m_leaderboardsButton != null)
      {
        if (Game.isWP7TrialMode())
        {
          this.m_leaderboardsButton.m_pos.Y = -12333f;
          this.m_leaderboardsButton.m_pos.X = -12333f;
        }
        else
        {
          this.m_leaderboardsButton.m_pos.Y = num3;
          this.m_leaderboardsButton.m_pos.X = num2;
          num3 += num4;
        }
      }
      if (this.m_helpOptionsButton != null)
      {
        this.m_helpOptionsButton.m_pos.Y = num3;
        this.m_helpOptionsButton.m_pos.X = num2;
        num3 += num4;
      }
      if (!Game.isWP7TrialMode() && this.m_marketplaceButton != null)
      {
        this.m_marketplaceButton.m_pos.Y = num3;
        this.m_marketplaceButton.m_pos.X = num2;
        num3 += num4;
      }
      if (this.m_purchaseButton == null)
        return;
      if (!Game.isWP7TrialMode())
      {
        this.m_purchaseButton.m_pos.X = -12333f;
      }
      else
      {
        this.m_purchaseButton.m_pos.Y = num3;
        this.m_purchaseButton.m_pos.X = num2;
      }
      float num5 = num3 + num4;
    }

    private void ReadFinished(int result)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      if (this.m_state == MainScreen.MS.MS_GAME || (this.m_state == MainScreen.MS.MS_ABOUT || this.m_state == MainScreen.MS.MS_DOJO) && (double) this.m_time == 0.0)
        return;
      if (!MainScreen.madeTris)
      {
        MainScreen.madeTris = true;
        Color color = new Color(0, 0, 0, 128);
        for (int index = 0; index < 3; ++index)
        {
          MainScreen.top_tri[index].X = MainScreen.top_tri[index].Y = MainScreen.top_tri[index].Z = MainScreen.top_tri[index].nx = MainScreen.top_tri[index].ny = 0.0f;
          MainScreen.top_tri[index].nz = 1f;
          MainScreen.top_tri[index].color = color;
          MainScreen.top_tri[index].u = 0.5f;
          MainScreen.top_tri[index].v = 0.5f;
        }
        MainScreen.top_tri[0].X = -0.5f;
        MainScreen.top_tri[0].Y = -0.5f;
        MainScreen.top_tri[1].X = 3.5f;
        MainScreen.top_tri[1].Y = 1f;
        MainScreen.top_tri[2].X = -0.5f;
        MainScreen.top_tri[2].Y = 1f;
      }
      if (this.m_fruitTex != null)
      {
        this.m_fruitTex.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(this.m_scale);
        MatrixManager.GetInstance().Translate(this.m_pos);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawTriList(MainScreen.top_tri, 3, true);
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(Vector3.Multiply(Vector3.Multiply(
            new Vector3((float) this.m_fruitTex.GetWidth(),
            (float) this.m_fruitTex.GetHeight(), 0.0f), Game.GAME_MODE_SCALE_FIX), 0.85f));

        MatrixManager.GetInstance().Translate(this.m_fruitPos);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_fruitTex.UnSet();
      }
      if (this.m_ninjaTex != null)
      {
        this.m_ninjaTex.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(Vector3.Multiply(
            new Vector3((float) this.m_ninjaTex.GetWidth(), 
            (float) this.m_ninjaTex.GetHeight(), 0.0f), Game.GAME_MODE_SCALE_FIX));
        MatrixManager.GetInstance().Translate(this.m_ninjaPos);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_ninjaTex.UnSet();
      }
      if (this.m_tuteTex != null)
      {
        this.m_tuteTex.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(Vector3.Multiply(
            new Vector3((float) this.m_tuteTex.GetWidth(),
            (float) this.m_tuteTex.GetHeight(), 0.0f), Game.GAME_MODE_SCALE_FIX));
        MatrixManager.GetInstance().Translate(this.m_tutePos);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_tuteTex.UnSet();
      }
      if (this.m_leaderBoardComingSoonTexture != null && this.m_newGame != null)
      {
        this.m_leaderBoardComingSoonTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(Vector3.Multiply(
            new Vector3(0.5f, (float) (0.5 * 
            ((double) this.m_leaderBoardComingSoonTexture.GetHeight()
            / (double) this.m_leaderBoardComingSoonTexture.GetWidth())), 1f),
            this.m_newGame.m_scale.X));

        MatrixManager.GetInstance().Translate(MainScreen.LEADERBOARD_BUTTON_POS);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_leaderBoardComingSoonTexture.UnSet();
      }
      TheGame.instance.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
      TheGame.instance.spriteBatch.Draw(this.m_live, new Vector2(MainScreen.liveX, MainScreen.liveY), Color.White);
      TheGame.instance.spriteBatch.End();
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public void Hide()
    {
      this.m_pos = new Vector3(-1000f, 1000f, -1000f);
      this.m_state = MainScreen.MS.MS_GAME;
    }

    public void ButtonDeleted(HUDControl control)
    {
      if (control != this.m_dojoButton)
        return;
      this.m_dojoButton = (MenuButton) null;
    }

    public enum MS
    {
      MS_IN,
      MS_WAIT,
      MS_OUT,
      MS_ABOUT,
      MS_DOJO,
      MS_RETURN,
      MS_OPENFEINT,
      MS_OPENFEINT_GAMES,
      MS_GAME_MODE,
      MS_GAME_MODE_MP,
      MS_GAME,
      MS_ACHIEVEMENTS,
    }
  }
}
