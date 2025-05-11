
// Type: FruitNinja2.ItemManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  internal class ItemManager
  {
    private static ItemManager instance;
    private ItemInfo[] equipedItems = new ItemInfo[2];
    private List<ItemInfo> itemInfoOrderedList = new List<ItemInfo>();
    private Dictionary<uint, ItemInfo> itemInfoList = new Dictionary<uint, ItemInfo>();
    private Dictionary<uint, ItemInfo>[] itemInfoTypeList = ArrayInit.CreateFilledArray<Dictionary<uint, ItemInfo>>(2);
    private static uint[] itemTypeHashes = new uint[2]
    {
      StringFunctions.StringHash("SLASH_MODIFIER"),
      StringFunctions.StringHash("BACKGROUND")
    };
    private static string ITEM_SAVE_DIRECTORY = "C:/ItemSave.xml";
    private static string SAVE_DIRECTORY = "FruitNinja";
    private static string SAVE_FILENAME = ItemManager.SAVE_DIRECTORY + "/itemsinfo.xml";
    private static int funcCalls = 2;

    public static ItemManager GetInstance()
    {
      if (ItemManager.instance == null)
        ItemManager.instance = new ItemManager();
      return ItemManager.instance;
    }

    private ItemManager()
    {
      for (int index = 0; index < 2; ++index)
        this.equipedItems[index] = (ItemInfo) null;
    }

    public int GetNumNewItems()
    {
      int numNewItems = 0;
      foreach (ItemInfo itemInfoOrdered in this.itemInfoOrderedList)
      {
        if (!itemInfoOrdered.hasBeenSeen)
          ++numNewItems;
      }
      return numNewItems;
    }

    public bool AreNewItems()
    {
      foreach (ItemInfo itemInfoOrdered in this.itemInfoOrderedList)
      {
        if (!itemInfoOrdered.hasBeenSeen)
          return true;
      }
      return false;
    }

    public static ItemType ParseItemType(string text)
    {
      if (text != null)
      {
        uint num = StringFunctions.StringHash(text);
        for (int itemType = 0; itemType < 2; ++itemType)
        {
          if ((int) num == (int) ItemManager.itemTypeHashes[itemType])
            return (ItemType) itemType;
        }
      }
      return ItemType.ITEM_NONE;
    }

    public void LoadItemData()
    {
      try
      {
        XDocument dl = MortarXml.Load("xmlwp7/itemList.xml");
        this.itemInfoList.Clear();
        this.itemInfoOrderedList.Clear();
        for (int index = 0; index < 2; ++index)
          this.equipedItems[index] = (ItemInfo) null;
        if (dl != null)
        {
          if (true)
          {
            for (XElement xelement = dl.FirstChildElement("itemManagerFile").FirstChildElement("item"); xelement != null; xelement = xelement.NextSiblingElement("item"))
            {
              ItemType itemType = ItemManager.ParseItemType(xelement.AttributeStr("type"));
              ItemInfo itemInfo;
              if (itemType == ItemType.ITEM_SLASH_MODIFIER)
              {
                itemInfo = (ItemInfo) new SlashModInfo();
                itemInfo.type = itemType;
              }
              else
              {
                itemInfo = new ItemInfo();
                itemInfo.type = itemType;
              }
              itemInfo.Parse(xelement);
              if (Game.game_work.saveData.IsAchievementUnlocked(itemInfo.nameHash))
                itemInfo.cost = -1;
              else if (!AchievementManager.GetInstance().AchievementExists(itemInfo.nameHash) && itemInfo.cost > 0)
              {
                itemInfo.cost = -1;
                itemInfo.hasBeenSeen = false;
                ShopScreen.NewItem();
              }
              this.itemInfoOrderedList.Add(itemInfo);
              this.itemInfoList[itemInfo.nameHash] = itemInfo;
              this.itemInfoTypeList[(int) itemInfo.type][itemInfo.nameHash] = itemInfo;
              if (this.equipedItems[(int) itemInfo.type] == null)
                this.equipedItems[(int) itemInfo.type] = itemInfo;
            }
          }
          Delete.SAFE_DELETE<XDocument>(ref dl);
        }
      }
      catch
      {
      }
      try
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        if (storeForApplication.FileExists(ItemManager.SAVE_FILENAME))
        {
          IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(ItemManager.SAVE_FILENAME, FileMode.Open);
          if (storageFileStream.Length > 0L)
          {
            byte[] numArray = new byte[storageFileStream.Length];
            storageFileStream.Read(numArray, 0, (int) storageFileStream.Length);
            XDocument dl = XDocument.Parse(Encoding.UTF8.GetString(numArray, 0, numArray.Length));
            if (true)
            {
              XElement element1 = dl.FirstChildElement("item_save_file");
              element1.QueryIntAttribute("coins", ref Game.game_work.coins);
              element1.QueryIntAttribute("coinsTotal", ref Game.game_work.coinsTotal);
              element1.QueryIntAttribute("levelStartCoins", ref Game.game_work.levelStartCoins);
              XElement element2 = element1.FirstChildElement("boughtItems");
              if (element2 != null)
              {
                for (XElement element3 = element2.FirstChildElement("item"); element3 != null; element3 = element3.NextSiblingElement("item"))
                {
                  string s = element3.AttributeStr("name");
                  ItemInfo itemInfo;
                  if (s != null && this.itemInfoList.TryGetValue(StringFunctions.StringHash(s), out itemInfo))
                  {
                    itemInfo.cost = -1;
                    string str = element3.AttributeStr("seen");
                    itemInfo.hasBeenSeen = str != null && str == "true";
                  }
                }
              }
              XElement element4 = element1.FirstChildElement("equippedItems");
              if (element4 != null)
              {
                for (XElement element5 = element4.FirstChildElement("item"); element5 != null; element5 = element5.NextSiblingElement("item"))
                {
                  string s = element5.AttributeStr("name");
                  if (s != null)
                  {
                    ItemInfo itemInfo = this.GetItem(StringFunctions.StringHash(s));
                    if (itemInfo != null)
                      this.equipedItems[(int) itemInfo.type] = itemInfo;
                  }
                }
              }
            }
            Delete.SAFE_DELETE<XDocument>(ref dl);
          }
          storageFileStream.Close();
        }
      }
      catch (Exception ex)
      {
      }
      for (int type = 0; type < 2; ++type)
        this.SetEquippedItem((ItemType) type, this.equipedItems[type]);
    }

    public void UnLoadItemData() => this.itemInfoList.Clear();

    public void SaveItemInfo()
    {
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
      try
      {
        if (!storeForApplication.DirectoryExists("FruitNinja"))
          storeForApplication.CreateDirectory("FruitNinja");
        XDocument xdocument = new XDocument();
        xdocument.Declaration = new XDeclaration("1.0", "utf-8", "yes");
        XElement xelement1 = new XElement((XName) "item_save_file");
        xdocument.Add((object) xelement1);
        xelement1.Add((object) new XAttribute((XName) "coins", (object) Game.game_work.coins));
        xelement1.Add((object) new XAttribute((XName) "coinsTotal", (object) Game.game_work.coinsTotal));
        xelement1.Add((object) new XAttribute((XName) "levelStartCoins", (object) Game.game_work.levelStartCoins));
        if (this.itemInfoOrderedList.Count > 0)
        {
          XElement xelement2 = new XElement((XName) "boughtItems");
          foreach (ItemInfo itemInfoOrdered in this.itemInfoOrderedList)
          {
            if (itemInfoOrdered.cost < 0)
            {
              XElement xelement3 = new XElement((XName) "item");
              xelement3.SetAttribute((XName) "name", (object) itemInfoOrdered.name);
              xelement3.SetAttribute((XName) "seen", itemInfoOrdered.hasBeenSeen ? (object) "true" : (object) "false");
              xelement2.LinkEndChild(xelement3);
            }
          }
          xelement1.LinkEndChild(xelement2);
        }
        XElement xelement4 = new XElement((XName) "equippedItems");
        for (int index = 0; index < 2; ++index)
        {
          ItemInfo equipedItem = this.equipedItems[index];
          if (equipedItem != null && equipedItem.cost <= 0)
          {
            XElement xelement5 = new XElement((XName) "item");
            xelement5.SetAttribute((XName) "name", (object) equipedItem.name);
            xelement4.LinkEndChild(xelement5);
          }
        }
        xelement1.LinkEndChild(xelement4);
        MemoryStream memoryStream = new MemoryStream();
        xdocument.Save((Stream) memoryStream);
        byte[] array = memoryStream.ToArray();
        Encoding.UTF8.GetString(array, 0, array.Length);
        storageFileStream = storeForApplication.OpenFile(ItemManager.SAVE_FILENAME, FileMode.Create);
        storageFileStream.Write(array, 3, array.Length - 3);
        storageFileStream?.Close();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        storageFileStream?.Close();
      }
    }

    public bool IsEquipped(ItemInfo item)
    {
      return item != null && this.equipedItems[(int) item.type] == item;
    }

    public ItemInfo GetEquippedItem(ItemType type) => this.equipedItems[(int) type];

    public void SetEquippedItem(ItemType type, ItemInfo item)
    {
      switch (type)
      {
        case ItemType.ITEM_SLASH_MODIFIER:
          if (item != null)
          {
            item.SetEquipped();
            break;
          }
          SlashEntity.InitModColors();
          break;
        case ItemType.ITEM_BACKGROUND:
          if (ItemManager.funcCalls <= 0 || ItemManager.funcCalls > 0 && GameTask.GetCurrentBackground() == null)
          {
            GameTask.ChangeBackground(item?.textureName);
            break;
          }
          break;
      }
      if (ItemManager.funcCalls > 0)
        --ItemManager.funcCalls;
      this.equipedItems[(int) type] = item;
      if (item == null)
        return;
      item.hasBeenSeen = true;
    }

    public ItemInfo GetItem(uint hash)
    {
      ItemInfo itemInfo;
      return this.itemInfoList.TryGetValue(hash, out itemInfo) ? itemInfo : (ItemInfo) null;
    }

    public bool UnlockItem(uint hash)
    {
      ItemInfo itemInfo1;
      if (!this.itemInfoList.TryGetValue(hash, out itemInfo1))
        return false;
      ItemInfo itemInfo2 = itemInfo1;
      itemInfo2.cost = -1;
      itemInfo2.hasBeenSeen = false;
      return true;
    }

    public bool BuyItem(uint hash)
    {
      ItemInfo itemInfo1;
      if (this.itemInfoList.TryGetValue(hash, out itemInfo1))
      {
        ItemInfo itemInfo2 = itemInfo1;
        if (itemInfo2.cost >= 0 && Game.game_work.coins >= itemInfo2.cost)
        {
          Game.AddCoins(-itemInfo2.cost);
          itemInfo2.cost = -1;
          return true;
        }
      }
      return false;
    }

    public bool EquipItem(uint hash)
    {
      ItemInfo itemInfo1;
      if (this.itemInfoList.TryGetValue(hash, out itemInfo1))
      {
        ItemInfo itemInfo2 = itemInfo1;
        if (itemInfo2.cost < 0)
        {
          this.SetEquippedItem(itemInfo2.type, itemInfo2);
          return true;
        }
      }
      return false;
    }

    public bool UnequipItem(uint hash)
    {
      ItemInfo itemInfo;
      if (!this.itemInfoList.TryGetValue(hash, out itemInfo))
        return false;
      this.SetEquippedItem(itemInfo.type, (ItemInfo) null);
      return true;
    }

    public void UnequipItemType(ItemType type) => this.equipedItems[(int) type] = (ItemInfo) null;

    public ItemInfo GetFirst(ref int it)
    {
      it = 0;
      return it == this.itemInfoOrderedList.Count ? (ItemInfo) null : this.itemInfoOrderedList[it];
    }

    public ItemInfo GetNext(ref int it)
    {
      ++it;
      return it == this.itemInfoOrderedList.Count ? (ItemInfo) null : this.itemInfoOrderedList[it];
    }
  }
}
