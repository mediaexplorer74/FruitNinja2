
// Type: Mortar.StringManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  internal class StringManager
  {
    public static StringTable[] tables;
    public string defaultLanguage;
    public static StringManager instance;

    public static StringManager GetInstance()
    {
      if (StringManager.instance == null)
        StringManager.instance = new StringManager();
      return StringManager.instance;
    }

    private StringManager() => this.defaultLanguage = "english_us";

    public void Init(int numTables) => StringManager.tables = new StringTable[numTables];

    public void LoadTable(string filename, int idx)
    {
      StringManager.tables[idx] = new StringTable();
      StringManager.tables[idx].LoadHeader(filename);
    }

    public void UnloadTable(int idx) => StringManager.tables[idx] = (StringTable) null;

    public void UnloadAll() => StringManager.tables = new StringTable[StringManager.tables.Length];

    public void SetDefaultLanguage(string lng) => this.defaultLanguage = lng;
  }
}
