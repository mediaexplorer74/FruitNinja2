
// Type: Mortar.MParser
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System;
using System.Globalization;

#nullable disable
namespace Mortar
{
  public static class MParser
  {
    public static int ParseInt(string astr)
    {
      return int.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static float ParseFloat(string astr)
    {
      return float.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static uint ParseUInt(string astr)
    {
      return uint.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static ushort ParseUShort(string astr)
    {
      return ushort.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static ulong ParseULong(string astr)
    {
      return ulong.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static byte ParseByte(string astr)
    {
      return byte.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
