
// Type: Mortar.ColAABB
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace Mortar
{
  public class ColAABB : Col
  {
    public Vector3 extents;

    public ColAABB()
    {
      this.centre = Vector3.Zero;
      this.extents = Vector3.One;
      this.ClearCollideFlag();
    }

    public ColAABB(Vector3 pos, Vector3 ext)
    {
      this.centre = pos;
      this.extents = ext;
      this.ClearCollideFlag();
    }

    public override COLISIONOBJECT GetType() => COLISIONOBJECT.COL_AABB;

    public override bool Collide(Col obj2, out Vector3 proj)
    {
      bool flag = false;
      switch (obj2.GetType())
      {
        case COLISIONOBJECT.COL_AABB:
          proj = Vector3.Zero;
          throw new MissingMethodException();
        case COLISIONOBJECT.COL_SPHERE:
          proj = Vector3.Zero;
          throw new MissingMethodException();
        case COLISIONOBJECT.COL_LINE:
          proj = Vector3.Zero;
          break;
        default:
          flag = obj2.Collide((Col) this, out proj);
          break;
      }
      return flag;
    }

    public override void DrawDebug() => throw new MissingMethodException();
  }
}
