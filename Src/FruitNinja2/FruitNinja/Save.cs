
// Type: FruitNinja2.Save
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class Save
  {
    public const int MAX_FRUIT_TYPES_TO_PICK_FROM = 32;
    public const int MAX_SLICE_COMBO = 11;
    public const int MAX_METRIC_TOTALS = 5;
    public const int MIN_BYTES_FREE = 8192;
    private static string SAVE_DIRECTORY = "FruitNinja";
    private static string SAVE_FILENAME = Save.SAVE_DIRECTORY + "/savegame.xml";
    public static bool force_save = false;
    private static uint[] universalHashes = new uint[7]
    {
      StringFunctions.StringHash("all"),
      StringFunctions.StringHash("games"),
      StringFunctions.StringHash("totalscore"),
      StringFunctions.StringHash("sessions"),
      StringFunctions.StringHash("bomb"),
      StringFunctions.StringHash("coming_soon"),
      StringFunctions.StringHash("unrated_games")
    };

    private static void QueryIntAttribute(XmlReader reader, string name, ref int val)
    {
      string attribute = reader.GetAttribute(name);
      if (attribute != null)
        val = MParser.ParseInt(attribute);
      else
        val = 0;
    }

    private static void QueryFloatAttribute(XmlReader reader, string name, ref float val)
    {
      string attribute = reader.GetAttribute(name);
      if (attribute != null)
        val = MParser.ParseFloat(attribute);
      else
        val = 0.0f;
    }

    private static void ParseAchievements(XDocument doc, FruitSaveData saveData, bool queList)
    {
      XmlReader reader = doc.CreateReader();
      while (reader.Read())
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.Name)
          {
            case "ach":
              AchievementItem achievementItem = new AchievementItem();
              achievementItem.time = 0.0f;
              string attribute1 = reader.GetAttribute("name");
              if (attribute1 != null)
              {
                uint key = StringFunctions.StringHash(attribute1);
                achievementItem.name = attribute1;
                string attribute2 = reader.GetAttribute("time");
                if (attribute2 != null)
                  achievementItem.time = MParser.ParseFloat(attribute2);
                if (queList)
                {
                  saveData.AchievementQue[key] = achievementItem;
                  continue;
                }
                saveData.unlockedAchievements[key] = achievementItem;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }

    private static void ParseWaveInfo(XDocument doc, FruitSaveData saveData)
    {
      XmlReader reader = doc.CreateReader();
      saveData.waves.Clear();
      WaveState waveState = new WaveState();
      waveState.spawners = new LinkedList<SpawnState>();
      while (reader.Read())
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.Name)
          {
            case "wave":
              waveState = new WaveState();
              waveState.spawners = new LinkedList<SpawnState>();
              Save.QueryFloatAttribute(reader, "inc", ref waveState.inc);
              Save.QueryIntAttribute(reader, "index", ref waveState.index);
              saveData.waves.AddLast(waveState);
              continue;
            case "spawner":
              SpawnState spawnState = new SpawnState();
              Save.QueryFloatAttribute(reader, "delay", ref spawnState.delayWait);
              Save.QueryIntAttribute(reader, "toSpawn", ref spawnState.toSpawnThisWave);
              if (waveState.spawners != null)
              {
                waveState.spawners.AddLast(spawnState);
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }

    private static void ParseSaveFile(XDocument doc, FruitSaveData saveData)
    {
      XmlReader reader = doc.CreateReader();
      while (reader.Read())
      {
        switch (reader.NodeType)
        {
          case XmlNodeType.Element:
            if (string.Compare(reader.Name, "save_file") == 0)
            {
              XElement element = doc.Element((XName) "save_file");
              element.QueryIntAttribute("highscore", ref saveData.highScore);
              element.QueryIntAttribute("critical_chance", ref saveData.criticalProgression);
              int num1 = 0;
              int num2 = 0;
              int num3 = 0;
              element.QueryIntAttribute("major", ref num1);
              element.QueryIntAttribute("minor", ref num2);
              element.QueryIntAttribute("patch", ref num3);
              XAttribute xattribute = element.Attribute((XName) "rated");
              saveData.game_rated = xattribute != null && string.Compare(xattribute.Value, "true") == 0;
              saveData.highScores[0] = saveData.highScore;
              for (int mode = 0; mode < 4; ++mode)
              {
                string AttributeName = string.Format("{0}highscore", (object) Initialise.GetModeName((Game.GAME_MODE) mode));
                element.QueryIntAttribute(AttributeName, ref saveData.highScores[mode]);
              }
              saveData.saveVersion = num1 * 10000 + num2 * 100 + num3;
              continue;
            }
            if (string.Compare(reader.Name, "que") == 0)
            {
              Save.ParseAchievements(doc, saveData, true);
              continue;
            }
            if (string.Compare(reader.Name, "unlocked") == 0)
            {
              Save.ParseAchievements(doc, saveData, false);
              continue;
            }
            if (string.Compare(reader.Name, "total") == 0)
            {
              int amount = 0;
              string attribute1 = reader.GetAttribute("score");
              if (attribute1 != null)
                amount = MParser.ParseInt(attribute1);
              bool universal = false;
              string attribute2 = reader.GetAttribute("u");
              if (attribute2 != null && string.Compare(attribute2, "true") == 0)
                universal = true;
              string attribute3 = reader.GetAttribute("type");
              if (attribute3 != null && amount > 0)
              {
                uint hash = StringFunctions.StringHash(attribute3);
                if (!universal)
                {
                  for (int index = 0; index < Save.universalHashes.Length; ++index)
                  {
                    if ((int) Save.universalHashes[index] == (int) hash)
                    {
                      universal = true;
                      break;
                    }
                  }
                }
                saveData.AddToTotal(attribute3, hash, amount, universal, false);
                continue;
              }
              continue;
            }
            if (string.Compare(reader.Name, "ent") == 0)
            {
              if (saveData.mode < 4 && saveData.saveVersion == Game.GetVersionTotal())
              {
                EntityState entityState = new EntityState();
                string attribute4 = reader.GetAttribute("pos");
                entityState.pos = Save.ParseVector(attribute4);
                string attribute5 = reader.GetAttribute("vel");
                entityState.vel = Save.ParseVector(attribute5);
                string attribute6 = reader.GetAttribute("grav");
                entityState.grav = Save.ParseVector(attribute6);
                string attribute7 = reader.GetAttribute("wait");
                if (attribute7 != null)
                  entityState.wait = MParser.ParseFloat(attribute7);
                string attribute8 = reader.GetAttribute("hit");
                entityState.hit = attribute8 != null && string.Compare(attribute8, "true") == 0;
                string attribute9 = reader.GetAttribute("type");
                if (attribute9 != null)
                  entityState.type = MParser.ParseInt(attribute9);
                saveData.entities.AddLast(entityState);
                continue;
              }
              continue;
            }
            if (string.Compare(reader.Name, "powers") == 0)
            {
              PowerUpManager.GetInstance().LoadActivePowerUps(doc.Element((XName) "save_file").Element((XName) "powers"), saveData.mode);
              continue;
            }
            if (string.Compare(reader.Name, "state") == 0)
            {
              saveData.entities.Clear();
              string attribute10 = reader.GetAttribute("mode");
              if (attribute10 != null)
                saveData.mode = (int) Initialise.ParseGameMode(StringFunctions.StringHash(attribute10));
              if (saveData.mode < 4 && saveData.saveVersion == Game.GetVersionTotal())
              {
                Save.QueryIntAttribute(reader, "score", ref saveData.score);
                Save.QueryIntAttribute(reader, "misses", ref saveData.misses);
                string attribute11 = reader.GetAttribute("hasDropped");
                saveData.hasDropped = attribute11 != null && string.Compare(attribute11, "true") == 0;
                Save.QueryIntAttribute(reader, "consecutiveCount", ref saveData.consecutiveCount);
                Save.QueryIntAttribute(reader, "consecutiveType", ref saveData.consecutiveType);
                Save.QueryFloatAttribute(reader, "timer", ref saveData.timer);
                Save.QueryIntAttribute(reader, "go_head", ref saveData.go_head);
                Save.QueryIntAttribute(reader, "go_body", ref saveData.go_body);
                Save.QueryIntAttribute(reader, "go_fruit", ref saveData.go_fruit);
                Save.QueryIntAttribute(reader, "go_fact", ref saveData.go_fact);
                string attribute12 = reader.GetAttribute("go_showHighScore");
                saveData.go_showHighScore = attribute12 != null && string.Compare(attribute12, "true") == 0;
                string attribute13 = reader.GetAttribute("go_setScore");
                saveData.go_setScore = attribute13 != null && string.Compare(attribute13, "true") == 0;
                string attribute14 = reader.GetAttribute("randSeed");
                if (attribute14 != null)
                  Mortar.Math.g_random.x = MParser.ParseULong(attribute14);
                string attribute15 = reader.GetAttribute("randMul");
                if (attribute15 != null)
                  Mortar.Math.g_random.mul = MParser.ParseULong(attribute15);
                string attribute16 = reader.GetAttribute("randAdd");
                if (attribute16 != null)
                  Mortar.Math.g_random.add = MParser.ParseULong(attribute16);
                Save.QueryIntAttribute(reader, "go_state", ref saveData.go_state);
                Save.QueryFloatAttribute(reader, "go_time", ref saveData.go_time);
                Save.QueryFloatAttribute(reader, "go_bombHitTime", ref saveData.go_bombHitTime);
                Save.QueryFloatAttribute(reader, "go_transition", ref saveData.go_transition);
                Save.QueryFloatAttribute(reader, "shake_time", ref saveData.shake_time);
                Save.QueryFloatAttribute(reader, "shake_max_time", ref saveData.shake_max_time);
                Save.QueryFloatAttribute(reader, "speedLossTime", ref saveData.speedLossTime);
                Save.QueryFloatAttribute(reader, "desiredSpeed", ref saveData.desiredSpeed);
                Save.QueryFloatAttribute(reader, "nextComboBonus", ref saveData.nextComboBonus);
                continue;
              }
              continue;
            }
            if (string.Compare(reader.Name, "wave_info") == 0)
            {
              if (saveData.mode < 4 && saveData.saveVersion == Game.GetVersionTotal())
              {
                Save.QueryIntAttribute(reader, "waveCount", ref saveData.currentWave);
                Save.QueryFloatAttribute(reader, "waveDelay", ref saveData.currentWaveDelay);
                Save.QueryFloatAttribute(reader, "waveWait", ref saveData.currentWaveWait);
                Save.QueryIntAttribute(reader, "blitzSpawnedThisGame", ref saveData.m_blitzSpawnedThisGame);
                Save.QueryIntAttribute(reader, "blitzForceSpawnedCounter", ref saveData.m_blitzForceSpawnedCounter);
                Save.QueryFloatAttribute(reader, "blitzSpawnTime", ref saveData.m_blitzSpawnTime);
                Save.ParseWaveInfo(doc, saveData);
                continue;
              }
              continue;
            }
            bool flag = false;
            for (int mode = 0; mode < 4; ++mode)
            {
              string name = "wave_counts_" + Initialise.GetModeName((Game.GAME_MODE) mode);
              if (name == reader.Name)
              {
                flag = true;
                for (XElement element = doc.Element((XName) "save_file").Element((XName) name).FirstChildElement("game_count"); element != null; element = element.NextSiblingElement("game_count"))
                {
                  int key = -1;
                  int num = -1;
                  element.QueryIntAttribute("waveIdx", ref key);
                  element.QueryIntAttribute("games", ref num);
                  saveData.waveGameCount[mode][key] = num;
                }
              }
            }
            int num4 = flag ? 1 : 0;
            continue;
          default:
            continue;
        }
      }
    }

    public static bool LoadGame(FruitSaveData saveData)
    {
      
      try
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream)null;
        if (storeForApplication.FileExists(Save.SAVE_FILENAME))
        {
          storageFileStream = storeForApplication.OpenFile(Save.SAVE_FILENAME, FileMode.Open);
          if (storageFileStream.Length > 0L)
          {
            byte[] numArray = new byte[storageFileStream.Length];
            storageFileStream.Read(numArray, 0, (int) storageFileStream.Length);
            Save.ParseSaveFile(XDocument.Parse(Encoding.UTF8.GetString(numArray, 0, numArray.Length)), saveData);
            if (saveData.mode >= 4)
              saveData.mode = 0;
            if (saveData.saveVersion != Game.GetVersionTotal())
            {
              saveData.ClearTotal(StringFunctions.StringHash("unrated_games"));
              for (int index = 0; index < 4; ++index)
                saveData.waveGameCount[index].Clear();
              saveData.game_rated = false;
            }
          }
         
          storageFileStream.Close();
        }
        else
        {
          if (storeForApplication.AvailableFreeSpace < 8192L)
            throw new Exception("Not enough space to create save");
          if (!storeForApplication.DirectoryExists(Save.SAVE_DIRECTORY))
            storeForApplication.CreateDirectory(Save.SAVE_DIRECTORY);
          storageFileStream = storeForApplication.OpenFile(Save.SAVE_FILENAME, FileMode.CreateNew);
        
         storageFileStream.Close();
        }
      }
      catch (Exception ex)
      {
      }
      //finally
      //{
        //storageFileStream?.Close();
      //}
      return false;
    }

    public static bool SaveGame(FruitSaveData saveData)
    {
      bool flag = false;
     
      try
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream)null;
        if (!storeForApplication.DirectoryExists("FruitNinja"))
          storeForApplication.CreateDirectory("FruitNinja");
        XDocument xdocument = new XDocument();
        xdocument.Declaration = new XDeclaration("1.0", "utf-8", "yes");
        XElement content1 = new XElement((XName) "save_file");
        xdocument.Add((object) content1);
        for (int mode = 0; mode < 4; ++mode)
        {
          string name = string.Format("{0}highscore", (object) Initialise.GetModeName((Game.GAME_MODE) mode));
          content1.Add((object) new XAttribute((XName) name, (object) saveData.highScores[mode]));
        }
        content1.Add((object) new XAttribute((XName) "highscore", (object) saveData.highScore));
        content1.Add((object) new XAttribute((XName) "critical_chance", (object) saveData.criticalProgression));
        content1.Add((object) new XAttribute((XName) "rated", saveData.game_rated ? (object) "true" : (object) "false"));
        content1.Add((object) new XAttribute((XName) "major", (object) Game.GetVersionMajor()));
        content1.Add((object) new XAttribute((XName) "minor", (object) Game.GetVersionMinor()));
        content1.Add((object) new XAttribute((XName) "batch", (object) Game.GetVersionPatch()));
        foreach (KeyValuePair<uint, SliceTotal> total in saveData.totals)
        {
          XElement content2 = new XElement((XName) "total");
          content2.Add((object) new XAttribute((XName) "type", (object) total.Value.name));
          content2.Add((object) new XAttribute((XName) "score", (object) total.Value.total));
          content1.Add((object) content2);
        }
        foreach (KeyValuePair<uint, SliceTotal> universalTotal in saveData.universalTotals)
        {
          XElement content3 = new XElement((XName) "total");
          content3.Add((object) new XAttribute((XName) "u", (object) true));
          content3.Add((object) new XAttribute((XName) "type", (object) universalTotal.Value.name));
          content3.Add((object) new XAttribute((XName) "score", (object) universalTotal.Value.total));
          content1.Add((object) content3);
        }
        if (saveData.AchievementQue.Count > 0)
        {
          XElement content4 = new XElement((XName) "que");
          foreach (KeyValuePair<uint, AchievementItem> keyValuePair in saveData.AchievementQue)
          {
            XElement content5 = new XElement((XName) "ach");
            content5.Add((object) new XAttribute((XName) "name", (object) keyValuePair.Value.name));
            content5.Add((object) new XAttribute((XName) "time", (object) keyValuePair.Value.time));
            content4.Add((object) content5);
          }
          content1.Add((object) content4);
        }
        if (saveData.unlockedAchievements.Count > 0)
        {
          XElement content6 = new XElement((XName) "unlocked");
          foreach (KeyValuePair<uint, AchievementItem> unlockedAchievement in saveData.unlockedAchievements)
          {
            XElement content7 = new XElement((XName) "ach");
            content7.Add((object) new XAttribute((XName) "name", (object) unlockedAchievement.Value.name));
            content6.Add((object) content7);
          }
          content1.Add((object) content6);
        }
        if (saveData.inGame || (double) saveData.go_bombHitTime > 0.0 || Save.force_save)
        {
          XElement content8 = new XElement((XName) "state");
          content8.Add((object) new XAttribute((XName) "hasDropped", saveData.hasDropped ? (object) "true" : (object) "false"));
          content8.Add((object) new XAttribute((XName) "score", (object) saveData.score));
          content8.Add((object) new XAttribute((XName) "misses", (object) saveData.misses));
          content8.Add((object) new XAttribute((XName) "mode", (object) Initialise.GetModeName((Game.GAME_MODE) saveData.mode)));
          content8.Add((object) new XAttribute((XName) "consecutiveCount", (object) saveData.consecutiveCount));
          content8.Add((object) new XAttribute((XName) "consecutiveType", (object) saveData.consecutiveType));
          content8.Add((object) new XAttribute((XName) "timer", (object) saveData.timer));
          content8.Add((object) new XAttribute((XName) "globalWaveDt", (object) saveData.globalWaveDt));
          string str1 = (string) null;
          for (int index = 0; index < saveData.numFruitTypesToPickFrom; ++index)
            str1 = index != 0 ? string.Format("{0},{1}", (object) str1, (object) saveData.fruitTypesToPickFrom[index]) : string.Format("{0}", (object) saveData.fruitTypesToPickFrom[index]);
          if (str1 != null)
            content8.Add((object) new XAttribute((XName) "typesToPickFrom", (object) str1));
          for (int index = 0; index < saveData.numFruitTypesInSliceCombo; ++index)
            str1 = index != 0 ? string.Format("{0},{1}", (object) str1, (object) saveData.sliceComboFruitTypes[index]) : string.Format("{0}", (object) saveData.sliceComboFruitTypes[index]);
          if (str1 != null)
            content8.Add((object) new XAttribute((XName) "best_combo", (object) str1));
          content8.Add((object) new XAttribute((XName) "go_state", (object) saveData.go_state));
          content8.Add((object) new XAttribute((XName) "go_time", (object) saveData.go_time));
          content8.Add((object) new XAttribute((XName) "go_bombHitTime", (object) saveData.go_bombHitTime));
          content8.Add((object) new XAttribute((XName) "go_transition", (object) saveData.go_transition));
          content8.Add((object) new XAttribute((XName) "go_body", (object) saveData.go_body));
          content8.Add((object) new XAttribute((XName) "go_head", (object) saveData.go_head));
          content8.Add((object) new XAttribute((XName) "go_fruit", (object) saveData.go_fruit));
          content8.Add((object) new XAttribute((XName) "go_fact", (object) saveData.go_fact));
          content8.Add((object) new XAttribute((XName) "go_showHighScore", saveData.go_showHighScore ? (object) "true" : (object) "false"));
          content8.Add((object) new XAttribute((XName) "go_setScore", saveData.go_setScore ? (object) "true" : (object) "false"));
          content8.Add((object) new XAttribute((XName) "randSeed", (object) Mortar.Math.g_random.x));
          content8.Add((object) new XAttribute((XName) "randMul", (object) Mortar.Math.g_random.mul));
          content8.Add((object) new XAttribute((XName) "randAdd", (object) Mortar.Math.g_random.add));
          content8.Add((object) new XAttribute((XName) "shake_time", (object) saveData.shake_time));
          content8.Add((object) new XAttribute((XName) "shake_max_time", (object) saveData.shake_time));
          if ((double) saveData.desiredSpeed > 0.0)
          {
            content8.Add((object) new XAttribute((XName) "speedLossTime", (object) saveData.speedLossTime));
            content8.Add((object) new XAttribute((XName) "desiredSpeed", (object) saveData.desiredSpeed));
            content8.Add((object) new XAttribute((XName) "nextComboBonus", (object) saveData.nextComboBonus));
          }
          if (saveData.waves.Count > 0)
          {
            XElement content9 = new XElement((XName) "wave_info");
            content9.Add((object) new XAttribute((XName) "waveCount", (object) saveData.currentWave));
            content9.Add((object) new XAttribute((XName) "waveDelay", (object) saveData.currentWaveDelay));
            content9.Add((object) new XAttribute((XName) "waveWait", (object) saveData.currentWaveWait));
            content9.Add((object) new XAttribute((XName) "blitzSpawnedThisGame", (object) saveData.m_blitzSpawnedThisGame));
            content9.Add((object) new XAttribute((XName) "blitzForceSpawnedCounter", (object) saveData.m_blitzForceSpawnedCounter));
            content9.Add((object) new XAttribute((XName) "blitzSpawnTime", (object) saveData.m_blitzSpawnTime));
            foreach (WaveState wave in saveData.waves)
            {
              XElement content10 = new XElement((XName) "wave");
              content10.Add((object) new XAttribute((XName) "inc", (object) (float) ((double) wave.inc + 0.0099999997764825821)));
              content10.Add((object) new XAttribute((XName) "index", (object) wave.index));
              foreach (SpawnState spawner in wave.spawners)
              {
                XElement content11 = new XElement((XName) "spawner");
                content11.Add((object) new XAttribute((XName) "delay", (object) spawner.delayWait));
                content11.Add((object) new XAttribute((XName) "toSpawn", (object) spawner.toSpawnThisWave));
                content10.Add((object) content11);
              }
              content9.Add((object) content10);
            }
            content8.Add((object) content9);
          }
          LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
          for (Fruit fruit = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
          {
            if (!fruit.m_isSliced && !fruit.m_isMenuItem)
            {
              XElement content12 = new XElement((XName) "ent");
              string str2 = string.Format("{0},{1},{2}", (object) fruit.m_pos.X, (object) fruit.m_pos.Y, (object) fruit.m_pos.Z);
              content12.Add((object) new XAttribute((XName) "pos", (object) str2));
              string str3 = string.Format("{0},{1},{2}", (object) fruit.m_vel.X, (object) fruit.m_vel.Y, (object) fruit.m_vel.Z);
              content12.Add((object) new XAttribute((XName) "vel", (object) str3));
              string str4 = string.Format("{0},{1},{2}", (object) fruit.m_gravity.X, (object) fruit.m_gravity.Y, (object) fruit.m_gravity.Z);
              content12.Add((object) new XAttribute((XName) "grav", (object) str4));
              content12.Add((object) new XAttribute((XName) "type", (object) fruit.GetFruitType()));
              content12.Add((object) new XAttribute((XName) "wait", (object) fruit.GetWait()));
              content8.Add((object) content12);
            }
          }
          for (Bomb bomb = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
          {
            if (!bomb.m_isMenuItem)
            {
              XElement content13 = new XElement((XName) "ent");
              string str5 = string.Format("{0},{1},{2}", (object) bomb.m_pos.X, (object) bomb.m_pos.Y, (object) bomb.m_pos.Z);
              content13.Add((object) new XAttribute((XName) "pos", (object) str5));
              string str6 = string.Format("{0},{1},{2}", (object) bomb.m_vel.X, (object) bomb.m_vel.Y, (object) bomb.m_vel.Z);
              content13.Add((object) new XAttribute((XName) "vel", (object) str6));
              string str7 = string.Format("{0},{1},{2}", (object) bomb.m_gravity.X, (object) bomb.m_gravity.Y, (object) bomb.m_gravity.Z);
              content13.Add((object) new XAttribute((XName) "grav", (object) str7));
              content13.Add((object) new XAttribute((XName) "type", (object) Fruit.MAX_FRUIT_TYPES));
              content13.Add((object) new XAttribute((XName) "hit", bomb.m_hit ? (object) "true" : (object) "false"));
              content13.Add((object) new XAttribute((XName) "wait", (object) bomb.GetWait()));
              content8.Add((object) content13);
            }
          }
          for (BombBlast bombBlast = (BombBlast) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB_BLAST, ref iterator); bombBlast != null; bombBlast = (BombBlast) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB_BLAST, ref iterator))
          {
            XElement content14 = new XElement((XName) "ent");
            string str8 = string.Format("{0},{1},{2}", (object) bombBlast.m_pos.X, (object) bombBlast.m_pos.Y, (object) bombBlast.m_pos.Z);
            content14.Add((object) new XAttribute((XName) "pos", (object) str8));
            content14.Add((object) new XAttribute((XName) "wait", (object) bombBlast.m_time));
            content14.Add((object) new XAttribute((XName) "type", (object) -1));
            content8.Add((object) content14);
          }
          content1.Add((object) content8);
        }
        for (int mode = 0; mode < 4; ++mode)
        {
          string name = string.Format("wave_counts_{0}", (object) Initialise.GetModeName((Game.GAME_MODE) mode));
          if (saveData.waveGameCount[mode].Count > 0)
          {
            XElement content15 = new XElement((XName) name);
            foreach (KeyValuePair<int, int> keyValuePair in saveData.waveGameCount[mode])
            {
              XElement content16 = new XElement((XName) "game_count");
              content16.Add((object) new XAttribute((XName) "waveIdx", (object) keyValuePair.Key));
              content16.Add((object) new XAttribute((XName) "games", (object) keyValuePair.Value));
              content15.Add((object) content16);
            }
            content1.Add((object) content15);
          }
        }
        XElement xelement = new XElement((XName) "powers");
        PowerUpManager.GetInstance().SaveActivePowerUps(xelement);
        content1.Add((object) xelement);
        MemoryStream memoryStream = new MemoryStream();
        xdocument.Save((Stream) memoryStream);
        byte[] array = memoryStream.ToArray();
        Encoding.UTF8.GetString(array, 0, array.Length);
        storageFileStream = storeForApplication.OpenFile(Save.SAVE_FILENAME, FileMode.Create);
        storageFileStream.Write(array, 3, array.Length - 3);
        storageFileStream?.Close();
        flag = true;
      }
      catch (Exception ex)
      {
      }
      //finally
      //{
      //  storageFileStream?.Close();
      //}
      return flag;
    }

    public static Vector3 ParseVector(string text)
    {
      if (text == null)
        return Vector3.Zero;
      int length = text.IndexOf(',');
      Vector3 vector = new Vector3();
      if (length == -1)
      {
        vector.X = MParser.ParseFloat(text);
      }
      else
      {
        vector.X = MParser.ParseFloat(text.Substring(0, length));
        int num = text.IndexOf(',', length + 1);
        if (num == -1)
        {
          vector.Y = MParser.ParseFloat(text.Substring(length + 1, text.Length - length - 1));
        }
        else
        {
          vector.Y = MParser.ParseFloat(text.Substring(length + 1, num - length - 1));
          vector.Z = MParser.ParseFloat(text.Substring(num + 1, text.Length - num - 1));
        }
      }
      return vector;
    }

    public static bool ParseVector(string text, ref Vector3 vec)
    {
      if (text == null || text.Length <= 0)
        return false;
      vec = Save.ParseVector(text);
      return true;
    }
  }
}
