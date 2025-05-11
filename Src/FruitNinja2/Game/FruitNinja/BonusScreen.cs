
// Type: GameManager.BonusScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class BonusScreen : HUDControl3d
  {
    private int m_lastAwardSound;
    private int m_awardSoundCount;
    private static Vector3 TRANSITION_MOVE = new Vector3(0.0f, Game.SCREEN_HEIGHT * 0.75f, 0.0f);
    private static float TRANSITION_IN_TIME = 0.333f;
    private static float TRANSITION_OUT_TIME = 0.25f;
    private static float FIRST_AWARD = 0.666f;
    private static float TOTAL_TIME = 7f;
    private static float TIME_PER_AWARD = 0.8f;
    private static float WORDS_IN_TIME = 0.0f;
    private static float WORDS_END_TIME = 0.1f;
    private static float NUMBER_IN_TIME = 0.2f;
    private static float NUMBER_END_TIME = 0.3f;
    private static Vector3 TOTAL_POS = new Vector3(40f, -60f, 0.0f);
    private static Vector3 PARTICLE_POS_RIGHT = new Vector3(Game.SCREEN_WIDTH * 0.45f, 0.0f, 0.0f);
    private static Vector3 PARTICLE_POS_LEFT = new Vector3((float) (-(double) Game.SCREEN_WIDTH * 0.44999998807907104), 0.0f, 0.0f);
    private static float AWARD_Y_DIF = -42f;
    private static float POINTS_OFFSET_X = 250f;
    private static Vector3 FIRST_NAME_OFFSET = new Vector3(-105f, 40f, 0.0f);
    protected int m_currentTotal;
    protected int m_total;
    protected List<BonusAwardHud> m_awards = new List<BonusAwardHud>();
    protected float m_shakeRadius;
    protected float m_shakeTime;
    protected float m_shakeTimeTotal;
    protected ushort m_shakeDir;
    protected Vector3 m_shake;
    protected float m_totalScale;
    protected bool m_hasPostedScore;
    public float m_time;
    public MortarSound m_drumRollSfx;
    public Vector3 m_offset;
    private static int oneInThree = 0;
    public static bool bomb_magnet = false;

    public void Shake(float time, float radius)
    {
      this.m_shakeTimeTotal = this.m_shakeTime = time;
      this.m_shakeRadius = radius;
      this.m_shakeDir = (ushort) Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(359f));
    }

    public static void LoadContent()
    {
    }

    public static void UnLoadContent()
    {
    }

    private static void AddToScoreOnArrival(Coin coin)
    {
      if (BonusScreen.oneInThree < 3)
        BonusScreen.oneInThree = 3;
      ++BonusScreen.oneInThree;
      if (BonusScreen.oneInThree == 3 || BonusScreen.oneInThree == 6 || BonusScreen.oneInThree >= 9)
      {
        Vector3 origin= new Vector3((float)(-(double) Game.SCREEN_WIDTH / 2.0 + 20.0),
            (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 20.0), 1f);
        Game.game_work.camera.CreateCameraShake(origin, (float) (((double) BonusScreen.WORDS_END_TIME - (double) BonusScreen.WORDS_IN_TIME) * 1.5), 0.75f);
        SoundManager.GetInstance().SFXPlay("Bonus-Firework-Explode");
        PSPParticleEmitter pspParticleEmitter1 = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("bonus_mode_fx_red"), (Action<PSPParticleEmitter>) null);
        if (pspParticleEmitter1 != null)
          pspParticleEmitter1.pos = origin;
        PSPParticleEmitter pspParticleEmitter2 = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("arcade_confetti"), (Action<PSPParticleEmitter>) null);
        if (pspParticleEmitter2 != null)
        {
          PSPParticleEmitter pspParticleEmitter3 = pspParticleEmitter2;
          double num1 = (double) Mortar.Math.g_random.RandF(Game.SCREEN_WIDTH * 0.05f) - (double) Game.SCREEN_WIDTH * 0.02500000037252903;
          double num2 = (double) Game.SCREEN_WIDTH * 0.11999999731779099;
          double num3;
          switch (BonusScreen.oneInThree)
          {
            case 3:
              num3 = 0.0;
              break;
            case 6:
              num3 = 1.0;
              break;
            default:
              num3 = -1.0;
              break;
          }
          double num4 = num2 * num3;
          Vector3 vector3 = new Vector3((float) (num1 + num4), (float) ((double) Game.SCREEN_HEIGHT / 2.0 + (double) Mortar.Math.g_random.RandF(10f) + 3.0), 0.0f);
          pspParticleEmitter3.pos = vector3;
        }
        if (BonusScreen.oneInThree >= 9)
          BonusScreen.oneInThree = 0;
      }
      SoundManager.GetInstance().SFXPlay("Bonus-Point-Get");
      Game.AddToCurrentScore(coin.GetWorth());
    }

    public BonusScreen()
    {
      this.m_texture = TextureManager.GetInstance().Load("arcade_diolog_box.tex", true);
      this.m_scale = Vector3.Multiply(new Vector3((float) (this.m_texture.GetWidth() / 2U), (float) (this.m_texture.GetHeight() / 2U), 0.0f), Game.GAME_MODE_SCALE_FIX);
      this.m_pos = Vector3.Zero;
      this.m_offset = Vector3.Zero;
      this.m_time = -BonusScreen.TRANSITION_IN_TIME;
      this.m_total = 0;
      this.m_currentTotal = 0;
      this.m_awards.Clear();
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
      this.m_shakeTime = 0.0f;
      this.m_shakeTimeTotal = 1f;
      this.m_shakeRadius = 0.0f;
      this.m_shakeDir = (ushort) 0;
      this.m_shake = Vector3.Zero;
      this.m_totalScale = 1f;
      this.m_hasPostedScore = false;
      this.m_lastAwardSound = 0;
      this.m_awardSoundCount = 0;
    }

    public override void Reset()
    {
    }

    private void SET_DEFINES()
    {
      BonusScreen.TRANSITION_MOVE = new Vector3(0.0f, Game.SCREEN_HEIGHT * 0.75f, 0.0f);
      BonusScreen.TRANSITION_IN_TIME = 0.333f;
      BonusScreen.TRANSITION_OUT_TIME = 0.25f;
      BonusScreen.FIRST_AWARD = 0.666f;
      BonusScreen.TIME_PER_AWARD = 1.2f;
      BonusScreen.TOTAL_TIME = 7f;
      BonusScreen.AWARD_Y_DIF = -42f;
      BonusScreen.TOTAL_POS = new Vector3(50f, -88f, 0.0f);
      BonusScreen.PARTICLE_POS_RIGHT = new Vector3(Game.SCREEN_WIDTH * 0.35f, 0.0f, 0.0f);
      BonusScreen.PARTICLE_POS_LEFT = new Vector3((float) (-(double) Game.SCREEN_WIDTH * 0.34999999403953552), 0.0f, 0.0f);
    }

    public override void Update(float dt)
    {
      this.SET_DEFINES();
      Game.game_work.canFastForward = true;
      float num1 = this.m_time - dt;
      float num2 = BonusScreen.FIRST_AWARD + BonusScreen.TIME_PER_AWARD * ((float) this.m_awards.Count + 0.25f);
      if (this.m_drumRollSfx == null && (double) this.m_time > 0.0 && (double) this.m_time < (double) num2)
      {
        this.m_drumRollSfx = SoundManager.CreateNewSound();
        if (Game.game_work.soundEnabled)
        {
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_BONUS_DRUM_ROLL, 0U, this.m_drumRollSfx);
          if (this.m_drumRollSfx != null)
            this.m_drumRollSfx.SetVolume(0.0f);
        }
      }
      if ((double) this.m_time > (double) BonusScreen.TOTAL_TIME + (double) BonusScreen.TRANSITION_OUT_TIME)
      {
        this.m_terminate = true;
        Game.game_work.inBonusScreen = false;
      }
      if ((double) this.m_time > (double) BonusScreen.TOTAL_TIME)
      {
        float num3 = (this.m_time - BonusScreen.TOTAL_TIME) / BonusScreen.TRANSITION_OUT_TIME;
        for (int index = 0; index < 3; ++index)
          Game.game_work.hud.m_backTint[index] += (float) ((0.5 - (double) Game.game_work.hud.m_backTint[index]) * (1.0 - (double) num3));
        float num4 = num3 * num3;
        this.m_offset = Vector3.Multiply(BonusScreen.TRANSITION_MOVE, num4);
      }
      else if ((double) this.m_time < 0.0)
      {
        float num5 = 1f - Mortar.Math.ABS(this.m_time / BonusScreen.TRANSITION_IN_TIME);
        for (int index = 0; index < 3; ++index)
          Game.game_work.hud.m_backTint[index] += (0.5f - Game.game_work.hud.m_backTint[index]) * num5;
        this.m_offset = Vector3.Multiply(Vector3.Negate(BonusScreen.TRANSITION_MOVE), (float) (1.0 - (double) Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(100f * num5)) / (double) Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(100f))));
      }
      else
      {
        if ((double) this.m_time >= (double) num2 && this.m_drumRollSfx != null)
        {
          SoundManager.GetInstance().Release(this.m_drumRollSfx);
          this.m_drumRollSfx = (MortarSound) null;
        }
        this.m_offset = Vector3.Zero;
        for (int index = 0; index < 3; ++index)
          Game.game_work.hud.m_backTint[index] = 0.5f;
      }
      this.m_currentTotal = 0;
      Vector3 vector3_1 = Vector3.Add(Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.FIRST_NAME_OFFSET), Vector3.Multiply(Vector3.UnitX, BonusScreen.POINTS_OFFSET_X));
      float v = (float) (0.5 + (double) Mortar.Math.CLAMP(this.m_time / (BonusScreen.FIRST_AWARD + 2f * BonusScreen.TIME_PER_AWARD), 0.0f, 1f) * 0.66600000858306885);
      for (int index = 0; index < this.m_awards.Count; ++index)
      {
        BonusAwardHud award = this.m_awards[index];
        if ((double) this.m_time - (double) BonusScreen.FIRST_AWARD < (double) index * (double) BonusScreen.TIME_PER_AWARD)
        {
          award.colour.A = (byte) 0;
          award.visiblePoints = 0;
          award.numberScale = 0.0f;
        }
        else if ((double) this.m_time - (double) BonusScreen.FIRST_AWARD < (double) index * (double) BonusScreen.TIME_PER_AWARD + (double) BonusScreen.TIME_PER_AWARD)
        {
          float num6 = (this.m_time - BonusScreen.FIRST_AWARD) % BonusScreen.TIME_PER_AWARD;
          float num7 = num6 - dt;
          float num8 = Mortar.Math.CLAMP((float) (((double) num6 - (double) BonusScreen.WORDS_IN_TIME) / ((double) BonusScreen.WORDS_END_TIME - (double) BonusScreen.WORDS_IN_TIME)), 0.0f, 1f);
          award.colour.A = (byte) ((double) num8 * (double) byte.MaxValue);
          if ((double) num7 - (double) BonusScreen.WORDS_IN_TIME <= 0.0)
            ;
          float num9 = Mortar.Math.CLAMP((float) (((double) num6 - (double) BonusScreen.NUMBER_IN_TIME) / ((double) BonusScreen.NUMBER_END_TIME - (double) BonusScreen.NUMBER_IN_TIME)), 0.0f, 1f);
          if ((double) num7 - (double) BonusScreen.NUMBER_IN_TIME <= 0.0 && (double) num9 > 0.0)
          {
            PSPParticleEmitter pspParticleEmitter1 = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("bonus_mode_fx_red"), (Action<PSPParticleEmitter>) null);
            if (pspParticleEmitter1 != null)
              pspParticleEmitter1.pos = new Vector3((float) (-(double) Game.SCREEN_WIDTH * 0.31000000238418579 * (index % 2 != 0 ? 1.0 : -1.0)), vector3_1.Y, 0.0f);
            PSPParticleEmitter pspParticleEmitter2 = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("bonus_mode_fx_blue"), (Action<PSPParticleEmitter>) null);
            if (pspParticleEmitter2 != null)
              pspParticleEmitter2.pos = new Vector3((float) ((double) Game.SCREEN_WIDTH * 0.31000000238418579 * (index % 2 != 0 ? 1.0 : -1.0)), vector3_1.Y, 0.0f);
            PSPParticleEmitter pspParticleEmitter3 = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("impact_fx"), (Action<PSPParticleEmitter>) null);
            if (pspParticleEmitter3 != null)
              pspParticleEmitter3.pos = vector3_1;
            this.Shake(BonusScreen.NUMBER_END_TIME - BonusScreen.NUMBER_IN_TIME, 10f);
            int num10 = award.points > 15 ? (award.points > 25 ? 5 : 3) : 1;
            if (num10 == this.m_lastAwardSound)
            {
              ++this.m_awardSoundCount;
              this.m_lastAwardSound = num10;
              int num11 = num10 + this.m_awardSoundCount;
            }
            else
            {
              this.m_awardSoundCount = 0;
              this.m_lastAwardSound = num10;
            }
            string filename = string.Format("Bonus-Explosion-{0}", (object) (index * 2 + 1));
            SoundManager.GetInstance().SFXPlay(filename);
          }
          award.visiblePoints = (double) num9 <= 0.0 ? 0 : (int) ((double) (award.points * award.multiplyer) * ((double) num9 * 0.5 + 0.5));
          award.numberScale = Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(120f * num9)) / Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(120f));
        }
        else
        {
          award.colour.A = byte.MaxValue;
          award.visiblePoints = award.points * award.multiplyer;
          award.numberScale = 1f;
        }
        vector3_1.Y += BonusScreen.AWARD_Y_DIF;
        this.m_currentTotal += award.visiblePoints;
      }
      if (this.m_drumRollSfx != null && Game.game_work.soundEnabled)
      {
        this.m_drumRollSfx.Repeat();
        this.m_drumRollSfx.SetVolume(v);
      }
      if ((double) this.m_time > (double) num2)
      {
        if ((double) num1 <= (double) num2)
        {
          Vector3 vector3_2 = new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + 20.0), 
              (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 20.0), 0.0f);
         
          if (this.m_total >= 5)
          {
            Coin.MakeCoins(5, this.m_total, Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.TOTAL_POS), (ushort) 0, Mortar.Math.DEGREE_TO_IDX(359f), new Vector3?(vector3_2), -0.05f, -0.5f, "bonus_star_trail", "bonus_star_impact", new Coin.CoinArrivedCallback(BonusScreen.AddToScoreOnArrival), false);
            Coin.MakeCoins(this.m_total - 5, 5, Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.TOTAL_POS), (ushort) 0, Mortar.Math.DEGREE_TO_IDX(359f), new Vector3?(vector3_2), -0.05f, -0.3f, "bonus_star_trail", (string) null, new Coin.CoinArrivedCallback(BonusScreen.AddToScoreOnArrival), false);
          }
          else
            Coin.MakeCoins(this.m_total, 5, Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.TOTAL_POS), (ushort) 0, Mortar.Math.DEGREE_TO_IDX(359f), new Vector3?(vector3_2), -0.05f, -0.3f, "bonus_star_trail", "bonus_star_impact", new Coin.CoinArrivedCallback(BonusScreen.AddToScoreOnArrival), false);
          Game.game_work.camera.CreateCameraShake(Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.TOTAL_POS), (float) (((double) BonusScreen.WORDS_END_TIME - (double) BonusScreen.WORDS_IN_TIME) * 2.0));
          PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash("impact_fx"), (Action<PSPParticleEmitter>) null);
          if (pspParticleEmitter != null)
            pspParticleEmitter.pos = Vector3.Add(Vector3.Add(Vector3.Add(this.m_pos, this.m_offset), this.m_shake), BonusScreen.TOTAL_POS);
          int num12 = Game.game_work.currentScore + this.m_total;
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_FAN_FARE);
          BonusScreen.oneInThree = 3;
          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE && num12 < 20)
            AchievementManager.AwardAchievementWP7("Underachiever");
          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE && num12 > 400)
            AchievementManager.AwardAchievementWP7("Overachiever");
          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE && num12 > 250 && BonusScreen.bomb_magnet)
            AchievementManager.AwardAchievementWP7("Bomb Magnet");
        }
        float num13 = Mortar.Math.CLAMP((float) (((double) this.m_time - (double) num2) / 0.20000000298023224), 0.0f, 1f);
        this.m_currentTotal = (int) ((double) this.m_total * ((double) num13 * 0.5 + 0.5));
        this.m_totalScale = Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(num13 * 115f)) / Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(115f));
      }
      else
      {
        this.m_currentTotal = 0;
        this.m_totalScale = 0.0f;
      }
      if ((double) this.m_shakeTime > 0.0)
      {
        this.m_shakeTime -= dt;
        float num14 = this.m_shakeRadius * this.m_shakeTime / this.m_shakeTimeTotal;
        Vector3 vector3_3 = new Vector3(Mortar.Math.SinIdx(this.m_shakeDir) * num14, 
            Mortar.Math.CosIdx(this.m_shakeDir) * num14, 0.0f);
        Vector3 vector3_4 = Vector3.Subtract(vector3_3, this.m_shake);
        if ( (double) vector3_4.LengthSquared() < 25.0 )
          this.m_shakeDir += Mortar.Math.DEGREE_TO_IDX((float) (150.0 + 60.0
              * (double) Mortar.Math.g_random.RandF(1f)));

        BonusScreen bonusScreen = this;
        bonusScreen.m_shake = Vector3.Add(bonusScreen.m_shake, 
            Vector3.Multiply(vector3_4, 0.2f));
      }
      else
        this.m_shake = Vector3.Zero;
    }

    public override void Draw(float[] tintChannels)
    {
      Vector3 pos = this.m_pos;
      BonusScreen bonusScreen1 = this;
      bonusScreen1.m_pos = Vector3.Add(bonusScreen1.m_pos, Vector3.Add(
          this.m_offset, this.m_shake));
      base.Draw(tintChannels);
      string stringToDraw1 = string.Format("{0}", (object) this.m_currentTotal);
      Game.game_work.pNumberFontBlue2.DrawString(stringToDraw1, Vector3.Add(this.m_pos, 
          BonusScreen.TOTAL_POS), Color.White, (float) (26.0 + 14.0 * (this.m_currentTotal > 0 ? (double) this.m_currentTotal / (double) this.m_total : 0.0)) * this.m_totalScale, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
      BonusScreen bonusScreen2 = this;
      bonusScreen2.m_pos = Vector3.Add(bonusScreen2.m_pos, BonusScreen.FIRST_NAME_OFFSET);
      for (int index = 0; index < this.m_awards.Count; ++index)
      {
        BonusAwardHud award = this.m_awards[index];
        if ((double) this.m_time - (double) BonusScreen.FIRST_AWARD >= (double) index * (double) BonusScreen.TIME_PER_AWARD)
        {
          if (award.texture != null)
          {
            award.texture.Set();
            MatrixManager.GetInstance().SetMatrix(Matrix.Identity);
            MatrixManager.GetInstance().Scale(new Vector3((float) ((double) award.texture.GetWidth() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) award.texture.GetHeight() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 0.0f));
            MatrixManager.GetInstance().Translate(Vector3.Subtract(this.m_pos, Vector3.Multiply(Vector3.UnitX, 35f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(award.colour);
            award.texture.UnSet();
          }
          Game.game_work.pGameFont.DrawString(award.text, this.m_pos, award.colour, 16f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_LEFT);
          string stringToDraw2 = string.Format("{0}", (object) award.visiblePoints);
          Game.game_work.pGameFont.DrawString(stringToDraw2, Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitX, BonusScreen.POINTS_OFFSET_X)), award.colour, 24f * award.numberScale, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
          this.m_pos.Y += BonusScreen.AWARD_Y_DIF;
        }
        else
          break;
      }
      this.m_pos = pos;
    }

    private Vector3 ConvertPos(Vector3 pos)
    {
      Vector3 zero = Vector3.Zero;
      pos.X -= Game.game_work.camera.m_cameraShake.X;
      pos.Y -= Game.game_work.camera.m_cameraShake.Y;
      Vector3 vector3 = pos;
      vector3.X += 240f;
      vector3.Y += 160f;
      vector3.Y = 320f - vector3.Y;
      return Vector3.Multiply(vector3, 1.6f);
    }

    public void AddAward(Color colour, Texture texture, string text, int points)
    {
      if (string.Compare(Mortar.Game1.instance.stringTable.GetString(616), text) == 0)
                BonusScreen.bomb_magnet = true;
      BonusAwardHud bonusAwardHud = new BonusAwardHud();
      if (text != null)
        bonusAwardHud.text = text;
      bonusAwardHud.texture = texture;
      bonusAwardHud.points = points;
      this.m_total += points;
      bonusAwardHud.visiblePoints = 0;
      bonusAwardHud.colour = colour;
      bonusAwardHud.numberColour = colour;
      bonusAwardHud.numberScale = 0.0f;
      this.m_awards.Add(bonusAwardHud);
    }
  }
}
