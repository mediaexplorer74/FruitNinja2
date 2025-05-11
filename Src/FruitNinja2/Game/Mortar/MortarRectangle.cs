
// Type: Mortar.MortarRectangle
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public struct MortarRectangle
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public int Width() => this.right - this.left;

    public int Height() => this.bottom - this.top;

    public Point Centre()
    {
      return new Point(this.left + (this.Width() >> 1), this.top + (this.Height() >> 1));
    }
  }
}
