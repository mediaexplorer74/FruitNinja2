
// Type: GameManager.GameTask
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Mortar;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace GameManager
{
  public static class GameTask
  {
    private const float START_DELAY = 1.5f;
    public static Font hud_font;
    public static Font challenge_font;
    public static bool unpause_game = false;
    public static float unpauseDelay = 0.0f;
    public static float repauseDelay = 0.0f;
    public static bool challengeOver = false;
    public static bool debugMenu = false;
    public static bool clearInput = false;
    public static bool initialised = false;
    public static bool movieNotLoaded = false;
    public static Mortar.Texture backgroundTexture = (Mortar.Texture) null;
    public static MortarSound s_bombSound = (MortarSound) null;
    public static float s_startFadeInTime = 1.5f;
    public static Mortar.Texture s_HBlogo = (Mortar.Texture) null;
    public static float s_musicFade = 0.0f;
    public static int s_songPlaying = 0;
    public static int MAX_SLASHES = 16;
    public static SlashEntity[] inputEnts = ArrayInit.CreateFilledArray<SlashEntity>(GameTask.MAX_SLASHES);
    public static Model[] s_sliceModel = new Model[2];
    public static Vector3[] fruitSliceFrameScales = new Vector3[7]
    {
      new Vector3(1f, 1f, 1f),
      new Vector3(1.7f, 0.3f, 1f),
      new Vector3(8f, 0.1f, 1f),
      new Vector3(20f, 0.1f, 1f),
      new Vector3(4f, 0.1f, 1f),
      new Vector3(0.1f, 0.1f, 0.1f),
      new Vector3(0.1f, 0.1f, 0.1f)
    };
    public static LinkedList<GameTask.SliceEffect> s_slices = (LinkedList<GameTask.SliceEffect>) null;
    public static float[,] missPositions = new float[3, 4]
    {
      {
        79f,
        10f,
        -5f,
        0.75f
      },
      {
        52f,
        13f,
        5f,
        1f
      },
      {
        20f,
        18f,
        10f,
        1.2f
      }
    };
    public static MainScreen s_mainScreen = (MainScreen) null;
    public static PauseScreen s_pauseScreen = (PauseScreen) null;
    public static float END_SCREEN_DELAY = 0.33f;
    public static float end_screen_delay = GameTask.END_SCREEN_DELAY;
    public static Vector3 s_bombHitPos = Vector3.Zero;
    public static Vector3 s_critHitPos = Vector3.Zero;
    private static bool showdlg = true;
    public static bool s_menuBombHit = false;
    private static float slowTime = 1f;
    private static float slowTimeTime = 0.0f;
    private static float slowTimeSpeed = 1f;
    private static float paticlesDt = 1f;
    public static float quickener = 1f;
    public static Mortar.Texture s_flashTexture = (Mortar.Texture) null;
    public static Color s_flashColor = Color.White;
    public static bool __isSaving = false;
    public static float GetBombZPositionz = -10f;
    public static float GetFruitZPositionz = -500f;

    public static void SaveCurrentData() => GameTask.SaveCurrentData(false);

    public static void AddSlice(Vector3 pos, float angle, float mag)
    {
      GameTask.AddSlice(pos, angle, mag, false);
    }

    public static void SkipToPause() => GameTask.SkipToPause(true);

    public static float UNPAUSE_DELAY_TIME => 0.25f;

    public static float REPAUSE_DELAY_TIME => 0.4f;

    public static float CROSS_SPACING => 4f;

    public static float SLICE_FRAMES => 6f;

    public static float TOTAL_SLICE_LENGTH => 0.15f;

    public static float SLICE_SPEED
    {
      get
      {
        return (float) (1.0 / ((double) GameTask.TOTAL_SLICE_LENGTH / (double) GameTask.SLICE_FRAMES));
      }
    }

    public static float HIT_BOMB_WAIT => 3.2f;

    public static float BOMB_FLASH_START => 2f;

    public static float BOMB_FLASH_FULL => 1.55f;

    public static float BOMB_FLASH_START_FADE => 1f;

    public static float BOMB_FLASH_START_GAME_OVER => 1.5f;

    public static float BACKGROUND_UVS_U0 => 0.125f;

    public static float BACKGROUND_UVS_U1 => 0.875f;

    public static float BACKGROUND_UVS_V0 => 7f / 32f;

    public static float BACKGROUND_UVS_V1 => 25f / 32f;

    public static float RETRY_TIME_DELAY => 0.1f;

    private static void AllowMusicCallback(IAsyncResult result)
    {
      int? nullable = default;//Guide.EndShowMessageBox(result);
      if (!nullable.HasValue)
        return;
      //Guide.EndShowMessageBox(result);
      if (nullable.HasValue && nullable.Value == 0)
      {
        SoundManager.GetInstance().AllowMusic();
        SoundManager.GetInstance().SongPlay("Music-menu");
      }
      else
        SoundManager.GetInstance().CustomMusic = true;
    }

    private static void AllowMusicCallback2(IAsyncResult result)
    {
      int? nullable = default;//Guide.EndShowMessageBox(result);
      if (!nullable.HasValue)
        return;
      //Guide.EndShowMessageBox(result);
      if (nullable.HasValue && nullable.Value == 0)
      {
        SoundManager.GetInstance().AllowMusic();
        SoundManager.GetInstance().SongPlay("background");
      }
      else
        SoundManager.GetInstance().CustomMusic = true;
    }

    private static void ShowDialog()
    {
      if (SoundManager.GetInstance().UserPlayingMusic())
      {
        string[] buttons = new string[2]
        {
          Mortar.Game1.instance.stringTable.GetString(702),
          Mortar.Game1.instance.stringTable.GetString(707)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(890), 
        //    TheGame.instance.stringTable.GetString(891), 
        //    (IEnumerable<string>) buttons, 0, 
        //    MessageBoxIcon.Alert, new AsyncCallback(GameTask.AllowMusicCallback), (object) null);
      }
      else
      {
        SoundManager.GetInstance().AllowMusic();
        SoundManager.GetInstance().SongPlay("Music-menu");
      }
    }

    private static void ShowDialog2()
    {
      if (SoundManager.GetInstance().UserPlayingMusic())
      {
        string[] buttons = new string[2]
        {
          Mortar.Game1.instance.stringTable.GetString(702),
          Mortar.Game1.instance.stringTable.GetString(707)
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(890), TheGame.instance.stringTable.GetString(891), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(GameTask.AllowMusicCallback2), (object) null);
      }
      else
      {
        SoundManager.GetInstance().AllowMusic();
        SoundManager.GetInstance().SongPlay("background");
      }
    }

    public static void UpdateMusic(float dt)
    {
      if (!Game2.game_work.musicEnabled)
        return;
      if ((double) Game2.game_work.gameOverTransition < 0.0)
      {
        if (GameTask.s_songPlaying == -1)
          return;
        GameTask.s_songPlaying = -1;
        if (GameTask.showdlg)
        {
          GameTask.showdlg = false;
          GameTask.ShowDialog();
        }
        else
          SoundManager.GetInstance().SongPlay("Music-menu");
      }
      else
      {
        if (GameTask.s_songPlaying == 1)
          return;
        GameTask.s_songPlaying = 1;
        if (GameTask.showdlg)
        {
          GameTask.showdlg = false;
          GameTask.ShowDialog2();
        }
        else
          SoundManager.GetInstance().SongPlay("background");
      }
    }

    public static bool PointerMoveCallback(InputEvent e)
    {
      if (Mortar.Game1.exceptionThrown)
        return true;
      MortarRectangle windowSize = DisplayManager.GetInstance().GetWindowSize();
      Vector2 vector2 = new Vector2((float) (windowSize.right - windowSize.left), (float) (windowSize.bottom - windowSize.top));
      if (e.axis.axis == 116)
        Game2.game_work.mainPointer.X = (float) (((double) e.axis.absolutePos - (double) vector2.X / 2.0) * ((double) Game2.SCREEN_WIDTH / (double) Game2.SCREEN_SIZE_X));
      if (e.axis.axis == 117)
        Game2.game_work.mainPointer.Y = (float) (-((double) e.axis.absolutePos - (double) vector2.Y / 2.0) * ((double) Game2.SCREEN_HEIGHT / (double) Game2.SCREEN_SIZE_Y));
      if (e.axis.axis >= 153 && e.axis.axis < 169)
      {
        GameTask.inputEnts[e.axis.axis - 153].TouchMoveX(e);
        Game2.game_work.touchPositions[e.axis.axis - 153].X = (float) (((double) e.axis.absolutePos - (double) vector2.X / 2.0) * ((double) Game2.SCREEN_WIDTH / (double) Game2.SCREEN_SIZE_X));
      }
      if (e.axis.axis >= 169 && e.axis.axis < 185)
      {
        GameTask.inputEnts[e.axis.axis - 169].TouchMoveY(e);
        Game2.game_work.touchPositions[e.axis.axis - 169].Y = (float) (-((double) e.axis.absolutePos - (double) vector2.Y / 2.0) * ((double) Game2.SCREEN_HEIGHT / (double) Game2.SCREEN_SIZE_Y));
      }
      return true;
    }

    public static bool TouchDownCallback(InputEvent e)
    {
      if (Mortar.Game1.exceptionThrown || e.button.key < 137U || e.button.key >= 153U)
        return true;
      GameTask.inputEnts[(int)(e.button.key - 137U)].TouchDown(e);
      Game2.game_work.touchPositions[(int) (e.button.key - 137U)].Z 
                = (double) Game2.game_work.touchPositions[(int) (e.button.key - 137U)].Z != -1.0 
                ? 1f
                : 2f;

      return true;
    }

    public static bool PointerDownCallback(InputEvent e)
    {
      Game2.game_work.pointerDown = true;
      Game2.game_work.pointerPressed = true;
      return true;
    }

    public static bool PointerUpCallback(InputEvent e)
    {
      Game2.game_work.pointerDown = false;
      Game2.game_work.pointerReleased = true;
      return true;
    }

    public static bool GameReset(uint flags)
    {
      bool flag = false;
      try
      {
        if (GameTask.s_bombSound != null)
        {
          GameTask.s_bombSound.SetVolume(0.0f);
          GameTask.s_bombSound.Stop(0.0f);
          SoundManager.GetInstance().Release(GameTask.s_bombSound);
          Delete.SAFE_DELETE<MortarSound>(ref GameTask.s_bombSound);
        }
        GameTask.challengeOver = false;
        GameTask.unpause_game = false;
        GameTask.hud_font = (Font) null;
        GameTask.challenge_font = (Font) null;
        GameTask.s_sliceModel[0] = (Model) null;
        GameTask.s_sliceModel[1] = (Model) null;
        for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
          GameTask.inputEnts[index] = (SlashEntity) null;
        GameTask.s_slices.Clear();
        Delete.SAFE_DELETE<LinkedList<GameTask.SliceEffect>>(ref GameTask.s_slices);
        GameTask.s_flashTexture = (Mortar.Texture) null;
        PSPParticleManager.GetInstance().ClearEmitters();
        InputManager.GetInstance().ClearActions(0U);
        Game2.game_work.hud.Release();
        MissControl.CleanPool();
        ActorManager.GetInstance().Clear();
        ActorManager.GetInstance().ClearAllListeners();
        Game2.game_work.pause = false;
        GameTask.initialised = false;
        GameTask.backgroundTexture = (Mortar.Texture) null;
        //GC2.Collect();
        Game2.game_work.hud = (HUD) null;
        GameTask.GameInit(1U);
        flag = true;
      }
      catch
      {
      }
      return flag;
    }

    public static void GameInit(uint flags)
    {
      if (GameTask.initialised)
        return;
      GameTask.debugMenu = false;
      GameTask.challengeOver = false;
      if (Game2.game_work.hud == null)
        Game2.game_work.hud = new HUD();
      Game2.game_work.hud.Release();
      int num1 = 0;
      float num2 = num1 == 0 ? Game2.HUD_SCALE : 1f;
      for (int index = 0; index < (int) Game2.MAX_FRUIT_MISSES; ++index)
      {
        MissControl control = new MissControl();
        control.SetActive(true);
        if (num1 == 0)
          control.m_pos = new Vector3((float) ((double) Game2.SCREEN_WIDTH / 2.0 - (double) GameTask.missPositions[index, 0] * (double) num2), (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - (double) GameTask.missPositions[index, 1] * (double) num2), 50f);
        else
          control.m_pos = new Vector3((float) -((double) GameTask.missPositions[index, 0] - (double) GameTask.missPositions[1, 0] + 5.0) * num2, (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - ((double) GameTask.missPositions[index, 1] + 15.0) * (double) num2), 50f);
        control.m_rotation = -GameTask.missPositions[index, 2];
        control.m_scale = Vector3.Multiply(Vector3.Multiply(
            new Vector3(32f, 32f, 32f), GameTask.missPositions[index, 3]), num2);
        control.m_id = index;
        control.m_drawOrder = (HUD.HUD_ORDER) num1;
        Game2.game_work.hud.AddControl((HUDControl) control);
      }
      MissControl.CreatePool(12, Game2.game_work.hud);
      ScoreControl control1 = new ScoreControl();
      control1.m_texture = TextureManager.GetInstance().Load("textureswp7/hud_fruit.tex");
      control1.m_scroreTexture = TextureManager.GetInstance().Load("score.tex", true);
      control1.m_highScoreTexture = TextureManager.GetInstance().Load("new_best_score.tex", true);
      control1.m_scale = Vector3.Multiply(Vector3.One, 64f);
      control1.m_pos = new Vector3((float) (-(double) Game2.PIXEL_PLATFORMX(Game2.SCREEN_WIDTH / 2f - Game2.GAME_FONT_SIZE) + (double) control1.m_scale.X * 0.34999999403953552), Game2.PIXEL_PLATFORMY((float) ((double) Game2.SCREEN_HEIGHT / 2.0 + (double) Game2.GAME_FONT_SIZE * 4.0)) - control1.m_scale.Y * 0.35f, 0.0f);
      Game2.game_work.hud.AddControl((HUDControl) control1);
      Game2.game_work.timeControl = new TimeControl();
      Game2.game_work.timeControl.Init();
      Game2.game_work.timeControl.CountDown(90.9f);
      Game2.game_work.hud.AddControl((HUDControl) Game2.game_work.timeControl);
      if (GameTask.backgroundTexture == null)
        GameTask.backgroundTexture = Game2.IsFastHardware() ? TextureManager.GetInstance().Load("textureswp7/gb_game.tex") : TextureManager.GetInstance().Load("textureswp7/gb_game_sml.tex");
      GameTask.s_sliceModel[0] = MeshManager.GetInstance().Load("models/fruit/slice_fx.mmd");
      GameTask.s_sliceModel[1] = MeshManager.GetInstance().Load("models/fruit/slice_fx_crit.mmd");
      GameTask.s_slices = new LinkedList<GameTask.SliceEffect>();
      GameTask.s_slices.Clear();
      GameTask.hud_font = Game2.game_work.pGameFont;
      Game2.game_work.pause = false;
      GameTask.unpause_game = false;
      GameTask.clearInput = false;
      GameTask.initialised = true;
      GameTask.s_mainScreen = new MainScreen();
      GameTask.s_mainScreen.Init();
      Game2.game_work.mainScreen = GameTask.s_mainScreen;
      GameTask.s_pauseScreen = new PauseScreen();
      GameTask.s_pauseScreen.Init();
      Game2.game_work.tutorialControl = new TutorialControl();
      Game2.game_work.tutorialControl.Init();
      Game2.game_work.gameOver = true;
      Game2.game_work.gameOverTransition = -1f;
      Game2.game_work.hud.AddControl((HUDControl) GameTask.s_mainScreen);
      Game2.game_work.hud.AddControl((HUDControl) GameTask.s_pauseScreen);
      Game2.game_work.hud.AddControl((HUDControl) Game2.game_work.tutorialControl);
      Entity.HeapCreate(131072U);
      ActorManager.GetInstance().Initialise(5);
      ActorManager.GetInstance().RegisterFactory(new ActorManager.ActorFactory(EntityFactory.CreateEntity));
      ActorManager.GetInstance().RegisterHashConverter(new ActorManager.ActorTypeHashConvert(EntityFactory.HashTypeConvert));
      WaveManager.GetInstance().Init();
      GameTask.GameTaskInitInput();
      for (int index = 0; index < 30; ++index)
      {
        Entity entity1 = ActorManager.GetInstance().Add(EntityTypes.ENTITY_BEGIN);
        entity1.m_dormant = true;
        entity1.m_destroy = true;
        Entity entity2 = ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB);
        entity2.m_dormant = true;
        entity2.m_destroy = true;
        Entity entity3 = ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB_BLAST);
        entity3.m_dormant = true;
        entity3.m_destroy = true;
      }
      SplatEntity.CreatePool(128);
      WaveManager.GetInstance().Resume();
      BombFlash.CreatePool(32);
      SoundManager.GetInstance().Initialise("Sound/Win32Project/Win/FruitNinja", Game2.HEAP_SIZE_SOUND);
      SoundManager.GetInstance().SetSFXVolume(Game2.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
      if (flags != 1U || !Mortar.Game1.exceptionThrown)
        return;
            Mortar.Game1.exceptionThrown = false;
    }

    public static void SlowTime(float factor, float length)
    {
      GameTask.slowTimeTime = 1f;
      GameTask.slowTime = factor;
      GameTask.slowTimeSpeed = 1f / length;
    }

    public static void GameUpdate(float dt, bool update)
    {
      Game2.game_work.canFastForward = false;
      Game2.game_work.pointerReleased = false;
      Game2.game_work.pointerPressed = false;
      PopOverControl.Update(dt);
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
      {
        if ((double) Game2.game_work.touchPositions[index].Z > 0.0)
          Game2.game_work.touchPositions[index].Z = 0.0f;
        else if ((double) Game2.game_work.touchPositions[index].Z == 0.0)
          Game2.game_work.touchPositions[index].Z = -1f;
      }
      float dt1 = dt;
      if ((double) GameTask.s_startFadeInTime <= 0.0)
      {
        InputManager.GetInstance().Update(dt);
      }
      else
      {
        if (GameTask.s_HBlogo == null)
          GameTask.s_HBlogo = TextureManager.GetInstance().Load("textureswp7/HB_logo.tex");
        GameTask.s_startFadeInTime = Mortar.Math.MAX(GameTask.s_startFadeInTime - dt * 2f, 0.0f);
        dt = 0.0f;
        dt1 = 0.0f;
        Game2.game_work.dt = 0.0f;
        if ((double) GameTask.s_startFadeInTime <= 0.0)
          GameTask.s_HBlogo = (Mortar.Texture) null;
      }
      SoundManager.GetInstance().Update(dt);
      GameTask.UpdateMusic(dt);
      double endScreenDelay = (double) GameTask.end_screen_delay;
      float dt2 = dt;
      if (update)
      {
        Game2.game_work.saveData.inGame = false;
        float dt3 = dt1;
        if ((double) Game2.game_work.critHitTime > 0.0)
          Game2.game_work.critHitTime -= dt;
        float num = 1f;
        if ((double) GameTask.slowTimeTime > 0.0)
        {
          GameTask.slowTimeTime -= dt * GameTask.slowTimeSpeed;
          num += (GameTask.slowTime - num) * GameTask.slowTimeTime;
        }
        GameTask.quickener = Mortar.Math.MAX(GameTask.quickener - dt * (Game2.game_work.canFastForward ? 2.5f : 5f), 1f);
        dt *= GameTask.quickener * num;
        dt1 *= GameTask.quickener * num;
        Game2.game_work.dt *= GameTask.quickener * num;
        dt2 *= GameTask.quickener * num;
        SlashEntity.PreUpdate(dt3);
        SplatEntity.UpdateActiveSplats(dt3);
        float dt4;
        if ((double) Game2.game_work.hitBombTime > 0.0)
        {
          if (!GameTask.s_menuBombHit)
            Game2.game_work.canFastForward = true;
          float hitBombTime = Game2.game_work.hitBombTime;
          Game2.game_work.hitBombTime -= dt;
          if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && (double) Game2.game_work.gameOverTransition < 1.0)
            Game2.game_work.hitBombTime -= dt;
          if ((double) Game2.game_work.hitBombTime > (double) GameTask.BOMB_FLASH_FULL)
          {
            dt2 = -dt2;
          }
          else
          {
            GameTask.UpdateBombHit(hitBombTime);
            dt2 += dt2;
          }
          if ((double) Game2.game_work.hitBombTime <= (double) GameTask.BOMB_FLASH_START_GAME_OVER && (double) hitBombTime > (double) GameTask.BOMB_FLASH_START_GAME_OVER && !Game2.game_work.gameOver && !GameTask.s_menuBombHit)
            Game2.GameOver();
          if ((double) Game2.game_work.hitBombTime < 0.0)
            Game2.game_work.hitBombTime = 0.0f;
          dt4 = 0.0f;
        }
        else
        {
          Game2.game_work.saveData.inGame = true;
          WaveManager.GetInstance().Update(dt);
          dt4 = dt1 * WaveManager.GetInstance().GetWavedt();
        }
        if ((double) dt4 == 0.0 && (double) Game2.game_work.gameOverTransition < 0.0)
          dt4 = dt1;
        BombFlash.UpdateActiveFlashes(dt4);
        ActorManager.GetInstance().Update(dt4);
      }
      else
      {
        if ((double) Mortar.Math.ABS(Game2.game_work.gameOverTransition) > 0.99900001287460327)
          Game2.game_work.pause = false;
        if (Game2.game_work.pause)
          Game2.game_work.saveData.inGame = true;
        SlashEntity.PreUpdate(0.0f);
        for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
        {
          GameTask.inputEnts[index].Update(dt1);
          GameTask.inputEnts[index].DrawUpdate(dt1);
        }
        WaveManager.GetInstance().Update(0.0f);
      }
      GameTask.paticlesDt = 1f;
      if (!Game2.game_work.pause)
        GameTask.paticlesDt = Mortar.Math.MAX(1f, 1f / WaveManager.GetInstance().GetWavedt());
      PSPParticleManager.GetInstance().Update(dt * GameTask.paticlesDt, Game2.game_work.pause);
      Game2.game_work.camera.UpdateShake(dt2);
      Game2.game_work.hud.Update(0.0f);
      Game2.game_work.hud.Update(dt1);
      if (Game2.game_work.canFastForward)
        GameTask.quickener = Mortar.Math.MIN(5f, GameTask.quickener * (Game2.game_work.pointerPressed ? 1.75f : 1f));
      float num1 = Game2.game_work.gameOver ? 0.0f : Bomb.GetHeighestBomb();
      if ((double) num1 > 0.0 && !Game2.game_work.pause)
      {
        if (GameTask.s_bombSound == null || GameTask.s_bombSound.inst.State 
                    == Microsoft.Xna.Framework.Audio.SoundState.Stopped)
        {
          GameTask.s_bombSound = SoundManager.CreateNewSound();
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_BOMB_FUSE, 0U, GameTask.s_bombSound);
        }
        GameTask.s_bombSound.SetVolume(Mortar.Math.CLAMP(num1 / 100f, 0.0f, 1f));
      }
      else if (GameTask.s_bombSound != null)
        GameTask.s_bombSound.SetVolume(0.0f);
      if (!Game2.game_work.inRetrySequence)
        return;
      if ((double) Game2.game_work.retryTimer <= 0.0)
      {
        GameTask.EndRetryLevel();
        Game2.game_work.retryTimer = 0.0f;
      }
      else
      {
        GameTask.RetryUpdate(dt);
        Game2.game_work.retryTimer -= dt;
      }
    }

    public static void RetryUpdate(float dt)
    {
      Vector3 zero = Vector3.Zero;
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      float num1 = (GameTask.RETRY_TIME_DELAY - Game2.game_work.retryTimer) / GameTask.RETRY_TIME_DELAY;
      float num2 = num1 * num1;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
      {
        Vector3 origScale = ((Bomb) entity).m_orig_scale;
        entity.m_cur_scale = Vector3.Subtract(origScale, Vector3.Multiply(
            Vector3.Subtract(origScale, zero), num2));
        entity.m_vel = new Vector3(0.0f, 0.0f, 0.0f);
      }
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
      {
        Vector3 origScale = ((Fruit) entity).m_orig_scale;
        entity.m_cur_scale = Vector3.Subtract(origScale, Vector3.Multiply(
            Vector3.Subtract(origScale, zero), num2));
        entity.m_vel = new Vector3(0.0f, 0.0f, 0.0f);
        ((Fruit) entity).m_vel2 = new Vector3(0.0f, 0.0f, 0.0f);
      }
    }

    public static void GameDraw(float dt, bool draw)
    {
      if (draw)
      {
        float[] numArray = new float[3];
        for (int index = 0; index < 3; ++index)
          numArray[index] = Game2.game_work.hud.m_tint[index];
        DisplayManager.GetInstance().SetDepthBufferWrite(false);
        DisplayManager.GetInstance().SetDepthBuffer(false);
        Color color = new Color(64, 64, 64, (int) byte.MaxValue);
        Color white = Color.White;
        Color black = Color.Black;
        DisplayManager.GetInstance().SetGlobalAmbience(black);
        DisplayManager.GetInstance().SetLightDirection(new Vector3(Game2.game_work.mainPointer.X, Game2.game_work.mainPointer.Y, 100f));
        Game2.game_work.camera.SetupPerspective();
        if (GameTask.backgroundTexture == null)
          return;
        GameTask.backgroundTexture.Set();
        MatrixManager.instance.Reset();
        if ((double) Game2.game_work.camera.m_cameraShake.X == 0.0 && (double) Game2.game_work.camera.m_cameraShake.Y == 0.0)
        {
          MatrixManager.instance.SetMatrix(Matrix.Identity);
          MatrixManager.instance.Scale(new Vector3(Game2.SCREEN_WIDTH + 1f, Game2.SCREEN_HEIGHT + 1f, 0.0f));
          MatrixManager.instance.UploadCurrentMatrices();
          Mesh.DrawQuad(HUDControl.TintWhite(Game2.game_work.hud.m_backTint), (float) (0.5 - (double) Game2.SCREEN_WIDTH / 1024.0), (float) (0.5 + (double) Game2.SCREEN_WIDTH / 1024.0), (float) (0.5 - (double) Game2.SCREEN_HEIGHT / 1024.0), (float) (0.5 + (double) Game2.SCREEN_HEIGHT / 1024.0));
        }
        else
        {
          MatrixManager.instance.Scale(new Vector3(513f, 361f, 0.0f));
          MatrixManager.instance.Translate(new Vector3(0.0f, 0.0f, -5999f));
          MatrixManager.instance.UploadCurrentMatrices();
          Mesh.DrawQuad(HUDControl.TintWhite(Game2.game_work.hud.m_backTint), 0.0f, 1f, 19f / 128f, 109f / 128f);
        }
        GameTask.backgroundTexture.UnSet();
        PopOverControl.DrawBack();
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT);
        DisplayManager.GetInstance().SetDepthBufferWrite(true);
        DisplayManager.GetInstance().SetDepthBuffer(true);
        DisplayManager.GetInstance().SetGlobalAmbience(white);
        ActorManager.GetInstance().Draw();
        DisplayManager.GetInstance().SetGlobalAmbience(black);
        DisplayManager.GetInstance().SetDepthBuffer(true);
        DisplayManager.GetInstance().SetDepthBufferWrite(false);
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT);
        if (!PopOverControl.IsInPopup)
          SplatEntity.DrawActiveSplats();
        BombBlast.DrawActiveBlasts();
        BombFlash.DrawActiveFlashes();
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT);
        PSPParticleManager.GetInstance().Draw(dt, Game2.game_work.pause, -1);
        DisplayManager.GetInstance().SetDepthBuffer(false);
        if (!Game2.game_work.inBonusScreen)
          PSPParticleManager.GetInstance().Draw(dt, Game2.game_work.pause, 0);
        DisplayManager.GetInstance().SetGlobalAmbience(white);
        GameTask.DrawSlices(dt);
        DisplayManager.GetInstance().SetGlobalAmbience(black);
        Game2.game_work.hud.Draw();
        PSPParticleManager.GetInstance().Draw(dt, Game2.game_work.pause, 1);
        if (Game2.game_work.inBonusScreen)
          PSPParticleManager.GetInstance().Draw(dt, Game2.game_work.pause, 0);
        for (int index = 0; index < 3; ++index)
          Game2.game_work.hud.m_tint[index] = 1f;
        WaveManager.GetInstance().Draw();
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_POST);
        if (Game2.IsFastHardware() && (double) Game2.game_work.critHitTime > 0.0)
          GameTask.DrawCritHit();
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_BEFORE_BOMB);
        if ((double) Game2.game_work.hitBombTime > 0.0)
          GameTask.DrawBombHit();
        Game2.game_work.hud.Draw(HUD.HUD_ORDER.HUD_ORDER_AFTER_BOMB);
        for (int index = 0; index < 3; ++index)
          Game2.game_work.hud.m_tint[index] = numArray[index];
        if ((double) GameTask.s_startFadeInTime > 0.0)
          GameTask.DrawStartFade();
        PopOverControl.DrawFront();
      }
      if (GameTask.unpause_game && Game2.game_work.pause)
      {
        GameTask.unpause_game = false;
        InputManager.GetInstance().ClearActions(StringFunctions.StringHash("Input/PauseMenu.txt"));
        GameTask.debugMenu = false;
        Game2.game_work.pause = !Game2.game_work.pause;
      }
      if (!GameTask.clearInput)
        return;
      InputManager.GetInstance().ClearActions(StringFunctions.StringHash("Input/Input.txt"));
      GameTask.clearInput = false;
    }

    public static void ResetGameEntities()
    {
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
        GameTask.inputEnts[index].Reset();
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
      {
        entity.m_pos.Y = (float) (-(double) Game2.SCREEN_HEIGHT * 1.5);
        entity.m_vel.Y = -1.5f;
        ((Bomb) entity).Chuck();
        entity.Update(0.0f);
      }
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
      {
        ((Fruit) entity).Chuck();
        if (Game2.game_work.inRetrySequence)
          ((Fruit) entity).m_isSliced = true;
        if (!((Fruit) entity).m_isSliced)
        {
          Vector3 proj = Vector3.Subtract(GameTask.s_bombHitPos, entity.m_pos);
          if ((double) proj.LengthSquared() > 400.0)
          {
            proj.Normalize();
            proj = Vector3.Multiply(proj, 20f);
          }
          entity.CollisionResponse((Entity) null, 0U, 0U, ref proj);
          ((Fruit) entity).Slice();
        }
        entity.m_pos.Y = (float) (-(double) Game2.SCREEN_HEIGHT * 1.5);
        entity.m_vel.Y = -1.5f;
        ((Fruit) entity).m_pos2.Y = (float) (-(double) Game2.SCREEN_HEIGHT * 1.5);
        ((Fruit) entity).m_vel2.Y = -1.5f;
        entity.Update(0.0f);
      }
    }

    public static void RemoveFlashEntities()
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB_BLAST, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB_BLAST, ref iterator))
      {
        entity.m_destroy = true;
        entity.m_dormant = true;
      }
    }

    public static void GameOver() => GameTask.GameOver(-1);

    public static void GameOver(int state) => GameTask.GameOver(state, -1f);

    public static void GameOver(int state, float time) => GameTask.GameOver(state, time, -1);

    public static void GameOver(int state, float time, int player)
    {
      if (Game2.game_work.gameOver)
        return;
      Game2.game_work.gameOver = true;
      WaveManager.GetInstance().ClearUnspawned();
      Game2.game_work.gameOverScreen = new GameOverScreen((string) null, state, time, Game2.game_work.saveData.go_head, Game2.game_work.saveData.go_body, Game2.game_work.saveData.go_fruit, Game2.game_work.saveData.go_fact);
      Game2.game_work.saveData.go_head = Game2.game_work.saveData.go_body = Game2.game_work.saveData.go_fruit = Game2.game_work.saveData.go_fact = -1;
      Game2.game_work.gameOverScreen.Init();
      Game2.game_work.hud.AddControl((HUDControl) Game2.game_work.gameOverScreen);
    }

    public static void RetryLevel()
    {
      Game2.game_work.inRetrySequence = true;
      Game2.game_work.retryTimer = GameTask.RETRY_TIME_DELAY;
      WaveManager.GetInstance().ResetGlobalDt();
      Game2.game_work.gameOver = true;
      for (int index = 0; index < SplatEntity.poolCount; ++index)
      {
        SplatEntity splatEntity = SplatEntity.pool[index];
        splatEntity.m_fadeTime = Mortar.Math.MIN(GameTask.RETRY_TIME_DELAY * 1.5f, splatEntity.m_fadeTime);
        splatEntity.m_fadeSpeed = 0.25f;
      }
      if (GameTask.s_bombSound != null)
        GameTask.s_bombSound.SetVolume(0.0f);
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_DANANANA_SCHWING);
    }

    public static void EndRetryLevel()
    {
      GameTask.s_mainScreen.m_state = MainScreen.MS.MS_IN;
      GameTask.s_mainScreen.m_transitionWait = 0.5f;
      Game2.game_work.currentScore = 0;
      Game2.game_work.saveData.go_head = Game2.game_work.saveData.go_body = Game2.game_work.saveData.go_fruit = Game2.game_work.saveData.go_fact = -1;
      Game2.game_work.levelStartCoins = Game2.game_work.coins;
      GameTask.ResetGameEntities();
      GameTask.RemoveFlashEntities();
      WaveManager.GetInstance().Reset(true);
      Game2.game_work.gameOverTransition = 0.0f;
      Game2.game_work.gameOver = false;
      Game2.game_work.inRetrySequence = false;
      Game2.game_work.mainScreen.m_state = MainScreen.MS.MS_GAME;
    }

    public static void QuitToMenu()
    {
      WaveManager.GetInstance().ResetGlobalDt();
      Game2.game_work.gameOver = true;
      GameTask.s_mainScreen.m_state = MainScreen.MS.MS_IN;
      GameTask.s_mainScreen.m_transitionWait = 0.5f;
      Game2.game_work.currentScore = 0;
    }

    public static void HitBomb(Vector3 pos)
    {
      if (Game2.game_work.gameOver)
        return;
      uint hash = StringFunctions.StringHash("bomb");
      Game2.game_work.saveData.AddToTotal("bomb", hash, 1);
      Game2.game_work.hitBombTime = GameTask.HIT_BOMB_WAIT;
      Game2.game_work.camera.CreateCameraShake(pos, GameTask.HIT_BOMB_WAIT * 0.5f, 2f);
      GameTask.s_bombHitPos = pos;
      GameTask.s_menuBombHit = false;
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_BOMB_EXPLODE);
    }

    public static void HitMenuBomb(Vector3 pos)
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      Game2.game_work.hitBombTime = GameTask.BOMB_FLASH_START;
      GameTask.s_bombHitPos = pos;
      GameTask.s_menuBombHit = true;
    }

    public static bool BombFlashFull()
    {
      return (double) Game2.game_work.hitBombTime < (double) GameTask.BOMB_FLASH_FULL && (double) Game2.game_work.hitBombTime < (double) GameTask.BOMB_FLASH_START_FADE;
    }

    public static void UpdateBombHit(float prev)
    {
      if ((double) prev > (double) GameTask.BOMB_FLASH_FULL - 0.05000000074505806 && (double) Game2.game_work.hitBombTime <= (double) GameTask.BOMB_FLASH_FULL - 0.05000000074505806)
        GameTask.ResetGameEntities();
      if ((double) Game2.game_work.hitBombTime <= 0.0 || (double) Game2.game_work.hitBombTime >= (double) GameTask.BOMB_FLASH_FULL)
        return;
      GameTask.RemoveFlashEntities();
    }

    public static void DrawStartFade()
    {
      if ((double) GameTask.s_startFadeInTime <= 0.0)
        return;
      Game2.game_work.camera.SetupPerspective(FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL_NO_SHAKE, true);
      float num = 1f;
      float v;
      if ((double) GameTask.s_startFadeInTime > 0.5)
      {
        v = (GameTask.s_startFadeInTime - 0.5f) * 2f;
        num += (float) ((1.0 - (double) Mortar.Math.CLAMP(v, 0.0f, 1f)) * (1.0 - (double) Mortar.Math.CLAMP(v, 0.0f, 1f)));
      }
      else
      {
        v = 0.0f;
        double startFadeInTime = (double) GameTask.s_startFadeInTime;
      }
      GameTask.s_HBlogo.Set();
      MatrixManager.instance.Reset();
      MatrixManager.instance.Scale(Vector3.Multiply(new Vector3(
          Game2.SCREEN_WIDTH + 1f, Game2.SCREEN_HEIGHT + 1f, 1f), num));

      MatrixManager.instance.UploadCurrentMatrices();
      Mortar.Math.CLAMP((int) ((double) v * (double) byte.MaxValue), 0, (int) byte.MaxValue);
      GameTask.s_HBlogo.UnSet();
    }

    public static void DrawBombHit()
    {
      if (GameTask.s_flashTexture == null)
        GameTask.s_flashTexture = TextureManager.GetInstance().Load("textureswp7/flash.tex");
      if ((double) Game2.game_work.hitBombTime >= (double) GameTask.BOMB_FLASH_START)
        return;
      float num = Mortar.Math.CLAMP((float) (1.0 - ((double) Game2.game_work.hitBombTime - (double) GameTask.BOMB_FLASH_FULL) / ((double) GameTask.BOMB_FLASH_START - (double) GameTask.BOMB_FLASH_FULL)), 0.0f, 1f) * 20000f;
      GameTask.s_flashTexture.Set();
      MatrixManager.instance.Reset();
      MatrixManager.instance.Scale(new Vector3(num, num, 1f));
      MatrixManager.instance.Translate(GameTask.s_bombHitPos);
      MatrixManager.instance.UploadCurrentMatrices();
      Mesh.DrawQuad(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, Mortar.Math.CLAMP((int) ((double) Game2.game_work.hitBombTime * (double) byte.MaxValue / (double) GameTask.BOMB_FLASH_START_FADE), 0, (int) byte.MaxValue)));
      GameTask.s_flashTexture.UnSet();
    }

    public static void CriticalFlash(Vector3 pos, Color col)
    {
      GameTask.s_critHitPos = pos;
      GameTask.s_flashColor = col;
      Game2.game_work.critHitTime = Fruit.CRITICAL_FLASH_TIME;
    }

    public static void DrawCritHit()
    {
      if (GameTask.s_flashTexture == null)
        GameTask.s_flashTexture = TextureManager.GetInstance().Load("textureswp7/flash.tex");
      if ((double) Game2.game_work.critHitTime >= (double) Fruit.CRITICAL_FLASH_TIME)
        return;
      float v1 = Mortar.Math.CLAMP((float) (1.0 - ((double) Game2.game_work.critHitTime - (double) Fruit.CRITICAL_FLASH_FULL) / ((double) Fruit.CRITICAL_FLASH_TIME - (double) Fruit.CRITICAL_FLASH_FULL)), 0.0f, 1f) * 15000f;
      GameTask.s_flashTexture.UnSet();
      MatrixManager.instance.Reset();
      if (Game2.IsMultiplayer())
      {
        MatrixManager.instance.Scale(new Vector3(Mortar.Math.MIN(v1, Game2.SCREEN_WIDTH / 2f), Mortar.Math.MIN(v1, Game2.SCREEN_HEIGHT), 1f));
        if ((double) GameTask.s_critHitPos.X < 0.0)
          MatrixManager.instance.Translate(new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 4.0), 0.0f, 0.0f));
        else
          MatrixManager.instance.Translate(new Vector3(Game2.SCREEN_WIDTH / 4f, 0.0f, 0.0f));
      }
      else
      {
        MatrixManager.instance.Scale(new Vector3(Mortar.Math.MIN(v1, Game2.SCREEN_WIDTH), Mortar.Math.MIN(v1, Game2.SCREEN_HEIGHT), 1f));
        MatrixManager.instance.Translate(Vector3.Zero);
      }
      MatrixManager.instance.UploadCurrentMatrices();
      Mesh.DrawQuad(new Color((int) GameTask.s_flashColor.R, 
          (int)  GameTask.s_flashColor.G, 
          (int) GameTask.s_flashColor.B, 
          Mortar.Math.CLAMP((int) ((double) Game2.game_work.critHitTime 
          * (double) GameTask.s_flashColor.A / (double) GameTask.BOMB_FLASH_START_FADE),
          0, (int) GameTask.s_flashColor.A)));
      GameTask.s_flashTexture.UnSet();
    }

    public static void ClearMenuItems()
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
      {
        if (!((Fruit) entity).m_isSliced)
        {
          ((Fruit) entity).m_isSliced = true;
          entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(10f) - 5f, Mortar.Math.g_random.RandF(5f), 0.0f);
          entity.m_vel.X = System.Math.Abs(entity.m_vel.X) * (float) Mortar.Math.MATH_SIGN(entity.m_pos.X);
          ((Fruit) entity).m_vel2 = entity.m_vel;
        }
      }
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
      {
        if (((Bomb) entity).Enabled())
        {
          ((Bomb) entity).Disable();
          entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(10f) - 5f, Mortar.Math.g_random.RandF(5f), 0.0f);
        }
        ((Bomb) entity).EnableGravity(true);
      }
    }

    public static void SkipToPause(bool atStart)
    {
      if (!atStart && (GameTask.s_pauseScreen == null || !GameTask.s_pauseScreen.IsEnabled()))
        return;
      Game2.game_work.gameOverTransition = 0.0f;
      GameTask.s_pauseScreen.SkipTo();
      Game2.game_work.pause = true;
      Game2.game_work.gameOver = false;
      GameTask.s_mainScreen.Hide();
      Game2.game_work.hud.Skip();
    }

    public static void SkipToGameOver(
      int state,
      float time,
      float transition,
      float bombtime,
      int winner)
    {
      GameTask.s_mainScreen.Hide();
      Game2.game_work.hitBombTime = bombtime;
      Game2.game_work.gameOverTransition = transition;
      Game2.game_work.saveData.timer = 0.0f;
      if (Game2.game_work.timeControl != null)
        Game2.game_work.timeControl.SetTime(0.0f);
      Game2.game_work.gameOver = false;
      if (state > -1 && (double) time > -1.0)
        Game2.GameOver(state, time, winner);
      GameTask.s_menuBombHit = false;
      Game2.game_work.hud.Skip();
    }

    public static bool GetIsSavingBool() => GameTask.__isSaving;

    public static void SetIsSavingBool(bool s) => GameTask.__isSaving = s;

    public static void SaveCurrentData(bool isClosingSave)
    {
      if (Game2.game_work.saveData == null)
        return;
      GameTask.SetIsSavingBool(true);
      ItemManager.GetInstance().SaveItemInfo();
      FruitSaveData saveData = Game2.game_work.saveData.CLOAN();
      saveData.hasDropped = Game2.game_work.hasDroppedFruit;
      saveData.score = Game2.game_work.currentScore;
      saveData.misses = (int) Game2.game_work.currentMissCount;
      saveData.criticalProgression = Game2.game_work.criticalChance;
      saveData.consecutiveType = Fruit.s_consecutiveType;
      saveData.consecutiveCount = Fruit.s_consecutiveCount;
      saveData.mode = (int) Game2.game_work.gameMode;
      int num = Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE ? ((double) Game2.game_work.gameOverTransition >= 0.0099999997764825821 || (double) Game2.game_work.hitBombTime <= 0.0 ? 0 : (GameTask.s_mainScreen.m_state == MainScreen.MS.MS_GAME ? 1 : 0)) : (GameTask.s_menuBombHit ? 0 : ((double) Game2.game_work.hitBombTime > 0.0 ? 1 : 0));
      saveData.go_bombHitTime = num == 0 ? 0.0f : Game2.game_work.hitBombTime;
      if (Game2.game_work.gameOverScreen != null && !Game2.game_work.inRetrySequence && (Game2.game_work.gameOverScreen.GetState() == 0 || Game2.game_work.gameOverScreen.GetState() == 1 && (double) Game2.game_work.gameOverTransition < 1.0))
      {
        saveData.go_state = Game2.game_work.gameOverScreen.GetState();
        saveData.go_time = Game2.game_work.gameOverScreen.GetTime();
        saveData.go_transition = Game2.game_work.gameOverTransition;
        saveData.go_head = Game2.game_work.gameOverScreen.m_head;
        saveData.go_body = Game2.game_work.gameOverScreen.m_body;
        saveData.go_fruit = Game2.game_work.gameOverScreen.m_fruit;
        saveData.go_fact = Game2.game_work.gameOverScreen.m_fact;
        saveData.go_setScore = Game2.game_work.gameOverScreen.m_hasSetScore;
      }
      else
      {
        if (Game2.game_work.gameOverScreen != null)
        {
          saveData.inGame = false;
          saveData.score = Game2.game_work.currentScore;
        }
        else if (Game2.game_work.inRetrySequence)
          saveData.inGame = false;
        saveData.go_state = -1;
        saveData.go_time = -1f;
        saveData.go_head = -1;
        saveData.go_body = -1;
        saveData.go_fruit = -1;
        saveData.go_fact = -1;
        saveData.go_showHighScore = false;
        saveData.go_setScore = false;
        saveData.go_transition = 0.0f;
      }
      saveData.shake_max_time = Game2.game_work.camera.m_cameraShakeMaxTime;
      saveData.shake_time = Game2.game_work.camera.m_cameraShakeTime;
      if (isClosingSave)
        WaveManager.GetInstance().SaveWaveInfo(saveData);
      else
        saveData.inGame = false;
      if (!Game2.game_work.soundEnabled)
        saveData.AddToTotal("soundOff", StringFunctions.StringHash("soundOff"), 1, false);
      if (!Game2.game_work.musicEnabled)
        saveData.AddToTotal("musicOff", StringFunctions.StringHash("musicOff"), 1, false);
      Save.SaveGame(saveData);
      GameTask.SetIsSavingBool(false);
    }

    public static void GameExit()
    {
      Coin.ClearCoins(true);
      if (!GameTask.GetIsSavingBool())
      {
        Game2.game_work.hud.Save();
        GameTask.SaveCurrentData(true);
      }
      if (GameTask.s_bombSound != null)
      {
        GameTask.s_bombSound.SetVolume(0.0f);
        GameTask.s_bombSound.Stop(0.0f);
        SoundManager.GetInstance().Release(GameTask.s_bombSound);
        Delete.SAFE_DELETE<MortarSound>(ref GameTask.s_bombSound);
      }
      GameTask.challengeOver = false;
      GameTask.unpause_game = false;
      GameTask.hud_font = (Font) null;
      GameTask.challenge_font = (Font) null;
      GameTask.s_sliceModel[0] = (Model) null;
      GameTask.s_sliceModel[1] = (Model) null;
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
        GameTask.inputEnts[index] = (SlashEntity) null;
      GameTask.s_slices.Clear();
      Delete.SAFE_DELETE<LinkedList<GameTask.SliceEffect>>(ref GameTask.s_slices);
      GameTask.s_flashTexture = (Mortar.Texture) null;
      PSPParticleManager.GetInstance().ClearEmitters();
      InputManager.GetInstance().ClearActions(0U);
      Game2.game_work.hud.Release();
      Delete.SAFE_DELETE<HUD>(ref Game2.game_work.hud);
      Game2.game_work.hud = (HUD) null;
      MissControl.CleanPool();
      ActorManager.GetInstance().Clear();
      ActorManager.GetInstance().ClearAllListeners();
      Game2.game_work.pause = false;
      GameTask.initialised = false;
      GameTask.backgroundTexture = (Mortar.Texture) null;
      GameTask.s_HBlogo = (Mortar.Texture) null;
    }

    public static bool PauseGame(InputEvent e)
    {
      if (e == null)
        return true;
      if (!Game2.game_work.pause)
        GameTask.PauseGame();
      else
        GameTask.UnpauseGame();
      return true;
    }

    public static void AddSlice(Vector3 pos, float angle, float mag, bool isBlue)
    {
      mag *= 0.6f;
      GameTask.SliceEffect sliceEffect;
      sliceEffect.time = 0.0f;
      sliceEffect.mag = mag;
      sliceEffect.angle = angle;
      sliceEffect.pos = pos;
      sliceEffect.isBlue = isBlue;
      if ((double) mag > 2.5 && Mortar.Math.g_random.Rand32(3) == 0)
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_VISCERAL_IMPACT);
      GameTask.s_slices.AddFirst(sliceEffect);
    }

    public static void DrawSlices(float dt)
    {
      if (GameTask.s_slices == null)
        return;
      for (LinkedListNode<GameTask.SliceEffect> node = GameTask.s_slices.First; node != null; node = node.Next)
      {
        GameTask.SliceEffect sliceEffect = node.Value;
        sliceEffect.time += (float) ((double) dt * (double) GameTask.SLICE_SPEED * (sliceEffect.isBlue ? 0.75 : 1.0));
        if ((double) sliceEffect.time < (double) GameTask.SLICE_FRAMES)
        {
          if ((double) sliceEffect.time > 0.0)
          {
            Vector3 vector3 = Vector3.Multiply(Vector3.Add(
                GameTask.fruitSliceFrameScales[(int) sliceEffect.time],
                Vector3.Multiply(Vector3.Subtract(GameTask.fruitSliceFrameScales[(int) sliceEffect.time + 1], GameTask.fruitSliceFrameScales[(int) sliceEffect.time]), sliceEffect.time - (float) (int) sliceEffect.time)), sliceEffect.mag);
            Matrix matrix;
            Matrix.CreateScale(ref vector3, out matrix);
            matrix = Matrix.Multiply(Matrix.Multiply(matrix, 
                Matrix.CreateRotationZ(MathHelper.ToRadians(sliceEffect.angle))), Matrix.CreateTranslation(sliceEffect.pos));
            GameTask.s_sliceModel[sliceEffect.isBlue ? 1 : 0].Draw(new Matrix?(matrix));
          }
          node.Value = sliceEffect;
        }
        else
          GameTask.s_slices.Remove(node);
      }
    }

    public static void PauseGame()
    {
      GameTask.unpauseDelay = GameTask.UNPAUSE_DELAY_TIME;
      Game2.game_work.pause = true;
      GameTask.unpause_game = false;
    }

    public static void UnpauseGame()
    {
      GameTask.repauseDelay = GameTask.REPAUSE_DELAY_TIME;
      GameTask.unpause_game = true;
    }

    public static float GetPauseAmount()
    {
      return GameTask.s_pauseScreen == null ? 0.0f : Mortar.Math.CLAMP(GameTask.s_pauseScreen.GetTime(), 0.0f, 1f);
    }

    public static int GetPausedBy() => 0;

    public static float GetBombZPosition()
    {
      GameTask.GetBombZPositionz -= 50f;
      if ((double) GameTask.GetBombZPositionz < -400.0)
        GameTask.GetBombZPositionz = -10f;
      return GameTask.GetBombZPositionz;
    }

    public static float GetFruitZPosition()
    {
      GameTask.GetFruitZPositionz -= 100f;
      if ((double) GameTask.GetFruitZPositionz < -2499.0)
        GameTask.GetFruitZPositionz = -500f;
      return GameTask.GetFruitZPositionz;
    }

    public static void MoveFruitZPositionToBack(ref float z)
    {
      z += 500f;
      z /= 2f;
      z -= 2600f;
    }

    public static void ChangeBackground(string texture)
    {
      string texture1 = string.Format("textureswp7/{0}.tex", texture != null ? (object) texture : (object) "gb_game", (object) "");
      GameTask.backgroundTexture = TextureManager.GetInstance().Load(texture1);
    }

    public static void ChangeBackground(Mortar.Texture texture) => GameTask.backgroundTexture = texture;

    public static Texture GetCurrentBackground() => GameTask.backgroundTexture;

    public static int IsTouchDown(int touch)
    {
      if (touch >= 0 && touch < GameTask.MAX_SLASHES)
      {
        if ((double) Game2.game_work.touchPositions[touch].Z == 1.0)
          return 1;
        if ((double) Game2.game_work.touchPositions[touch].Z == 2.0)
          return 2;
      }
      return 0;
    }

    public static int TouchInRegion(float xMin, float xMax, float yMin, float yMax, int touch)
    {
      if (touch >= 0 && touch < GameTask.MAX_SLASHES && (double) Game2.game_work.touchPositions[touch].Z > 0.0 && (double) Game2.game_work.touchPositions[touch].X >= (double) xMin && (double) Game2.game_work.touchPositions[touch].X <= (double) xMax && (double) Game2.game_work.touchPositions[touch].Y >= (double) yMin && (double) Game2.game_work.touchPositions[touch].Y <= (double) yMax)
        return touch;
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
      {
        if ((double) Game2.game_work.touchPositions[index].Z > 0.0 && (double) Game2.game_work.touchPositions[index].X >= (double) xMin && (double) Game2.game_work.touchPositions[index].X <= (double) xMax && (double) Game2.game_work.touchPositions[index].Y >= (double) yMin && (double) Game2.game_work.touchPositions[index].Y <= (double) yMax)
          return index;
      }
      return -1;
    }

    public static void GameTaskInitInput()
    {
      InputManager.GetInstance().LoadConfigFile("Input/Input.txt");
      for (int index = 0; index < GameTask.MAX_SLASHES; ++index)
      {
        Game2.game_work.touchPositions[index] = Vector3.Zero;
        GameTask.inputEnts[index] = (SlashEntity) ActorManager.GetInstance().Add(EntityTypes.ENTITY_SLASH);
        Vector3 one = Vector3.One;
        GameTask.inputEnts[index].Init((byte[]) null, 0, new Vector3?(one));
        string hash1 = string.Format("TouchMove_X{0}", (object) index);
        string hash2 = string.Format("TouchDown_{0}", (object) index);
        InputManager.GetInstance().RegisterInputCallback(hash1, new InputActionMapper.InputCallback(GameTask.PointerMoveCallback));
        string hash3 = hash1.Replace('X', 'Y');
        InputManager.GetInstance().RegisterInputCallback(hash3, new InputActionMapper.InputCallback(GameTask.PointerMoveCallback));
        InputManager.GetInstance().RegisterInputCallback(hash2, new InputActionMapper.InputCallback(GameTask.TouchDownCallback));
      }
      InputManager.GetInstance().RegisterInputCallback("PointerMove", new InputActionMapper.InputCallback(GameTask.PointerMoveCallback));
      InputManager.GetInstance().RegisterInputCallback("PointerPressed", new InputActionMapper.InputCallback(GameTask.PointerDownCallback));
      InputManager.GetInstance().RegisterInputCallback("PointerReleased", new InputActionMapper.InputCallback(GameTask.PointerUpCallback));
      InputManager.GetInstance().RegisterInputCallback("PauseGame", new InputActionMapper.InputCallback(GameTask.PauseGame));
    }

    public struct SliceEffect
    {
      public float time;
      public float mag;
      public float angle;
      public Vector3 pos;
      public bool isBlue;
    }
  }
}
