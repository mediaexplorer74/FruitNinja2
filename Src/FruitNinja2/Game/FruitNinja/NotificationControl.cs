
// Type: GameManager.NotificationControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace GameManager
{
  internal class NotificationControl : HUDControl3d
  {
    public static Texture s_banner;
    public static Texture s_unlockBanner;
    public float m_textScale;
    public float m_time;
    public int m_score;
    public string m_text;
    public string m_numberText;
    public NotificationControl.NotificationType m_type;

    public static float NOTIFICATION_TIME => 1f;

    public static Vector3 NOTIFICATION_START_POS(NotificationControl.NotificationType type)
    {
      return type == NotificationControl.NotificationType.ITEM_UNLOCK ? new Vector3(-95f, (float) ((double) Game.SCREEN_HEIGHT / 2.0 + 32.0), 0.0f) : new Vector3(-95f, (float) ((double) Game.SCREEN_HEIGHT / 2.0 + 24.0), 0.0f);
    }

    public static Vector3 NOTIFICATION_DEST_POS(NotificationControl.NotificationType type)
    {
      return type == NotificationControl.NotificationType.ITEM_UNLOCK ? new Vector3(-95f, (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 32.0), 0.0f) : new Vector3(-95f, (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 13.0), 0.0f);
    }

    public static Vector3 NOTIFICATION_POS_DELTA(NotificationControl.NotificationType type)
    {
      return type == NotificationControl.NotificationType.ITEM_UNLOCK ? new Vector3(0.0f, -64f, 0.0f) : new Vector3(0.0f, -37f, 0.0f);
    }

    public static float NOTIFICATION_IN_TIME => 0.2f;

    public static float NOTIFICATION_WAIT_TIME => 2.5f;

    public static float NOTIFICATION_OUT_TIME => 0.2f;

    public static float NOTIFICATION_IN_TIME_TOTAL => NotificationControl.NOTIFICATION_IN_TIME;

    public static float NOTIFICATION_WAIT_TIME_TOTAL
    {
      get
      {
        return NotificationControl.NOTIFICATION_IN_TIME_TOTAL + NotificationControl.NOTIFICATION_WAIT_TIME;
      }
    }

    public static float NOTIFICATION_OUT_TIME_TOTAL
    {
      get
      {
        return NotificationControl.NOTIFICATION_WAIT_TIME_TOTAL + NotificationControl.NOTIFICATION_OUT_TIME;
      }
    }

    public static float NOTIFICATION_IN_TIME_PROGRESS(float t)
    {
      return t / NotificationControl.NOTIFICATION_IN_TIME;
    }

    public static float NOTIFICATION_WAIT_TIME_PROGRESS(float t)
    {
      return (t - NotificationControl.NOTIFICATION_IN_TIME_TOTAL) / NotificationControl.NOTIFICATION_WAIT_TIME;
    }

    public static float NOTIFICATION_OUT_TIME_PROGRESS(float t)
    {
      return (t - NotificationControl.NOTIFICATION_WAIT_TIME_TOTAL) / NotificationControl.NOTIFICATION_OUT_TIME;
    }

    public NotificationControl(string text, int score)
      : this(text, score, (Texture) null)
    {
    }

    public NotificationControl(string text, int score, Texture texture)
      : this(text, score, texture, NotificationControl.NotificationType.ACHIEVEMENT_UNLOCK)
    {
    }

    public NotificationControl(
      string text,
      int score,
      Texture texture,
      NotificationControl.NotificationType type)
    {
      this.m_type = type;
      this.m_texture = texture;
      this.m_selfCleanUp = false;
      this.m_text = text.ToUpper();
      float num1 = 170f;
      if (score >= 0 && type == NotificationControl.NotificationType.ACHIEVEMENT_UNLOCK)
      {
        this.m_numberText = string.Concat((object) score);
      }
      else
      {
        num1 = 195f;
        this.m_numberText = "";
      }
      this.m_textScale = 16f;
      this.m_type = type;
      if (type == NotificationControl.NotificationType.ITEM_UNLOCK)
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_FAN_FARE);
      float num2 = Game.game_work.pGameFont.MeasureString(this.m_text) * this.m_textScale;
      if ((double) num2 > (double) num1)
        this.m_textScale *= num1 / num2;
      this.m_time = 0.0f;
      this.m_score = score;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      this.m_pos = new Vector3(-996f, -996f, 0.0f);
    }

    public override void Reset()
    {
    }

    public override void Release() => this.m_texture = (Texture) null;

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      float time = this.m_time;
      this.m_time += dt;
      if ((double) this.m_time > (double) NotificationControl.NOTIFICATION_WAIT_TIME_TOTAL)
      {
        if ((double) this.m_time > (double) NotificationControl.NOTIFICATION_OUT_TIME_TOTAL)
        {
          this.m_time = NotificationControl.NOTIFICATION_OUT_TIME_TOTAL;
          this.m_terminate = true;
        }
        float num1 = 1f - NotificationControl.NOTIFICATION_OUT_TIME_PROGRESS(this.m_time);
        float num2 = num1 * num1;
        this.m_pos = Vector3.Add(NotificationControl.NOTIFICATION_START_POS(this.m_type), Vector3.Multiply(NotificationControl.NOTIFICATION_POS_DELTA(this.m_type), num2));
      }
      else if ((double) this.m_time > (double) NotificationControl.NOTIFICATION_IN_TIME_TOTAL)
      {
        this.m_pos = NotificationControl.NOTIFICATION_DEST_POS(this.m_type);
      }
      else
      {
        float num3 = NotificationControl.NOTIFICATION_IN_TIME_PROGRESS(this.m_time);
        float num4 = num3 * num3;
        this.m_pos = Vector3.Add(NotificationControl.NOTIFICATION_START_POS(this.m_type), Vector3.Multiply(NotificationControl.NOTIFICATION_POS_DELTA(this.m_type), num4));
      }
      if (this.m_type != NotificationControl.NotificationType.ITEM_UNLOCK || (int) ((double) time * 8.0) == (int) ((double) this.m_time * 8.0) || (int) ((double) this.m_time * 8.0) >= 4)
        return;
      uint hash = StringFunctions.StringHash("confetti");
      PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(hash, (Action<PSPParticleEmitter>) null);
      if (pspParticleEmitter != null)
      {
        pspParticleEmitter.pos = new Vector3((float) ((double) Mortar.Math.g_random.RandF(20f) - 10.0 - 100.0) + (float) (100 * (int) ((double) time * 8.0)), (float) ((double) Game.SCREEN_HEIGHT / 2.0 - (double) Mortar.Math.g_random.Rand32(5) - 25.0), 0.0f);
        pspParticleEmitter.updateEvenIfPaused = true;
        pspParticleEmitter.sizeScale = 1f;
      }
      Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(20f));
      int idx = (int) Mortar.Math.DEGREE_TO_IDX(10f);
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      switch (this.m_type)
      {
        case NotificationControl.NotificationType.NETWORK_NOTIFICATION:
        case NotificationControl.NotificationType.ACHIEVEMENT_UNLOCK:
          if (NotificationControl.s_banner != null)
          {
            NotificationControl.s_banner.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(257f, 65f, 1f));
            MatrixManager.GetInstance().Translate(new Vector3(0.0f, this.m_pos.Y - 16f, 1f));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, this.m_numberText.Length > 0 ? 0.0f : 0.5f, this.m_numberText.Length > 0 ? 15f / 32f : 31f / 32f);
            NotificationControl.s_banner.UnSet();
          }
          if (this.m_texture != null)
          {
            this.m_texture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(30f, 30f, 30f));
            MatrixManager.GetInstance().Translate(this.m_pos);
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White);
            this.m_texture.UnSet();
          }
          float num1 = 18f;
          float num2 = 186f;
          Game.game_work.pGameFont.DrawString(this.m_text, this.m_pos.X + num1, this.m_pos.Y, 0.0f, new Color(50, 50, 50, (int) byte.MaxValue), this.m_textScale, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER);
          if (this.m_numberText.Length <= 0)
            break;
          Game.game_work.pGameFont.DrawString(this.m_numberText, this.m_pos.X + num2, this.m_pos.Y, 0.0f, new Color(50, 50, 50, (int) byte.MaxValue), this.m_textScale, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER);
          break;
        case NotificationControl.NotificationType.ITEM_UNLOCK:
          if (NotificationControl.s_unlockBanner != null)
          {
            NotificationControl.s_unlockBanner.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(257f, 65f, 1f));
            MatrixManager.GetInstance().Translate(new Vector3(0.0f, this.m_pos.Y, 1f));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 63f / 64f);
            NotificationControl.s_unlockBanner.UnSet();
          }
          if (this.m_texture != null)
          {
            this.m_texture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(32f, 32f, 32f));
            MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3(0.0f, 16f, 0.0f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White);
            this.m_texture.UnSet();
          }
          Game.game_work.pGameFont.DrawString(this.m_text, this.m_pos.X + 18f, this.m_pos.Y + 16f, 0.0f, new Color(50, 50, 50, (int) byte.MaxValue), this.m_textScale, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_VCENTER);
          break;
      }
    }

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_NOTIFICATION;

    public enum NotificationType
    {
      NETWORK_NOTIFICATION,
      ACHIEVEMENT_UNLOCK,
      ITEM_UNLOCK,
    }
  }
}
