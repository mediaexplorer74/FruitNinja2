
// Type: Mortar.ArrayInit
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;

#nullable disable
namespace Mortar
{
  public static class ArrayInit
  {
    public static List<T> CreateInitedList<T>(int initsize)
    {
      List<T> initedList = new List<T>(initsize);
      for (int index = 0; index < initsize; ++index)
        initedList.Add(default (T));
      return initedList;
    }

    public static T[] CreateFilledArray<T>(int size) where T : new()
    {
      T[] filledArray = new T[size];
      for (int index = 0; index < size; ++index)
        filledArray[index] = new T();
      return filledArray;
    }

    public static T[,] CreateFilledArray<T>(int size, int size2) where T : new()
    {
      T[,] filledArray = new T[size, size2];
      for (int index1 = 0; index1 < size; ++index1)
      {
        for (int index2 = 0; index2 < size2; ++index2)
          filledArray[index1, index2] = new T();
      }
      return filledArray;
    }
  }
}
