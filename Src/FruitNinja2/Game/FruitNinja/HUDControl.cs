
// Type: GameManager.HUDControl
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class HUDControl
  {
    public const int NUM_TEXT_ALIGN_RIGHT = 1;
    public const int NUM_TEXT_ALIGN_CENTER = 2;
    public const int NUM_TEXT_DISABLE_MTX_RESET = 4;
    public const int TINT_CHANNELS = 3;
    public Vector3 m_pos;
    public Vector3 m_scale;
    public float m_rotation;
    public bool m_update;
    public bool m_forceDraw;
    public bool m_selfCleanUp;
    public bool m_terminate;
    public bool partOfPopup;
    public HUD.HUD_ORDER m_drawOrder;
    public HUDControl.HUDControlDeletedCallback m_deleteCall;
    public Color m_color;
    public bool m_canBeTinted;
    public Vector2[] m_uvs = new Vector2[2];

    public static Color TintColor(Color col, float[] tints)
    {
      col.R = (byte) Math.CLAMP((float) col.R * tints[0], 0.0f, (float) byte.MaxValue);
      col.G = (byte) Math.CLAMP((float) col.G * tints[1], 0.0f, (float) byte.MaxValue);
      col.B = (byte) Math.CLAMP((float) col.B * tints[2], 0.0f, (float) byte.MaxValue);
      return col;
    }

    public static Color TintWhite(float[] tints)
    {
      return new Color((int) Math.CLAMP((float) byte.MaxValue * tints[0], 0.0f, (float) byte.MaxValue), (int) Math.CLAMP((float) byte.MaxValue * tints[1], 0.0f, (float) byte.MaxValue), (int) Math.CLAMP((float) byte.MaxValue * tints[2], 0.0f, (float) byte.MaxValue), (int) byte.MaxValue);
    }

    public static void DefaultDeleteCallback(HUDControl control)
    {
    }

    public HUDControl()
    {
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
      this.m_update = true;
      this.m_deleteCall = new HUDControl.HUDControlDeletedCallback(HUDControl.DefaultDeleteCallback);
      this.m_color = Color.White;
      this.m_canBeTinted = true;
      this.m_uvs[0] = Vector2.Zero;
      this.m_uvs[1] = Vector2.One;
    }

    public virtual void Init()
    {
    }

    public virtual void Release()
    {
    }

    public virtual void Reset()
    {
    }

    public virtual void PreDraw(float[] tintChannels)
    {
    }

    public virtual void Draw(float[] tintChannels)
    {
    }

    public virtual void Update(float dt)
    {
    }

    public bool ForceDraw() => this.m_forceDraw;

    public void SetActive(bool state) => this.m_update = state;

    public bool GetActive() => this.m_update;

    public virtual bool Terminate() => this.m_terminate;

    public virtual HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_2D;

    public virtual void Skip()
    {
    }

    public virtual void Save()
    {
    }

    public delegate void HUDControlDeletedCallback(HUDControl bob);
  }
}
