
// Type: FruitNinja2.Game
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Mortar;
using System;
using System.Collections.Generic;
using System.Threading;


#nullable disable
namespace FruitNinja2
{
  public class Game
  {
    public const int HEAP_SIZE_MESH = 158720;
    public const int HEAP_SIZE_ANIMATION = 512000;
    public const int HEAP_SIZE_TEXTURE = 51200;
    public const int SAVE_SIZE = 160;
    public const int DEFAULT_MUSIC_VOLUME = 60;
    public const int DEFAULT_SFX_VOLUME = 80;
    public static int MAX_SYSTEM_ACHIEVEMENTS = 30;
    private static bool trialModeState = false;
    private static int version_major = 1;
    private static int version_minor = 0;
    private static int version_patch = 0;
    public static float SCREEN_WIDTH = 480f;
    public static float SCREEN_HEIGHT = 320f;
    public static float SCREEN_SIZE_X = 800f;
    public static float SCREEN_SIZE_Y = 480f;
    public static float HUD_SCALE = 1f;
    public static float GAME_MODE_SCALE_FIX = 1f;
    public static float GRAVITY = 0.2f;
    private static Game.GameInitFunction[] task_init = new Game.GameInitFunction[3]
    {
      new Game.GameInitFunction(Game.SplashInit),
      new Game.GameInitFunction(Game.FrontendInit),
      new Game.GameInitFunction(GameTask.GameInit)
    };
    private static Game.GameFunction[] task_main = new Game.GameFunction[3]
    {
      new Game.GameFunction(Game.SplashUpdate),
      new Game.GameFunction(Game.FrontendUpdate),
      new Game.GameFunction(GameTask.GameUpdate)
    };
    private static Game.GameFunctionDraw[] task_draw = new Game.GameFunctionDraw[3]
    {
      new Game.GameFunctionDraw(Game.SplashDraw),
      new Game.GameFunctionDraw(Game.FrontendDraw),
      new Game.GameFunctionDraw(GameTask.GameDraw)
    };
    private static Game.GameExitFunction[] task_exit = new Game.GameExitFunction[3]
    {
      new Game.GameExitFunction(Game.SplashExit),
      new Game.GameExitFunction(Game.FrontendExit),
      new Game.GameExitFunction(GameTask.GameExit)
    };
    private static Game.Task old_routine = Game.Task.TASK_SPLASHSCREEN;
    private static bool task_initialised = false;
    private static bool updated = false;
    private static float drawDt = 0.0f;
    public static bool trialModeEnded = false;
    private static bool FIRST = true;
    private static Game.ScoreDelegate s_scoreDelagate = new Game.ScoreDelegate(Game.DefaultScoreDelegate);
    private static float GetFruitZPositionz = -500f;
    private static float GetBombZPositionz = -10f;
    public static Game.GameWork game_work = new Game.GameWork();
    private static Color DefaultBackgroundColor = Color.Black;

    public static Color TintColour(Color col, float[] tints)
    {
      col.R = (byte) Mortar.Math.CLAMP((float) col.R * tints[0], 0.0f, (float) byte.MaxValue);
      col.G = (byte) Mortar.Math.CLAMP((float) col.G * tints[1], 0.0f, (float) byte.MaxValue);
      col.B = (byte) Mortar.Math.CLAMP((float) col.B * tints[2], 0.0f, (float) byte.MaxValue);
      return col;
    }

    public static Color TintWhite(float[] tints)
    {
      return new Color((int) (byte) Mortar.Math.CLAMP((float) byte.MaxValue * tints[0], 0.0f, (float) byte.MaxValue), (int) (byte) Mortar.Math.CLAMP((float) byte.MaxValue * tints[1], 0.0f, (float) byte.MaxValue), (int) (byte) Mortar.Math.CLAMP((float) byte.MaxValue * tints[2], 0.0f, (float) byte.MaxValue), (int) byte.MaxValue);
    }

