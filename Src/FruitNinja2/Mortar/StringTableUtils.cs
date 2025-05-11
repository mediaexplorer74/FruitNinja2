
// Type: Mortar.StringTableUtils
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class StringTableUtils
  {
    private const int NUM_STRING_TABLES = 2;

    public static void StringTableUtilInit()
    {
    }

    public static void StringTableUtilLoadStrings()
    {
    }

    public static void StringTableUtilLoadStringsTable(int i)
    {
    }

    public static void StringTableUtilUnload()
    {
    }

    public static void StringTableUtilUnloadTable(int i)
    {
    }

    public static bool StringTableUtilLoaded() => false;

    public enum Table
    {
      TABLE_COMMON,
      TABLE_LEVEL,
      TABLE_MAX,
    }

    public enum Language
    {
      LANGUAGE_ENGLISH,
      LANGUAGE_ENGLISH_UK,
      LANGUAGE_FRENCH,
      LANGUAGE_SPANISH,
      LANGUAGE_GERMAN,
      LANGUAGE_ITALIAN,
      LANGUAGE_DUTCH,
      LANGUAGE_SWEDISH,
      LANGUAGE_DANISH,
      LANGUAGE_NORWEGIAN,
      LANGUAGE_FINNISH,
      LANGUAGE_KOREAN,
      LANGUAGE_JAPANESE,
      LANGUAGE_MAX,
    }

    public enum LM
    {
      LEVEL_MENU,
      CHAPTER_TUTORIAL,
      CHAPTER_1,
      CHAPTER_2,
      CHAPTER_3,
      CHAPTER_4,
      CHAPTER_5,
      CHAPTER_6A,
      CHAPTER_6B,
      LEVEL_CREDITS,
    }
  }
}
