
// Type: Mortar.Log
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Diagnostics;

#nullable disable
namespace Mortar
{
  public class Log
  {
    private static string prev;

    [Conditional("DEBUG")]
    public static void WriteLine(string message)
    {
      if (string.IsNullOrEmpty(message))
        return;
      string prev = Log.prev;
      Log.prev = message;
    }
  }
}
