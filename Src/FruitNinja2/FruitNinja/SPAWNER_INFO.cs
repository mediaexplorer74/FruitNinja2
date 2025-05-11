
// Type: FruitNinja2.SPAWNER_INFO
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;

#nullable disable
namespace FruitNinja2
{
  public class SPAWNER_INFO
  {
    public int[] randomTypes;
    public List<string> types = new List<string>();
    public int typeCount;
    public float dt;
    public Vector3 gravity;
    public float velXscale;
    public float velYscale;
    public float horizontalMin;
    public float horizontalMax;
    public SPAWN_PLACEMENTS placement;
    public float minSpawn;
    public float minInc;
    public float maxSpawn;
    public float maxInc;
    public float delay;
    public float delayInc;
    private static uint[] bombHashes = new uint[2]
    {
      StringFunctions.StringHash("bomb"),
      StringFunctions.StringHash("Bomb")
    };
    private static uint one_fruit = StringFunctions.StringHash("1fruit");
    public int toSpawnThisWave;
    public int maxToSpawnThisWave;
    public int bombsSpawnedThisWave;
    public float delayWait;
    public bool mirror;
    public bool isClone;

    public void ResetDelay(float inc)
    {
      this.delayWait = Math.MAX(0.0f, this.delay + inc * this.delayInc);
    }

    public void Reset(float inc)
    {
      this.toSpawnThisWave = this.GetRandCount(inc);
      this.maxToSpawnThisWave = this.toSpawnThisWave;
      this.bombsSpawnedThisWave = 0;
      this.SelectTypes();
      this.ResetDelay(inc);
    }

    public void SelectTypes()
    {
      for (int index = 0; index < this.typeCount; ++index)
      {
        this.randomTypes[index] = -1;
        uint num = StringFunctions.StringHash(this.types[index]);
        this.randomTypes[index] = (int) num == (int) SPAWNER_INFO.bombHashes[0] || (int) num == (int) SPAWNER_INFO.bombHashes[1] ? -2 : ((int) num != (int) SPAWNER_INFO.one_fruit ? Fruit.FruitType(this.types[index]) : Fruit.RandomFruit(false));
      }
    }

    public int GetRandCount(float inc)
    {
      int num = Math.MAX(0, (int) ((double) this.minSpawn + (double) inc * (double) this.minInc));
      int max = Math.MAX(0, (int) ((double) this.maxSpawn + (double) inc * (double) this.maxInc)) - num;
      return num + (max <= 0 ? 0 : Math.g_random.Rand32(max));
    }

    public SPAWNER_INFO()
    {
      this.dt = 1f;
      this.isClone = false;
      this.horizontalMin = -1f;
      this.horizontalMax = 1f;
      this.gravity = new Vector3(0.0f, -1f, 0.0f);
      this.placement = SPAWN_PLACEMENTS.SPAWNER_BOTTOM;
      this.delay = 0.0f;
      this.delayInc = 0.0f;
      this.typeCount = 0;
      this.minSpawn = this.minInc = this.maxSpawn = this.maxInc = 0.0f;
      this.toSpawnThisWave = 0;
      this.delayWait = 0.0f;
      this.randomTypes = (int[]) null;
      this.velXscale = 1f;
      this.velYscale = 1f;
      this.mirror = false;
    }
  }
}
