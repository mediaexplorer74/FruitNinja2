
// Type: FruitNinja2.PopOverControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  public class PopOverControl
  {
    private static PopOverControl _instance = new PopOverControl();
    private Texture backgroundTex;
    private float m_time;
    private static PopOverControl.POC m_state = PopOverControl.POC.OUT;
    public static bool IsInPopup = false;
    public PopOverControl.WhenIn whenIndel;

    public static PopOverControl Instance => PopOverControl._instance;

    public static PopOverControl.POC State => PopOverControl.m_state;

    public PopOverControl()
    {
      this.backgroundTex = TextureManager.GetInstance().Load("textureswp7/BG_store.tex");
      PopOverControl.m_state = PopOverControl.POC.OUT;
      this.m_time = 0.0f;
    }

    public static void Update(float dt) => PopOverControl._instance._Update(dt);

    private void _Update(float dt)
    {
      switch (PopOverControl.m_state)
      {
        case PopOverControl.POC.MOVING_IN:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.25);
          if ((double) this.m_time <= 0.99900001287460327)
            break;
          this.m_time = 1f;
          PopOverControl.m_state = PopOverControl.POC.IN;
          PopOverControl.IsInPopup = true;
          if (this.whenIndel == null)
            break;
          this.whenIndel();
          this.whenIndel = (PopOverControl.WhenIn) null;
          break;
        case PopOverControl.POC.MOVING_OUT:
          PopOverControl.IsInPopup = false;
          this.m_time *= 0.75f;
          if ((double) this.m_time >= 1.0 / 1000.0)
            break;
          this.m_time = 0.0f;
          PopOverControl.m_state = PopOverControl.POC.OUT;
          break;
      }
    }

    public static void DrawBack() => PopOverControl._instance._DrawBack();

    public static void DrawFront() => PopOverControl._instance._DrawFront();

    public void In()
    {
      if (PopOverControl.m_state == PopOverControl.POC.IN)
        return;
      PopOverControl.m_state = PopOverControl.POC.MOVING_IN;
    }

    public void In(PopOverControl.WhenIn del)
    {
      this.whenIndel = del;
      if (PopOverControl.m_state == PopOverControl.POC.IN)
        return;
      PopOverControl.m_state = PopOverControl.POC.MOVING_IN;
    }

    public void Out()
    {
      if (PopOverControl.m_state != PopOverControl.POC.OUT)
        PopOverControl.m_state = PopOverControl.POC.MOVING_OUT;
      PopOverControl.IsInPopup = false;
    }

    private void _DrawBack()
    {
      if (PopOverControl.m_state != PopOverControl.POC.IN)
        return;
      this._Draw();
    }

    private void _DrawFront()
    {
      if (PopOverControl.m_state != PopOverControl.POC.MOVING_IN && PopOverControl.m_state != PopOverControl.POC.MOVING_OUT)
        return;
      this._Draw();
    }

    public void _Draw()
    {
      float num1 = (float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) ShopScreen.SHOP_BACK_RIGHT_SIDE / 2.0);
      if ((double) this.m_time < 1.0)
      {
        float num2 = num1 + (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_RIGHT_SIDE * 1.5);
        this.backgroundTex.Set();
        float num3 = ShopScreen.SHOP_LIST_POS_X - (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_LEFT_SIDE * 1.5);
        Matrix mtx = Matrix.Multiply(Matrix.CreateScale(new Vector3(ShopScreen.SHOP_BACK_LEFT_SIDE + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f)), Matrix.CreateTranslation(new Vector3(num3, 0.0f, 0.0f)));
        MatrixManager.instance.Reset();
        MatrixManager.instance.SetMatrix(mtx);
        MatrixManager.instance.UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, (float) ((512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) (((double) ShopScreen.SHOP_BACK_LEFT_SIDE + (512.0 - (double) Game.SCREEN_WIDTH) / 2.0) / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
        Math.Scale44(new Vector3(ShopScreen.SHOP_BACK_RIGHT_SIDE + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f), out mtx);
        Vector3 scl = new Vector3(num2, 0.0f, 0.0f);
        Math.GlobalTranslate44(ref mtx, scl);
        MatrixManager.instance.Reset();
        MatrixManager.instance.SetMatrix(mtx);
        MatrixManager.instance.UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, (float) (((double) ShopScreen.SHOP_BACK_LEFT_SIDE + (512.0 - (double) Game.SCREEN_WIDTH) / 2.0) / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
        this.backgroundTex.UnSet();
      }
      else
      {
        Matrix mtx;
        Math.Scale44(new Vector3(Game.SCREEN_WIDTH + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f), out mtx);
        MatrixManager.instance.Reset();
        MatrixManager.instance.SetMatrix(mtx);
        MatrixManager.instance.UploadCurrentMatrices();
        this.backgroundTex.Set();
        Mesh.DrawQuad(Color.White, (float) ((512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
        this.backgroundTex.UnSet();
      }
    }

    public enum POC
    {
      IN,
      MOVING_IN,
      OUT,
      MOVING_OUT,
    }

    public delegate void WhenIn();
  }
}
