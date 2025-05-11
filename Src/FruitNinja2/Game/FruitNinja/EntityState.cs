
// Type: GameManager.EntityState
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public struct EntityState
  {
    public Vector3 vel;
    public Vector3 pos;
    public Vector3 grav;
    public bool hit;
    public int type;
    public float wait;
  }
}