    public static void SetTrialModeState()
    {
      try
      {
        Game.trialModeState = false;//Guide.IsTrialMode;
      }
      catch
      {
        Game.trialModeState = true;
      }
    }

    public static bool isWP7TrialMode() => Game.trialModeState;

    public static void ShowBuyMessageBox()
    {
      try
      {
        string str1 = TheGame.instance.stringTable.GetString(707);
        string str2 = TheGame.instance.stringTable.GetString(702);
        string title = TheGame.instance.stringTable.GetString(846);
        string text = TheGame.instance.stringTable.GetString(847);
        string[] buttons = new string[2]{ str2, str1 };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (Guide.IsVisible)
        //  return;
        //Guide.BeginShowMessageBox(title, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(Game.ExitGameCallback), (object) null);
      }
      catch
      {
      }
    }

    private static void ExitGameCallback(IAsyncResult result)
    {
      int? nullable = default;//Guide.EndShowMessageBox(result);
      if (!nullable.HasValue || !nullable.HasValue || nullable.Value != 1)
        return;
      SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[PlayerIndex.One];
           
      //Guide.EndShowMessageBox(result);
      if (signedInGamer == null)
        return;
      //while (Guide.IsVisible)
      //  Thread.Sleep(32);
      //if (Guide.IsVisible || !signedInGamer.Privileges.AllowPurchaseContent)
      //  return;
      /*
      if (Game.isWP7TrialMode())
        Guide.ShowMarketplace((PlayerIndex) 0);
      else
        new MarketplaceDetailTask()
        {
          ContentType = ((MarketplaceContentType) 1)
        }.Show();
      */
    }

    public static float PLATFORM_IPHONEOS_RES_SCALE => 1f;

    public static float PSP_SCREEN_WIDTH => 480f;

    public static float PSP_SCREEN_HEIGHT => 272f;

    public static float PC_SCREEN_WIDTH => Game.SCREEN_WIDTH;

    public static float PC_SCREEN_HEIGHT => Game.SCREEN_HEIGHT;

    public static bool USE_ARCADE_GO_SCREEN => false;

    public static bool USE_ZEN_GO_SCREEN
    {
      get
      {
        return Game.game_work.gameMode == Game.GAME_MODE.GM_ZEN || Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE;
      }
    }

    public static float SCREEN_SCALE_X => Game.SCREEN_WIDTH / Game.SCREEN_SIZE_X;

    public static float SCREEN_SCALE_Y => Game.SCREEN_HEIGHT / Game.SCREEN_SIZE_Y;

    public static float PIXEL_SCREEN_X(float x) => Game.SCREEN_SCALE_X * x;

    public static float PIXEL_SCREEN_Y(float y) => Game.SCREEN_SCALE_Y * y;

    public static float SPLIT_SCREEN_SCALE => Game.SCREEN_WIDTH * 0.5f / Game.SCREEN_HEIGHT;

    public static float SPLIT_SCREEN_SCALE_INV => Game.SCREEN_HEIGHT / (Game.SCREEN_WIDTH * 0.5f);

    public static float SPLIT_SCREEN_WIDTH => Game.SCREEN_HEIGHT / Game.SPLIT_SCREEN_SCALE;

    public static float SPLIT_SCREEN_HIEGHT => Game.SCREEN_HEIGHT;

    public static float SCREEN_TO_PLATFORMX(float x)
    {
      return x * (float) DisplayManager.GetInstance().GetWindowSize().right;
    }

    public static float SCREEN_TO_PLATFORMY(float y)
    {
      return y * (float) DisplayManager.GetInstance().GetWindowSize().bottom;
    }

    public static float PSP_PIXEL_SCREENX(float x) => x / Game.PSP_SCREEN_WIDTH;

    public static float PSP_PIXEL_SCREENY(float x) => x / Game.PSP_SCREEN_HEIGHT;

    public static float PC_PIXEL_SCREENX(float x) => x / Game.PC_SCREEN_WIDTH;

