
// Type: GameManager.ScoreMultiplyerBoard
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace GameManager
{
  public class ScoreMultiplyerBoard : HUDControl3d
  {
    public Vector3 m_moveFrom;
    public PowerUp m_power;
    public int m_score;
    public int m_finalScore;
    public float m_time;
    public float m_textScaleAmt;

    public static float MOVE_DOWN_TIME => 0.3f;

    public static float START_WAIT_TIME => 0.1f;

    public static float SCALE_DOWN_TIME => 0.2f;

    public static float SCALE_UP_TIME => 0.2f;

    public static float SCALE_WAIT_TIME => 0.3f;

    public static float MOVE_LEFT_TIME => 0.25f;

    public static Vector3 MOVE_DOWN_AMOUNT => new Vector3(0.0f, -70f, 0.0f);

    public static Vector3 MOVE_LEFT_AMOUNT => new Vector3(-150f, 0.0f, 0.0f);

    public static float MOVE_DOWN_TIME_START => 0.0f;

    public static float START_WAIT_TIME_START
    {
      get => ScoreMultiplyerBoard.MOVE_DOWN_TIME_START + ScoreMultiplyerBoard.START_WAIT_TIME;
    }

    public static float SCALE_DOWN_TIME_START
    {
      get => ScoreMultiplyerBoard.START_WAIT_TIME_START + ScoreMultiplyerBoard.SCALE_DOWN_TIME;
    }

    public static float SCALE_UP_TIME_START
    {
      get => ScoreMultiplyerBoard.SCALE_DOWN_TIME_START + ScoreMultiplyerBoard.SCALE_UP_TIME;
    }

    public static float SCALE_WAIT_TIME_START
    {
      get => ScoreMultiplyerBoard.SCALE_UP_TIME_START + ScoreMultiplyerBoard.SCALE_WAIT_TIME;
    }

    public static float MOVE_LEFT_TIME_START
    {
      get => ScoreMultiplyerBoard.SCALE_WAIT_TIME_START + ScoreMultiplyerBoard.MOVE_LEFT_TIME;
    }

    public static float MOVE_DOWN_TIME_TOTAL => ScoreMultiplyerBoard.MOVE_DOWN_TIME;

    public static float START_WAIT_TIME_TOTAL
    {
      get => ScoreMultiplyerBoard.MOVE_DOWN_TIME_TOTAL + ScoreMultiplyerBoard.START_WAIT_TIME;
    }

    public static float SCALE_DOWN_TIME_TOTAL
    {
      get => ScoreMultiplyerBoard.START_WAIT_TIME_TOTAL + ScoreMultiplyerBoard.SCALE_DOWN_TIME;
    }

    public static float SCALE_UP_TIME_TOTAL
    {
      get => ScoreMultiplyerBoard.SCALE_DOWN_TIME_TOTAL + ScoreMultiplyerBoard.SCALE_UP_TIME;
    }

    public static float SCALE_WAIT_TIME_TOTAL
    {
      get => ScoreMultiplyerBoard.SCALE_UP_TIME_TOTAL + ScoreMultiplyerBoard.SCALE_WAIT_TIME;
    }

    public static float MOVE_LEFT_TIME_TOTAL
    {
      get => ScoreMultiplyerBoard.SCALE_WAIT_TIME_TOTAL + ScoreMultiplyerBoard.MOVE_LEFT_TIME;
    }

    public static float ANIMATION_TIME_TOTAL => ScoreMultiplyerBoard.MOVE_LEFT_TIME_TOTAL;

    public static float MOVE_DOWN_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.MOVE_DOWN_TIME_START) / ScoreMultiplyerBoard.MOVE_DOWN_TIME, 0.0f, 1f);
    }

    public static float START_WAIT_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.START_WAIT_TIME_START) / ScoreMultiplyerBoard.START_WAIT_TIME, 0.0f, 1f);
    }

    public static float SCALE_DOWN_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.SCALE_DOWN_TIME_START) / ScoreMultiplyerBoard.SCALE_DOWN_TIME, 0.0f, 1f);
    }

    public static float SCALE_UP_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.SCALE_UP_TIME_START) / ScoreMultiplyerBoard.SCALE_UP_TIME, 0.0f, 1f);
    }

    public static float SCALE_WAIT_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.SCALE_WAIT_TIME_START) / ScoreMultiplyerBoard.SCALE_WAIT_TIME, 0.0f, 1f);
    }

    public static float MOVE_LEFT_PROG(float t)
    {
      return Mortar.Math.CLAMP((t - ScoreMultiplyerBoard.MOVE_LEFT_TIME_START) / ScoreMultiplyerBoard.MOVE_LEFT_TIME, 0.0f, 1f);
    }

    private int AddScoreNomals(int score) => score;

    public ScoreMultiplyerBoard()
    {
      this.m_score = 0;
      this.m_finalScore = -1;
      this.m_power = (PowerUp) null;
      this.m_time = 0.0f;
      this.m_textScaleAmt = 1f;
      this.m_moveFrom = Vector3.Zero;
    }

    public override void Reset()
    {
      this.m_score = 0;
      this.m_finalScore = 0;
      this.m_terminate = true;
    }

    public override void Update(float dt)
    {
      if (this.m_power != null)
      {
        this.m_score = this.m_power.GetDeferedPoints();
      }
      else
      {
        if (Game.game_work.pause)
          return;
        float time = this.m_time;
        this.m_time += dt;
        float num1 = 1f - ScoreMultiplyerBoard.MOVE_DOWN_PROG(this.m_time);
        float num2 = 1f - num1 * num1;
        if (this.m_finalScore == 0)
        {
          this.m_pos = Vector3.Subtract(this.m_moveFrom, Vector3.Multiply(ScoreMultiplyerBoard.MOVE_DOWN_AMOUNT, num2));
          if ((double) num2 <= 0.99000000953674316)
            return;
          this.m_terminate = true;
        }
        else
        {
          float num3 = ScoreMultiplyerBoard.MOVE_LEFT_PROG(this.m_time);
          float num4 = num3 * num3;
          this.m_pos = Vector3.Add(Vector3.Add(this.m_moveFrom, Vector3.Multiply(ScoreMultiplyerBoard.MOVE_DOWN_AMOUNT, num2)), Vector3.Multiply(ScoreMultiplyerBoard.MOVE_LEFT_AMOUNT, num4));
          if ((double) this.m_time < (double) ScoreMultiplyerBoard.SCALE_UP_TIME_START)
          {
            this.m_textScaleAmt = 1f - ScoreMultiplyerBoard.SCALE_DOWN_PROG(this.m_time);
            this.m_textScaleAmt = Mortar.Math.SinIdx((ushort) ((double) this.m_textScaleAmt * (double) Mortar.Math.DEGREE_TO_IDX(90f)));
          }
          else
          {
            if ((double) time < (double) ScoreMultiplyerBoard.SCALE_UP_TIME_START)
            {
              PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("bonus_star_impact"), (Action<PSPParticleEmitter>) null);
              if (pspParticleEmitter != null)
                pspParticleEmitter.pos = Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitY, 10f));
            }
            this.m_textScaleAmt = ScoreMultiplyerBoard.SCALE_UP_PROG(this.m_time);
            this.m_textScaleAmt = (float) ((double) Mortar.Math.SinIdx((ushort) ((double) this.m_textScaleAmt * (double) Mortar.Math.DEGREE_TO_IDX(113f))) / (double) Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(113f)) * 1.3500000238418579);
          }
          if ((double) this.m_time > (double) ScoreMultiplyerBoard.SCALE_WAIT_TIME_START && this.m_score > 0)
          {
            Game.SetScoreDelegate(new Game.ScoreDelegate(this.AddScoreNomals));
            Game.AddToCurrentScore(this.m_finalScore);
            PowerUpManager.GetInstance().SetAppropriateScoreCallback();
            this.m_score = 0;
          }
          if ((double) this.m_time <= (double) ScoreMultiplyerBoard.ANIMATION_TIME_TOTAL)
            return;
          this.m_terminate = true;
        }
      }
    }

    public override void Draw(float[] tintChannels)
    {
      float[] tintChannels1 = new float[3]{ 1f, 1f, 1f };
      this.m_color = Color.White;
      base.Draw(tintChannels1);
      Font font = Game.game_work.pNumberFont;
      string stringToDraw;
      if ((double) this.m_time < (double) ScoreMultiplyerBoard.SCALE_UP_TIME_START)
      {
        this.m_color = new Color(20, 150, 20);
        stringToDraw = string.Format("{0}", (object) this.m_score);
      }
      else
      {
        font = Game.game_work.pNumberFontBlue2;
        stringToDraw = string.Format("{0}", (object) this.m_finalScore);
      }
      font.DrawString(stringToDraw, Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitY, 10f)), this.m_color, 35f * this.m_textScaleAmt, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
    }

    public override void Save()
    {
      if (this.m_power != null || this.m_score <= 0)
        return;
      Game.SetScoreDelegate(new Game.ScoreDelegate(this.AddScoreNomals));
      Game.AddToCurrentScore(this.m_finalScore);
      PowerUpManager.GetInstance().SetAppropriateScoreCallback();
      this.m_score = 0;
    }
  }
}
