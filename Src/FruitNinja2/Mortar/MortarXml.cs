
// Type: Mortar.MortarXml
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System;
using System.Xml.Linq;

#nullable disable
namespace Mortar
{
  public class MortarXml
  {
    public static void Save(XDocument doc, string name)
    {
    }

    public static XDocument Load(string name)
    {
      string text;
      try
      {
        text = TheGame.instance.Content.Load<string>(name);
      }
      catch (Exception ex)
      {
        return (XDocument) null;
      }
      if (text == null)
        return (XDocument) null;
      return XDocument.Parse(text);
    }
  }
}