    public static float PC_PIXEL_SCREENY(float x) => x / Game.PC_SCREEN_HEIGHT;

    public static float PIXEL_SCREENX(float x) => x / Game.SCREEN_WIDTH;

    public static float PIXEL_SCREENY(float y) => y / Game.SCREEN_HEIGHT;

    public static float PIXEL_PLATFORMX(float x) => Game.SCREEN_TO_PLATFORMX(Game.PIXEL_SCREENX(x));

    public static float PIXEL_PLATFORMY(float y) => Game.SCREEN_TO_PLATFORMY(Game.PIXEL_SCREENX(y));

    public static float PSP_PIXEL_PLATFORMX(float x)
    {
      return Game.SCREEN_TO_PLATFORMX(Game.PSP_PIXEL_SCREENX(x));
    }

    public static float PSP_PIXEL_PLATFORMY(float y)
    {
      return Game.SCREEN_TO_PLATFORMY(Game.PSP_PIXEL_SCREENY(y));
    }

    public static float PC_PIXEL_PLATFORMX(float x)
    {
      return Game.SCREEN_TO_PLATFORMX(Game.PC_PIXEL_SCREENX(x));
    }

    public static float PC_PIXEL_PLATFORMY(float y)
    {
      return Game.SCREEN_TO_PLATFORMY(Game.PC_PIXEL_SCREENY(y));
    }

    public static float SCREEN_LEFT => Game.SCREEN_TO_PLATFORMX(-0.5f);

    public static float SCREEN_RIGHT => Game.SCREEN_TO_PLATFORMX(0.5f);

    public static float SCREEN_TOP => Game.SCREEN_TO_PLATFORMY(0.5f);

    public static float SCREEN_BOTTOM => Game.SCREEN_TO_PLATFORMY(-0.5f);

    public static float SCREEN_TOP_LEFTX => Game.SCREEN_TO_PLATFORMX(-0.5f);

    public static float SCREEN_TOP_LEFTY => Game.SCREEN_TO_PLATFORMY(0.5f);

    public static float SCREEN_TOP_RIGHTX => Game.SCREEN_TO_PLATFORMX(0.5f);

    public static float SCREEN_TOP_RIGHTY => Game.SCREEN_TO_PLATFORMY(0.5f);

    public static float SCREEN_BOTTOM_LEFTX => Game.SCREEN_TO_PLATFORMX(0.0f);

    public static float SCREEN_BOTTOM_LEFTY => Game.SCREEN_TO_PLATFORMY(-0.5f);

    public static float SCREEN_BOTTOM_RIGHTX => Game.SCREEN_TO_PLATFORMX(0.5f);

    public static float SCREEN_BOTTOM_RIGHTY => Game.SCREEN_TO_PLATFORMY(-0.5f);

    public static float SCREEN_CENTREX => Game.SCREEN_TO_PLATFORMX(0.0f);

    public static float SCREEN_CENTREY => Game.SCREEN_TO_PLATFORMY(0.0f);

    public static float GAME_FONT_SIZE => 16f;

    public static float GAME_FONT_PIXEL_SIZE => 14f;

    public static float PETITA_BIG_SIZE => 32f;

    public static float PETITA_SMALL_SIZE => 22f;

    public static float DEFAULT_HUD_SPACING => 8f;

    public static int MAX_PLAYERS => 1;

    public static void OnActivate()
    {
      if ((double) Game.game_work.gameOverTransition != 0.0)
        return;
      if (Game.game_work.hud != null)
        Game.game_work.hud.OnPause();
      GameTask.SkipToPause(false);
    }

    public static void ParticleGameInit(uint flags)
    {
    }

    public static void ParticleGameUpdate(float dt, bool update)
    {
    }

    public static void ParticleGameDraw(float dt, bool draw)
    {
    }

    public static void ParticleGameExit()
    {
    }

    public static void SplashInit(uint flags)
    {
    }

