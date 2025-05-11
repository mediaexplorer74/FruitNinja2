
// Type: GameManager.FruitSaveData
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class FruitSaveData
  {
    public Dictionary<uint, SliceTotal> totals = new Dictionary<uint, SliceTotal>();
    public Dictionary<uint, SliceTotal> universalTotals = new Dictionary<uint, SliceTotal>();
    public bool resume;
    public bool inGame;
    public bool game_rated;
    public LinkedList<EntityState> entities = new LinkedList<EntityState>();
    public int highScore;
    public int[] highScores = new int[4];
    public int score;
    public int misses;
    public int mode;
    public bool hasDropped;
    public int consecutiveType;
    public int consecutiveCount;
    public int numFruitTypesToPickFrom;
    public int[] fruitTypesToPickFrom = new int[32];
    public float speedLossTime;
    public float desiredSpeed;
    public float nextComboBonus;
    public float timer;
    public int criticalProgression;
    public int go_state;
    public float go_time;
    public int go_body;
    public int go_head;
    public int go_fruit;
    public int go_fact;
    public bool go_showHighScore;
    public bool go_setScore;
    public float go_bombHitTime;
    public float go_transition;
    public float shake_time;
    public float shake_max_time;
    public int currentWave;
    public float currentWaveDelay;
    public float currentWaveWait;
    public float globalWaveDt;
    public LinkedList<WaveState> waves = new LinkedList<WaveState>();
    public Dictionary<uint, AchievementItem> AchievementQue = new Dictionary<uint, AchievementItem>();
    public Dictionary<uint, AchievementItem> unlockedAchievements = new Dictionary<uint, AchievementItem>();
    public int m_blitzSpawnedThisGame;
    public int m_blitzForceSpawnedCounter;
    public float m_blitzSpawnTime;
    public Dictionary<int, int>[] waveGameCount = ArrayInit.CreateFilledArray<Dictionary<int, int>>(4);
    public int saveVersion;
    private static uint[] IDS = new uint[17]
    {
      StringFunctions.StringHash("261754"),
      StringFunctions.StringHash("261464"),
      StringFunctions.StringHash("261514"),
      StringFunctions.StringHash("261524"),
      StringFunctions.StringHash("261544"),
      StringFunctions.StringHash("261554"),
      StringFunctions.StringHash("261574"),
      StringFunctions.StringHash("261584"),
      StringFunctions.StringHash("261654"),
      StringFunctions.StringHash("261794"),
      StringFunctions.StringHash("324154"),
      StringFunctions.StringHash("324144"),
      StringFunctions.StringHash("324524"),
      StringFunctions.StringHash("350264"),
      StringFunctions.StringHash("350274"),
      StringFunctions.StringHash("370914"),
      StringFunctions.StringHash("374004")
    };
    private static string[] wp7Keys = new string[18]
    {
      "Wake Up",
      "Great Fruit Ninja",
      "Ultimate Fruit Ninja",
      "Fruit Fight",
      "Fruit Blitz",
      "Fruit Frenzy",
      "Fruit Rampage",
      "Fruit Annihilation",
      "Lucky Ninja",
      "Almost A Century",
      "Mango Magic",
      "Its All Pear Shaped",
      "Deja Vu",
      "Combo Mambo",
      "Moment Of Zen",
      "Year Of The Dragon",
      "The Lovely Bunch",
      "AAA"
    };
    public int numFruitTypesInSliceCombo;
    public int[] sliceComboFruitTypes = new int[11];

    public FruitSaveData()
    {
      this.score = 0;
      this.misses = 0;
      this.game_rated = false;
      this.go_time = -1f;
      this.go_transition = -1f;
      this.go_state = -1;
      this.go_bombHitTime = 0.0f;
      this.go_head = -1;
      this.go_body = -1;
      this.go_fruit = -1;
      this.go_fact = -1;
      this.go_showHighScore = false;
      this.go_setScore = false;
      this.criticalProgression = 70;
      this.shake_max_time = 1f;
      this.shake_time = 0.0f;
      this.highScore = 0;
      this.consecutiveCount = 0;
      this.consecutiveType = -1;
      this.mode = 0;
      this.timer = -1f;
      this.numFruitTypesToPickFrom = 0;
      for (int index = 0; index < 25; ++index)
        this.fruitTypesToPickFrom[index] = -1;
      for (int index = 0; index < 4; ++index)
      {
        this.highScores[index] = 0;
        this.waveGameCount[index].Clear();
      }
      this.currentWave = 0;
      this.currentWaveDelay = 0.0f;
      this.currentWaveWait = 0.0f;
      this.waves.Clear();
      this.AchievementQue.Clear();
      this.unlockedAchievements.Clear();
      this.saveVersion = 0;
      this.resume = false;
      this.inGame = false;
      this.hasDropped = false;
      this.totals.Clear();
      this.entities.Clear();
      this.numFruitTypesInSliceCombo = 0;
      for (int index = 0; index < 11; ++index)
        this.sliceComboFruitTypes[index] = -1;
      this.desiredSpeed = 0.0f;
      this.speedLossTime = 0.0f;
      this.nextComboBonus = 0.0f;
    }

    public FruitSaveData CLOAN()
    {
      FruitSaveData fruitSaveData = new FruitSaveData();
      if (this.waveGameCount != null)
      {
        fruitSaveData.waveGameCount = new Dictionary<int, int>[this.waveGameCount.Length];
        for (int index = 0; index < this.waveGameCount.Length; ++index)
        {
          if (this.waveGameCount[index] != null)
            fruitSaveData.waveGameCount[index] = this.waveGameCount[index].Duplicate<int, int>();
        }
      }
      fruitSaveData.totals = this.totals.Duplicate<uint, SliceTotal>();
      fruitSaveData.universalTotals = this.universalTotals.Duplicate<uint, SliceTotal>();
      fruitSaveData.entities = this.entities.Duplicate<EntityState>();
      fruitSaveData.waves = this.waves.Duplicate<WaveState>();
      fruitSaveData.highScores = (int[]) this.highScores.Clone();
      fruitSaveData.fruitTypesToPickFrom = (int[]) this.fruitTypesToPickFrom.Clone();
      fruitSaveData.resume = this.resume;
      fruitSaveData.inGame = this.inGame;
      fruitSaveData.game_rated = this.game_rated;
      fruitSaveData.highScore = this.highScore;
      fruitSaveData.score = this.score;
      fruitSaveData.misses = this.misses;
      fruitSaveData.mode = this.mode;
      fruitSaveData.hasDropped = this.hasDropped;
      fruitSaveData.consecutiveType = this.consecutiveType;
      fruitSaveData.consecutiveCount = this.consecutiveCount;
      fruitSaveData.numFruitTypesToPickFrom = this.numFruitTypesToPickFrom;
      fruitSaveData.timer = this.timer;
      fruitSaveData.criticalProgression = this.criticalProgression;
      fruitSaveData.go_state = this.go_state;
      fruitSaveData.go_time = this.go_time;
      fruitSaveData.go_body = this.go_body;
      fruitSaveData.go_head = this.go_head;
      fruitSaveData.go_fruit = this.go_fruit;
      fruitSaveData.go_fact = this.go_fact;
      fruitSaveData.go_showHighScore = this.go_showHighScore;
      fruitSaveData.go_setScore = this.go_setScore;
      fruitSaveData.go_bombHitTime = this.go_bombHitTime;
      fruitSaveData.go_transition = this.go_transition;
      fruitSaveData.shake_time = this.shake_time;
      fruitSaveData.shake_max_time = this.shake_max_time;
      fruitSaveData.currentWave = this.currentWave;
      fruitSaveData.currentWaveDelay = this.currentWaveDelay;
      fruitSaveData.saveVersion = this.saveVersion;
      fruitSaveData.m_blitzSpawnedThisGame = this.m_blitzSpawnedThisGame;
      fruitSaveData.m_blitzForceSpawnedCounter = this.m_blitzForceSpawnedCounter;
      fruitSaveData.m_blitzSpawnTime = this.m_blitzSpawnTime;
      fruitSaveData.AchievementQue = this.AchievementQue.Duplicate<uint, AchievementItem>();
      fruitSaveData.unlockedAchievements = this.unlockedAchievements.Duplicate<uint, AchievementItem>();
      return fruitSaveData;
    }

    public void SaveGameState() => throw new MissingMethodException();

    public int AddToTotal(string name, uint hash, int amount)
    {
      return this.AddToTotal(name, hash, amount, true);
    }

    public int AddToTotal(string name, uint hash, int amount, bool universal)
    {
      return this.AddToTotal(name, hash, amount, universal, true);
    }

    public int AddToTotal(
      string name,
      uint hash,
      int amount,
      bool universal,
      bool achievementUpload)
    {
      Dictionary<uint, SliceTotal> dictionary = universal ? this.universalTotals : this.totals;
      SliceTotal sliceTotal1;
      if (dictionary.TryGetValue(hash, out sliceTotal1))
      {
        sliceTotal1.total += amount;
        dictionary[hash] = sliceTotal1;
        if (achievementUpload)
          AchievementManager.GetInstance().UnlockSpecificFruitAchievement(sliceTotal1.total, hash);
        return sliceTotal1.total;
      }
      SliceTotal sliceTotal2;
      sliceTotal2.hash = hash;
      sliceTotal2.name = name;
      sliceTotal2.total = amount;
      dictionary[hash] = sliceTotal2;
      AchievementManager.GetInstance().UnlockSpecificFruitAchievement(sliceTotal2.total, hash);
      return sliceTotal2.total;
    }

    public void UnlockTotals()
    {
      foreach (KeyValuePair<uint, SliceTotal> universalTotal in this.universalTotals)
        AchievementManager.GetInstance().UnlockSpecificFruitAchievement(universalTotal.Value.total, universalTotal.Key);
      foreach (KeyValuePair<uint, SliceTotal> total in this.totals)
        AchievementManager.GetInstance().UnlockSpecificFruitAchievement(total.Value.total, total.Key);
    }

    public int GetTotal(uint hash)
    {
      SliceTotal sliceTotal;
      return this.totals.TryGetValue(hash, out sliceTotal) || this.universalTotals.TryGetValue(hash, out sliceTotal) ? sliceTotal.total : 0;
    }

    public bool IsAchievementUnlocked(uint nameHash)
    {
      foreach (KeyValuePair<uint, AchievementItem> keyValuePair in this.AchievementQue)
      {
        if ((int) keyValuePair.Key == (int) nameHash)
          return true;
      }
      foreach (KeyValuePair<uint, AchievementItem> unlockedAchievement in this.unlockedAchievements)
      {
        if ((int) unlockedAchievement.Key == (int) nameHash)
          return true;
      }
      return false;
    }

    public bool AddToQue(string name, uint nameHash)
    {
      if (this.IsAchievementUnlocked(nameHash))
        return false;
      AchievementItem achievementItem = new AchievementItem();
      achievementItem.name = name;
      achievementItem.time = 2.9f;
      if (this.AchievementQue.Count > 0)
        achievementItem.time += 0.1f;
      this.AchievementQue[nameHash] = achievementItem;
      if (!Mortar.Game1.emulator)
      {
        uint num = StringFunctions.StringHash(name);
        for (int index = 0; index < FruitSaveData.IDS.Length; ++index)
        {
          if ((int)FruitSaveData.IDS[index] == (int) num)
                        AchievementManager.AwardAchievementWP7(FruitSaveData.wp7Keys[index]);
        }
      }
      return true;
    }

    public void Update(float dt, HUD hud)
    {
      float num = 100f;
      bool flag = false;
      KeyValuePair<uint, AchievementItem> keyValuePair1 = new KeyValuePair<uint, AchievementItem>();
      foreach (KeyValuePair<uint, AchievementItem> keyValuePair2 in this.AchievementQue)
      {
        if ((double) keyValuePair2.Value.time < (double) num)
        {
          num = keyValuePair2.Value.time;
          keyValuePair1 = keyValuePair2;
          flag = true;
        }
      }
      if (!flag)
        return;
      float time = keyValuePair1.Value.time;
      AchievementItem achievementItem = keyValuePair1.Value;
      achievementItem.time -= dt;
      this.AchievementQue[keyValuePair1.Key] = achievementItem;
      if ((double) time >= 2.8900001049041748 && (double) achievementItem.time < 2.8900001049041748)
      {
        if (Mortar.Math.BETWEEN((int) keyValuePair1.Value.name[0], 48, 57))
          NetworkManager.GetInstance().UnlockAchievement(keyValuePair1.Value.name);
        else
          ItemManager.GetInstance().UnlockItem(keyValuePair1.Key);
        AchievementManager.GetInstance().UnlockedAchievement(keyValuePair1.Key, hud);
      }
      if ((double) keyValuePair1.Value.time > 0.0)
        return;
      this.unlockedAchievements[keyValuePair1.Key] = keyValuePair1.Value;
      this.AchievementQue.Remove(keyValuePair1.Key);
    }

    public void ClearTotals() => this.totals.Clear();

    public void ClearTotal(uint hash)
    {
      this.totals.Remove(hash);
      this.universalTotals.Remove(hash);
    }

    public void ClearCombo()
    {
      this.numFruitTypesInSliceCombo = 0;
      for (int index = 0; index < 11; ++index)
        this.sliceComboFruitTypes[index] = -1;
    }

    public void FinishedGame()
    {
      for (uint index = 0; index < 4U; ++index)
      {
        List<int> intList = new List<int>();
        foreach (KeyValuePair<int, int> keyValuePair in this.waveGameCount[(int) index])
        {
          if (keyValuePair.Value > -1)
            intList.Add(keyValuePair.Key);
        }
        foreach (int key in intList)
        {
          int num = this.waveGameCount[(int) index][key] - 1;
          this.waveGameCount[(int) index][key] = num;
        }
      }
    }
  }
}
