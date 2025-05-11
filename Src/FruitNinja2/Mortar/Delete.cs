
// Type: Mortar.Delete
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class Delete
  {
    public static void SAFE_DELETE<T>(ref T dl) => dl = default (T);

    public static void SAFE_DELETE_ARRAY<T>(ref T dl) => dl = default (T);
  }
}