    public static void SplashUpdate(float dt, bool update)
    {
    }

    public static void SplashDraw(float dt, bool draw)
    {
    }

    public static void SplashExit()
    {
    }

    public static void FrontendInit(uint flags)
    {
    }

    public static void FrontendUpdate(float dt, bool update)
    {
    }

    public static void FrontendDraw(float dt, bool draw)
    {
    }

    public static void FrontendExit()
    {
    }

    public static byte MAX_FRUIT_MISSES => 3;

    public static int HEAP_SIZE_SOUND => 524288;

    public static void SetLanguage()
    {
    }

    public static int GetCurrentModeHighscore()
    {
      return Game.game_work.gameMode >= Game.GAME_MODE.GM_CLASSIC && Game.game_work.gameMode < Game.GAME_MODE.GM_MAX && Game.game_work.saveData != null ? Game.game_work.saveData.highScores[(int) Game.game_work.gameMode] : 0;
    }

    public static int GetCurrentModeHighscore(int mode)
    {
      return mode >= 0 && mode < 4 && Game.game_work.saveData != null ? Game.game_work.saveData.highScores[mode] : 0;
    }

    public static bool SetCurrentModeHighscore(int score)
    {
      if (Game.game_work.gameMode < Game.GAME_MODE.GM_CLASSIC || Game.game_work.gameMode >= Game.GAME_MODE.GM_MAX || Game.game_work.saveData == null || score <= Game.game_work.saveData.highScores[(int) Game.game_work.gameMode])
        return false;
      Game.game_work.saveData.highScores[(int) Game.game_work.gameMode] = score;
      int mode = -1;
      switch (Game.game_work.gameMode)
      {
        case Game.GAME_MODE.GM_CLASSIC:
          mode = 0;
          LeaderboardsScreen.SetStartLeaderboard(0);
          break;
        case Game.GAME_MODE.GM_ARCADE:
          LeaderboardsScreen.SetStartLeaderboard(2);
          mode = 2;
          break;
        case Game.GAME_MODE.GM_ZEN:
          LeaderboardsScreen.SetStartLeaderboard(1);
          mode = 1;
          break;
      }
      if (mode > -1)
        Leaderboards.Write(mode, (long) score);
      return true;
    }

    public static bool CombosEnabled() => false;

    public static void AddToCurrentScore(int score) => Game.AddToCurrentScore(score, 0);

    public static void AddToCurrentScore(int score, int player)
    {
      int currentScore = Game.game_work.currentScore;
      if (Game.isWP7TrialMode() && currentScore + score >= 150)
      {
        Game.trialModeEnded = true;
        Game.GameOver();
        score = 150 - currentScore;
        TheGame.TriggerShowBuyMessageBox = true;
      }
      int num = Game.game_work.currentScore += Game.s_scoreDelagate(score * Game.GetScoreMultiplyer());
      if (num < 0)
        Game.game_work.currentScore = num = 0;
      if (currentScore / Fruit.NEW_LIFE_AT < num / Fruit.NEW_LIFE_AT && Game.game_work.currentMissCount > (byte) 0)
      {
        --Game.game_work.currentMissCount;
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_EXTRA_LIFE);
      }
      uint hash = StringFunctions.StringHash("all");
      if (score <= 0)
        return;
      Game.game_work.totalScore = (uint) Game.game_work.saveData.AddToTotal("all", hash, score, true, false);
    }

    public static void AddCoins(int coins)
    {
    }

    public static float HIT_BOMB_WAIT => 3.2f;

    public static void HitBomb(Vector3 pos) => GameTask.HitBomb(pos);

    public static void HitMenuBomb(Vector3 pos) => GameTask.HitMenuBomb(pos);

    public static bool BombFlashFull()
    {
      return (double) Game.game_work.hitBombTime < (double) GameTask.BOMB_FLASH_FULL && (double) Game.game_work.hitBombTime < (double) GameTask.BOMB_FLASH_START_FADE;
    }

