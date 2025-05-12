
// Type: GameManager.BonusManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  internal class BonusManager
  {
    private List<BonusType> m_bonusTypes = new List<BonusType>();
    private LinkedList<Bonus> m_bestBonuses = new LinkedList<Bonus>();
    private List<int> m_comboBonusPoints = new List<int>();
    public static BonusManager instance = (BonusManager) null;
    private static Color[] cols = new Color[3]
    {
      new Color(173, 126, 0),
      new Color(160, 5, 5),
      new Color(1, 92, 149)
    };
    private static int[] totes = new int[7]
    {
      1,
      2,
      3,
      5,
      8,
      13,
      21
    };

    public static BonusManager GetInstance()
    {
      if (BonusManager.instance == null)
        BonusManager.instance = new BonusManager();
      return BonusManager.instance;
    }

    public void Init()
    {
      XDocument element1 = MortarXml.Load("xmlwp7/bonusAwards.xml");
      if (element1 == null)
        return;
      if (true)
      {
        XElement element2 = element1.FirstChildElement("bonusAwardsFile");
        for (XElement xelement = element2.FirstChildElement("bonusType"); xelement != null; xelement = xelement.NextSiblingElement("bonusType"))
        {
          BonusType bonusType = new BonusType();
          bonusType.Parse(xelement);
          this.m_bonusTypes.Add(bonusType);
        }
        int num = 0;
        for (XElement element3 = element2.FirstChildElement("combo"); element3 != null; element3 = element3.NextSiblingElement("combo"))
        {
          element3.QueryIntAttribute("bonus", ref num);
          this.m_comboBonusPoints.Add(num);
        }
      }
    }

    public void SetUpBonusScreen(BonusScreen screen)
    {
      Game2.game_work.scoreBeforeBonuses = Game2.game_work.currentScore;
      Game2.game_work.inBonusScreen = true;
      BonusScreen.bomb_magnet = false;
      this.m_bestBonuses.Clear();
      List<int> intList1 = new List<int>();
      for (int index = 0; index < this.m_bonusTypes.Count; ++index)
        intList1.Add(index);
      List<int> intList2 = new List<int>();
      for (int index1 = 0; index1 < this.m_bonusTypes.Count; ++index1)
      {
        int index2 = Mortar.Math.g_random.Rand32(intList1.Count);
        intList2.Add(intList1[index2]);
        intList1.RemoveAt(index2);
      }
      for (int index = 0; index < this.m_bonusTypes.Count; ++index)
      {
        Bonus best = this.m_bonusTypes[intList2[index]].GetBest();
        if (best != null)
          this.m_bestBonuses.AddLast(best);
      }
      LinkedList<Bonus> linkedList = new LinkedList<Bonus>();
      foreach (Bonus bonus in (IEnumerable<Bonus>) this.m_bestBonuses.OrderBy<Bonus, int>((Func<Bonus, int>) (fred => fred.points)))
        linkedList.AddLast(bonus);
      this.m_bestBonuses = linkedList;
      int index3 = -this.m_bestBonuses.Count + 3;
      LinkedListNode<Bonus> node = this.m_bestBonuses.First;
      if (screen == null)
        return;
      while (node != null)
      {
        if (index3 >= 0)
        {
          screen.AddAward(BonusManager.cols[index3], node.Value.texture, node.Value.GetText(), node.Value.GetPoints());
          node = node.Next;
        }
        else
        {
          LinkedListNode<Bonus> next = node.Next;
          this.m_bestBonuses.Remove(node);
          node = next;
        }
        ++index3;
      }
    }

    public void AddCombo(int length)
    {
      Game2.game_work.saveData.AddToTotal("combo_bonus", StringFunctions.StringHash("combo_bonus"), this.m_comboBonusPoints[Mortar.Math.CLAMP(length - 3, 0, this.m_comboBonusPoints.Count - 1)], false, false);
      uint hash = StringFunctions.StringHash("best_combo");
      int total = Game2.game_work.saveData.GetTotal(hash);
      Game2.game_work.saveData.AddToTotal("best_combo", hash, Mortar.Math.MAX(0, length - total), false, false);
    }

    public static int GetBonusTotal(uint hash)
    {
      uint num = StringFunctions.StringHash("score");
      return (int) hash == (int) num ? Game2.game_work.currentScore : Game2.game_work.saveData.GetTotal(hash);
    }

    public Bonus GetFirstBestBonus(ref LinkedListNode<Bonus> it)
    {
      it = this.m_bestBonuses.First;
      return it != null ? it.Value : (Bonus) null;
    }

    public Bonus GetNextBestBonus(ref LinkedListNode<Bonus> it)
    {
      it = it.Next;
      return it != null ? it.Value : (Bonus) null;
    }
  }
}
