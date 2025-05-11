
// Type: GameManager.WaveState
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public struct WaveState
  {
    public LinkedList<SpawnState> spawners;
    public float inc;
    public int index;
  }
}
