
// Type: Mortar.Col
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public abstract class Col
  {
    public Vector3 centre;
    public static Color normalColor;
    public static Color colideColor;
    protected bool m_collided;

    public abstract COLISIONOBJECT GetType();

    public abstract bool Collide(Col obj2, out Vector3 proj);

    public abstract void DrawDebug();

    public void AddCollision() => this.m_collided = true;

    public void ClearCollideFlag() => this.m_collided = false;
  }
}
