
// Type: GameManager.ScrollingMenuItem
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class ScrollingMenuItem
  {
    protected Vector3 m_pos;
    protected ScrollingMenu m_parentList;
    protected Color m_colour;
    protected Vector3 m_textOffset;
    protected float m_height;
    protected float m_width;
    protected bool m_highlighted;
    protected bool m_isOnScreen;
    protected ScrollingMenuItem.ClickedMenuItemCallback m_clickedCallback;
    public string m_text;

    public static void DefaultClickedMenuItemCallback(ScrollingMenuItem item)
    {
    }

    public void CallClickedMenuItemCallback() => this.m_clickedCallback(this);

    public void SetClickedFocusedCallback()
    {
      this.SetClickedFocusedCallback(new ScrollingMenuItem.ClickedMenuItemCallback(ScrollingMenuItem.DefaultClickedMenuItemCallback));
    }

    public void SetClickedFocusedCallback(ScrollingMenuItem.ClickedMenuItemCallback call)
    {
      this.m_clickedCallback = call;
    }

    public ScrollingMenuItem()
    {
      this.m_textOffset = Vector3.Zero;
      this.m_text = (string) null;
      this.m_colour = Color.White;
      this.m_height = 25f;
      this.m_width = 0.0f;
      this.m_clickedCallback = new ScrollingMenuItem.ClickedMenuItemCallback(ScrollingMenuItem.DefaultClickedMenuItemCallback);
    }

    public ScrollingMenuItem(float height, float width, string text)
      : this(height, width, text, new ScrollingMenuItem.ClickedMenuItemCallback(ScrollingMenuItem.DefaultClickedMenuItemCallback))
    {
    }

    public ScrollingMenuItem(
      float height,
      float width,
      string text,
      ScrollingMenuItem.ClickedMenuItemCallback call)
    {
      this.m_clickedCallback = call;
      this.m_textOffset = Vector3.Zero;
      this.m_text = (string) null;
      this.SetText(text);
      this.m_colour = Color.White;
      this.m_height = height;
      this.m_width = width;
    }

    public virtual float GetHeight() => this.m_height;

    public virtual float GetWidth() => this.m_width;

    public virtual void SetHeight(float v) => this.m_height = v;

    public virtual void SetWidth(float v) => this.m_width = v;

    public virtual void Move(Vector3 toPos) => this.m_pos = toPos;

    public virtual void Remove() => Delete.SAFE_DELETE_ARRAY<string>(ref this.m_text);

    public void Highlight(bool highlight) => this.m_highlighted = highlight;

    public virtual void SetParent(ScrollingMenu parent) => this.m_parentList = parent;

    public virtual void SetOnscreen(bool onscreen) => this.m_isOnScreen = onscreen;

    public virtual void SetText(string text) => this.m_text = text;

    public virtual void Draw()
    {
      if (this.m_text == null)
        return;
      Vector3 pos = Vector3.Add(this.m_pos, this.m_textOffset);
      MortarRectangleDec? rect = new MortarRectangleDec?();
      if (this.m_parentList != null)
      {
        MortarRectangleDec mortarRectangleDec;
        mortarRectangleDec.top = this.m_parentList.m_pos.Y + this.m_parentList.GetHeight() / 2f;
        mortarRectangleDec.bottom = this.m_parentList.m_pos.Y - this.m_parentList.GetHeight() / 2f;
        mortarRectangleDec.left = this.m_parentList.m_pos.X - this.m_parentList.GetWidth() / 2f;
        mortarRectangleDec.right = this.m_parentList.m_pos.X + this.m_parentList.GetWidth() / 2f;
        rect = new MortarRectangleDec?(mortarRectangleDec);
      }
      Game2.game_work.pGameFont.DrawString(this.m_text, pos, this.m_colour, 30f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER, 1f, rect);
    }

    public delegate void ClickedMenuItemCallback(ScrollingMenuItem fdsdf);
  }
}
