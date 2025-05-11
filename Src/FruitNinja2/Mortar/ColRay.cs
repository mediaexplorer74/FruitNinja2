
// Type: Mortar.ColRay
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public struct ColRay(Vector3 v1, Vector3 v2)
  {
    private Vector3 o = v1;
    private Vector3 d = v2;
  }
}