    public static void GameOver() => Game.GameOver(-1, -1f, -1);

    public static void GameOver(int state, float time, int player)
    {
      if (Game.game_work.gameOver)
        return;
      Game.game_work.gameOver = true;
      WaveManager.GetInstance().ClearUnspawned();
      Game.game_work.gameOverScreen = new GameOverScreen((string) null, state, time, Game.game_work.saveData.go_head, Game.game_work.saveData.go_body, Game.game_work.saveData.go_fruit, Game.game_work.saveData.go_fact);
      Game.game_work.saveData.go_head = Game.game_work.saveData.go_body = Game.game_work.saveData.go_fruit = Game.game_work.saveData.go_fact = -1;
      Game.game_work.gameOverScreen.Init();
      Game.game_work.hud.AddControl((HUDControl) Game.game_work.gameOverScreen);
    }

    public static void QuitToMenu()
    {
      Game.game_work.gameOver = true;
      GameTask.s_mainScreen.m_state = MainScreen.MS.MS_IN;
      GameTask.s_mainScreen.m_transitionWait = 0.5f;
      Game.game_work.currentScore = 0;
    }

    public static void ClearMenuItems()
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
      {
        if ((entity.partOfPopup || !PopOverControl.IsInPopup) && !((Fruit) entity).m_isSliced)
        {
          ((Fruit) entity).m_isSliced = true;
          entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(10f) - 5f, Mortar.Math.g_random.RandF(5f), 0.0f);
          entity.m_vel.X = System.Math.Abs(entity.m_vel.X) * (float) Mortar.Math.MATH_SIGN(entity.m_pos.X);
          ((Fruit) entity).m_vel2 = entity.m_vel;
        }
      }
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
      {
        if (entity.partOfPopup || !PopOverControl.IsInPopup)
        {
          if (((Bomb) entity).Enabled())
          {
            ((Bomb) entity).Disable();
            entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(10f) - 5f, Mortar.Math.g_random.RandF(5f), 0.0f);
          }
          ((Bomb) entity).EnableGravity(true);
        }
      }
    }

    public static void SetScoreDelegate(Game.ScoreDelegate del) => Game.s_scoreDelagate = del;

    public static int DefaultScoreDelegate(int score)
    {
      if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
      {
        if (score > 0)
          score *= PowerUpManager.GetInstance().GetScoreGainMultiplier();
        else
          score *= PowerUpManager.GetInstance().GetScoreLossMultiplier();
      }
      return score;
    }

    public static void GameTaskUpdate(float dt)
    {
      Game.drawDt += dt;
      Game.game_work.dt = dt;
      Game.game_work.gameSeedValue += (uint) ((double) dt * 1000.0);
      if (Game.task_initialised)
      {
        if (Game.game_work.routine_0 != Game.old_routine)
        {
          Game.Task routine0 = Game.game_work.routine_0;
          Game.game_work.routine_0 = routine0;
          Game.old_routine = Game.game_work.routine_0;
        }
        else
        {
          bool update = !Game.game_work.pause;
          Game.updated = true;
          Game.old_routine = Game.game_work.routine_0;
          if ((double) Game.game_work.hitBombTime <= 0.0)
            Game.game_work.saveData.Update(dt, Game.game_work.hud);
          Game.task_main[(int) Game.game_work.routine_0](dt, update);
        }
      }
      else
      {
        Game.old_routine = Game.game_work.routine_0;
        Game.task_init[(int) Game.game_work.routine_0](0U);
        Game.task_initialised = true;
        if (!Game.FIRST)
          return;
        Game.FIRST = false;
        Game.GameTaskUpdate(dt);
      }
    }

    public static void GameTaskDraw(float dt)
    {
      Game.game_work.dt = Game.drawDt;
      if (Game.game_work.routine_0 == Game.old_routine && Game.updated)
        Game.task_draw[(int) Game.game_work.routine_0](Game.drawDt, true);
      Game.drawDt = 0.0f;
    }

    public static void GameTaskExit()
    {
    }

    public static bool PointerMoveCallback(InputEvent e) => false;

    public static bool PointerDownCallback(InputEvent e) => false;

    public static bool PointerUpCallback(InputEvent e) => false;

    public static int TouchInRegion(float xMin, float xMax, float yMin, float yMax)
    {
      return Game.TouchInRegion(xMin, xMax, yMin, yMax, -1);
    }

    public static int TouchInRegion(float xMin, float xMax, float yMin, float yMax, int touch)
    {
      if (touch >= 0 && touch < GameTask.MAX_SLASHES && (double) Game.game_work.touchPositions[touch].Z > 0.0 && (double) Game.game_work.touchPositions[touch].X >= (double) xMin && (double) Game.game_work.touchPositions[touch].X <= (double) xMax && (double) Game.game_work.touchPositions[touch].Y >= (double) yMin && (double) Game.game_work.touchPositions[touch].Y <= (double) yMax)
        return touch;
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
      {
        if ((double) Game.game_work.touchPositions[index].Z > 0.0 && (double) Game.game_work.touchPositions[index].X >= (double) xMin && (double) Game.game_work.touchPositions[index].X <= (double) xMax && (double) Game.game_work.touchPositions[index].Y >= (double) yMin && (double) Game.game_work.touchPositions[index].Y <= (double) yMax)
          return index;
      }
      return -1;
    }

    public static int IsTouchDown(int touch)
    {
      if (touch >= 0 && touch < GameTask.MAX_SLASHES)
      {
        if ((double) Game.game_work.touchPositions[touch].Z == 1.0)
          return 1;
        if ((double) Game.game_work.touchPositions[touch].Z == 2.0)
          return 2;
      }
      return 0;
    }

    public static string GetFormattedVersionString() => (string) null;

    public static string GetVersionString()
    {
      return Game.version_major.ToString() + "." + (object) Game.version_minor + "." + (object) Game.version_patch;
    }

    public static int GetVersionTotal()
    {
      return Game.version_major * 10000 + Game.version_minor * 100 + Game.version_patch;
    }

    public static int GetVersionMajor() => Game.version_major;

    public static int GetVersionMinor() => Game.version_minor;

    public static int GetVersionPatch() => Game.version_patch;

    public static bool IsFastHardware() => true;

    public static void MoveFruitZPositionToBack(ref float z)
    {
      z += 500f;
      z /= 2f;
      z -= 2600f;
    }

    public static float GetFruitZPosition()
    {
      Game.GetFruitZPositionz -= 100f;
      if ((double) Game.GetFruitZPositionz < -2499.0)
        Game.GetFruitZPositionz = -500f;
      return Game.GetFruitZPositionz;
    }

    public static float GetBombZPosition()
    {
      Game.GetBombZPositionz -= 50f;
      if ((double) Game.GetBombZPositionz < -400.0)
        Game.GetBombZPositionz = -10f;
      return Game.GetBombZPositionz;
    }

    public static bool InViewer() => false;

    public static void GameDestroy()
    {
    }

    private static void InitialiseData()
    {
      Game.SetLanguage();
      StringTableUtils.StringTableUtilInit();
      StringTableUtils.StringTableUtilLoadStrings();
      Game.game_work.loadedSaveState = false;
      Game.game_work.gameMode = Game.GAME_MODE.GM_CLASSIC;
      Game.game_work.saveData = new FruitSaveData();
      Save.LoadGame(Game.game_work.saveData);
      Game.game_work.gameMode = (Game.GAME_MODE) Game.game_work.saveData.mode;
      Game.game_work.saveData.AddToTotal("sessions", StringFunctions.StringHash("sessions"), 1, true, true);
      Game.game_work.routine_0 = Game.Task.TASK_GAME_UPDATE;
      Game.game_work.bombSize = 50f;
      Game.game_work.hitBombTime = 0.0f;
      Game.game_work.critHitTime = 0.0f;
      Game.game_work.criticalChance = Game.game_work.saveData.criticalProgression;
      Game.game_work.hasDroppedFruit = false;
      Game.game_work.inRetrySequence = false;
      Game.game_work.gameOverScreen = (GameOverScreen) null;
      Game.game_work.soundEnabled = Game.game_work.saveData.GetTotal(StringFunctions.StringHash("soundOff")) == 0;
      SoundManager.GetInstance().SetSFXVolume(Game.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
      Game.game_work.musicEnabled = Game.game_work.saveData.GetTotal(StringFunctions.StringHash("musicOff")) == 0;
      Game.game_work.saveData.AddToTotal("soundOff", StringFunctions.StringHash("soundOff"), -Game.game_work.saveData.GetTotal(StringFunctions.StringHash("soundOff")), false, true);
      Game.game_work.saveData.AddToTotal("musicOff", StringFunctions.StringHash("musicOff"), -Game.game_work.saveData.GetTotal(StringFunctions.StringHash("musicOff")), false, true);
      SlashEntity.InitModColors();
      AchievementManager.GetInstance().LoadAchievementInfo();
      Game.game_work.coins = 0;
      Game.game_work.coinsTotal = 0;
      Game.game_work.levelStartCoins = 0;
      ItemManager.GetInstance().LoadItemData();
      BonusManager.GetInstance().Init();
    }

    public static void GameInitialise(uint instance)
    {
      SystemManager.GetInstance().Init();
      MatrixManager.GetInstance().Init();
      DisplayManager.GetInstance().SetWindowSize(0, (int) Game.SCREEN_SIZE_Y, 0, (int) Game.SCREEN_SIZE_X);
      DisplayManager.GetInstance().Init("Rocket Racing");
      DisplayManager.GetInstance().SetClearColor(Game.DefaultBackgroundColor);
      DisplayManager.GetInstance().SetLightDirection(new Vector3(0.0f, -10f, -5f));
      TextureManager.GetInstance().Initialise();
      TextureManager.GetInstance().Initialise(51200);
      MeshManager.GetInstance().Initialise(158720);
      AnimationManager.GetInstance().Initialise(512000);
      InputManager.GetInstance().Init();
      PSPParticleManager.GetInstance().LoadFile("particles", "particles/particles_fast.xml");
      PowerUpManager.GetInstance().Load();
      Game.InitialiseData();
      Game.game_work.camera = new FruitCamera();
      Game.game_work.pointerReleased = false;
      Game.game_work.pointerPressed = false;
      Game.game_work.pointerDown = false;
      Game.game_work.mainPointer = Vector3.Zero;
      Game.game_work.timeControl = (TimeControl) null;
      Game.game_work.camera.Init(1f, 10000f, 16.95f, 11.3f);
      if (Game.game_work.pGameFont == null)
      {
        Game.game_work.pGameFont = new Font();
        Game.game_work.pGameFont.Load("fonts/font_fruit_ninja.fnt");
      }
      if (Game.game_work.pNumberFont == null)
      {
        Game.game_work.pNumberFont = new Font();
        Game.game_work.pNumberFont.Load("fonts/fruit_ninja_numbers.fnt");
      }
      if (Game.game_work.pNumberFontLeaderboard == null)
      {
        Game.game_work.pNumberFontLeaderboard = new Font();
        Game.game_work.pNumberFontLeaderboard.Load("fonts/arcade_results_numbers.fnt");
      }
      if (Game.game_work.pNumberFontBlue2 == null)
      {
        Game.game_work.pNumberFontBlue2 = new Font();
        Game.game_work.pNumberFontBlue2.Load("fonts/fruit_ninja_numbers_blue2.fnt");
      }
      if (Game.game_work.pNumberFontGreen == null)
      {
        Game.game_work.pNumberFontGreen = new Font();
        Game.game_work.pNumberFontGreen.Load("fonts/fruit_ninja_numbers_green.fnt");
      }
      Game.game_work.backTexture = TextureManager.GetInstance().Load("back_icon.tex", true);
      MenuButton.LoadContent();
      Fruit.LoadInfo();
      SplatEntity.LoadContent();
      SlashEntity.LoadContent();
      Bomb.LoadContent();
      GameOverScreen.LoadContent();
      DojoScreen.LoadContent();
      FruitFactControl.LoadContent();
      AboutScreen.LoadContent();
      GameModeScreen.LoadContent();
      Coin.LoadContent();
      CreditsScreen.LoadContent();
      LeaderboardsScreen.LoadContent();
      AchievementsScreen.LoadContent();
    }

    public static void SetScoreDelegate()
    {
      Game.SetScoreDelegate(new Game.ScoreDelegate(Game.DefaultScoreDelegate));
    }

    public static bool FailureEnabled()
    {
      return Game.game_work.gameMode != Game.GAME_MODE.GM_ZEN && Game.game_work.gameMode != Game.GAME_MODE.GM_ARCADE;
    }

    public static bool CoinsEnabled() => false;

    public static bool PowersEnabled() => Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE;

    public static int GetScoreMultiplyer() => Game.GetScoreMultiplyer(0);

    public static int GetScoreMultiplyer(int player) => 1;

    public static bool IsMultiplayer() => false;

    public delegate void GameInitFunction(uint flags);

    public delegate void GameFunction(float dt, bool update);

    public delegate void GameFunctionDraw(float dt, bool draw);

    public delegate void GameExitFunction();

    public enum TASK
    {
      TASK_SPLASHSCREEN,
      TASK_FRONTEND,
      TASK_GAME_UPDATE,
      TASK_PARTICLES_UPDATE,
    }

    public enum Task
    {
      TASK_SPLASHSCREEN,
      TASK_FRONTEND,
      TASK_GAME_UPDATE,
      TASK_PARTICLES_UPDATE,
    }

    public enum GAME_MODE
    {
      GM_CLASSIC,
      GM_CASINO,
      GM_ARCADE,
      GM_ZEN,
      GM_MAX,
    }

    public class GameWork
    {
      public bool inBonusScreen;
      public Game.Task routine_0;
      public Game.Task routine_1;
      public bool pause;
      public StringTableUtils.Language language;
      public Game.GAME_MODE gameMode;
      public bool gameOver;
      public bool inRetrySequence;
      public float retryTimer;
      public float gameOverTransition;
      public float hitBombTime;
      public byte currentMissCount;
      public int currentScore;
      public int scoreBeforeBonuses;
      public bool hasDroppedFruit;
      public int coins;
      public int coinsTotal;
      public int levelStartCoins;
      public float critHitTime;
      public int criticalChance;
      public bool canFastForward;
      public float dt;
      public HUD hud;
      public HUD menu;
      public bool soundEnabled;
      public bool musicEnabled;
      public FruitCamera camera;
      public FruitSaveData saveData;
      public Font pDebugFont;
      public Font pGameFont;
      public Font pNumberFont;
      public Font pNumberFontSilver;
      public Font pNumberFontGreen;
      public Font pNumberFontLeaderboard;
      public Font pNumberFontBlue2;
      public bool fullAmbience;
      public bool loadedSaveState;
      public float bombSize;
      public float bombCollision;
      public Vector3 mainPointer;
      public bool pointerPressed;
      public bool pointerReleased;
      public bool pointerDown;
      public Vector3[] touchPositions = new Vector3[16];
      public MainScreen mainScreen;
      public GameOverScreen gameOverScreen;
      public TutorialControl tutorialControl;
      public uint totalScore;
      public Texture backTexture;
      public TimeControl timeControl;
      public uint gameSeedValue;
    }

    public delegate int ScoreDelegate(int gdfh);
  }
}
