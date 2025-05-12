
// Type: GameManager.FruitFactControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class FruitFactControl : HUDControl3d
  {
    public string m_text;
    public int m_fruitType;
    public int m_fact;
    public Texture m_fruitTexture;
    public Vector3 m_fruitOffset;
    public Color m_fruitColor;
    public static Texture s_boardTexture;
    public static Texture s_bigBoardTexture;
    public static Texture s_noComboTexture;
    public static Texture s_senseiHead;
    public static Texture s_loadingTexture;
    public bool m_showCombo;
    public int[] m_comboFruits = new int[11];
    public int m_numComboFruits;
    public float m_fruitComboTime;
    public Texture m_stickerTexture;
    public COMBO_TYPE m_comboStickerType;
    public int m_mode;
    private int displayMode;
    private float m_loadingSymbolTime;
    public static LeaderboardList m_leaderboardList;
    public MenuButton m_leaderboardButton;
    public FruitFactControl.FFL m_leaderboardState;
    private bool start_download;
    private bool download_complete;
    private FruitFactControl.DownloadResult download_result;
    public MenuButton[] m_buttons = new MenuButton[2];
    public static Texture s_youTexture = (Texture) null;
    public static Texture s_noScoreTexture = (Texture) null;
    private static Texture s_buttonTexture = (Texture) null;
    private static Texture s_bigLeaderBoardTexture = (Texture) null;
    private static Texture s_bigLeaderBoardDialogTexture = (Texture) null;
    private static Texture s_bigBonusBoardTexture = (Texture) null;
    private static Texture s_xboxLive = (Texture) null;
    private static bool contentLoaded = false;
    private static string zenModeTitle;
    private static GameVertex[] symbo_tris = new GameVertex[48];

    public static bool USE_ZEN_FACT_SCREEN => Game2.game_work.gameMode == Game2.GAME_MODE.GM_ZEN;

    public static int NUM_ARCADE_MODE_MODES => 2;

    public static float LEADERBOARD_Y_DIFF => 47f;

    public static bool SetupQuad(
      GameVertex[] verts,
      Vector3 pos,
      float width,
      float height,
      MortarRectangleDec rectangle)
    {
      return FruitFactControl.SetupQuad(verts, pos, width, height, rectangle, Color.White);
    }

    private static bool SetupQuad(
      GameVertex[] verts,
      Vector3 pos,
      float width,
      float height,
      MortarRectangleDec rectangle,
      Color colour)
    {
      rectangle.bottom += height * 0.5f;
      rectangle.top += height * 0.5f;
      float num1 = pos.X - width * 0.5f;
      float num2 = pos.X + width * 0.5f;
      float num3 = 1f;
      float num4 = 0.0f;
      float num5 = pos.Y - height * 0.5f;
      float num6 = pos.Y + height * 0.5f;
      if ((double) num5 < (double) rectangle.bottom)
      {
        num5 = rectangle.bottom;
        num3 = (float) (0.5 - ((double) num5 - (double) pos.Y) / (double) height);
      }
      if ((double) num6 > (double) rectangle.top)
      {
        num6 = rectangle.top;
        num4 = (float) (0.5 - ((double) num6 - (double) pos.Y) / (double) height);
      }
      if ((double) num6 <= (double) rectangle.bottom || (double) num5 >= (double) rectangle.top)
        return false;
      for (int index = 0; index < 4; ++index)
      {
        verts[index].color = colour;
        verts[index].X = verts[index].Y = verts[index].Z = verts[index].nx = verts[index].ny = 0.0f;
        verts[index].nz = 1f;
        if (index % 2 == 0)
        {
          verts[index].X = num1;
          verts[index].u = 0.0f;
        }
        else
        {
          verts[index].X = num2;
          verts[index].u = 1f;
        }
        if (index < 2)
        {
          verts[index].Y = num5;
          verts[index].v = num3;
        }
        else
        {
          verts[index].Y = num6;
          verts[index].v = num4;
        }
      }
      return true;
    }

    public FruitFactControl()
    {
      FruitFactControl.LoadContent();
      this.m_rotation = 0.0f;
      this.m_selfCleanUp = true;
      this.m_fruitColor = new Color(116, 93, 59);
      this.m_text = (string) null;
      this.m_stickerTexture = (Texture) null;
      this.m_fruitComboTime = 0.0f;
      this.m_numComboFruits = 0;
      this.m_fruitType = -1;
      this.m_fact = -1;
      this.m_mode = 0;
      this.m_loadingSymbolTime = 0.0f;
      FruitFactControl.m_leaderboardList = (LeaderboardList) null;
      this.m_buttons[0] = (MenuButton) null;
      this.m_buttons[1] = (MenuButton) null;
      this.m_leaderboardButton = (MenuButton) null;

      //SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      //this.m_leaderboardState = signedInGamer == null || !signedInGamer.IsSignedInToLive || !Mortar.Game1.logInSucceeded ? FruitFactControl.FFL.FFL_OFFLINE : FruitFactControl.FFL.FFL_DOWNLOADING;
      this.start_download = false;
      this.download_complete = false;
      this.download_result = FruitFactControl.DownloadResult.Pending;
      this.Reset();
    }

    ~FruitFactControl() => this.Release();

    public override void Init()
    {
      this.m_fruitComboTime = -0.5f;
      this.m_stickerTexture = (Texture) null;
      this.m_texture = Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE ? FruitFactControl.s_bigLeaderBoardDialogTexture : (FruitFactControl.USE_ZEN_FACT_SCREEN ? FruitFactControl.s_bigBoardTexture : FruitFactControl.s_boardTexture);
      this.displayMode = Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE ? 1 : (FruitFactControl.USE_ZEN_FACT_SCREEN ? 2 : 3);
      if (FruitFactControl.USE_ZEN_FACT_SCREEN)
        this.m_scale = Vector3.Multiply(new Vector3((float) (this.m_texture.GetWidth() + 1U), (float) (this.m_texture.GetHeight() + 1U), 0.0f), Game2.GAME_MODE_SCALE_FIX);
      else
        this.m_scale = Vector3.Multiply(new Vector3((float) (this.m_texture.GetWidth() + 1U), (float) (this.m_texture.GetHeight() + 1U), 0.0f), Game2.GAME_MODE_SCALE_FIX);
      this.m_comboStickerType = COMBO_TYPE.CT_NONE;
      this.m_showCombo = FruitFactControl.USE_ZEN_FACT_SCREEN && Game2.game_work.saveData.numFruitTypesInSliceCombo > 2;
      this.m_fruitOffset = new Vector3(-69f, 53f, 0.0f);
      if (this.m_showCombo)
      {
                FruitFactControl.zenModeTitle = string.Format(Mortar.Game1.instance.stringTable.GetString(136).Replace("%i", "{0}"), (object)Game2.game_work.saveData.numFruitTypesInSliceCombo);
        this.m_numComboFruits = Game2.game_work.saveData.numFruitTypesInSliceCombo;
        for (int index = 0; index < this.m_numComboFruits; ++index)
          this.m_comboFruits[index] = Game2.game_work.saveData.sliceComboFruitTypes[index];
        int mostOf = 0;
        this.m_comboStickerType = ComboChecker.CheckCombo(this.m_comboFruits, this.m_numComboFruits, out mostOf);
        this.m_stickerTexture = ComboChecker.GetComboStarTexture(this.m_comboStickerType);
        if (this.m_fruitType != mostOf)
          this.m_fruitType = mostOf;
        this.m_fruitOffset = new Vector3(140f, -72f, 0.0f);
      }
      else
                FruitFactControl.zenModeTitle = Mortar.Game1.instance.stringTable.GetString(161);
      int num1 = 0;
      do
      {
        int num2;
        do
        {
          try
          {
            this.m_text = Fruit.GetFact((Action<int>) (v => this.m_fruitType = v), (Action<int>) (v => this.m_fact = v), this.m_fruitType, this.m_fact);
            ++num1;
          }
          catch
          {
            num1 = 100;
          }
          if (num1 == 100)
          {
            this.m_fruitType = -1;
            this.m_fact = -1;
            this.m_text = "";
            num1 = 0;
          }
          float num3 = 16f;
          num2 = 0;
          int num4 = 0;
          int num5 = -1;
          for (int index = 0; index < this.m_text.Length; ++index)
          {
            if (this.m_text[index] == ' ')
              num5 = index;
            ++num4;
            if ((double) num4 == (double) num3)
            {
              if ((double) (index - num5) < (double) num3 && index != num5)
                index = num5 + 1;
              num4 = 0;
              ++num2;
            }
          }
          if (num4 != 0)
            ++num2;
        }
        while (num2 > 6);
      }
      while (this.m_text.Trim().Length == 0);
      this.m_fruitColor = Fruit.FruitFactColor(this.m_fruitType);
      string str = string.Format("textureswp7/{0}.tex", (object) Fruit.FruitFactTexture(this.m_fruitType));
      if (MortarFile.Exists(str))
        this.m_fruitTexture = TextureManager.GetInstance().Load(str);
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.Reset();
    }

    public override void Release()
    {
      this.m_texture = (Texture) null;
      this.m_fruitTexture = (Texture) null;
      this.m_stickerTexture = (Texture) null;
      if (FruitFactControl.m_leaderboardList != null)
      {
        Game2.game_work.hud.RemoveControl((HUDControl) FruitFactControl.m_leaderboardList);
        Delete.SAFE_DELETE<LeaderboardList>(ref FruitFactControl.m_leaderboardList);
      }
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_buttons[index] != null)
          Game2.game_work.hud.RemoveControl((HUDControl) this.m_buttons[index]);
        Delete.SAFE_DELETE<MenuButton>(ref this.m_buttons[index]);
      }
      if (this.m_leaderboardButton == null)
        return;
      Game2.game_work.hud.RemoveControl((HUDControl) this.m_leaderboardButton);
      Delete.SAFE_DELETE<MenuButton>(ref this.m_leaderboardButton);
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      bool flag = false;
      if (this.m_drawOrder == HUD.HUD_ORDER.HUD_ORDER_POST)
      {
        FruitFactControl.s_senseiHead.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3((float) FruitFactControl.s_senseiHead.GetWidth() * Game2.GAME_MODE_SCALE_FIX, (float) FruitFactControl.s_senseiHead.GetHeight() * Game2.GAME_MODE_SCALE_FIX, 0.0f));
        if (Mortar.Game1.settings.language == 3 || Mortar.Game1.settings.language == 4)
                    MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3(156f, 68f, 0.0f)));
        else
                    MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3(140f, 68f, 0.0f)));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        FruitFactControl.s_senseiHead.UnSet();
      }
      else
      {
        if (FruitFactControl.USE_ZEN_FACT_SCREEN)
        {
          if (this.m_texture != null)
          {
            this.m_texture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(this.m_scale);
            MatrixManager.GetInstance().Translate(
                Vector3.Subtract(this.m_pos, new Vector3(8f, -8f, 0.0f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            this.m_texture.UnSet();
          }
          flag = true;
                    Game2.game_work.pGameFont.DrawString(
              Mortar.Game1.instance.stringTable.GetString(139), this.m_pos.X - 8f, this.m_pos.Y,
              0.0f, this.m_fruitColor, 16f, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_HCENTER);
          Game2.game_work.pGameFont.DrawString(
              this.m_text, (float) ((double) this.m_pos.X - (double) sbyte.MaxValue - 8.0),
              this.m_pos.Y - 40f, 0.0f, new Color(116, 93, 59), 16f, (float) byte.MaxValue,
              0.0f, ALIGNMENT_TYPE.ALIGN_CENTER);
          Game2.game_work.pGameFont.DrawString(
              FruitFactControl.zenModeTitle, this.m_pos.X - 8f, this.m_pos.Y + 89f, 0.0f,
              this.m_fruitColor, 20f, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_HCENTER);
          if (this.m_showCombo)
          {
            float num1 = 40f;
            float num2 = (float) (this.m_numComboFruits - 1) * num1;
            if ((double) num2 > 220.0)
            {
              num2 = 220f;
              num1 = num2 / (float) (this.m_numComboFruits - 1);
            }
            for (int index = 0; index < this.m_numComboFruits; ++index)
            {
              Texture zenIcon = Fruit.FruitInfo(this.m_comboFruits[index]).zenIcon;
              if (zenIcon != null)
              {
                float num3;
                if ((int) ((double) this.m_fruitComboTime + 2.0) == index + 2)
                  num3 = (float) (65.0 * (double) Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(135f * Mortar.Math.CLAMP((float) (((double) this.m_fruitComboTime - (double) index - 0.5) * 2.0), 0.0f, 1f))) / 0.70710676908493042);
                else if ((double) this.m_fruitComboTime > (double) index)
                  num3 = 65f;
                else
                  break;
                zenIcon.Set();
                MatrixManager.GetInstance().Reset();
                MatrixManager.GetInstance().Scale(Vector3.Multiply(Vector3.One, num3));
                MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3((float) (-8.0 - (double) num2 / 2.0 + (double) index * (double) num1), 37f, 0.0f)));
                MatrixManager.GetInstance().UploadCurrentMatrices();
                Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
                zenIcon.UnSet();
              }
            }
            if (this.m_stickerTexture != null && (double) this.m_fruitComboTime > (double) this.m_numComboFruits)
            {
              float num4 = Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(135f * Mortar.Math.CLAMP((float) (((double) this.m_fruitComboTime - (double) this.m_numComboFruits - 0.5) * 2.0), 0.0f, 1f))) / 0.707106769f;
              this.m_stickerTexture.Set();
              MatrixManager.GetInstance().Reset();
              MatrixManager.GetInstance().Scale(Vector3.Multiply(Vector3.Multiply(new Vector3((float) (this.m_stickerTexture.GetWidth() + 1U), (float) (this.m_stickerTexture.GetHeight() + 1U), 0.0f), num4), Game2.GAME_MODE_SCALE_FIX));
              MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3((float) ((double) num2 / 2.0 - 8.0), (float) (37.0 + (double) this.m_stickerTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX / 2.0), 0.0f)));
              MatrixManager.GetInstance().UploadCurrentMatrices();
              Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
              this.m_stickerTexture.UnSet();
            }
          }
          else
          {
            float num = Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(135f * Mortar.Math.CLAMP((float) (((double) this.m_fruitComboTime - (double) this.m_numComboFruits - 0.5) * 2.0), 0.0f, 1f))) / 0.707106769f;
            FruitFactControl.s_noComboTexture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(Vector3.Multiply(new Vector3((float) ((double) FruitFactControl.s_noComboTexture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) FruitFactControl.s_noComboTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX + 1.0), 0.0f), num));
            MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, new Vector3(-8f, 37f, 0.0f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            FruitFactControl.s_noComboTexture.UnSet();
          }
        }
        else if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && this.m_mode == 1)
        {
          flag = true;
          this.DrawLeaderboard();
        }
        else if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && this.m_mode == 0)
        {
          flag = true;
          if (FruitFactControl.s_bigBonusBoardTexture != null)
          {
            FruitFactControl.s_bigBonusBoardTexture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(this.m_scale);
            MatrixManager.GetInstance().Translate(
                Vector3.Subtract(this.m_pos, new Vector3(8f, -8f, 0.0f)));

            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            FruitFactControl.s_bigBonusBoardTexture.UnSet();
          }
          LinkedListNode<Bonus> it = (LinkedListNode<Bonus>) null;
          Bonus bonus = BonusManager.GetInstance().GetFirstBestBonus(ref it);
          Color[] colorArray = new Color[3]
          {
            new Color(173, 126, 0),
            new Color(160, 5, 5),
            new Color(1, 92, 149)
          };
          float num = 20f;
          Vector3 pos = Vector3.Add(this.m_pos, new Vector3(-114f, 58f, 0.0f));
          int index = 0;
          while (bonus != null)
          {
            Game2.game_work.pGameFont.DrawString(bonus.GetText(), pos, colorArray[index], 16f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_LEFT);
            string stringToDraw = string.Format("{0}", (object) bonus.GetPoints());
            Game2.game_work.pGameFont.DrawString(stringToDraw, Vector3.Add(pos, Vector3.Multiply(Vector3.UnitX, 209f)), colorArray[index], 16f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
            Matrix mtx;
            Mortar.Math.Scale44((float) bonus.texture.GetWidth() * 0.5f * Game2.GAME_MODE_SCALE_FIX, (float) bonus.texture.GetHeight() * 0.5f * Game2.GAME_MODE_SCALE_FIX, 0.0f, out mtx);
            Mortar.Math.GlobalTranslate44(ref mtx, Vector3.Subtract(pos, Vector3.Multiply(Vector3.UnitX, 20f)));
            MatrixManager.GetInstance().SetMatrix(mtx);
            MatrixManager.GetInstance().UploadCurrentMatrices();
            bonus.texture.Set();
            Mesh.DrawQuad(colorArray[index], 0.0f, 1f, 0.0f, 1f);
            bonus.texture.UnSet();
            bonus = BonusManager.GetInstance().GetNextBestBonus(ref it);
            pos.Y -= num;
            index = Mortar.Math.MIN(index + 1, colorArray.Length);
          }
                    Game2.game_work.pGameFont.DrawString(Mortar.Game1.instance.stringTable.GetString(139), this.m_pos.X - 8f, this.m_pos.Y - (Game2.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH || Game2.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH_UK ? 0.0f : 4f), 0.0f, this.m_fruitColor, 16f, 0.0f, 0.0f, ALIGNMENT_TYPE.ALIGN_HCENTER);
          Game2.game_work.pGameFont.DrawString(this.m_text, (float) ((double) this.m_pos.X - (double) sbyte.MaxValue - 8.0), (float) ((double) this.m_pos.Y - 40.0 - (Game2.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH || Game2.game_work.language == StringTableUtils.Language.LANGUAGE_ENGLISH_UK ? 0.0 : 4.0)), 0.0f, new Color(116, 93, 59), 16f, (float) byte.MaxValue, 0.0f, ALIGNMENT_TYPE.ALIGN_CENTER);
        }
        else
        {
          if (this.m_texture != null)
          {
            this.m_texture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(this.m_scale);
            MatrixManager.GetInstance().Translate(Vector3.Subtract(this.m_pos, new Vector3(8f, -8f, 0.0f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            this.m_texture.UnSet();
          }
          Game2.game_work.pGameFont.DrawString(this.m_text, (float) ((double) this.m_pos.X - 64.0 - 5.0), this.m_pos.Y - 14f, 0.0f, new Color(116, 93, 59), 16f, 128f, 0.0f, ALIGNMENT_TYPE.ALIGN_CENTER);
          if (this.m_fruitTexture != null)
          {
            this.m_fruitTexture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3((float) ((double) this.m_fruitTexture.GetWidth() * (double) Game2.GAME_MODE_SCALE_FIX * (double) Game2.SCREEN_SCALE_X * 1.5), (float) ((double) this.m_fruitTexture.GetHeight() * (double) Game2.GAME_MODE_SCALE_FIX * (double) Game2.SCREEN_SCALE_X * 1.5), 0.0f));
            MatrixManager.GetInstance().Translate(Vector3.Add(Vector3.Add(this.m_pos, this.m_fruitOffset), new Vector3(-12f, 24f, 0.0f)));
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
            this.m_fruitTexture.UnSet();
          }
        }
        if (FruitFactControl.s_senseiHead == null || !flag)
          return;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
      }
    }

    private Vector3 ConvertPos(Vector3 pos)
    {
      Vector3 zero = Vector3.Zero;
      pos.X -= Game2.game_work.camera.m_cameraShake.X;
      pos.Y -= Game2.game_work.camera.m_cameraShake.Y;
      Vector3 vector3 = pos;
      vector3.X += 240f;
      vector3.Y += 160f;
      vector3.Y = 320f - vector3.Y;
      return Vector3.Multiply(vector3, 1.6f);
    }

    private void LeftButton()
    {
      --this.m_mode;
      if (this.m_mode >= 0)
        return;
      this.m_mode = FruitFactControl.NUM_ARCADE_MODE_MODES - 1;
    }

    private void RightButton()
    {
      ++this.m_mode;
      if (this.m_mode < FruitFactControl.NUM_ARCADE_MODE_MODES)
        return;
      this.m_mode = 0;
    }

    public override void Update(float dt)
    {
      this.m_loadingSymbolTime += dt * 8f;
      if ((double) this.m_loadingSymbolTime >= 8.0)
        this.m_loadingSymbolTime = 0.0f;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE)
      {
        if (this.m_buttons[0] == null)
        {
          this.m_buttons[0] = new MenuButton(FruitFactControl.s_buttonTexture, this.m_pos, new MenuButton.MenuCallback(this.LeftButton));
          this.m_buttons[0].m_selfCleanUp = true;
          this.m_buttons[0].Init();
          this.m_buttons[0].m_uvs[0].X = 1f;
          this.m_buttons[0].m_uvs[1].X = 0.0f;
          Game2.game_work.hud.AddControl((HUDControl) this.m_buttons[0]);
        }
        if (this.m_buttons[1] == null)
        {
          this.m_buttons[1] = new MenuButton(FruitFactControl.s_buttonTexture, this.m_pos, new MenuButton.MenuCallback(this.RightButton));
          this.m_buttons[1].Init();
          this.m_buttons[1].m_selfCleanUp = true;
          Game2.game_work.hud.AddControl((HUDControl) this.m_buttons[1]);
        }
        this.m_buttons[0].m_pos = Vector3.Add(this.m_pos, new Vector3(-158f, 8f, 0.0f));
        this.m_buttons[1].m_pos = Vector3.Add(this.m_pos, new Vector3(142f, 8f, 0.0f));
        if (FruitFactControl.m_leaderboardList == null)
        {
          FruitFactControl.m_leaderboardList = new LeaderboardList();
          FruitFactControl.m_leaderboardList.Init();
          FruitFactControl.m_leaderboardList.SetItemHeight(FruitFactControl.LEADERBOARD_Y_DIFF);
          FruitFactControl.m_leaderboardList.SetWidth(Game2.SCREEN_WIDTH / 2f);
          FruitFactControl.m_leaderboardList.SetHeight(FruitFactControl.LEADERBOARD_Y_DIFF * 3f);
          FruitFactControl.m_leaderboardList.m_pos = new Vector3(75f, 45f, 0.0f);
          FruitFactControl.m_leaderboardList.m_initialTouchRegion.top = FruitFactControl.LEADERBOARD_Y_DIFF * 1f;
          FruitFactControl.m_leaderboardList.m_initialTouchRegion.bottom = (float) (-(double) FruitFactControl.LEADERBOARD_Y_DIFF * 2.0);
          Game2.game_work.hud.AddControl((HUDControl) FruitFactControl.m_leaderboardList);
        }
        if (FruitFactControl.m_leaderboardList != null)
          FruitFactControl.m_leaderboardList.m_update = false;
        if (this.m_leaderboardButton != null)
          this.m_leaderboardButton.m_update = false;
      }
      if (FruitFactControl.USE_ZEN_FACT_SCREEN)
      {
        if ((double) Game2.game_work.gameOverTransition > 0.75 && (double) this.m_fruitComboTime < (double) this.m_numComboFruits)
        {
          float num = this.m_fruitComboTime - (float) (int) this.m_fruitComboTime;
          this.m_fruitComboTime += dt * 4f;
          if ((double) this.m_fruitComboTime > 0.0 && (double) num < 0.5 && (double) this.m_fruitComboTime - (double) (int) this.m_fruitComboTime >= 0.5)
          {
            string filename = string.Format("popup-{0}", (object) Mortar.Math.MIN((int) this.m_fruitComboTime + 1, 8));
            SoundManager.GetInstance().SFXPlay(filename);
          }
          if ((double) this.m_fruitComboTime < (double) this.m_numComboFruits)
            ;
        }
        else
        {
          if ((double) this.m_fruitComboTime < (double) this.m_numComboFruits || (double) this.m_fruitComboTime >= (double) (this.m_numComboFruits + 1))
            return;
          float num = this.m_fruitComboTime - (float) (int) this.m_fruitComboTime;
          this.m_fruitComboTime += dt * 2f;
          if (this.m_stickerTexture == null || (double) this.m_fruitComboTime <= 0.0 || (double) num >= 0.5 || (double) this.m_fruitComboTime - (double) (int) this.m_fruitComboTime < 0.5)
            return;
          SoundManager.GetInstance().SFXPlay("achievement");
        }
      }
      else if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && this.m_mode == 1)
      {
        this.UpdateLeaderboard();
      }
      else
      {
        if (Game2.game_work.gameMode != Game2.GAME_MODE.GM_ARCADE || this.m_mode != 0)
          return;
        LinkedListNode<Bonus> it = (LinkedListNode<Bonus>) null;
        if (BonusManager.GetInstance().GetFirstBestBonus(ref it) != null)
          return;
        BonusManager.GetInstance().SetUpBonusScreen((BonusScreen) null);
      }
    }

    private void UpdateLeaderboard()
    {
      if (FruitFactControl.m_leaderboardList != null)
        FruitFactControl.m_leaderboardList.m_pos = Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.Multiply(Vector3.UnitY, FruitFactControl.LEADERBOARD_Y_DIFF), 1.5f));
      if ((this.m_leaderboardState == FruitFactControl.FFL.FFL_NO_FRIENDS || this.m_leaderboardState == FruitFactControl.FFL.FFL_OFFLINE) && this.m_leaderboardButton != null)
      {
        this.m_leaderboardButton.m_update = true;
        this.m_leaderboardButton.m_pos = Vector3.Subtract(this.m_pos, new Vector3(8f, 44f, 0.0f));
      }
            //SignedInGamer signedInGamer1 = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      //if (signedInGamer1 == null || signedInGamer1 != null && !signedInGamer1.IsSignedInToLive || !Mortar.Game1.logInSucceeded)
      //{
      //  this.m_leaderboardState = FruitFactControl.FFL.FFL_OFFLINE;
      //  this.m_texture = FruitFactControl.s_bigLeaderBoardDialogTexture;
      //}
      switch (this.m_leaderboardState)
      {
        case FruitFactControl.FFL.FFL_NO_FRIENDS:
          if (this.m_leaderboardButton == null)
            break;
          break;
        case FruitFactControl.FFL.FFL_DOWNLOADING:
          //SignedInGamer signedInGamer2 = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
          //if (signedInGamer2 == null || signedInGamer2 != null && !signedInGamer2.IsSignedInToLive)
          //{
          //  this.m_leaderboardState = FruitFactControl.FFL.FFL_OFFLINE;
          //  break;
          //}
          if (FruitFactControl.m_leaderboardList != null)
          {
            this.m_texture = FruitFactControl.s_bigLeaderBoardDialogTexture;
            if (!this.start_download)
            {
              Leaderboards.StartRead(3, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
              this.start_download = true;
            }
            if (this.download_complete)
            {
              if (this.download_result == FruitFactControl.DownloadResult.Success)
              {
                this.m_leaderboardState = FruitFactControl.FFL.FFL_LEADERBOARD;
                this.m_texture = FruitFactControl.s_bigLeaderBoardTexture;
                break;
              }
              this.m_leaderboardState = FruitFactControl.FFL.FFL_DOWNLOAD_FAILED;
              break;
            }
            break;
          }
          break;
        case FruitFactControl.FFL.FFL_LEADERBOARD:
          if (FruitFactControl.m_leaderboardList != null)
          {
            FruitFactControl.m_leaderboardList.m_update = true;
            break;
          }
          break;
      }
      if (this.m_leaderboardButton == null)
        return;
      if (this.m_leaderboardButton.m_texture == null)
        this.m_leaderboardButton.SetActive(false);
      else
        this.m_leaderboardButton.SetActive(true);
    }

    private void ReadFinished(int result)
    {
      List<LeaderboardsScreen.LeaderboardData> leaderboardDataList = new List<LeaderboardsScreen.LeaderboardData>();
      this.download_complete = true;
      //SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      LeaderboardReader reader = Leaderboards.GetReader();
      if (reader != null)
      {
        this.download_result = FruitFactControl.DownloadResult.Success;
        for (int index1 = 0; index1 < reader.Entries.Count; ++index1)
        {
          LeaderboardEntry entry = reader.Entries[index1];
                    int valueInt32 = 0;//entry.Columns.GetValueInt32("BestScore");
          int num1 = valueInt32 >> 24;
          int num2 = valueInt32 >> 16 & (int) byte.MaxValue;
          int num3 = valueInt32 & (int) ushort.MaxValue;
          int num4 = DateTime.Now.DayOfYear / 7;
          int num5 = DateTime.Now.Year - 2000;
          if (leaderboardDataList.Count == 0)
            leaderboardDataList.Add(new LeaderboardsScreen.LeaderboardData()
            {
              scoreAsInt = Mortar.Game1.settings.bestThisWeek,
              score = Mortar.Game1.settings.bestThisWeek.ToString(),
              isPlayer = true,
              gamerTag = "tag"//signedInGamer.Gamertag
            });

          /*if (string.Compare(entry.Gamer.Gamertag, signedInGamer.Gamertag) != 0)
          {
            LeaderboardsScreen.LeaderboardData leaderboardData1 = new LeaderboardsScreen.LeaderboardData();
            leaderboardData1.gotWeekly = num4 == num2 && num5 == num1;
            leaderboardData1.gamerTag = "gamertag";//entry.Gamer.Gamertag;
            leaderboardData1.isPlayer = false;
            if (leaderboardData1.gotWeekly)
            {
              leaderboardData1.score = num3.ToString();
              leaderboardData1.scoreAsInt = num3;
            }
            else
            {
              leaderboardData1.score = "0";
              leaderboardData1.scoreAsInt = 0;
            }
            int index2 = 0;
            bool flag = false;
            foreach (LeaderboardsScreen.LeaderboardData leaderboardData2 in leaderboardDataList)
            {
              if (leaderboardData1.scoreAsInt > leaderboardData2.scoreAsInt)
              {
                leaderboardDataList.Insert(index2, leaderboardData1);
                flag = true;
                break;
              }
              ++index2;
            }
            if (!flag)
              leaderboardDataList.Add(leaderboardData1);
          }*/
        }
        int num6 = 0;
        int num7 = 0;
        if (FruitFactControl.m_leaderboardList == null)
          return;
        foreach (LeaderboardsScreen.LeaderboardData leaderboardData in leaderboardDataList)
        {
          FruitFactControl.FriendLeaderboardItem friendLeaderboardItem = new FruitFactControl.FriendLeaderboardItem(leaderboardData.gamerTag, num6 + 1, leaderboardData.scoreAsInt);
          FruitFactControl.m_leaderboardList.AddItem((ScrollingMenuItem) friendLeaderboardItem);
          ++num6;
          if (leaderboardData.isPlayer)
          {
            friendLeaderboardItem.isPlayer = true;
            num7 = num6;
          }
        }
        if (num6 < 3)
        {
          FruitFactControl.m_leaderboardList.SetHeight(FruitFactControl.LEADERBOARD_Y_DIFF * (float) num6);
          FruitFactControl.m_leaderboardList.m_disabled = true;
        }
        int v = num7 - 1;
        FruitFactControl.m_leaderboardList.m_offset.Y = (float) -Mortar.Math.CLAMP(v, 0, FruitFactControl.m_leaderboardList.GetNumItems() - 1) * FruitFactControl.LEADERBOARD_Y_DIFF;
      }
      else
        this.download_result = FruitFactControl.DownloadResult.Failed;
    }

    private void DrawXboxLive()
    {
      Texture leaderboardTitleTexture = FruitFactControl.GetLeaderboardTitleTexture();
      leaderboardTitleTexture.Set();
      Matrix scale = Matrix.CreateScale((float) leaderboardTitleTexture.GetWidth() * 0.7249971f, (float) leaderboardTitleTexture.GetHeight() * 0.7249971f, 0.0f);
      Matrix matrix = scale;
      Matrix mtx1 = Matrix.Multiply(scale, Matrix.CreateTranslation(
          Vector3.Subtract(this.m_pos, new Vector3(8f, -48f, 0.0f))));
      Matrix mtx2 = Matrix.Multiply(matrix, Matrix.CreateTranslation(
          Vector3.Subtract(this.m_pos, new Vector3(7f, -47f, 0.0f))));
      MatrixManager.GetInstance().SetMatrix(mtx2);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, 
          (int) sbyte.MaxValue, (int) sbyte.MaxValue));
      MatrixManager.GetInstance().SetMatrix(mtx1);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(Color.White);
      leaderboardTitleTexture.UnSet();
    }

    private void DrawLeaderboard()
    {
      if (this.m_texture != null)
      {
        this.m_texture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(this.m_scale);
        MatrixManager.GetInstance().Translate(Vector3.Subtract(this.m_pos, 
            new Vector3(8f, -8f, 0.0f)));

        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_texture.UnSet();
      }
      string str1 = (string) null;
      switch (this.m_leaderboardState)
      {
        case FruitFactControl.FFL.FFL_NO_FRIENDS:
          str1 = Mortar.Game1.instance.stringTable.GetString(160);
          this.DrawXboxLive();
          break;
        case FruitFactControl.FFL.FFL_OFFLINE:
          str1 = Mortar.Game1.instance.stringTable.GetString(867);
          this.DrawXboxLive();
          break;
        case FruitFactControl.FFL.FFL_DOWNLOADING:
          if ((double) this.m_loadingSymbolTime >= 0.0 && FruitFactControl.s_loadingTexture != null)
          {
            this.DrawDownloadIcon();
            string stringToDraw = Mortar.Game1.instance.stringTable.GetString(99);
            Game2.game_work.pGameFont.DrawString(stringToDraw, Vector3.Add(this.m_pos, new Vector3(-125f, 27f, 0.0f)), new Color(72, 44, 21), 18f, new Vector2(234f, 0.0f), ALIGNMENT_TYPE.ALIGN_CENTER);
            break;
          }
          break;
        case FruitFactControl.FFL.FFL_DOWNLOAD_FAILED:
          str1 = Mortar.Game1.instance.stringTable.GetString(867);
          this.DrawXboxLive();
          break;
      }
      float num1 = 294f;
      if (str1 == null)
        return;
      Color color = new Color(79, 159, 0);
      Vector2 vector2_1 = Mortar.Game1.instance.font3.MeasureString(" ");
      string[] strArray = str1.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      string str2 = "";
      string str3 = "";
      string str4 = "";
      float num2 = 0.0f;
      int num3 = 0;
      foreach (string str5 in strArray)
      {
        string str6 = str5.Trim();
        Vector2 vector2_2 = Mortar.Game1.instance.font3.MeasureString(str6);
        if ((double) num2 + (double) vector2_2.X + (double) vector2_1.X <= (double) num1)
        {
          num2 += vector2_2.X + vector2_1.X;
        }
        else
        {
          num2 = vector2_2.X + vector2_1.X;
          ++num3;
        }
        switch (num3)
        {
          case 0:
            str2 = str2 + str5 + " ";
            break;
          case 1:
            str3 = str3 + str5 + " ";
            break;
          default:
            str4 = str4 + str5 + " ";
            break;
        }
      }
            Mortar.Game1.instance.spriteBatch.Begin();
      float num4 = this.m_pos.X + 440f;
      float num5 = 140f;
      float num6 = 24f;
      if (str2.Length > 0)
      {
        string str7 = str2.Trim();
        Vector2 vector2_3 = Mortar.Game1.instance.font3.MeasureString(str7);
                Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str7, new Vector2(num4 - vector2_3.X / 2f, num5), color);
        num5 += num6;
      }
      if (str3.Length > 0)
      {
        string str8 = str3.Trim();
        Vector2 vector2_4 = Mortar.Game1.instance.font3.MeasureString(str8);
                Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str8, new Vector2(num4 - vector2_4.X / 2f, num5), color);
        num5 += num6;
      }
      if (str4.Length > 0)
      {
        string str9 = str4.Trim();
        Vector2 vector2_5 = Mortar.Game1.instance.font3.MeasureString(str9);
                Mortar.Game1.instance.spriteBatch.DrawString(Mortar.Game1.instance.font3, str9, new Vector2(num4 - vector2_5.X / 2f, num5), color);
        float num7 = num5 + num6;
      }
            Mortar.Game1.instance.spriteBatch.End();
    }

    private static Texture GetLeaderboardTitleTexture() => FruitFactControl.s_xboxLive;

    private void DrawDownloadIcon()
    {
      int num1 = 7 - (int) this.m_loadingSymbolTime % 8;
      if (true)
      {
        float num2 = 0.6f;
        float num3 = 0.075f;
        for (int index1 = 0; index1 < 8; ++index1)
        {
          Vector3 vector3_1 = new Vector3(Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX((float) (index1 * 45))) * 0.5f, Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX((float) (index1 * 45))) * 0.5f, 0.0f);
          Vector3 vector3_2 = new Vector3(Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX((float) (index1 * 45 + 90))) * num3, Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX((float) (index1 * 45 + 90))) * num3, 0.0f);
          FruitFactControl.symbo_tris[index1 * 6].X = vector3_1.X - vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6].Y = vector3_1.Y - vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6].u = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6].v = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6 + 1].X = vector3_1.X + vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6 + 1].Y = vector3_1.Y + vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6 + 1].u = 1f;
          FruitFactControl.symbo_tris[index1 * 6 + 1].v = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6 + 2].X = vector3_1.X * num2 - vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6 + 2].Y = vector3_1.Y * num2 - vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6 + 2].u = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6 + 2].v = 1f;
          FruitFactControl.symbo_tris[index1 * 6 + 3].X = vector3_1.X * num2 - vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6 + 3].Y = vector3_1.Y * num2 - vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6 + 3].u = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6 + 3].v = 1f;
          FruitFactControl.symbo_tris[index1 * 6 + 4].X = vector3_1.X + vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6 + 4].Y = vector3_1.Y + vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6 + 4].u = 1f;
          FruitFactControl.symbo_tris[index1 * 6 + 4].v = 0.0f;
          FruitFactControl.symbo_tris[index1 * 6 + 5].X = vector3_1.X * num2 + vector3_2.X;
          FruitFactControl.symbo_tris[index1 * 6 + 5].Y = vector3_1.Y * num2 + vector3_2.Y;
          FruitFactControl.symbo_tris[index1 * 6 + 5].u = 1f;
          FruitFactControl.symbo_tris[index1 * 6 + 5].v = 1f;
          for (int index2 = 0; index2 < 6; ++index2)
          {
            FruitFactControl.symbo_tris[index1 * 6 + index2].nz = 1f;
            FruitFactControl.symbo_tris[index1 * 6 + index2].Z = 0.0f;
          }
        }
      }
      for (int index3 = 0; index3 < 8; ++index3)
      {
        int num4 = Mortar.Math.CLAMP((num1 + index3) % 8 * 32, 64, (int) byte.MaxValue);
        Color color = new Color(num4, num4, num4, 200);
        for (int index4 = 0; index4 < 6; ++index4)
          FruitFactControl.symbo_tris[index3 * 6 + index4].color = color;
      }
      Vector3 vector3 = new Vector3(-7f, -23f, 0.0f);
      FruitFactControl.s_loadingTexture.Set();
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3(32f, 32f, 0.0f));
      MatrixManager.GetInstance().Translate(Vector3.Add(this.m_pos, vector3));
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawTriList(FruitFactControl.symbo_tris, 48, false);
      FruitFactControl.s_loadingTexture.UnSet();
    }

    public override void Reset()
    {
    }

    public static void LoadContent()
    {
      if (FruitFactControl.contentLoaded)
        return;
      FruitFactControl.s_boardTexture = TextureManager.GetInstance().Load("fact_board.tex", true);
      FruitFactControl.s_bigBoardTexture = TextureManager.GetInstance().Load("diolog_box_big.tex", true);
      FruitFactControl.s_noComboTexture = TextureManager.GetInstance().Load("combo_description.tex", true);
      FruitFactControl.s_senseiHead = TextureManager.GetInstance().Load("textureswp7/sensei_head.tex");
      FruitFactControl.contentLoaded = true;
      FruitFactControl.s_buttonTexture = TextureManager.GetInstance().Load("textureswp7/arcade_results_arrow.tex");
      FruitFactControl.s_bigLeaderBoardTexture = TextureManager.GetInstance().Load("arcade_results_score_box.tex", true);
      FruitFactControl.s_bigLeaderBoardDialogTexture = TextureManager.GetInstance().Load("arcade_results_diolog_box.tex", true);
      FruitFactControl.s_bigBonusBoardTexture = TextureManager.GetInstance().Load("arcade_results_bonus_box.tex", true);
      FruitFactControl.s_loadingTexture = TextureManager.GetInstance().Load("textureswp7/blurry_backing.tex");
      FruitFactControl.s_xboxLive = TextureManager.GetInstance().Load("textureswp7/xboxliveonwindowsphone_4cko_s.tex");
      FruitFactControl.s_youTexture = TextureManager.GetInstance().Load("score_you.tex", true);
      FruitFactControl.s_noScoreTexture = TextureManager.GetInstance().Load("no_score_this_week.tex", true);
    }

    public static void UnLoadContent()
    {
      FruitFactControl.s_boardTexture = (Texture) null;
      FruitFactControl.s_bigBoardTexture = (Texture) null;
      FruitFactControl.s_noComboTexture = (Texture) null;
      FruitFactControl.s_senseiHead = (Texture) null;
      FruitFactControl.s_buttonTexture = (Texture) null;
      FruitFactControl.s_bigLeaderBoardTexture = (Texture) null;
      FruitFactControl.s_bigLeaderBoardDialogTexture = (Texture) null;
      FruitFactControl.s_bigBonusBoardTexture = (Texture) null;
      FruitFactControl.s_youTexture = (Texture) null;
      FruitFactControl.s_noScoreTexture = (Texture) null;
    }

    public bool LeftPressed(InputEvent evnt)
    {
      --this.m_fact;
      if (this.m_fact < 0)
        this.m_fact = Fruit.FruitInfo(this.m_fruitType).factCount - 1;
      this.m_text = Fruit.GetFact((Action<int>) null, (Action<int>) null, this.m_fruitType, this.m_fact);
      return true;
    }

    public bool RightPressed(InputEvent evnt)
    {
      ++this.m_fact;
      if (this.m_fact >= Fruit.FruitInfo(this.m_fruitType).factCount)
        this.m_fact = 0;
      this.m_text = Fruit.GetFact((Action<int>) null, (Action<int>) null, this.m_fruitType, this.m_fact);
      return true;
    }

    public bool UpPressed(InputEvent evnt)
    {
      do
      {
        ++this.m_fruitType;
        if (this.m_fruitType >= Fruit.MAX_FRUIT_TYPES)
          this.m_fruitType = 0;
      }
      while (Fruit.FruitInfo(this.m_fruitType).factCount <= 0);
      this.m_text = Fruit.GetFact((Action<int>) null, (Action<int>) (v => this.m_fact = v), this.m_fruitType);
      this.m_fruitColor = Fruit.FruitFactColor(this.m_fruitType);
      string texture = string.Format("textureswp7/{0}.tex", (object) Fruit.FruitFactTexture(this.m_fruitType));
      this.m_fruitTexture = TextureManager.GetInstance().Load(texture);
      return true;
    }

    public bool DownPressed(InputEvent evnt)
    {
      do
      {
        --this.m_fruitType;
        if (this.m_fruitType < 0)
          this.m_fruitType = Fruit.MAX_FRUIT_TYPES - 1;
      }
      while (Fruit.FruitInfo(this.m_fruitType).factCount <= 0);
      this.m_text = Fruit.GetFact((Action<int>) null, (Action<int>) (v => this.m_fact = v), this.m_fruitType);
      this.m_fruitColor = Fruit.FruitFactColor(this.m_fruitType);
      string texture = string.Format("textureswp7/{0}.tex", (object) Fruit.FruitFactTexture(this.m_fruitType));
      this.m_fruitTexture = TextureManager.GetInstance().Load(texture);
      return true;
    }

    public enum FFL
    {
      FFL_NO_FRIENDS,
      FFL_OFFLINE,
      FFL_DOWNLOADING,
      FFL_LEADERBOARD,
      FFL_DOWNLOAD_FAILED,
    }

    public enum DownloadResult
    {
      Pending,
      Success,
      Failed,
    }

    private class FriendLeaderboardItem : LeaderboardItem
    {
      public bool isPlayer;
      private Vector3 o;

      public FriendLeaderboardItem() => this.isPlayer = false;

      public FriendLeaderboardItem(string name, int rank, int score)
        : base(name, rank, score)
      {
        this.isPlayer = false;
        this.m_rank = rank;
        this.m_score = score;
        this.m_height = FruitFactControl.LEADERBOARD_Y_DIFF;
        this.m_colour = new Color(116, 93, 59);
        this.m_textOffset = Vector3.Multiply(Vector3.UnitX, -68f);
        this.m_text = name.ToUpper();
      }

      public float AdjustScale()
      {
        Vector3 vector3_1 = Vector3.Add(this.m_pos, this.m_textOffset);
        float num1 = 23f;
        Vector3 vector3_2 = Vector3.Add(vector3_1, Vector3.Multiply(Vector3.UnitX, 115f));
        this.o = Vector3.Zero;
        float num2 = num1;
        if ((double) vector3_2.Y > 114.0)
        {
          float num3 = vector3_2.Y - 114f;
          num2 = Mortar.Math.MAX(0.0f, num2 - num3);
          this.o.Y = -num3;
        }
        else if ((double) vector3_2.Y < -10.0)
        {
          float num4 = -10f - vector3_2.Y;
          num2 = Mortar.Math.MAX(0.0f, num2 - num4);
          this.o.Y = num4;
        }
        return num2;
      }

      private Color TintColour(Color col, float[] tints)
      {
        col.R = (byte) Mortar.Math.CLAMP((float) col.R * tints[0], 0.0f, (float) byte.MaxValue);
        col.G = (byte) Mortar.Math.CLAMP((float) col.G * tints[1], 0.0f, (float) byte.MaxValue);
        col.B = (byte) Mortar.Math.CLAMP((float) col.B * tints[2], 0.0f, (float) byte.MaxValue);
        return col;
      }

      public override void Draw()
      {
        Vector3 pos = Vector3.Add(this.m_pos, this.m_textOffset);
        if (this.m_rank < 0 || !this.m_isOnScreen)
          return;
        float num1 = 23f;
        float num2 = Game2.game_work.pGameFont.MeasureString(this.m_text);
        if ((double) num2 > 145.0)
          num1 *= 145f / num2;
        MortarRectangleDec? rect = new MortarRectangleDec?();
        MortarRectangleDec rectangle;
        rectangle.left = 0.0f;
        rectangle.right = 0.0f;
        rectangle.top = 0.0f;
        rectangle.bottom = 0.0f;
        if (this.m_parentList != null)
        {
          rectangle.top = 105f;
          rectangle.bottom = -30f;
          rectangle.left = -Game2.SCREEN_WIDTH;
          rectangle.right = Game2.SCREEN_WIDTH;
          rect = new MortarRectangleDec?(rectangle);
        }
        Color white = new Color(64, 34, 23);
        Font font = Game2.game_work.pGameFont;
        if (this.isPlayer)
        {
          num1 *= 1.2f;
          font = Game2.game_work.pNumberFontLeaderboard;
          white = Color.White;
          GameVertex[] gameVertexArray = new GameVertex[4];
          if (FruitFactControl.SetupQuad(gameVertexArray, pos, (float) FruitFactControl.s_youTexture.GetWidth(), (float) FruitFactControl.s_youTexture.GetHeight(), rectangle, Color.White))
          {
            FruitFactControl.s_youTexture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().UploadCurrentMatrices();
            Mesh.DrawTriStrip(gameVertexArray, 4);
            FruitFactControl.s_youTexture.UnSet();
          }
        }
        else
        {
          float num3 = this.AdjustScale();
          Game2.game_work.pGameFont.DrawString(this.m_text, Vector3.Add(pos, this.o), white, new Vector2(num1, num3), Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER, 1f, rect);
        }
        if (this.m_score > 0)
        {
          float num4 = this.AdjustScale();
          font.DrawString(this.m_score.ToString(), Vector3.Add(Vector3.Add(pos, Vector3.Multiply(Vector3.UnitX, 115f)), this.o), white, new Vector2(num1, num4), Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER, 1f, rect);
          float[] tints = new float[3]{ 5f, 5f, 5f };
          Game2.game_work.pNumberFontLeaderboard.DrawString(this.m_rank.ToString(), Vector3.Add(Vector3.Add(pos, Vector3.Multiply(Vector3.UnitX, 175f)), this.o), this.TintColour(white, tints), new Vector2(num1, num4), Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER, 1f, rect);
        }
        else
        {
          rectangle.top -= 16f;
          rectangle.bottom -= 16f;
          GameVertex[] gameVertexArray = new GameVertex[4];
          if (!FruitFactControl.SetupQuad(gameVertexArray, Vector3.Add(Vector3.Add(pos, 
              Vector3.Divide(Vector3.Multiply(Vector3.UnitX, 290f), 2f)), new Vector3(-4f, 0.0f, 0.0f)), (float) (FruitFactControl.s_noScoreTexture.GetWidth() / 2U), (float) (FruitFactControl.s_noScoreTexture.GetHeight() / 2U), rectangle, Color.White))
            return;
          FruitFactControl.s_noScoreTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawTriStrip(gameVertexArray, 4);
          FruitFactControl.s_noScoreTexture.UnSet();
        }
      }
    }
  }

    internal class LeaderboardEntry
    {
        internal object Columns;
        internal object Gamer;
    }

    internal class LeaderboardReader
    {
        internal List<LeaderboardEntry> Entries;

        //internal static void BeginRead(LeaderboardIdentity id, Gamer signedInGamer1, int v, AsyncCallback asyncCallback, object signedInGamer2)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
