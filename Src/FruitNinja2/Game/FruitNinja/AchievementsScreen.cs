
// Type: GameManager.AchievementsScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  internal class AchievementsScreen : HUDControl3d
  {
    private float m_time;
    private int m_state;
    private MenuButton m_quitButton;
    private float prevTextOffset;
    private float textOffset;
    private float offset;
    private float snapY;
    private float tx;
    private float ty;
    private float initialX;
    private float initialY;
    private bool down;
    private bool prevTouch;
    private bool currTouch;
    private float renderSize = 1584f;
    private AchievementsScreen.ScrollState scrollState;
    private static Texture2D m_backing;
    private static Mortar.Texture m_title;
    private static Mortar.Texture m_box;
    private static Texture2D m_live;
    private static float liveX;
    private static float liveY;
    public static int[] achIconMap = new int[20]
    {
      8,
      7,
      6,
      2,
      9,
      5,
      13,
      4,
      14,
      3,
      1,
      15,
      10,
      12,
      16,
      17,
      11,
      18,
      19,
      20
    };
    private static AchievementsScreen.MapInfo[] mapdata = new AchievementsScreen.MapInfo[20]
    {
      new AchievementsScreen.MapInfo("Fruit Blitz", 4),
      new AchievementsScreen.MapInfo("Year Of The Dragon", 16),
      new AchievementsScreen.MapInfo("Fruit Fight", 3),
      new AchievementsScreen.MapInfo("Deja Vu", 12),
      new AchievementsScreen.MapInfo("Wake Up", 15),
      new AchievementsScreen.MapInfo("Great Fruit Ninja", 1),
      new AchievementsScreen.MapInfo("Ultimate Fruit Ninja", 2),
      new AchievementsScreen.MapInfo("Fruit Frenzy", 5),
      new AchievementsScreen.MapInfo("Fruit Rampage", 6),
      new AchievementsScreen.MapInfo("Fruit Annihilation", 7),
      new AchievementsScreen.MapInfo("Lucky Ninja", 8),
      new AchievementsScreen.MapInfo("Almost A Century", 9),
      new AchievementsScreen.MapInfo("Mango Magic", 10),
      new AchievementsScreen.MapInfo("Combo Mambo", 13),
      new AchievementsScreen.MapInfo("Moment Of Zen", 14),
      new AchievementsScreen.MapInfo("The Lovely Bunch", 17),
      new AchievementsScreen.MapInfo("Its All Pear Shaped", 11),
      new AchievementsScreen.MapInfo("Underachiever", 18),
      new AchievementsScreen.MapInfo("Overachiever", 19),
      new AchievementsScreen.MapInfo("Bomb Magnet", 20)
    };
    private static Dictionary<string, Texture2D> imgMapInfo = (Dictionary<string, Texture2D>) null;
    private static float py = 0.0f;
    private static float sy = 0.0f;

    public static int ABOUT_CENTRE_X => 190;

    public static int ABOUT_CENTRE_Y => 97;

    public static float ABOUT_SCREEN_HEIGHT => 320f;

    public AchievementsScreen()
    {
      this.m_selfCleanUp = false;
      this.m_quitButton = (MenuButton) null;
      this.m_state = 0;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_time = 0.0f;
    }

    public void QuitGameCallback()
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      this.m_state = 2;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(5f) + 5f, -Mortar.Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public static void LoadContent()
    {
      if (AchievementsScreen.m_backing == null)
        AchievementsScreen.m_backing = TextureManager.GetInstance().Load("textureswp7/leaderboard_back.tex").intex;
      if (AchievementsScreen.m_title == null)
        AchievementsScreen.m_title = TextureManager.GetInstance().Load("achievements.tex", true);
      if (AchievementsScreen.m_box == null)
        AchievementsScreen.m_box = TextureManager.GetInstance().Load("achievements_box.tex", true);
      if (AchievementsScreen.m_live != null)
        return;
      AchievementsScreen.m_live = TextureManager.GetInstance().Load("textureswp7/xboxliveonwindowsphone_4cko_s.tex").intex;
    }

    public static void UnLoadContent()
    {
    }

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = null;
      this.m_time = 0.0f;
    }

    public override void Init()
    {
      this.Reset();
      AchievementsScreen.imgMapInfo = new Dictionary<string, Texture2D>();
      foreach (AchievementsScreen.MapInfo mapInfo in AchievementsScreen.mapdata)
                AchievementsScreen.imgMapInfo.Add(mapInfo.key, Mortar.Game1.instance.Content.Load<Texture2D>("textureswp7/achicon_" + (object) mapInfo.index + ".tga"));
    }

    private bool InTextBox()
    {
      return Mortar.Math.BETWEEN((int) this.tx, 100, 582) && Mortar.Math.BETWEEN((int) this.ty, 172, 410);
    }

    public override void Update(float dt)
    {
      if (this.m_state == 1)
      {
        if (Touch.GetInstance().GetTouchCount() == 1)
        {
          Vector2 position = Touch.GetInstance().GetPosition();
          this.tx = position.X;
          this.ty = position.Y;
          this.currTouch = true;
        }
        else
          this.currTouch = false;
        float num = 268f;
        if (this.prevTouch != this.currTouch)
        {
          this.prevTouch = this.currTouch;
          if (this.currTouch)
          {
            if ((double) Mortar.Math.Abs(AchievementsScreen.sy) < 4.0)
            {
              this.down = true;
              AchievementsScreen.sy = 0.0f;
              if (this.InTextBox())
              {
                this.initialX = this.tx;
                this.initialY = this.ty;
                this.textOffset = 0.0f;
                this.prevTextOffset = 0.0f;
                this.scrollState = AchievementsScreen.ScrollState.Scroll;
              }
            }
          }
          else
            this.down = false;
        }
        if (this.down && this.scrollState == AchievementsScreen.ScrollState.Scroll && this.InTextBox())
        {
          this.prevTextOffset = this.textOffset;
          this.textOffset = this.ty - this.initialY;
          float t = this.ty - AchievementsScreen.py;
          if ((double) Mortar.Math.Abs(t) > 1.0 && (double) Mortar.Math.Abs(t) < 64.0)
            AchievementsScreen.sy = t;
          if ((double) this.offset + (double) this.textOffset > 0.0)
          {
            this.snapY = this.textOffset - this.prevTextOffset;
            this.textOffset = this.prevTextOffset;
          }
          if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - (double) num))
          {
            this.snapY = this.textOffset - this.prevTextOffset;
            this.textOffset = this.prevTextOffset;
          }
          AchievementsScreen.py = this.ty;
        }
        else if ((double) Mortar.Math.Abs(AchievementsScreen.sy) > 1.0)
        {
          this.prevTextOffset = this.textOffset;
          this.textOffset += AchievementsScreen.sy;
          AchievementsScreen.sy -= AchievementsScreen.sy / 4f;
          if ((double) this.offset + (double) this.textOffset > 0.0)
          {
            this.snapY = this.textOffset - this.prevTextOffset;
            this.textOffset = this.prevTextOffset;
            AchievementsScreen.sy = 0.0f;
          }
          if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - (double) num))
          {
            this.snapY = this.textOffset - this.prevTextOffset;
            this.textOffset = this.prevTextOffset;
            AchievementsScreen.sy = 0.0f;
          }
        }
        else
        {
          if (this.scrollState == AchievementsScreen.ScrollState.Scroll)
          {
            this.scrollState = AchievementsScreen.ScrollState.Idle;
            this.offset += this.textOffset;
            this.textOffset = 0.0f;
          }
          this.snapY -= this.snapY / 8f;
        }
      }
      else
      {
        this.scrollState = AchievementsScreen.ScrollState.Idle;
        this.initialX = this.tx = -1f;
        this.initialY = this.ty = -1f;
        this.textOffset = 0.0f;
      }
      Game2.game_work.tutorialControl.ResetTutePos();
      switch (this.m_state)
      {
        case 0:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_time = 1f;
          this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game2.SCREEN_WIDTH / 2.0), (float) ((double) AchievementsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
          this.m_quitButton.Init();
          this.m_quitButton.m_triggerOnBackPress = true;
          Game2.game_work.hud.AddControl((HUDControl) this.m_quitButton);
          Game2.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
          MenuButton quitButton = this.m_quitButton;
          quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
          Entity entity = this.m_quitButton.m_entity;
          entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
          this.m_state = 1;
          break;
        case 1:
          GamePadState state = GamePad.GetState((PlayerIndex) 0);
          GamePadButtons buttons = state.Buttons;
          if (buttons.Back != ButtonState.Pressed)
            break;
          this.QuitGameCallback();
          break;
        case 2:
          this.m_time *= 0.75f;
          if ((double) this.m_time >= 1.0 / 1000.0)
            break;
          Game2.game_work.mainScreen.m_state = MainScreen.MS.MS_RETURN;
          this.m_terminate = true;
          break;
      }
    }

    public override void Draw(float[] tintChannels)
    {
      if (AchievementsScreen.m_backing != null)
      {
                Mortar.Game1.instance.spriteBatch.Begin();
                Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_backing, new Vector2(0.0f, 0.0f), Color.White);
                Mortar.Game1.instance.spriteBatch.End();
      }
      if (AchievementsScreen.m_title != null)
      {
                Mortar.Game1.instance.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
                Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_title.intex, new Vector2(-80f, 0.0f), Color.White);
                Mortar.Game1.instance.spriteBatch.End();
      }
      float num1 = 0.0f;
      if (AchievementsScreen.m_box != null)
      {
        float num2 = 480f;
        float num3 = 16f;
        num1 = num2 - (num2 - num3) * Mortar.Math.Abs(this.m_time);
                Mortar.Game1.instance.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
                Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) num1, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16, AchievementsScreen.m_box.intex.Height), Color.White);
                Mortar.Game1.instance.spriteBatch.End();
      }
            Mortar.Game1.instance.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
      if (AchievementsScreen.m_live != null)
      {
        AchievementsScreen.liveY = 64f;
        AchievementsScreen.liveX = (float) (400.0 + (double) num1 * 2.0);
      }
            Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_live, new Vector2(AchievementsScreen.liveX, AchievementsScreen.liveY), Color.White);
      float num4 = num1 + 138f;
      float num5 = 272f;
      Vector2 vector2_1 = Mortar.Game1.instance.font3.MeasureString(" ");
      for (int index = 0; index < 20; ++index)
      {
        Color color = AchievementManager.Unlocked(index) ? Color.Black : new Color(0, 0, 0, 96);
        bool flag1 = false;
        float num6 = num1 + 138f + (float) (index * 80) + this.textOffset + this.offset + this.snapY;
        float num7 = num6 + 64f;
        int num8 = 0;
        int num9 = 0;
        int num10 = 0;
        float num11 = num6;
        string[] strArray = AchievementManager.GetDescription(index).Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries);
        string str1 = "";
        string str2 = "";
        string str3 = "";
        float num12 = 0.0f;
        int num13 = 0;
        foreach (string str4 in strArray)
        {
          string str5 = str4.Trim();
          Vector2 vector2_2 = Mortar.Game1.instance.font3.MeasureString(str5);
          if ((double) num12 + (double) vector2_2.X + (double) vector2_1.X <= 380.0)
          {
            num12 += vector2_2.X + vector2_1.X;
          }
          else
          {
            num12 = vector2_2.X + vector2_1.X;
            ++num13;
          }
          switch (num13)
          {
            case 0:
              str1 = str1 + str4 + " ";
              break;
            case 1:
              str2 = str2 + str4 + " ";
              break;
            default:
              str3 = str3 + str4 + " ";
              break;
          }
        }
        if ((double) num11 + 16.0 < (double) num4 - 20.0)
          flag1 = true;
        if ((double) num11 + 16.0 > (double) num4 + (double) num5 - 12.0)
          flag1 = true;
        if (!flag1)
        {
                    Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str1, new Vector2(100f, num11 + 16f), color);
                    Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, AchievementManager.GetGamerScore(index).ToString(), new Vector2(510f, num11 + 16f), color);
        }
        bool flag2 = false;
        if ((double) num11 + 36.0 < (double) num4 - 20.0)
          flag2 = true;
        if ((double) num11 + 36.0 > (double) num4 + (double) num5 - 12.0)
          flag2 = true;
        if (!flag2 && str2.Length > 0)
                    Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str2, new Vector2(100f, num11 + 36f), color);
        Texture2D texture2D = AchievementsScreen.imgMapInfo[AchievementManager.GetKey(index)];
        bool flag3 = false;
        if ((double) num7 > (double) num4 + (double) num5)
        {
          float num14 = num7 - (num4 + num5);
          if ((double) num14 > 64.0)
          {
            flag3 = true;
          }
          else
          {
            num8 = (int) -(double) num14;
            num9 = num8 / 2;
            if (texture2D.Height == 64)
              num9 = num8;
          }
        }
        if ((double) num6 < (double) num4)
        {
          float num15 = num4 - num6;
          if ((double) num15 > 64.0)
          {
            flag3 = true;
          }
          else
          {
            num6 = num4;
            num10 = (int) num15;
            num9 = -num10;
            num8 = num9;
            if (texture2D.Height == 32)
            {
              num10 /= 2;
              num9 /= 2;
            }
          }
        }
        if (!flag3)
                    Mortar.Game1.instance.spriteBatch.Draw(texture2D, new Rectangle(26, (int) num6, 64, 64 + num8), new Rectangle?(new Rectangle(0, num10, texture2D.Width, texture2D.Height + num9)), Color.White);
      }
            Mortar.Game1.instance.spriteBatch.End();
            Mortar.Game1.instance.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
            Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) num1, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16, 137), new Rectangle?(new Rectangle(0, 0, AchievementsScreen.m_box.intex.Width, 137)), Color.White);
            Mortar.Game1.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) num1 + 400, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16, 48), new Rectangle?(new Rectangle(0, 400, AchievementsScreen.m_box.intex.Width, 48)), Color.White);
            Mortar.Game1.instance.spriteBatch.End();
    }

    private class MapInfo
    {
      public string key;
      public int index;

      public MapInfo(string s, int i)
      {
        this.key = s;
        this.index = i;
      }
    }

    private enum ScrollState
    {
      Idle,
      Scroll,
    }

    public enum AS
    {
      AS_IN,
      AS_WAIT,
      AS_OUT,
    }
  }
}
