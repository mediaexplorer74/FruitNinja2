
// Type: Mortar.XAttributeExtensions
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Mortar
{
  public static class XAttributeExtensions
  {
    public static Dictionary<T, G> Duplicate<T, G>(this Dictionary<T, G> d)
    {
      Dictionary<T, G> dictionary = new Dictionary<T, G>();
      foreach (KeyValuePair<T, G> keyValuePair in d)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      return dictionary;
    }

    public static LinkedList<T> Duplicate<T>(this LinkedList<T> l)
    {
      LinkedList<T> linkedList = new LinkedList<T>();
      foreach (T obj in l)
        linkedList.AddLast(obj);
      return linkedList;
    }

    public static XElement FirstChildElement(this XDocument element)
    {
      using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      return (XElement) null;
    }

    public static XElement FirstChildElement(this XDocument element, string ename)
    {
      return element.Element((XName) ename);
    }

    public static XElement FirstChildElement(this XElement element)
    {
      using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      return (XElement) null;
    }

    public static XElement FirstChildElement(this XElement element, string ename)
    {
      return element.Element((XName) ename);
    }

    public static bool QueryIntAttribute(
      this XElement element,
      string AttributeName,
      ref int value)
    {
      XAttribute xattribute = element.Attribute((XName) AttributeName);
      if (xattribute == null)
        return false;
      value = MParser.ParseInt(xattribute.Value);
      return true;
    }

    public static bool QueryFloatAttribute(
      this XElement element,
      string AttributeName,
      ref float value)
    {
      XAttribute xattribute = element.Attribute((XName) AttributeName);
      if (xattribute == null)
        return false;
      value = MParser.ParseFloat(xattribute.Value);
      return true;
    }

    public static string AttributeStr(this XElement element, string attribName)
    {
      return element.Attribute((XName) attribName)?.Value;
    }

    public static XElement NextSiblingElement(this XElement element, string elementName)
    {
      using (IEnumerator<XElement> enumerator = element.ElementsAfterSelf((XName) elementName).GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      return (XElement) null;
    }

    public static XElement NextSiblingElement(this XElement element)
    {
      using (IEnumerator<XElement> enumerator = element.ElementsAfterSelf().GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      return (XElement) null;
    }

    public static void SetAttribute(this XElement element, XName name, object obj)
    {
      XAttribute content = new XAttribute(name, obj);
      element.Add((object) content);
    }

    public static void LinkEndChild(this XElement element, XElement child)
    {
      element.Add((object) child);
    }

    public static string GetText(this XElement element) => element.Value;
  }
}
