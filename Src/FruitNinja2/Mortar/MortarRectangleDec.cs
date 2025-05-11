
// Type: Mortar.MortarRectangleDec
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public struct MortarRectangleDec(float l, float t, float r, float b)
  {
    public float left = l;
    public float top = t;
    public float right = r;
    public float bottom = b;

    public float Width() => this.right - this.left;

    public float Height() => this.bottom - this.top;

    public PointDec Centre()
    {
      return new PointDec(this.left + this.Width() * 0.5f, this.top + this.Height() * 0.5f);
    }
  }
}
