
// Type: FruitNinja2.SpecificOrder
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace FruitNinja2
{
  public class SpecificOrder
  {
    private uint[,] orderList = new uint[AchievementManager.MAX_ORDER_LIST_TYPES, AchievementManager.MAX_ORDER_LIST_TYPES + 1];

    public SpecificOrder(string text)
    {
    }

    public bool Check(uint hash) => false;

    public uint GetFirstFruitTypeHash() => 0;
  }
}
