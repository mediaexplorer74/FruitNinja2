
// Type: FruitNinja2.GameOverScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace FruitNinja2
{
  public class GameOverScreen : HUDControl3d
  {
    protected float m_alertTime;
    protected int m_state;
    protected float m_time;
    protected Vector3 m_originalScale;
    protected MenuButton m_retry;
    protected MenuButton m_leaderboardButton;
    protected MenuButton m_quitButton;
    protected static Texture[] m_senseiHeads = new Texture[3];
    protected static Texture[] m_senseiBody = new Texture[3];
    protected static Texture s_gameOverTexture;
    protected static Texture s_timeUpTexture;
    protected static Texture m_highScoreTexture;
    protected static Texture s_retryTexture;
    protected static Texture s_leaderboardTexture;
    protected static Texture s_quitTexture;
    protected int m_highScoreFrame;
    protected Vector3 m_senseiPos;
    protected FruitFactControl m_fruitFact;
    protected BonusScreen m_bonusScreen;
    protected bool m_alreadyPosted;
    protected bool m_waitingForPost;
    protected string m_coinsEarnedText;
    protected Texture m_oldHighScoreTexture;
    protected int m_mostOfFruit;
    protected int m_mostOfFruitSliced;
    public bool m_hasSetScore;
    public int m_head;
    public int m_body;
    public int m_fruit;
    public int m_fact;
    public float m_leaderbordPressedTime;
    private static string[] adjectives = new string[6]
    {
      "delicious",
      "tasty",
      "juicy",
      "luscious",
      "succulent",
      "lush"
    };
    private static bool s_isContentLoaded = false;
    private static bool buttonCalled = false;
    private static Texture s_timeUpGreenTexture = (Texture) null;
    private static Texture s_leaderboardButtonTexture = (Texture) null;
    private static Texture s_quitButtonTexture = (Texture) null;
    private static Texture s_coinsBundle = (Texture) null;

    public static float BEGINNING_WAIT => 1.9f;

    public static float BEGINNING_Y => 0.0f;

    public static float BEGINNING_SCALE => 2f;

    public static float NORMAL_SCALE => 1f;

    public static float NORMAL_Y => Game.SCREEN_HEIGHT * 0.7f;

    public static float POP_IN_TIME => 0.2f;

    public static float POP_SINE => (float) Mortar.Math.DEGREE_TO_IDX(110f);

    public static float SIDE_BUTTONS_X => 150f;

    public static float SIDE_BUTTONS_Y => Game.USE_ARCADE_GO_SCREEN ? -130f : 5f;

    public static float TWITTER_X => (float) (-(double) Game.SCREEN_WIDTH / 2.0 + 50.0);

    public static float FACEBOOK_X
    {
      get
      {
        return (float) (-(double) Game.SCREEN_WIDTH / 2.0 + (Game.USE_ARCADE_GO_SCREEN ? 20.0 : 80.0));
      }
    }

    public static float SENSEI_CENTER_X => (float) (-(double) Game.SCREEN_WIDTH / 2.0 + 222.0);

    public static float SENSEI_CENTER_Y => 55f;

    public static float SENSEI_OFFSCREEN_X => (float) ((double) Game.SCREEN_WIDTH / 2.0 + 128.0);

    public static Vector3 SENSEI_HEAD_OFFSET => new Vector3(9f, 40f, 0.0f);

    public static Vector3 HIGH_SCORE_OFFSET => new Vector3(-80f, 70f, 0.0f);

    public static Vector3 SENSEI_FACT_OFFSET_X => new Vector3(183f, 12f, 0.0f);

    public static float NEW_HIGHSCORE_FLASH => 1000f;

    public static float ZEN_BOARD_X => (float) (-(double) Game.SCREEN_WIDTH / 2.0 + 315.0);

    public static float ZEN_BOARD_Y => 53f;

    public static Vector3 QUIT_POS
    {
      get => new Vector3(163f, (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 64.0), 0.0f);
    }

    public static Vector3 RETRY_POS
    {
      get => new Vector3(0.0f, (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 64.0), 0.0f);
    }

    public static Vector3 LEADERBOARD_POS
    {
      get => new Vector3(-163f, (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 64.0), 0.0f);
    }

    public static Vector3 LEADERBOARD_BUTTON_POS => new Vector3(190f, -50f, 0.0f);

    public static Vector3 QUIT_BUTTON_POS => new Vector3(190f, -125f, 0.0f);

    public GameOverScreen()
      : this((string) null)
    {
    }

    public GameOverScreen(string texture)
      : this(texture, -1)
    {
    }

    public GameOverScreen(string texture, int state)
      : this(texture, state, -1f)
    {
    }

    public GameOverScreen(string texture, int state, float time)
      : this(texture, state, time, -1)
    {
    }

    public GameOverScreen(string texture, int state, float time, int head)
      : this(texture, state, time, head, -1)
    {
    }

    public GameOverScreen(string texture, int state, float time, int head, int body)
      : this(texture, state, time, head, body, -1)
    {
    }

    public GameOverScreen(string texture, int state, float time, int head, int body, int fruit)
      : this(texture, state, time, head, body, fruit, -1)
    {
    }

    public GameOverScreen(
      string texture,
      int state,
      float time,
      int head,
      int body,
      int fruit,
      int fact)
    {
      this.m_deleteCall = (HUDControl.HUDControlDeletedCallback) (l => l.Release());
      if (!GameOverScreen.s_isContentLoaded)
        GameOverScreen.LoadContent();
      this.m_mostOfFruit = 0;
      this.m_mostOfFruitSliced = 0;
      this.m_alertTime = 0.0f;
      this.m_fruit = fruit;
      this.m_fact = fact;
      this.m_texture = Game.USE_ZEN_GO_SCREEN || Game.USE_ARCADE_GO_SCREEN ? (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE ? GameOverScreen.s_timeUpGreenTexture : GameOverScreen.s_timeUpTexture) : GameOverScreen.s_gameOverTexture;
      this.m_originalScale = new Vector3((float) this.m_texture.GetWidth() / 2.2f, (float) this.m_texture.GetHeight() / 2.2f, 0.0f);
      this.m_state = 0;
      this.m_selfCleanUp = false;
      this.m_time = 0.0f;
      this.m_oldHighScoreTexture = (Texture) null;
      GameOverScreen.m_highScoreTexture = (Texture) null;
      this.m_highScoreFrame = 0;
      this.m_retry = (MenuButton) null;
      this.m_hasSetScore = false;
      this.m_bonusScreen = (BonusScreen) null;
      this.m_head = head;
      this.m_body = body;
      this.m_leaderboardButton = (MenuButton) null;
      this.m_quitButton = (MenuButton) null;
      if (this.m_head < 1)
      {
        this.m_head = 1;
        this.m_mostOfFruitSliced = 0;
        this.m_mostOfFruit = -1;
        for (int type = 0; type < Fruit.MAX_FRUIT_TYPES - 1; ++type)
        {
          int total = Game.game_work.saveData.GetTotal(Fruit.FruitTypeHash(type));
          if (total > this.m_mostOfFruitSliced)
          {
            this.m_mostOfFruitSliced = total;
            this.m_mostOfFruit = type;
          }
        }
        if (Game.game_work.currentScore > Game.GetCurrentModeHighscore() / 2)
          this.m_head = 2 + Mortar.Math.g_random.Rand32(2);
      }
      if (this.m_body < 1)
        this.m_body = Mortar.Math.g_random.Rand32(3) + 1;
      this.m_pos = new Vector3(0.0f, GameOverScreen.BEGINNING_Y, 0.0f);
      this.m_senseiPos = new Vector3(GameOverScreen.SENSEI_OFFSCREEN_X, GameOverScreen.SENSEI_CENTER_Y, 0.0f);
      this.m_fruitFact = (FruitFactControl) null;
      this.m_alreadyPosted = false;
      this.m_waitingForPost = false;
      this.m_coinsEarnedText = string.Format("YOU JUST EARNT {0} COINS", (object) (Game.game_work.coins - Game.game_work.levelStartCoins));
      if ((double) time < 0.0 || state < 0)
        return;
      this.m_mostOfFruitSliced = 0;
      this.m_mostOfFruit = -1;
      for (int type = 0; type < Fruit.MAX_FRUIT_TYPES - 1; ++type)
      {
        int total = Game.game_work.saveData.GetTotal(Fruit.FruitTypeHash(type));
        if (total > this.m_mostOfFruitSliced)
        {
          this.m_mostOfFruitSliced = total;
          this.m_mostOfFruit = type;
        }
      }
      if (state != 0 && (double) Game.game_work.gameOverTransition > 0.99900001287460327)
      {
        Game.game_work.gameOverTransition = 0.998f;
        this.m_state = 2;
        this.m_hasSetScore = true;
        this.Update(0.0f);
      }
      this.m_time = time;
      this.m_state = state;
      this.m_texture = (Texture) null;
    }

    ~GameOverScreen() => this.Release();

    public void RetryCallback()
    {
      if (this.m_state != 0 && (this.m_state != 2 || (double) Game.game_work.gameOverTransition <= 0.99900001287460327))
        return;
      Mortar.Math.g_random.Seed(Game.game_work.gameSeedValue);
      Game.game_work.saveData.ClearCombo();
      this.m_state = 3;
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_DANANANA_SCHWING);
    }

    public void LeaderboardsCallback()
    {
      if (this.m_state != 0 && (this.m_state != 2 || (double) Game.game_work.gameOverTransition <= 0.99900001287460327))
        return;
      this.m_time = 0.0f;
      this.m_state = 6;
      this.m_leaderbordPressedTime = 0.0f;
    }

    public void QuitCallback()
    {
      if (this.m_state != 0 && this.m_state != 2)
        return;
      Game.game_work.saveData.ClearCombo();
      this.m_state = 5;
      Game.HitMenuBomb(new Vector3((float) (403.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 256.0), 0.0f));
    }

    public void FacebookCallback() => throw new MissingMemberException();

    public void TwitterCallback() => throw new MissingMemberException();

    public void PostCallback(int state) => throw new MissingMemberException();

    public int GetState() => this.m_state;

    public float GetTime() => this.m_time;

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      if (Game.game_work.gameOverScreen == this)
      {
        Game.game_work.saveData.go_head = Game.game_work.saveData.go_body = Game.game_work.saveData.go_fruit = Game.game_work.saveData.go_fact = -1;
        Game.game_work.saveData.go_showHighScore = false;
        Game.game_work.gameOverScreen = (GameOverScreen) null;
      }
      if (this.m_fruitFact != null)
      {
        this.m_fruitFact.Release();
        Game.game_work.hud.RemoveControl((HUDControl) this.m_fruitFact);
      }
      if (this.m_leaderboardButton != null)
        Game.game_work.hud.RemoveControl((HUDControl) this.m_leaderboardButton);
      if (this.m_quitButton != null)
        Game.game_work.hud.RemoveControl((HUDControl) this.m_quitButton);
      Delete.SAFE_DELETE<FruitFactControl>(ref this.m_fruitFact);
      Delete.SAFE_DELETE<MenuButton>(ref this.m_leaderboardButton);
      Delete.SAFE_DELETE<MenuButton>(ref this.m_quitButton);
    }

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      this.m_highScoreFrame += (int) ((double) dt * 1000.0);
      if ((double) this.m_highScoreFrame >= (double) GameOverScreen.NEW_HIGHSCORE_FLASH)
        this.m_highScoreFrame -= (int) GameOverScreen.NEW_HIGHSCORE_FLASH;
      if (GameOverScreen.buttonCalled)
        GameOverScreen.buttonCalled = false;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      switch (this.m_state)
      {
        case 0:
          if (!this.m_hasSetScore)
            Game.game_work.canFastForward = true;
          this.m_time += dt;
          this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
          if ((double) this.m_time < (double) GameOverScreen.POP_IN_TIME)
            this.m_scale = Vector3.Multiply(Vector3.Divide(Vector3.Multiply(this.m_originalScale, Mortar.Math.SinIdx((ushort) ((double) Mortar.Math.MIN(GameOverScreen.POP_SINE, GameOverScreen.POP_SINE) * ((double) this.m_time / (double) GameOverScreen.POP_IN_TIME)))), Mortar.Math.SinIdx((ushort) GameOverScreen.POP_SINE)), GameOverScreen.BEGINNING_SCALE);
          else
            this.m_scale = Vector3.Multiply(this.m_originalScale, GameOverScreen.BEGINNING_SCALE);
          if ((double) this.m_time > (double) GameOverScreen.BEGINNING_WAIT)
          {
            if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
            {
              this.m_state = 1;
              this.m_time = -0.333f;
            }
            else
              this.SetStateWait();
          }
          this.m_pos = new Vector3(0.0f, GameOverScreen.BEGINNING_Y, 0.0f);
          break;
        case 1:
          if (ActorManager.GetInstance().GetNumEntities(0) <= 0U && ActorManager.GetInstance().GetNumEntities(1) <= 0U)
          {
            if (this.m_bonusScreen == null)
            {
              this.m_bonusScreen = new BonusScreen();
              this.m_bonusScreen.m_pos = new Vector3(0.0f, -20f, 0.0f);
              this.m_bonusScreen.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.DeletedBonusScreen);
              Game.game_work.hud.AddControl((HUDControl) this.m_bonusScreen);
              BonusManager.GetInstance().SetUpBonusScreen(this.m_bonusScreen);
            }
            else
            {
              this.m_pos.Y = Mortar.Math.MAX(this.m_pos.Y, (float) ((double) this.m_bonusScreen.m_pos.Y + (double) this.m_bonusScreen.m_offset.Y + 135.0));
              this.m_scale = Vector3.Multiply(Vector3.Multiply(this.m_originalScale, GameOverScreen.BEGINNING_SCALE), (float) (1.0 - (double) this.m_pos.Y / ((double) Game.SCREEN_HEIGHT * 0.699999988079071)));
            }
            this.m_time += dt;
            this.m_bonusScreen.m_time = this.m_time;
            break;
          }
          break;
        case 2:
          if (Game.USE_ARCADE_GO_SCREEN)
          {
            if (this.m_leaderboardButton == null && GameOverScreen.s_leaderboardButtonTexture != null)
            {
              this.m_leaderboardButton = new MenuButton(GameOverScreen.s_leaderboardButtonTexture, GameOverScreen.LEADERBOARD_BUTTON_POS, new MenuButton.MenuCallback(this.LeaderboardsCallback));
              this.m_leaderboardButton.Init();
              this.m_leaderboardButton.SetInnerBound(-30f);
              Game.game_work.hud.AddControl((HUDControl) this.m_leaderboardButton);
            }
            if (this.m_quitButton == null && GameOverScreen.s_quitButtonTexture != null)
            {
              this.m_quitButton = new MenuButton(GameOverScreen.s_quitButtonTexture, GameOverScreen.QUIT_BUTTON_POS, new MenuButton.MenuCallback(this.QuitCallback));
              this.m_quitButton.Init();
              this.m_quitButton.m_triggerOnBackPress = true;
              this.m_quitButton.SetInnerBound(-30f);
              Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
            }
          }
          else if (this.m_fruitFact == null)
          {
            this.m_fruitFact = new FruitFactControl();
            this.m_fruitFact.m_pos = Vector3.Add(this.m_senseiPos, GameOverScreen.SENSEI_FACT_OFFSET_X);
            this.m_fruitFact.m_fruitType = this.m_fruit;
            this.m_fruitFact.m_fact = this.m_fact;
            this.m_fruitFact.Init();
            Game.game_work.hud.AddControl((HUDControl) this.m_fruitFact);
          }
          if ((double) Game.game_work.gameOverTransition < 0.99900001287460327)
          {
            double gameOverTransition = (double) Game.game_work.gameOverTransition;
            Game.game_work.gameOverTransition += (float) ((1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
            if ((double) Game.game_work.gameOverTransition >= 0.99900001287460327)
            {
              if (!this.m_hasSetScore)
              {
                int currentScore = Game.game_work.currentScore;
                this.m_hasSetScore = true;
                Game.game_work.saveData.go_setScore = false;
                uint hash1 = StringFunctions.StringHash("games");
                uint hash2 = StringFunctions.StringHash("totalscore");
                Game.game_work.saveData.AddToTotal("games", hash1, 1);
                if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                {
                  Game.game_work.saveData.AddToTotal("totalscore", hash2, Game.game_work.scoreBeforeBonuses);
                  Game.game_work.totalScore -= (uint) (currentScore - Game.game_work.scoreBeforeBonuses);
                }
                else
                  Game.game_work.saveData.AddToTotal("totalscore", hash2, currentScore);
                Game.game_work.saveData.UnlockTotals();
                AchievementManager.GetInstance().UnlockScoreAchievement(currentScore);
                AchievementManager.GetInstance().UnlockTotalFruitAchievement((int) Game.game_work.totalScore);
                AchievementManager.GetInstance().UnlockEndScoreAchievement(currentScore, Game.GetCurrentModeHighscore());
                if (this.m_fruitFact != null && Game.USE_ZEN_GO_SCREEN && this.m_fruitFact.m_comboStickerType > COMBO_TYPE.CT_NONE && this.m_fruitFact.m_comboStickerType < COMBO_TYPE.CT_MAX)
                  AchievementManager.GetInstance().UnlockComboStarAchievement(this.m_fruitFact.m_numComboFruits, StringFunctions.StringHash(ComboChecker.GetComboName(this.m_fruitFact.m_comboStickerType)));
                Game.game_work.saveData.go_showHighScore = false;
                if (currentScore > Game.GetCurrentModeHighscore() / 2)
                  Game.game_work.saveData.go_showHighScore = Game.SetCurrentModeHighscore(currentScore);
                Game.game_work.saveData.FinishedGame();
                Game.game_work.saveData.ClearTotals();
                GameTask.SaveCurrentData();
                if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                  TheGame.settings.tf += Game.game_work.scoreBeforeBonuses;
                else
                  TheGame.settings.tf += currentScore;
                Leaderboards.Write(4, (long) TheGame.settings.tf);
                if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                {
                  int num1 = DateTime.Now.DayOfYear / 7;
                  int num2 = DateTime.Now.Year - 2000;
                  if (num2 >= 10)
                  {
                    if (num1 != TheGame.settings.week)
                    {
                      TheGame.settings.bestThisWeek = 0;
                      TheGame.settings.week = num1;
                    }
                    if (TheGame.settings.bestThisWeek < currentScore)
                    {
                      TheGame.settings.bestThisWeek = currentScore;
                      Leaderboards.Write(3, (long) (num2 << 24 | num1 << 16 | currentScore));
                    }
                  }
                }
                TheGame.SaveConfig();
                switch (Game.game_work.gameMode)
                {
                  case Game.GAME_MODE.GM_CLASSIC:
                    LeaderboardsScreen.SetStartLeaderboard(0);
                    break;
                  case Game.GAME_MODE.GM_ARCADE:
                    LeaderboardsScreen.SetStartLeaderboard(2);
                    break;
                  case Game.GAME_MODE.GM_ZEN:
                    LeaderboardsScreen.SetStartLeaderboard(1);
                    break;
                }
              }
              Game.game_work.gameOverTransition = 1f;
              this.m_state = 2;
              this.m_retry = new MenuButton(GameOverScreen.s_retryTexture, GameOverScreen.RETRY_POS, new MenuButton.MenuCallback(this.RetryCallback), 0);
              this.m_retry.Init();
              Game.game_work.hud.AddControl((HUDControl) this.m_retry);
              if (Game.USE_ARCADE_GO_SCREEN)
              {
                this.m_retry.m_clearOthers = false;
                this.m_retry.m_pos.Y = (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 240.0);
                this.m_retry.m_pos.X = (float) (-(double) Game.SCREEN_WIDTH / 2.0 + 321.0);
                Game.game_work.tutorialControl.ResetTutePos(this.m_retry);
              }
              else
              {
                MenuButton control = new MenuButton(GameOverScreen.s_quitTexture, GameOverScreen.QUIT_POS, new MenuButton.MenuCallback(this.QuitCallback), Fruit.MAX_FRUIT_TYPES);
                control.m_originalScale = this.m_retry.m_originalScale;
                control.Init();
                control.m_triggerOnBackPress = true;
                Game.game_work.hud.AddControl((HUDControl) control);
                Game.game_work.tutorialControl.ResetTutePos(this.m_retry);
              }
              if (!Game.USE_ARCADE_GO_SCREEN)
              {
                MenuButton control = new MenuButton(GameOverScreen.s_leaderboardTexture, GameOverScreen.LEADERBOARD_POS, new MenuButton.MenuCallback(this.LeaderboardsCallback), Fruit.FruitType("orange"));
                control.m_isTrialLockable = true;
                control.Init();
                Game.game_work.hud.AddControl((HUDControl) control);
              }
            }
          }
          if ((double) this.m_pos.Y < (double) GameOverScreen.NORMAL_Y * 0.949999988079071)
          {
            this.m_scale = Vector3.Multiply(this.m_originalScale, GameOverScreen.BEGINNING_SCALE + (GameOverScreen.NORMAL_SCALE - GameOverScreen.BEGINNING_SCALE) * Game.game_work.gameOverTransition);
            this.m_pos = new Vector3(0.0f, GameOverScreen.BEGINNING_Y + (GameOverScreen.NORMAL_Y - GameOverScreen.BEGINNING_Y) * Game.game_work.gameOverTransition, 0.0f);
            break;
          }
          break;
        case 3:
          if (ActorManager.GetInstance().GetNumEntities(0) <= 0U || this.m_leaderboardButton != null)
          {
            Game.game_work.levelStartCoins = Game.game_work.coins;
            WaveManager.GetInstance().Reset();
            Game.game_work.gameOver = true;
            this.m_state = 4;
            break;
          }
          Game.game_work.gameOverTransition = 1f;
          goto case 2;
        case 4:
          Game.game_work.gameOverTransition *= 0.75f;
          if ((double) Game.game_work.gameOverTransition < 1.0 / 1000.0)
          {
            WaveManager.GetInstance().Reset();
            Game.game_work.gameOverTransition = 0.0f;
            Game.game_work.gameOver = false;
            WaveManager.GetInstance().NewGame();
            this.m_terminate = true;
          }
          if ((double) this.m_pos.Y < (double) GameOverScreen.BEGINNING_Y * 0.949999988079071)
          {
            this.m_pos = new Vector3(0.0f, GameOverScreen.NORMAL_Y + (float) (((double) GameOverScreen.NORMAL_Y - (double) GameOverScreen.BEGINNING_Y) * (1.0 - (double) Game.game_work.gameOverTransition)), 0.0f);
            break;
          }
          break;
        case 5:
          Game.QuitToMenu();
          this.m_state = 7;
          break;
        case 6:
          if ((double) this.m_leaderbordPressedTime < 1.0)
          {
            this.m_leaderbordPressedTime += dt;
            if ((double) this.m_leaderbordPressedTime >= 1.0)
            {
              if (Game.isWP7TrialMode())
              {
                TheGame.instance.DoUpsell(false);
                break;
              }
              PopOverControl.Instance.In((PopOverControl.WhenIn) (() =>
              {
                LeaderboardsScreen control = new LeaderboardsScreen();
                control.Init();
                Game.game_work.hud.AddControl((HUDControl) control);
              }));
              break;
            }
            break;
          }
          if (((int) ActorManager.GetInstance().GetNumEntities(0) + (int) ActorManager.GetInstance().GetNumEntities(1) == 0 || this.m_leaderboardButton != null) && (PopOverControl.State == PopOverControl.POC.OUT || PopOverControl.State == PopOverControl.POC.MOVING_OUT))
          {
            if ((double) this.m_time <= 0.0)
              this.m_time = 0.0f;
            this.m_time += dt;
            if ((double) this.m_time > 0.20000000298023224)
            {
              this.m_state = 2;
              if (this.m_leaderboardButton == null)
                Game.game_work.gameOverTransition = 0.998f;
              this.m_time = GameOverScreen.BEGINNING_WAIT + 0.1f;
              break;
            }
            break;
          }
          break;
        case 7:
          if ((double) Game.game_work.gameOverTransition < 0.0)
          {
            this.m_terminate = true;
            break;
          }
          break;
      }
      if (this.m_state != 2 && this.m_state != 0 && this.m_retry != null && (double) this.m_retry.m_scale.X < 50.5)
        this.m_retry = (MenuButton) null;
      if (Game.USE_ZEN_GO_SCREEN && this.m_fruitFact != null)
      {
        this.m_fruitFact.m_pos = new Vector3(GameOverScreen.ZEN_BOARD_X + (float) (480.0 * (1.0 - (double) Game.game_work.gameOverTransition)), GameOverScreen.ZEN_BOARD_Y, 0.0f);
        this.m_senseiPos = Vector3.Add(this.m_fruitFact.m_pos, Vector3.Multiply(Vector3.UnitX, 200f));
      }
      else
      {
        this.m_senseiPos = new Vector3(GameOverScreen.SENSEI_OFFSCREEN_X + (GameOverScreen.SENSEI_CENTER_X - GameOverScreen.SENSEI_OFFSCREEN_X) * Game.game_work.gameOverTransition, GameOverScreen.SENSEI_CENTER_Y, 0.0f);
        if (this.m_fruitFact != null)
          this.m_fruitFact.m_pos = Vector3.Add(this.m_senseiPos, GameOverScreen.SENSEI_FACT_OFFSET_X);
      }
      if (this.m_leaderboardButton != null)
        this.m_leaderboardButton.m_pos = Vector3.Add(GameOverScreen.LEADERBOARD_BUTTON_POS, Vector3.Multiply(Vector3.Multiply(Vector3.UnitX, 1f - Game.game_work.gameOverTransition), 120f));
      if (this.m_quitButton == null)
        return;
      this.m_quitButton.m_pos = Vector3.Add(GameOverScreen.QUIT_BUTTON_POS, Vector3.Multiply(Vector3.Multiply(Vector3.UnitX, 1f - Game.game_work.gameOverTransition), 120f));
    }

    public override void PreDraw(float[] tintChannels)
    {
      if (this.m_drawOrder == HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT)
      {
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
        if (Game.USE_ARCADE_GO_SCREEN)
        {
          Vector3 vector3 = new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + 201.0), 
              (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 250.0 -
              (1.0 - (double) Game.game_work.gameOverTransition) 
              * (double) Game.SCREEN_WIDTH / 2.0), 0.0f);

          if (GameOverScreen.s_coinsBundle != null)
          {
            Matrix mtx;
            Mortar.Math.Scale44((float) GameOverScreen.s_coinsBundle.GetWidth(),
                (float) GameOverScreen.s_coinsBundle.GetHeight(), 0.0f, out mtx);
            Mortar.Math.GlobalTranslate44(ref mtx, vector3);
            MatrixManager.GetInstance().SetMatrix(mtx);
            MatrixManager.GetInstance().UploadCurrentMatrices();
            GameOverScreen.s_coinsBundle.Set();
            Mesh.DrawQuad(Color.White);
            GameOverScreen.s_coinsBundle.UnSet();
          }
          vector3.Y += 45f;
          Game.game_work.pNumberFontSilver.DrawString(this.m_coinsEarnedText, vector3,
              Color.White, 20f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
          vector3.Y -= 83f;
          string stringToDraw = string.Format("{0}", (object) Game.game_work.coins);
          Game.game_work.pNumberFontSilver.DrawString(stringToDraw, vector3, Color.White, 34f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        }
        else if (!Game.USE_ZEN_GO_SCREEN)
        {
          if (this.m_body > 0)
          {
            GameOverScreen.m_senseiBody[this.m_body - 1].Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(257f, 257f, 0.0f));
            MatrixManager.GetInstance().Translate(this.m_senseiPos);
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            GameOverScreen.m_senseiBody[this.m_body - 1].UnSet();
          }
          if (this.m_head > 0)
          {
            GameOverScreen.m_senseiHeads[this.m_head - 1].Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(129f, 129f, 0.0f));
            MatrixManager.GetInstance().Translate(Vector3.Add(this.m_senseiPos, GameOverScreen.SENSEI_HEAD_OFFSET));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            GameOverScreen.m_senseiHeads[this.m_head - 1].UnSet();
          }
        }
        if (this.m_retry == null || Game.game_work.saveData.highScore <= 0)
          return;
        string stringToDraw1 = string.Format("{0}", (object) Game.game_work.saveData.highScore);
        Game.game_work.pNumberFont.DrawString(stringToDraw1, new Vector3((float) (77.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 64.0), 0.0f), Color.White, this.m_retry.m_scale.X * 0.5f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        if (this.m_oldHighScoreTexture == null || (double) this.m_retry.m_scale.X <= 0.0 || (double) this.m_retry.m_scale.X >= 600.0)
          return;
        this.m_oldHighScoreTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(this.m_retry.m_scale);
        MatrixManager.GetInstance().Translate(new Vector3((float) (77.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 64.0), 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_oldHighScoreTexture.UnSet();
      }
      else
      {
        if (this.m_drawOrder != HUD.HUD_ORDER.HUD_ORDER_NORMAL)
          return;
        base.Draw(tintChannels);
      }
    }

    public override void Draw(float[] tintChannels)
    {
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_GAME_OVER_SCREEN;

    public static void LoadContent()
    {
      if (GameOverScreen.s_isContentLoaded)
        return;
      GameOverScreen.s_gameOverTexture = TextureManager.GetInstance().Load("gameover.tex", true);
      GameOverScreen.s_timeUpTexture = TextureManager.GetInstance().Load("time_up.tex", true);
      GameOverScreen.s_timeUpGreenTexture = TextureManager.GetInstance().Load("arcade_time_up.tex", true);
      GameOverScreen.s_retryTexture = TextureManager.GetInstance().Load("retry.tex", true);
      GameOverScreen.s_leaderboardTexture = TextureManager.GetInstance().Load("leaderboards.tex", true);
      GameOverScreen.s_quitTexture = TextureManager.GetInstance().Load("quit.tex", true);
      for (int index = 0; index < 3; ++index)
      {
        string texture1 = string.Format("textureswp7/sensei_head_0{0}.tex", (object) (index + 1));
        GameOverScreen.m_senseiHeads[index] = TextureManager.GetInstance().Load(texture1);
        string texture2 = string.Format("textureswp7/sensei_body_0{0}.tex", (object) (index + 1));
        GameOverScreen.m_senseiBody[index] = TextureManager.GetInstance().Load(texture2);
      }
      GameOverScreen.s_isContentLoaded = true;
    }

    public static void UnLoadContent()
    {
      GameOverScreen.s_gameOverTexture = (Texture) null;
      GameOverScreen.s_timeUpTexture = (Texture) null;
      GameOverScreen.s_retryTexture = (Texture) null;
      GameOverScreen.s_leaderboardTexture = (Texture) null;
      GameOverScreen.s_quitTexture = (Texture) null;
      GameOverScreen.s_quitButtonTexture = (Texture) null;
      GameOverScreen.s_timeUpGreenTexture = (Texture) null;
      GameOverScreen.s_leaderboardButtonTexture = (Texture) null;
      GameOverScreen.s_coinsBundle = (Texture) null;
      for (int index = 0; index < 3; ++index)
      {
        GameOverScreen.m_senseiHeads[index] = (Texture) null;
        GameOverScreen.m_senseiBody[index] = (Texture) null;
      }
    }

    public FruitFactControl GetFruitFactControl() => this.m_fruitFact;

    public void SetStateWait()
    {
      int currentScore = Game.game_work.currentScore;
      int total = Game.game_work.saveData.AddToTotal("unrated_games", StringFunctions.StringHash("unrated_games"), 1);
      if (!Game.game_work.saveData.game_rated && total >= 6 && currentScore > 50 && currentScore > Game.GetCurrentModeHighscore() - 10)
        Game.game_work.saveData.game_rated = true;
      else
        this.m_state = 2;
    }

    public void DeletedBonusScreen(HUDControl control)
    {
      if (control != this.m_bonusScreen)
        return;
      this.m_bonusScreen = (BonusScreen) null;
      this.m_state = 2;
    }

    public void SetWeeklyLeaderboardScore(string leaderboardID, int currentScore)
    {
    }

    public enum GOS
    {
      GOS_IN,
      GOS_BONUS_SCREEN,
      GOS_WAIT,
      GOS_OUT,
      GOS_RESTARTING,
      GOS_WAIT_TO_DIE,
      GOS_LEADERBOARDS,
      GOS_DIE,
      GOS_POSTING,
      GOS_CHANGE_MODE,
    }
  }
}
