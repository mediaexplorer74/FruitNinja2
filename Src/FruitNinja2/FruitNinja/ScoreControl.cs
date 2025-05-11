
// Type: FruitNinja2.ScoreControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  internal class ScoreControl : HUDControl3d
  {
    private bool firstRun;
    public ushort m_pulseTimer;
    public float m_scoreWait;
    public int m_currentScore;
    public int m_highScore;
    public float m_gameOverTransition;
    public float m_overAllScale;
    public Vector3 m_textPos;
    public Texture m_scroreTexture;
    public Texture m_highScoreTexture;
    public float m_highscoreTime;
    public ushort m_highscorePulse;
    public int m_multiplyer;
    public int m_multiplyerType;
    public float[] m_multiplyerScales = new float[ScoreControl.MAX_MULTIPLYERS];
    public Texture m_iconTexture;
    private static float disappearWait;
    private static int cycle;

    public static int MAX_MULTIPLYERS => 16;

    public static float SCALE_UP_TIME => 0.05f;

    public static float SCALE_DOWN_TIME => 2.55f;

    public static float FULL_PULSE => 32768f;

    public static float HALF_PULSE => 16384f;

    public static float SCALE_UP_RATE => ScoreControl.HALF_PULSE / ScoreControl.SCALE_UP_TIME;

    public static float SCALE_DOWN_RATE => ScoreControl.HALF_PULSE / ScoreControl.SCALE_DOWN_TIME;

    public static float MIN_SCALE => 40f * Game.HUD_SCALE;

    public static float MAX_SCALE => 50f * Game.HUD_SCALE;

    public static float GAME_SCREEN_X_IN => 2f * Game.HUD_SCALE;

    public static float GAME_SCREEN_Y_IN => 2f * Game.HUD_SCALE;

    public static Vector3 IN_GAME_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + (double) ScoreControl.MIN_SCALE / 2.0) + ScoreControl.GAME_SCREEN_X_IN, (float) ((double) Game.SCREEN_HEIGHT / 2.0 - (double) ScoreControl.MIN_SCALE / 2.0) - ScoreControl.GAME_SCREEN_Y_IN, 0.0f);
      }
    }

    public static float GAMEOVER_SCREEN_X_IN => 80f;

    public static float GAMEOVER_SCREEN_Y_IN => !Game.USE_ARCADE_GO_SCREEN ? 80f : 240f;

    public static Vector3 GAMEOVER_GAME_POS(float len)
    {
      return new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0) + ScoreControl.GAMEOVER_SCREEN_X_IN - len, 160f - ScoreControl.GAMEOVER_SCREEN_Y_IN, 0.0f);
    }

    public static float FONT_SIZE => 48f;

    public static float GAME_OVER_SCALE => (Game.USE_ARCADE_GO_SCREEN ? 1.5f : 2f) * Game.HUD_SCALE;

    public static float NEW_HIGHSCORE_FLASH => 1000f;

    public static float COMBO_SCALE_UP_RATE => 6f;

    public static float COMBO_SCALE_DOWN_RATE => 16f;

    public ScoreControl()
    {
      this.m_rotation = 0.0f;
      this.m_gameOverTransition = -1f;
      this.m_overAllScale = 1f;
      this.m_highscoreTime = -2f;
      this.m_scoreWait = 0.0f;
      this.m_highScore = 0;
      this.m_iconTexture = TextureManager.GetInstance().Load("textureswp7/hud_fruit.tex");
      this.Reset();
    }

    public override void Reset()
    {
      this.m_texture = this.m_iconTexture;
      this.firstRun = true;
      this.m_pulseTimer = (ushort) 0;
      this.m_scale = Vector3.Multiply(Vector3.One, ScoreControl.MIN_SCALE);
      for (int index = 0; index < ScoreControl.MAX_MULTIPLYERS; ++index)
        this.m_multiplyerScales[index] = 0.0f;
      this.m_multiplyerType = 0;
      this.m_multiplyer = 0;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
    }

    public override void Release()
    {
      this.m_iconTexture = (Texture) null;
      this.m_texture = (Texture) null;
      this.m_scroreTexture = (Texture) null;
      this.m_highScoreTexture = (Texture) null;
    }

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      int currentScore1 = Game.game_work.currentScore;
      this.m_multiplyer = Math.MIN(Fruit.s_consecutiveCount - 1, ScoreControl.MAX_MULTIPLYERS - 1);
      if (this.m_multiplyer > 0 && Game.game_work.gameMode == Game.GAME_MODE.GM_CASINO)
      {
        if (this.m_multiplyerType != Fruit.s_consecutiveType)
        {
          for (int index = 0; index < ScoreControl.MAX_MULTIPLYERS; ++index)
          {
            if ((double) this.m_multiplyerScales[index] > 0.0)
            {
              this.m_multiplyerScales[index] -= dt * ScoreControl.COMBO_SCALE_DOWN_RATE;
              if ((double) this.m_multiplyerScales[index] < 0.0)
                this.m_multiplyerScales[index] = 0.0f;
            }
            else
            {
              this.m_multiplyerScales[index] = 0.0f;
              if (index == 0)
              {
                this.m_multiplyerType = Fruit.s_consecutiveType;
                break;
              }
              break;
            }
          }
        }
        else
        {
          for (int index = 0; index < this.m_multiplyer; ++index)
          {
            if ((double) this.m_multiplyerScales[index] < 1.0)
            {
              this.m_multiplyerScales[index] += dt * ScoreControl.COMBO_SCALE_UP_RATE;
              if ((double) this.m_multiplyerScales[index] > 1.0)
                this.m_multiplyerScales[index] = 1f;
            }
          }
          ScoreControl.disappearWait = 0.0f;
        }
      }
      else if ((double) ScoreControl.disappearWait < 0.25)
      {
        ScoreControl.disappearWait += dt;
        for (int index = 0; index < ScoreControl.MAX_MULTIPLYERS; ++index)
        {
          if ((double) this.m_multiplyerScales[index] > 1.0 && (double) this.m_multiplyerScales[index] < 1.0)
          {
            this.m_multiplyerScales[index] += dt * ScoreControl.COMBO_SCALE_UP_RATE;
            if ((double) this.m_multiplyerScales[index] > 1.0)
              this.m_multiplyerScales[index] = 1f;
          }
        }
      }
      else
      {
        for (int index = 0; index < ScoreControl.MAX_MULTIPLYERS; ++index)
        {
          if ((double) this.m_multiplyerScales[index] > 0.0)
          {
            this.m_multiplyerScales[index] -= dt * ScoreControl.COMBO_SCALE_DOWN_RATE;
            if ((double) this.m_multiplyerScales[index] < 0.0)
              this.m_multiplyerScales[index] = 0.0f;
          }
          else
          {
            this.m_multiplyerScales[index] = 0.0f;
            break;
          }
        }
      }
      if (this.firstRun)
      {
        this.m_scoreWait = (float) currentScore1;
        this.m_currentScore = currentScore1;
        this.firstRun = false;
      }
      int currentScore2 = this.m_currentScore;
      this.m_scoreWait += Math.MIN((float) (((double) currentScore1 + 0.60000002384185791 - (double) this.m_scoreWait) * 0.10000000149011612), (float) (0.30000001192092896 * (double) Game.GetScoreMultiplyer() * (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE ? 10.0 : 1.0)));
      this.m_currentScore = (int) this.m_scoreWait;
      if (this.m_currentScore > currentScore2)
        this.m_pulseTimer = (ushort) ScoreControl.FULL_PULSE;
      if ((double) this.m_pulseTimer > (double) ScoreControl.HALF_PULSE)
        this.m_pulseTimer -= (ushort) ((double) dt * (double) ScoreControl.SCALE_UP_RATE);
      else if (this.m_pulseTimer > (ushort) 0)
      {
        this.m_pulseTimer -= (ushort) ((double) dt * (double) ScoreControl.SCALE_UP_RATE);
        if ((double) this.m_pulseTimer > (double) ScoreControl.FULL_PULSE)
          this.m_pulseTimer = (ushort) 0;
      }
      float num = ScoreControl.MIN_SCALE + (ScoreControl.MAX_SCALE - ScoreControl.MIN_SCALE) * Math.SinIdx(this.m_pulseTimer);
      this.m_overAllScale = Math.CLAMP(Game.game_work.gameOverTransition, 0.0f, 1f) * (ScoreControl.GAME_OVER_SCALE - Game.HUD_SCALE) + Game.HUD_SCALE;
      
      if (!Game.game_work.gameOver || currentScore1 == 0)
        this.m_highScore = Game.GetCurrentModeHighscore() != 0 
                    ? Math.MAX(Game.GetCurrentModeHighscore(), this.m_currentScore)
                    : 0;

      this.m_pos = Vector3.Subtract(ScoreControl.IN_GAME_POS,
          Vector3.Multiply(new Vector3(ScoreControl.MAX_SCALE * 2f, 0.0f, 0.0f), 
          Math.Abs(Game.game_work.gameOverTransition)));
      if ((double) Game.game_work.gameOverTransition > 0.0)
      {
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        this.m_textPos = Vector3.Add(ScoreControl.IN_GAME_POS, new Vector3(ScoreControl.MIN_SCALE * 0.6f, 0.0f, 0.0f));
        string stringToDraw = string.Format("{0}", (object) currentScore1);
        float len = (float) ((double) Game.game_work.pNumberFont.MeasureString(stringToDraw) * (double) this.m_overAllScale * (double) ScoreControl.FONT_SIZE * 0.5);
        ScoreControl scoreControl = this;
        scoreControl.m_textPos = Vector3.Add(scoreControl.m_textPos, Vector3.Multiply(Vector3.Subtract(ScoreControl.GAMEOVER_GAME_POS(len), this.m_textPos), Game.game_work.gameOverTransition));
      }
      else
      {
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
        this.m_textPos = Vector3.Add(this.m_pos, new Vector3(ScoreControl.MIN_SCALE * 0.6f, 0.0f, 0.0f));
      }
      if ((double) Game.game_work.gameOverTransition > 0.99900001287460327 && Game.game_work.saveData.go_showHighScore)
      {
        float highscoreTime = this.m_highscoreTime;
        this.m_highscoreTime = Math.MIN(1f, this.m_highscoreTime + dt * 5f);
        if ((double) this.m_highscoreTime == 1.0)
          this.m_highscorePulse += (ushort) ((double) dt * (double) Math.DEGREE_TO_IDX(270f));
        else
          this.m_highscorePulse = (ushort) 0;
        if ((double) this.m_highscoreTime > 0.0 && (double) highscoreTime <= 0.0 && !PopOverControl.IsInPopup)
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_NEW_BEST);
      }
      else
        this.m_highscoreTime = Math.MAX(-1.5f, this.m_highscoreTime - dt * 20f);
      this.m_scale.X = num;
      this.m_scale.Y = num;
    }

    public override void PreDraw(float[] tintChannels)
    {
      string stringToDraw1 = "";
      if ((double) Game.game_work.gameOverTransition > -1.0)
      {
        stringToDraw1 = string.Format("{0}", (object) this.m_currentScore);
        Game.game_work.pNumberFont.DrawString(stringToDraw1, this.m_textPos.X, this.m_textPos.Y, 0.0f, HUDControl.TintWhite(tintChannels), this.m_overAllScale * ScoreControl.FONT_SIZE, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_LEFT);
      }
      switch (Game.game_work.gameMode)
      {
        case Game.GAME_MODE.GM_CASINO:
          this.m_texture = Fruit.FruitInfo(Math.CLAMP(Fruit.s_consecutiveType, 0, Fruit.MAX_FRUIT_TYPES - 1)).icon;
          Color factColor = Fruit.FruitInfo(Math.CLAMP(Fruit.s_consecutiveType, 0, Fruit.MAX_FRUIT_TYPES - 1)).factColor;
          float num1 = (float) ((double) Game.game_work.pNumberFont.MeasureString(stringToDraw1) * (double) this.m_overAllScale * (double) ScoreControl.FONT_SIZE + 5.0);
          for (int index = 0; index < ScoreControl.MAX_MULTIPLYERS; ++index)
          {
            if ((double) this.m_multiplyerScales[index] > 0.0)
            {
              string stringToDraw2 = string.Format("{0}", (object) (1 << index + 1));
              float scale = Math.SinIdx(Math.DEGREE_TO_IDX(this.m_multiplyerScales[index] * 135f)) * (float) ((double) index * 6.0 + 45.0);
              Game.game_work.pGameFont.DrawString(stringToDraw2, this.m_textPos.X + num1, (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 5.0), 0.0f, factColor, scale, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_LEFT);
              num1 += (float) ((double) Game.game_work.pGameFont.MeasureString(stringToDraw2) * (double) scale + 5.0);
            }
          }
          break;
        case Game.GAME_MODE.GM_ARCADE:
          if (PowerUpManager.GetInstance().GetScoreGainMultiplier() > 1 && !Game.game_work.gameOver)
          {
            double num2 = (double) Game.game_work.pNumberFont.MeasureString(stringToDraw1);
            double fontSize = (double) ScoreControl.FONT_SIZE;
            string stringToDraw3 = string.Format("x{0}", (object) PowerUpManager.GetInstance().GetScoreGainMultiplier());
            Game.game_work.pNumberFontBlue2.DrawString(stringToDraw3, this.m_pos.X - ScoreControl.MIN_SCALE * 0.45f, this.m_pos.Y - ScoreControl.MIN_SCALE * 1.3f, 0.0f, HUDControl.TintWhite(tintChannels), (float) ((double) this.m_overAllScale * (double) ScoreControl.FONT_SIZE * 0.75), 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_LEFT);
            break;
          }
          break;
      }
      if ((double) Math.Abs(Game.game_work.gameOverTransition) < 1.0 && this.m_highScore > 0)
      {
        string str1 = string.Format("{0}", (object) this.m_highScore);
        Color color1 = new Color(180, 128, 5, 200);
        if (this.m_highScore == this.m_currentScore)
        {
          ScoreControl.cycle += Game.game_work.pause ? 0 : 6;
          if (ScoreControl.cycle >= 180)
            ScoreControl.cycle = 180;
          float num3 = Math.CosIdx(Math.DEGREE_TO_IDX((float) ScoreControl.cycle)) * -0.5f + 0.5f;
          Color color2 = new Color(100, 150, 25, 200);
          color1.R = (byte) Math.CLAMP((float) color1.R 
              + (float) ((int) color2.R - (int) color1.R) * num3, 0.0f, 254.99f);
          color1.G = (byte) Math.CLAMP((float) color1.G
              + (float) ((int) color2.G - (int) color1.G) * num3, 0.0f, 254.99f);
          color1.B = (byte) Math.CLAMP((float) color1.B 
              + (float) ((int) color2.B - (int) color1.B) * num3, 0.0f, 254.99f);
          color1.A = (byte) Math.CLAMP((float) color1.A
              + (float) ((int) color2.A - (int) color1.A) * num3, 0.0f, 254.99f);
        }
        color1 = HUDControl.TintColor(color1, tintChannels);
        string str2 = TheGame.instance.stringTable.GetString(165) + " " + str1;
        float num4 = 20f * Game.HUD_SCALE;
        float num5 = (float) ((double) Game.game_work.pGameFont.MeasureString(str2) 
                    * (double) num4 - 48.0);
        Game.game_work.pGameFont.DrawString(str2, new Vector3((float) ((double) this.m_pos.X 
            + (double) num5 + (double) ScoreControl.MIN_SCALE * 0.699999988079071), 
            this.m_pos.Y - (float) ((double) Game.HUD_SCALE
            * (double) ScoreControl.FONT_SIZE * 0.60000002384185791), 0.0f), 
            color1, 20f * Game.HUD_SCALE, Vector2.Zero,
            ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT, 0.9f);
      }
      else
        ScoreControl.cycle = 0;
      if ((double) Game.game_work.gameOverTransition <= 0.0)
        return;
      if (Game.USE_ARCADE_GO_SCREEN)
      {
        Color colour = new Color(180, 128, 5, 200);
        string stringToDraw4 = TheGame.instance.stringTable.GetString(165) +
                    " " + (object) this.m_highScore;
        Game.game_work.pGameFont.DrawString(stringToDraw4,
            (float) (-(double) Game.SCREEN_WIDTH / 2.0) + ScoreControl.GAMEOVER_SCREEN_X_IN, 
            (float) (160.0 - (double) ScoreControl.GAMEOVER_SCREEN_Y_IN - 50.0
            - (1.0 - (double) Game.game_work.gameOverTransition) * 200.0), 0.0f, 
            colour, 20f * Game.HUD_SCALE, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_CENTER);
      }
      if (this.m_scroreTexture != null)
      {
        this.m_scroreTexture.Set();
        MatrixManager.GetInstance().Reset();
        if (Game.USE_ARCADE_GO_SCREEN)
        {
          MatrixManager.GetInstance().Scale(
              new Vector3((float) ((double) this.m_scroreTexture.GetWidth() 
              * (double) Game.GAME_MODE_SCALE_FIX * 0.75),
              (float) ((double) this.m_scroreTexture.GetHeight()
              * (double) Game.GAME_MODE_SCALE_FIX * 0.75), 0.0f));

          MatrixManager.GetInstance().Translate(
              new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0) 
              + ScoreControl.GAMEOVER_SCREEN_X_IN, this.m_textPos.Y 
              + ScoreControl.FONT_SIZE * 0.85f, 0.0f));
        }
        else
        {
          MatrixManager.GetInstance().Scale(new Vector3((float) this.m_scroreTexture.GetWidth() * Game.GAME_MODE_SCALE_FIX, (float) this.m_scroreTexture.GetHeight() * Game.GAME_MODE_SCALE_FIX, 0.0f));
          if (Game.IsMultiplayer())
            MatrixManager.GetInstance().Translate(new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 - (double) ScoreControl.GAMEOVER_SCREEN_X_IN + (double) ScoreControl.GAMEOVER_SCREEN_X_IN * 2.0 * (double) Game.game_work.gameOverTransition), (float) ((double) this.m_textPos.Y + (double) ScoreControl.FONT_SIZE + 5.0), 0.0f));
          else
            MatrixManager.GetInstance().Translate(new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0) + ScoreControl.GAMEOVER_SCREEN_X_IN, (float) ((double) this.m_textPos.Y + (double) ScoreControl.FONT_SIZE + 5.0), 0.0f));
        }
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_scroreTexture.UnSet();
      }
      if ((double) this.m_highscoreTime <= 0.0 || this.m_highScoreTexture == null)
        return;
      this.m_highScoreTexture.Set();
      MatrixManager.GetInstance().Reset();
      Matrix mtx;
      Math.Scale44(Vector3.Multiply(Vector3.Divide(Vector3.Multiply(new Vector3((float) ((double) this.m_highScoreTexture.GetWidth() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) this.m_highScoreTexture.GetHeight() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 0.0f), Math.SinIdx((ushort) ((double) Math.DEGREE_TO_IDX(120f) * (double) this.m_highscoreTime))), Math.SinIdx(Math.DEGREE_TO_IDX(120f))), (float) (1.0 + (double) Math.SinIdx(this.m_highscorePulse) * 0.15000000596046448)), out mtx);
      Math.RotZ44(ref mtx, Math.DEGREE_TO_IDX(20f));
      Math.GlobalTranslate44(ref mtx, new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + (double) ScoreControl.GAMEOVER_SCREEN_X_IN + (double) this.m_scroreTexture.GetWidth() * 0.5 * (double) Game.GAME_MODE_SCALE_FIX), this.m_textPos.Y + ScoreControl.FONT_SIZE * 0.6f, 0.0f));
      MatrixManager.GetInstance().SetMatrix(mtx);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
      this.m_highScoreTexture.UnSet();
    }

    public override void Draw(float[] tintChannels)
    {
      if ((double) Game.game_work.gameOverTransition <= -1.0)
        return;
      base.Draw(tintChannels);
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_SCORE;

    public void AddMultipliyer(int score)
    {
    }

    public override void Skip()
    {
      this.m_currentScore = Game.game_work.currentScore;
      if (!Game.game_work.saveData.go_showHighScore)
        return;
      this.m_highscoreTime = 1f;
    }
  }
}
