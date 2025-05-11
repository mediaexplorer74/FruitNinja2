
// Type: GameManager.AboutScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Mortar;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace GameManager
{
  public class AboutScreen : HUDControl3d
  {
    protected float m_time;
    protected Vector3 m_originalScale;
    protected MenuButton m_quitButton;
    protected static Texture s_boardTexture;
    protected static Texture m_creditsTexture;
    protected static Texture m_senseiTexture;
    protected static MenuButton m_englishTexture;
    protected static MenuButton m_frenchTexture;
    protected static MenuButton m_italianTexture;
    protected static MenuButton m_germanTexture;
    protected static MenuButton m_spanishTexture;
    private static bool init = false;
    private int m_state;
    public static float AboutScreenTime = -1f;
    private float timer;
    private int show;

    public static int SENSEI_CENTRE_X => 400;

    public static int SENSEI_CENTRE_Y => 104;

    public static int CREDITS_CENTRE_X => 192;

    public static int CREDITS_CENTRE_Y => 265;

    public static int ABOUT_CENTRE_X => 190;

    public static int ABOUT_CENTRE_Y => 97;

    public static float ABOUT_SCREEN_HEIGHT => 320f;

    public static string VERSION_TITLE => "VERSION:";

    public AboutScreen(MainScreen dojo)
    {
      this.m_texture = AboutScreen.s_boardTexture;
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
      Game.game_work.tutorialControl.ResetTutePos();
    }

    public static void LoadContent()
    {
      AboutScreen.s_boardTexture = TextureManager.GetInstance().Load("haikus.tex", true);
      AboutScreen.m_creditsTexture = TextureManager.GetInstance().Load("options.tex", true);
      AboutScreen.m_senseiTexture = TextureManager.GetInstance().Load("textureswp7/sensei.tex");
      AboutScreen.AboutScreenTime = (float) (-((double) Game.SCREEN_HEIGHT / 2.0) - (double) AboutScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game.GAME_MODE_SCALE_FIX);
    }

    public static void UnLoadContent()
    {
      AboutScreen.s_boardTexture = (Texture) null;
      AboutScreen.m_creditsTexture = (Texture) null;
      AboutScreen.m_senseiTexture = (Texture) null;
    }

    public override void Reset()
    {
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_time = 0.0f;
    }

    public override void Init()
    {
      this.Reset();
      this.timer = 0.0f;
      this.show = 0;
    }

    private static void ChangeLanguageCallback(IAsyncResult result)
    {
      int? nullable = default;//Guide.EndShowMessageBox(result);
      if (!nullable.HasValue)
        return;
      //Guide.EndShowMessageBox(result);
      if (!nullable.HasValue || nullable.Value != 1)
        return;
            Mortar.Game1.switchLanguage = true;
    }

    private void ShowDialog(StringTableUtils.Language language)
    {
            Mortar.Game1.switchToLanguage = language;
      string[] buttons = new string[2]
      {
        Mortar.Game1.instance.stringTable.GetString(702),
        Mortar.Game1.instance.stringTable.GetString(707)
      };
      try
      {
        //if (Guide.IsVisible)
        //  Thread.Sleep(32);
        //Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(885), TheGame.instance.stringTable.GetString(886), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(AboutScreen.ChangeLanguageCallback), (object) null);
      }
      catch
      {
      }
    }

    private void Callback_English()
    {
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH_UK || Game.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH)
        return;
      this.ShowDialog(StringTableUtils.Language.LANGUAGE_ENGLISH);
    }

    private void Callback_French()
    {
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_FRENCH)
        return;
      this.ShowDialog(StringTableUtils.Language.LANGUAGE_FRENCH);
    }

    private void Callback_Italian()
    {
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_ITALIAN)
        return;
      this.ShowDialog(StringTableUtils.Language.LANGUAGE_ITALIAN);
    }

    private void Callback_German()
    {
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_GERMAN)
        return;
      this.ShowDialog(StringTableUtils.Language.LANGUAGE_GERMAN);
    }

    private void Callback_Spanish()
    {
      if (Game.game_work.language == StringTableUtils.Language.LANGUAGE_SPANISH)
        return;
      this.ShowDialog(StringTableUtils.Language.LANGUAGE_SPANISH);
    }

    public override void Update(float dt)
    {
      this.timer += dt;
      if ((double) this.timer > 3.0)
      {
        this.timer = 0.0f;
        this.show ^= 1;
      }
      if (!AboutScreen.init)
      {
        AboutScreen.init = true;
             
        // ISSUE: explicit constructor call
        //((Vector3) ref scale).\u002Ector(32f, 32f, 1f);
        Vector3 scale = new Vector3(32f, 32f, 1f);
      
        if (AboutScreen.m_englishTexture == null)
        {
          AboutScreen.m_englishTexture = new MenuButton("english.tex", 
              new Vector3(0.0f, 0.0f, 0.0f),
              new MenuButton.MenuCallback(this.Callback_English), 
              -1, scale, false);
          AboutScreen.m_englishTexture.Init();
          Game.game_work.hud.AddControl((HUDControl) AboutScreen.m_englishTexture);
          AboutScreen.m_englishTexture.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (AboutScreen.m_frenchTexture == null)
        {
          AboutScreen.m_frenchTexture = new MenuButton("francais.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Callback_French), -1, scale, false);
          AboutScreen.m_frenchTexture.Init();
          Game.game_work.hud.AddControl((HUDControl) AboutScreen.m_frenchTexture);
          AboutScreen.m_frenchTexture.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (AboutScreen.m_italianTexture == null)
        {
          AboutScreen.m_italianTexture = new MenuButton("italiano.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Callback_Italian), -1, scale, false);
          AboutScreen.m_italianTexture.Init();
          Game.game_work.hud.AddControl((HUDControl) AboutScreen.m_italianTexture);
          AboutScreen.m_italianTexture.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (AboutScreen.m_germanTexture == null)
        {
          AboutScreen.m_germanTexture = new MenuButton("deusch.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Callback_German), -1, scale, false);
          AboutScreen.m_germanTexture.Init();
          Game.game_work.hud.AddControl((HUDControl) AboutScreen.m_germanTexture);
          AboutScreen.m_germanTexture.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (AboutScreen.m_spanishTexture == null)
        {
          AboutScreen.m_spanishTexture = new MenuButton("espanol.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Callback_Spanish), -1, scale, false);
          AboutScreen.m_spanishTexture.Init();
          Game.game_work.hud.AddControl((HUDControl) AboutScreen.m_spanishTexture);
          AboutScreen.m_spanishTexture.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
      }
      float num1 = -80f;
      float num2 = 40f;
      if ((double) GameTask.GetPauseAmount() == 0.0)
      {
        float num3 = (float) (-((double) Game.SCREEN_HEIGHT / 2.0) - (double) AboutScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game.GAME_MODE_SCALE_FIX);
        float num4 = (float) -((double) Game.SCREEN_HEIGHT / 2.0) + (float) (320 - (AboutScreen.CREDITS_CENTRE_Y + 10));
        float num5 = num3 - (num3 - num4) * this.m_time;
        if (AboutScreen.m_englishTexture != null)
        {
          AboutScreen.m_englishTexture.m_pos.Y = num5;
          AboutScreen.m_englishTexture.m_pos.X = num1;
          num1 += num2;
        }
        if (AboutScreen.m_frenchTexture != null)
        {
          AboutScreen.m_frenchTexture.m_pos.Y = num5;
          AboutScreen.m_frenchTexture.m_pos.X = num1;
          num1 += num2;
        }
        if (AboutScreen.m_italianTexture != null)
        {
          AboutScreen.m_italianTexture.m_pos.Y = num5;
          AboutScreen.m_italianTexture.m_pos.X = num1;
          num1 += num2;
        }
        if (AboutScreen.m_germanTexture != null)
        {
          AboutScreen.m_germanTexture.m_pos.Y = num5;
          AboutScreen.m_germanTexture.m_pos.X = num1;
          num1 += num2;
        }
        if (AboutScreen.m_spanishTexture != null)
        {
          AboutScreen.m_spanishTexture.m_pos.Y = num5;
          AboutScreen.m_spanishTexture.m_pos.X = num1;
          float num6 = num1 + num2;
        }
        AboutScreen.AboutScreenTime = num5;
      }
      switch (this.m_state)
      {
        case 0:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_time = 1f;
          this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) ((double) AboutScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
          this.m_quitButton.Init();
          this.m_quitButton.m_triggerOnBackPress = true;
          Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
          Game.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
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
          Game.game_work.mainScreen.m_state = MainScreen.MS.MS_RETURN;
          this.m_terminate = true;
          break;
      }
    }

    public override void Draw(float[] tintChannels)
    {
      try
      {
        if (this.m_texture != null)
        {
          this.m_texture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3(400f * Game.GAME_MODE_SCALE_FIX, 320f * Game.GAME_MODE_SCALE_FIX, 1f));
          float num1 = (float) ((double) AboutScreen.ABOUT_SCREEN_HEIGHT / 2.0 + 110.0);
          float num2 = (float) ((double) AboutScreen.ABOUT_SCREEN_HEIGHT / 2.0 - (double) AboutScreen.ABOUT_CENTRE_Y - 16.0);
          float num3 = num1 - (num1 - num2) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3((float) AboutScreen.ABOUT_CENTRE_X - Game.SCREEN_WIDTH / 2f, num3, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_texture.UnSet();
        }
        if (AboutScreen.m_creditsTexture != null)
        {
          AboutScreen.m_creditsTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) (390.0 * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) (160.0 * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 1f));
          float num4 = (float) (-((double) Game.SCREEN_HEIGHT / 2.0) - (double) AboutScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game.GAME_MODE_SCALE_FIX);
          float num5 = (float) -((double) Game.SCREEN_HEIGHT / 2.0) + (float) (320 - AboutScreen.CREDITS_CENTRE_Y);
          float num6 = num4 - (num4 - num5) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3((float) AboutScreen.CREDITS_CENTRE_X - Game.SCREEN_WIDTH / 2f, num6, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          AboutScreen.m_creditsTexture.UnSet();
        }
        if (AboutScreen.m_senseiTexture != null)
        {
          AboutScreen.m_senseiTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) AboutScreen.m_senseiTexture.GetWidth() * Game.GAME_MODE_SCALE_FIX, (float) AboutScreen.m_senseiTexture.GetHeight() * Game.GAME_MODE_SCALE_FIX, 1f));
          float num7 = (float) ((double) AboutScreen.m_senseiTexture.GetWidth() * 0.5 + (double) Game.SCREEN_WIDTH / 2.0);
          float num8 = (float) AboutScreen.SENSEI_CENTRE_X - Game.SCREEN_WIDTH / 2f;
          float num9 = num7 - (num7 - num8) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3(num9, AboutScreen.ABOUT_SCREEN_HEIGHT / 2f - (float) AboutScreen.SENSEI_CENTRE_Y, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White);
          AboutScreen.m_senseiTexture.UnSet();
        }
        if (this.m_state != 1)
          return;
        string str1 = "v1.1  ";
        string str2;
        switch (Game.game_work.language)
        {
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            str2 = str1 + "Contactez-nous:";
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            str2 = str1 + "Contacte con nosotros:";
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            str2 = str1 + "Kontakt:";
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            str2 = str1 + "Contatti:";
            break;
          default:
            str2 = str1 + "Contact us:";
            break;
        }
        string str3 = str2 + " support@halfbrick.com";
                Mortar.Game1.instance.spriteBatch.Begin();
                Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str3, new Vector2(8f, 8f), Color.Black);
                Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str3, new Vector2(7f, 7f), Color.BlanchedAlmond);
                Mortar.Game1.instance.spriteBatch.End();
      }
      catch
      {
      }
    }

    public enum AS
    {
      AS_IN,
      AS_WAIT,
      AS_OUT,
    }
  }
}
