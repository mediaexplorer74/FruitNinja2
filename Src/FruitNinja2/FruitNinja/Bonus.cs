
// Type: FruitNinja2.Bonus
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  internal class Bonus
  {
    private int totalMin;
    private int totalMax;
    private Dictionary<uint, int> individualMins = new Dictionary<uint, int>();
    private Dictionary<uint, int> individualMaxs = new Dictionary<uint, int>();
    private int multipleOf;
    public int points;
    private string description;
    private string text;
    private List<uint> equalTypes = new List<uint>();
    public Texture texture;

    public Bonus()
    {
      this.totalMin = 0;
      this.totalMax = 10000000;
      this.points = 5;
      this.multipleOf = 0;
      this.texture = (Texture) null;
    }

    public void Parse(XElement parent)
    {
      parent.QueryIntAttribute("min", ref this.totalMin);
      parent.QueryIntAttribute("max", ref this.totalMax);
      int num1 = -1;
      parent.QueryIntAttribute("equals", ref num1);
      if (num1 >= 0)
        this.totalMin = this.totalMax = num1;
      parent.QueryIntAttribute("points", ref this.points);
      parent.QueryIntAttribute("multiple", ref this.multipleOf);
      this.texture = StringFunctions.LoadTexture(parent.AttributeStr("texture"));
      List<string> words = new List<string>();
      int num2 = StringFunctions.SplitWords(parent.AttributeStr("valuesEqual"), ref words);
      for (int index = 0; index < num2; ++index)
        this.equalTypes.Add(StringFunctions.StringHash(words[index]));
      for (XAttribute xattribute = parent.FirstAttribute; xattribute != null; xattribute = xattribute.NextAttribute)
      {
        string localName = xattribute.Name.LocalName;
        if (StringFunctions.StartsWithWord(localName, "min-"))
          this.individualMins[StringFunctions.StringHash(localName.Substring(4))] = MParser.ParseInt(xattribute.Value);
        else if (StringFunctions.StartsWithWord(localName, "max-"))
          this.individualMaxs[StringFunctions.StringHash(localName.Substring(4))] = MParser.ParseInt(xattribute.Value);
      }
      if (parent.Value != null && parent.Value.Length > 0)
        this.description = parent.Value;
      else
        this.description = "";
    }

    public int GetPoints() => this.points;

    public string GetText() => this.text;

    public int IsAchieved(int total, Dictionary<uint, int> individualTotals)
    {
      if (total < this.totalMin || total > this.totalMax || this.multipleOf > 0 && total % this.multipleOf != 0)
        return 0;
      foreach (KeyValuePair<uint, int> individualTotal in individualTotals)
      {
        int num1 = 0;
        int num2 = 1000000;
        int num3;
        if (this.individualMins.TryGetValue(individualTotal.Key, out num3))
          num1 = num3;
        if (this.individualMaxs.TryGetValue(individualTotal.Key, out num3))
          num2 = num3;
        if (individualTotal.Value < num1 || individualTotal.Value > num2)
          return 0;
      }
      int num4 = -1;
      bool flag = true;
      foreach (uint equalType in this.equalTypes)
      {
        int num5;
        if (!individualTotals.TryGetValue(equalType, out num5))
          return 0;
        if (flag)
        {
          num4 = num5;
          if (num4 <= 0)
            return 0;
          flag = false;
        }
        else if (num4 != num5)
          return 0;
      }
      this.text = string.Format(TheGame.instance.stringTable.GetString(this.description), (object) total);
      return this.points;
    }
  }
}
