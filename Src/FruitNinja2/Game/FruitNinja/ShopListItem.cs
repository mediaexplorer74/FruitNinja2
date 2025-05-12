
// Type: GameManager.ShopListItem
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class ShopListItem : ScrollingMenuItem
  {
    protected ShopScreen m_shop;
    protected string m_description;
    protected float m_newFade;
    protected float m_selectedFade;
    public float m_shakeTime;
    public Vector3 m_texturePos;
    public Texture m_texture;
    public ItemInfo m_info;
    public bool m_backing;
    public static bool s_isContentLoaded = false;
    public static Texture blankTex = (Texture) null;
    public static Texture descriptionBox = (Texture) null;
    public static Texture backGround = (Texture) null;
    public static Texture dividerTex = (Texture) null;
    public static Texture buyNowTexture = (Texture) null;
    public static Texture equipNowTexture = (Texture) null;
    public static Texture unequipNowTexture = (Texture) null;
    public static Texture swipe_to_scroll = (Texture) null;
    public static Texture coinsBuyTexture = (Texture) null;
    public static Texture coinsSpendTexture = (Texture) null;
    public static Texture selectedTexture = (Texture) null;
    public static Texture selectedSmallTexture = (Texture) null;
    public static Texture lockedStrokeTexture = (Texture) null;
    public static Texture newStrokeTexture = (Texture) null;
    public static Texture newItemTexture = (Texture) null;
    public static float newItemBob = 0.0f;
    public static ushort newItemBobTime = 0;
    public bool isFirst;
    public bool isLast;
    public float m_fadeTime;
    private static int lastDrawnType;
    private static float[] typeNameLengths = new float[2];

    public ShopListItem()
    {
      this.m_shakeTime = 0.0f;
      this.m_info = (ItemInfo) null;
      this.m_texture = (Texture) null;
      this.m_backing = true;
      this.isFirst = false;
      this.isLast = false;
      this.m_fadeTime = 0.0f;
      this.m_newFade = 0.0f;
      this.m_selectedFade = 0.0f;
    }

    ~ShopListItem()
    {
      this.m_text = (string) null;
      this.m_texture = (Texture) null;
      this.m_backing = false;
    }

    public void Create(ItemInfo info, ShopScreen shop)
    {
      this.m_fadeTime = 0.0f;
      this.m_text = "";
      this.m_info = info;
      this.m_height = ShopScreen.SHOP_ITEM_HEIGHT;
      this.m_width = ShopScreen.SHOP_BACK_LEFT_SIDE;
      this.m_shop = shop;
      this.m_textOffset = new Vector3(60f, 13f, 0.0f);
      if (info.textureName != null)
      {
        string texture = info.type != ItemType.ITEM_BACKGROUND ? string.Format("textureswp7/{0}.tex", (object) info.textureName) : string.Format("textureswp7/item_{0}.tex", (object) info.textureName);
        this.m_texture = TextureManager.GetInstance().Load(texture);
      }
      if (this.m_info.IsLocked())
      {
        if (this.m_info.unlockTotal != null)
        {
          int num = Game2.game_work.saveData.GetTotal(StringFunctions.StringHash(this.m_info.unlockTotal));
          if (this.m_info.unlockCountDownFrom > 0)
            num = Math.MAX(0, this.m_info.unlockCountDownFrom - num);
          this.m_description = Mortar.Game1.instance.stringTable.GetString(this.m_info.unlockDescription);
          this.m_description = string.Format(this.m_description, (object) num);
        }
        else
          this.m_description = Mortar.Game1.instance.stringTable.GetString(this.m_info.unlockDescription);
      }
      else
        this.m_description = Mortar.Game1.instance.stringTable.GetString(this.m_info.shopDescription);
      if (ItemManager.GetInstance().IsEquipped(this.m_info))
        this.m_selectedFade = 1f;
      if (this.m_info.hasBeenSeen)
        return;
      this.m_newFade = 1f;
    }

    public static float BOB_TIME => 0.5f;

    public static float BOB_AMT => 6f;

    public override void Move(Vector3 toPos)
    {
      if (this.isFirst)
      {
        ShopListItem.newItemBob = Math.ABS(Math.SinIdx(ShopListItem.newItemBobTime) * ShopListItem.BOB_AMT);
        ShopListItem.newItemBobTime += (ushort) ((double) Game2.game_work.dt * ((double) Math.DEGREE_TO_IDX(180f) / (double) ShopListItem.BOB_TIME) / 2.0);
      }
      this.m_pos = toPos;
      if (this.m_texture != null)
      {
        this.m_texturePos = this.m_pos;
        this.m_texturePos.X += this.m_textOffset.X + 35.2f;
        if ((double) this.m_shakeTime > 0.0)
        {
          this.m_shakeTime -= Game2.game_work.dt;
          ShopListItem shopListItem = this;
          shopListItem.m_texturePos = Vector3.Add(shopListItem.m_texturePos, new Vector3(Math.g_random.RandF(ShopScreen.SHAKE_RANGE) - ShopScreen.SHAKE_RANGE / 2f, Math.g_random.RandF(ShopScreen.SHAKE_RANGE) - ShopScreen.SHAKE_RANGE / 2f, 0.0f));
        }
      }
      this.m_fadeTime = this.m_shop == null || this.m_shop.GetSelected() != this ? Math.MAX(0.0f, this.m_fadeTime - Game2.game_work.dt * 5f) : Math.MIN(1f, this.m_fadeTime + Game2.game_work.dt * 5f);
      this.m_newFade = Math.CLAMP(this.m_newFade + Game2.game_work.dt * (this.m_info.hasBeenSeen ? -5f : 5f), 0.0f, 1f);
      this.m_selectedFade = Math.CLAMP(this.m_selectedFade + Game2.game_work.dt * (ItemManager.GetInstance().IsEquipped(this.m_info) ? 5f : -5f), 0.0f, 1f);
    }

    public void ButtonClicked()
    {
      if (this.m_shop == null)
        return;
      this.m_shop.SetSelected(this);
    }

    public override void Draw()
    {
      if (this.isFirst)
        ShopListItem.lastDrawnType = -1;
      if (this.m_isOnScreen)
      {
        string[] strArray = new string[2]
        {
          Mortar.Game1.instance.stringTable.GetString(167),
          Mortar.Game1.instance.stringTable.GetString(166)
        };
        Font pGameFont = Game2.game_work.pGameFont;
        Color white = Color.White;
        if (this.m_info.IsLocked())
        {
          white = new Color(200, 200, 200, (int) byte.MaxValue);
        }

        Vector3 pos = Vector3.Add(this.m_pos, this.m_textOffset);
        float num = pGameFont.MeasureString(this.m_info.shopTitle) *
                    25f * ShopScreen.SHOP_TEXT_SCALE;
        pGameFont.DrawString(Mortar.Game1.instance.stringTable.GetString(this.m_info.shopTitle),
            Vector3.Add(pos, new Vector3(4f, -4f, 0.0f)), 
            new Color(0, 0, 0, 64), 25f * ShopScreen.SHOP_TEXT_SCALE, 
            Vector2.Zero, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);

        pGameFont.DrawString(Mortar.Game1.instance.stringTable.GetString(this.m_info.shopTitle),
            pos, white, 25f * ShopScreen.SHOP_TEXT_SCALE, Vector2.Zero,
            ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);
        float scale = 20f * ShopScreen.SHOP_TEXT_SCALE;
        pos.Y -= 26f;
        for (int index = 0; index < strArray.Length; ++index)
          ShopListItem.typeNameLengths[index] = pGameFont.MeasureString(strArray[index]) * scale;
        string stringToDraw = strArray[(int) this.m_info.type];
        if (stringToDraw != null)
        {
          pGameFont.DrawString(stringToDraw, Vector3.Add(pos, new Vector3(4f, -4f, 0.0f)), 
              new Color(0, 0, 0, 64), scale, Vector2.Zero,
              ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);

          pGameFont.DrawString(stringToDraw, pos, white, scale, Vector2.Zero,
              ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT);
        }
        if ((double) this.m_newFade > 0.0)
        {
          Matrix mtx = Matrix.Multiply(Matrix.CreateScale(
              Vector3.Multiply(
                  Vector3.Multiply(new Vector3(65f, 33f, 0.0f), this.m_newFade), 
                  this.m_newFade)), Matrix.CreateTranslation(new Vector3(
                      (float) (int) ((double) pos.X - (double) num - 4.0), 
                      (float) (int) ((double) pos.Y + 34.0 + (double) ShopListItem.newItemBob), 
                      0.0f)));

          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          ShopListItem.newItemTexture.Set();
          Mesh.DrawQuad(Color.White);
          ShopListItem.newItemTexture.UnSet();
        }
        if ((double) this.m_selectedFade > 0.0)
        {
          Matrix mtx = Matrix.Multiply(Matrix.CreateScale(Vector3.Multiply(Vector3.Multiply(new Vector3(65f, 33f, 0.0f), this.m_selectedFade), this.m_selectedFade)), Matrix.CreateTranslation(new Vector3((float) (int) ((double) pos.X - (double) ShopListItem.typeNameLengths[(int) this.m_info.type] - 32.0), pos.Y, 0.0f)));
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          ShopListItem.selectedSmallTexture.Set();
          Mesh.DrawQuad(Color.White);
          ShopListItem.selectedSmallTexture.UnSet();
        }
        if (this.m_texture != null)
        {
          Matrix mtx = Matrix.Multiply(Matrix.CreateScale(new Vector3(64f, 64f, 0.0f)), Matrix.CreateTranslation(this.m_texturePos));
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          if (this.m_info.IsLocked())
          {
            ShopListItem.lockedStrokeTexture.Set();
            Mesh.DrawQuad(Color.White);
            ShopListItem.lockedStrokeTexture.UnSet();
          }
          else
          {
            this.m_texture.Set();
            Mesh.DrawQuad(Color.White);
            this.m_texture.UnSet();
          }
        }
        Matrix mtx1 = Matrix.Multiply(Matrix.CreateScale(new Vector3(257f, 17f, 0.0f)), Matrix.CreateTranslation(Vector3.Add(this.m_pos, Vector3.Divide(Vector3.Multiply(Vector3.UnitY, this.m_height), 2f))));
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().SetMatrix(mtx1);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        ShopListItem.dividerTex.Set();
        if ((ItemType) ShopListItem.lastDrawnType != this.m_info.type)
        {
          ShopListItem.lastDrawnType = (int) this.m_info.type;
          Mesh.DrawQuad(new Color(128, 128, 128, (int) byte.MaxValue));
        }
        else
          Mesh.DrawQuad(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 200));
        ShopListItem.dividerTex.UnSet();
        if (this.isLast)
        {
          Matrix mtx2 = Matrix.Multiply(Matrix.CreateScale(new Vector3(257f, 17f, 0.0f)), Matrix.CreateTranslation(Vector3.Subtract(this.m_pos, Vector3.Divide(Vector3.Multiply(Vector3.UnitY, this.m_height), 2f))));
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx2);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          ShopListItem.dividerTex.Set();
          Mesh.DrawQuad(new Color(128, 128, 128, (int) byte.MaxValue));
          ShopListItem.dividerTex.UnSet();
        }
      }
      float v1 = this.m_pos.Y;
      if (this.isFirst)
        v1 = Math.MAX(v1, 0.0f);
      if (this.isLast)
        Math.MIN(v1, 0.0f);
      int num1 = Math.CLAMP((int) ((double) byte.MaxValue * (double) this.m_fadeTime), 0, (int) byte.MaxValue);
      if (this.m_shop != null && this.m_info != null && num1 > 0)
        Game2.game_work.pGameFont.DrawString(this.m_description, this.m_shop.GetDescriptionTextXPos(), 0.0f, 0.0f, this.m_info.IsLocked() ? new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, num1) : new Color(116, 93, 59, num1), 18f * ShopScreen.SHOP_TEXT_SCALE, ShopScreen.DESCRIPTION_BOX_WIDTH, 0.0f, ALIGNMENT_TYPE.ALIGN_CENTER);
      if (!this.isLast)
        return;
      ShopListItem.blankTex.Set();
      Matrix mtx3 = Matrix.Multiply(Matrix.CreateScale(new Vector3(ShopScreen.SHOP_BACK_LEFT_SIDE, ShopScreen.DARKNESS_HEIGHT, 0.0f)), Matrix.CreateTranslation(new Vector3(this.m_parentList.m_pos.X - 2f, (float) ((double) Game2.SCREEN_HEIGHT / 2.0 - (double) ShopScreen.DARKNESS_HEIGHT / 2.0 + 5.0), 0.0f)));
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().SetMatrix(mtx3);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(new Color(0, 0, 0, 128));
      Matrix mtx4 = Matrix.Multiply(Matrix.CreateScale(new Vector3(ShopScreen.SHOP_BACK_LEFT_SIDE, ShopScreen.DARKNESS_HEIGHT, 0.0f)), Matrix.CreateTranslation(new Vector3(this.m_parentList.m_pos.X - 2f, (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + (double) ShopScreen.DARKNESS_HEIGHT / 2.0 - 5.0), 0.0f)));
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().SetMatrix(mtx4);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(new Color(0, 0, 0, 128));
      ShopListItem.blankTex.UnSet();
    }
  }
}
