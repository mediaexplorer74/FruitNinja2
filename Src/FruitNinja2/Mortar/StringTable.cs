
// Type: Mortar.StringTable
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System;

#nullable disable
namespace Mortar
{
  public class StringTable
  {
    private string name;
    private int defaultLangauge;
    private string[] stringHashes;
    private string[] languageHashes;
    private StringTableLanguageStringSet[] languages;
    private static int[] hashes = new int[6]
    {
      "english_us".GetHashCode(),
      "english_uk".GetHashCode(),
      "french".GetHashCode(),
      "spanish".GetHashCode(),
      "german".GetHashCode(),
      "italian".GetHashCode()
    };
    private static string[] langnames = new string[6]
    {
      "english_us",
      "english_uk",
      "french",
      "spanish",
      "german",
      "italian"
    };

    public int GetStringIdx(string str) => Array.BinarySearch<string>(this.stringHashes, str);

    public int GetLanguageIdx(string lng)
    {
      int hashCode = lng.GetHashCode();
      int languageIdx = 0;
      foreach (int hash in StringTable.hashes)
      {
        if (hashCode == hash)
          return languageIdx;
        ++languageIdx;
      }
      return 0;
    }

    public string GetString(string str) => this.GetString(str, this.defaultLangauge);

    public string GetString(int sidx) => this.GetString(sidx, this.defaultLangauge);

    public string GetString(int sidx, string lng) => this.GetString(sidx, this.GetLanguageIdx(lng));

    public string GetString(string str, string lng)
    {
      return this.GetString(str, this.GetLanguageIdx(lng));
    }

    public string GetString(string str, int lidx)
    {
       return "777";//this.GetString(this.GetStringIdx(str), lidx);
    }

    public string GetString(int sidx, int lidx)
    {
       return "888";//this.languages[lidx].strings[sidx];
    }

    public void UpdateDefaultLanguage()
    {
      this.defaultLangauge = this.GetLanguageIdx(StringManager.GetInstance().defaultLanguage);
    }

    public void LoadHeader(string filename)
    {
      this.name = filename;
      string[][] strArray = TheGame.instance.Content.Load<string[][]>(filename + "_header.str");
      this.stringHashes = strArray[0];
      this.languageHashes = strArray[1];
      this.languages = new StringTableLanguageStringSet[this.languageHashes.Length];
      this.UpdateDefaultLanguage();
    }

    public void LoadLanguage(string lng) => this.LoadLanguage(this.GetLanguageIdx(lng));

    public void LoadLanguage(int lidx)
    {
      if (this.languages[lidx] != null)
        return;
      this.languages[lidx] = new StringTableLanguageStringSet();
      this.languages[lidx].strings = TheGame.instance.Content.Load<string[]>(this.name + "_" + StringTable.langnames[lidx] + ".str");
      for (int index = 0; index < this.languages[lidx].strings.Length; ++index)
        this.languages[lidx].strings[index] = this.languages[lidx].strings[index].Replace("%i", "{0}");
    }

    public void UnloadLanguage(int lidx)
    {
      this.languages[lidx] = (StringTableLanguageStringSet) null;
    }

    public void LoadAllLanguages()
    {
      foreach (string languageHash in this.languageHashes)
        this.LoadLanguage(languageHash);
    }

    public void UnloadAllLanguages()
    {
      this.languages = new StringTableLanguageStringSet[this.languageHashes.Length];
    }
  }
}
