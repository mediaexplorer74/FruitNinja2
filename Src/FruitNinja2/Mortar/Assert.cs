
// Type: Mortar.Assert
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System;
using System.Diagnostics;

#nullable disable
namespace Mortar
{
  public class Assert
  {
    [Conditional("DEBUG")]
    public static void ASSERT(bool f)
    {
      if (!f)
        throw new MissingMethodException("From assert");
    }

    [Conditional("DEBUG")]
    public static void PANIC(string sdf) => throw new MissingMethodException(sdf);
  }
}
