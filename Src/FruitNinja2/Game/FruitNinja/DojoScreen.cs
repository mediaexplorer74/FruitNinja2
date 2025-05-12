
// Type: GameManager.DojoScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class DojoScreen : HUDControl3d
  {
    protected float m_time;
    protected MenuButton m_quitButton;
    protected MenuButton m_shopButton;
    protected MenuButton m_aboutButton;
    private int m_state;
    private static Texture blankTex = (Texture) null;
    private static Texture dojoTex = (Texture) null;
    private static Texture titleTexture = (Texture) null;
    private static Texture s_senseiTexture = (Texture) null;
    private int omode;
    private int skip;
    private static bool firstRender = false;
    private static GameVertex[] top_tri = new GameVertex[3];
    private static GameVertex[] btm_tri = new GameVertex[3];

    public static float DOJO_SCREEN_HEIGHT => 320f;

    public static Vector3 MODE_SELECT_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0 + 125.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 30.0), 0.0f);
      }
    }

    public static Vector3 TITLE_POS
    {
      get
      {
        return new Vector3((float) ((double) Game2.SCREEN_WIDTH / 2.0 - 58.0), (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - 23.0), 0.0f);
      }
    }

    public static Vector3 SENSEI_SELECT_POS
    {
      get
      {
        return new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0 + 60.0), (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 113.0), 0.0f);
      }
    }

    public DojoScreen()
    {
      this.m_texture = (Texture) null;
      this.m_selfCleanUp = false;
      this.m_quitButton = (MenuButton) null;
      this.m_shopButton = (MenuButton) null;
      this.m_aboutButton = (MenuButton) null;
      this.m_state = 0;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_time = 0.0f;
    }

    public void QuitCallback()
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      this.m_state = 5;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public void ShopCallback()
    {
      if (Game2.isWP7TrialMode())
        return;
      this.m_state = 2;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public void AboutCallback()
    {
      this.m_state = 3;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game2.game_work.tutorialControl.ResetTutePos();
    }

    public static void LoadContent()
    {
      DojoScreen.titleTexture = TextureManager.GetInstance().Load("textureswp7/sml_title.tex");
      DojoScreen.blankTex = TextureManager.GetInstance().Load("textureswp7/loading.tex");
      DojoScreen.dojoTex = TextureManager.GetInstance().Load("textureswp7/dojo.tex");
      DojoScreen.s_senseiTexture = TextureManager.GetInstance().Load("textureswp7/dojo_sensei.tex");
    }

    public static void UnLoadContent()
    {
      DojoScreen.titleTexture = (Texture) null;
      DojoScreen.blankTex = (Texture) null;
      DojoScreen.dojoTex = (Texture) null;
      DojoScreen.s_senseiTexture = (Texture) null;
    }

    public override void Reset() => this.m_state = 0;

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_time = 0.0f;
    }

    public override void Init() => this.Reset();

    public override void Update(float dt)
    {
      switch (this.m_state)
      {
        case 0:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.25);
          if ((double) this.m_time > 0.949999988079071)
          {
            if (this.m_quitButton == null)
            {
              this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game2.SCREEN_WIDTH / 2.0), 
                  (float) ((double) DojoScreen.DOJO_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitCallback), 
                  Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);

              this.m_quitButton.m_triggerOnBackPress = true;
              this.m_quitButton.Init();
              Game2.game_work.hud.AddControl((HUDControl) this.m_quitButton);
              Game2.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
              MenuButton quitButton = this.m_quitButton;
              quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
              Entity entity = this.m_quitButton.m_entity;
              entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
            }
            if (this.m_shopButton == null)
            {
              this.m_shopButton = new MenuButton("senseis_swag.tex", new Vector3((float) (222.0 - (double) Game2.SCREEN_WIDTH / 2.0),
                  (float) ((double) DojoScreen.DOJO_SCREEN_HEIGHT / 2.0 - 175.0), 0.0f), new MenuButton.MenuCallback(this.ShopCallback),
                  Fruit.FruitType("pineapple"), Vector3.Zero, true);

              this.m_shopButton.m_isTrialLockable = true;
              this.m_shopButton.m_originalScale = Vector3.Multiply(new Vector3((float) (this.m_shopButton.m_texture.GetWidth() + 1U), 
                  (float) (this.m_shopButton.m_texture.GetHeight() + 1U), 1f), Game2.GAME_MODE_SCALE_FIX);
              this.m_shopButton.m_overallScratchScale = 0.5f;
              MenuButton shopButton = this.m_shopButton;
              shopButton.m_newSymbolOffset = Vector3.Multiply(shopButton.m_newSymbolOffset, 0.575f);
              this.m_shopButton.m_deleteCall = (HUDControl.HUDControlDeletedCallback) (control =>
              {
                if (control != this.m_shopButton)
                  return;
                this.m_shopButton = (MenuButton) null;
              });
              this.m_shopButton.Init();
              Game2.game_work.hud.AddControl((HUDControl) this.m_shopButton);
              Game2.game_work.tutorialControl.ResetTutePos(this.m_shopButton);
              if (!Game2.isWP7TrialMode())
                this.m_shopButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
              else
                this.m_shopButton.m_clearOthers = false;
              this.omode = !Game2.isWP7TrialMode() ? 2 : 1;
            }
            if (this.m_aboutButton == null)
            {
              this.m_aboutButton = new MenuButton("credits.tex", new Vector3((float) (385.0 - (double) Game2.SCREEN_WIDTH / 2.0), 
                  (float) ((double) DojoScreen.DOJO_SCREEN_HEIGHT / 2.0 - 118.0), 0.0f), new MenuButton.MenuCallback(this.AboutCallback),
                  Fruit.FruitType("plum"), Vector3.Zero, true);

              this.m_aboutButton.Init();
              Game2.game_work.hud.AddControl((HUDControl) this.m_aboutButton);
            }
          }
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_time = 1f;
          this.m_state = 1;
          break;
        case 1:
          if (this.m_shopButton == null)
          {
            if (this.skip == 0)
            {
              this.skip = 1;
                            Mortar.Game1.instance.DoUpsell(false);
              break;
            }
            if (!Mortar.Game1.instance.UpsellComplete())
              break;
            this.skip = 0;
            this.m_shopButton = new MenuButton("senseis_swag.tex", new Vector3((float) (222.0 - (double) Game2.SCREEN_WIDTH / 2.0),
                (float) ((double) DojoScreen.DOJO_SCREEN_HEIGHT / 2.0 - 175.0), 0.0f), new MenuButton.MenuCallback(this.ShopCallback), 
                Fruit.FruitType("pineapple"), Vector3.Zero, true);

            this.m_shopButton.m_isTrialLockable = true;
            this.m_shopButton.m_originalScale = Vector3.Multiply(new Vector3((float) (this.m_shopButton.m_texture.GetWidth() + 1U),
                (float) (this.m_shopButton.m_texture.GetHeight() + 1U), 1f), Game2.GAME_MODE_SCALE_FIX);
            this.m_shopButton.m_overallScratchScale = 0.5f;
            MenuButton shopButton = this.m_shopButton;
            shopButton.m_newSymbolOffset = Vector3.Multiply(shopButton.m_newSymbolOffset, 0.575f);
            this.m_shopButton.m_deleteCall = (HUDControl.HUDControlDeletedCallback) (control =>
            {
              if (control != this.m_shopButton)
                return;
              this.m_shopButton = (MenuButton) null;
            });
            this.m_shopButton.Init();
            Game2.game_work.hud.AddControl((HUDControl) this.m_shopButton);
            Game2.game_work.tutorialControl.ResetTutePos(this.m_shopButton);
            if (!Game2.isWP7TrialMode())
            {
              this.m_shopButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
              break;
            }
            this.m_shopButton.m_clearOthers = false;
            break;
          }

          if (!Game2.isWP7TrialMode() && this.omode == 1)
          {
            this.omode = 2;
            this.m_shopButton.RemoveNoShow();
            Game2.game_work.hud.RemoveControl((HUDControl) this.m_shopButton);
            this.m_shopButton = (MenuButton) null;

            //RnD
            //GC2.Collect();
            this.m_shopButton = new MenuButton("senseis_swag.tex", 
                new Vector3((float) (222.0 - (double) Game2.SCREEN_WIDTH / 2.0), 
                (float) ((double) DojoScreen.DOJO_SCREEN_HEIGHT / 2.0 - 175.0), 0.0f), 
                new MenuButton.MenuCallback(this.ShopCallback), 
                Fruit.FruitType("pineapple"),
                Vector3.Zero, true);
            this.m_shopButton.m_isTrialLockable = true;
            this.m_shopButton.m_originalScale = Vector3.Multiply(
                new Vector3((float) (this.m_shopButton.m_texture.GetWidth() + 1U), 
                (float) (this.m_shopButton.m_texture.GetHeight() + 1U), 1f), 
                Game2.GAME_MODE_SCALE_FIX);
            this.m_shopButton.m_overallScratchScale = 0.5f;
            MenuButton shopButton = this.m_shopButton;
            shopButton.m_newSymbolOffset = Vector3.Multiply(
                shopButton.m_newSymbolOffset, 0.575f);
            this.m_shopButton.m_deleteCall = 
            (HUDControl.HUDControlDeletedCallback)
            (control =>
            {
              if (control != this.m_shopButton)
                return;
              this.m_shopButton = (MenuButton) null;
            });
            this.m_shopButton.Init();
            Game2.game_work.hud.AddControl((HUDControl) this.m_shopButton);
            Game2.game_work.tutorialControl.ResetTutePos(this.m_shopButton);
            if (!Game2.isWP7TrialMode())
            {
              this.m_shopButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
              break;
            }
            this.m_shopButton.m_clearOthers = false;
            break;
          }
          
          if (this.m_shopButton == null)
            break;
          this.m_shopButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
          break;
        case 2:
        case 3:
          this.m_time *= 0.75f;
          if ((double) this.m_time <= 0.0 || (double) this.m_time > 1.0 / 1000.0)
            break;
          this.m_quitButton = (MenuButton) null;
          this.m_shopButton = (MenuButton) null;
          this.m_aboutButton = (MenuButton) null;
          if (this.m_state == 3)
          {
            CreditsScreen control = new CreditsScreen(this);
            control.Init();
            Game2.game_work.hud.AddControl((HUDControl) control);
          }
          else if (this.m_state == 2)
          {
            ShopScreen control = new ShopScreen(this);
            control.Init();
            Game2.game_work.hud.AddControl((HUDControl) control);
          }
          this.m_time = 0.0f;
          break;
        case 5:
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
      if (!DojoScreen.firstRender)
      {
        DojoScreen.firstRender = true;
        Color color = new Color(0, 0, 0, 128);
        for (int index = 0; index < 3; ++index)
        {
          DojoScreen.top_tri[index].X = DojoScreen.top_tri[index].Y = DojoScreen.top_tri[index].Z = DojoScreen.top_tri[index].nx = DojoScreen.top_tri[index].ny = 0.0f;
          DojoScreen.top_tri[index].nz = 1f;
          DojoScreen.top_tri[index].color = color;
          DojoScreen.top_tri[index].u = 0.5f;
          DojoScreen.top_tri[index].v = 0.5f;
          DojoScreen.btm_tri[index].X = DojoScreen.btm_tri[index].Y = DojoScreen.btm_tri[index].Z = DojoScreen.btm_tri[index].nx = DojoScreen.btm_tri[index].ny = 0.0f;
          DojoScreen.btm_tri[index].nz = 1f;
          DojoScreen.btm_tri[index].color = color;
          DojoScreen.btm_tri[index].u = 0.5f;
          DojoScreen.btm_tri[index].v = 0.5f;
        }
        DojoScreen.top_tri[1].Y = DojoScreen.top_tri[2].Y = 82f;
        DojoScreen.top_tri[2].X = -656f;
        DojoScreen.btm_tri[1].Y = DojoScreen.btm_tri[2].Y = -82f;
        DojoScreen.btm_tri[2].X = 656f;
      }
      if ((double) this.m_time <= 0.0)
        return;
      MatrixManager.GetInstance().Reset();

      MatrixManager.GetInstance().Scale(
          new Vector3((float) (DojoScreen.s_senseiTexture.GetWidth() + 1U),
          (float) (DojoScreen.s_senseiTexture.GetHeight() + 1U), 0.0f));

      MatrixManager.GetInstance().Translate(
          Vector3.Subtract(DojoScreen.SENSEI_SELECT_POS, Vector3.Multiply(
              Vector3.Multiply(Vector3.UnitX, (float) DojoScreen.s_senseiTexture.GetWidth()), 
              1f - this.m_time)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      DojoScreen.s_senseiTexture.Set();
      Mesh.DrawQuad(Color.White);
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Translate(new Vector3(Game2.SCREEN_WIDTH / 2f, 
          (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - 48.0 * (double) this.m_time), 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawTriList(DojoScreen.top_tri, 3, true);
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Translate(new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0), 
          (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 60.0 * (double) this.m_time), 0.0f));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawTriList(DojoScreen.btm_tri, 3, true);
      DojoScreen.s_senseiTexture.UnSet();
      DojoScreen.dojoTex.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3((float) ((double) DojoScreen.dojoTex.GetWidth()
          * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) DojoScreen.dojoTex.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 0.0f));

      MatrixManager.GetInstance().Translate(new Vector3((float) (-(double) Game2.SCREEN_WIDTH / 2.0 + 56.0),
          (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + 24.0 - (1.0 - (double) this.m_time) * 60.0), 0.0f));

      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(Color.White);
      DojoScreen.dojoTex.UnSet();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3((float) (DojoScreen.titleTexture.GetWidth() + 1U),
          (float) (DojoScreen.titleTexture.GetHeight() + 1U), 0.0f));

      MatrixManager.GetInstance().Translate(Vector3.Add(DojoScreen.TITLE_POS, 
          Vector3.Multiply(Vector3.Multiply(Vector3.UnitY, 48f), 1f - this.m_time)));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      DojoScreen.titleTexture.Set();
      Mesh.DrawQuad(Color.White);
      DojoScreen.titleTexture.UnSet();
    }

    public void ButtonDeleted(HUDControl control)
    {
      if (control != this.m_shopButton)
        return;
      this.m_shopButton = (MenuButton) null;
    }

    public enum DS
    {
      DS_IN,
      DS_WAIT,
      DS_SHOP,
      DS_ABOUT,
      DS_TRAINING,
      DS_OUT,
    }
  }
}
