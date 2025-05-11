
// Type: GameManager.ActorManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class ActorManager
  {
    public const int MAX_NEW_ENTITIES = 512;
    public const int MAX_OLD_ENTITIES = 512;
    protected uint m_heap;
    protected int m_heap_size;
    protected Entity[] m_inactivelist = new Entity[512];
    protected uint m_inactiveCount;
    protected Entity[] m_kill_list = new Entity[512];
    protected uint m_killListSize;
    protected LinkedList<Entity>[] m_entities;
    protected LinkedList<MessageListener> m_listeners = new LinkedList<MessageListener>();
    protected int m_max_entities;
    protected byte m_collision_visible;
    protected bool PAD00;
    protected byte m_filler0;
    protected byte m_filler1;
    protected ActorManager.ActorFactory m_factory;
    protected ActorManager.ActorTypeHashConvert m_typeConverter;
    private static ActorManager instance = new ActorManager();

    private ActorManager()
    {
    }

    private void Deactivate(Entity p_ent)
    {
      for (LinkedListNode<Entity> node = this.m_entities[(int) p_ent.m_type].First; node != null; node = node.Next)
      {
        if (node.Value == p_ent)
        {
          this.m_entities[(int) p_ent.m_type].Remove(node);
          this.m_inactivelist[(int)(IntPtr) this.m_inactiveCount++] = p_ent;
          break;
        }
      }
    }

    public static ActorManager GetInstance() => ActorManager.instance;

    public void Initialise(int max_Entities) => this.Initialise(max_Entities, 1000);

    public void Initialise(int max_Entities, int heap_size)
    {
      this.m_heap = 1U;
      this.m_heap_size = heap_size;
      this.m_max_entities = max_Entities;
      this.m_entities = ArrayInit.CreateFilledArray<LinkedList<Entity>>(this.m_max_entities);
      for (int index = 0; index < this.m_max_entities; ++index)
        this.m_entities[index] = new LinkedList<Entity>();
      Entity.HeapExist();
    }

    public void PostLoad()
    {
      for (int index = 0; index < this.m_max_entities; ++index)
      {
        foreach (Entity entity in this.m_entities[index])
          entity.PostLoad();
      }
    }

    public void Destroy()
    {
      this.Clear();
      this.m_heap = 0U;
      this.m_heap_size = 0;
      this.m_entities = (LinkedList<Entity>[]) null;
      this.m_inactiveCount = 0U;
      this.m_killListSize = 0U;
    }

    public int GetHeapFree() => this.m_heap_size;

    public int GetHeapSize() => this.m_heap_size;

    public void HeapDisplay() => this.HeapDisplay(false);

    public void HeapDisplay(bool full) => throw new MissingMethodException();

    public bool LoadEntity(EntityChunk p_chunk, byte[] tpl_data, int tpl_size, int pos_scale)
    {
      bool b = false;
      int type = this.m_typeConverter(p_chunk.type_hash, ref b);
      if (-1 == type)
        return false;
      Entity entity = this.Add(type, b);
      if (entity == null)
        return false;
      entity.m_id = p_chunk.name_hash;
      Vector3 vector3_1 = new Vector3(p_chunk.volume_max.X
          * (float) (1 << pos_scale), p_chunk.volume_max.Y
          * (float) (1 << pos_scale), p_chunk.volume_max.Z * (float) (1 << pos_scale));

      Vector3 vector3_2 = new Vector3(p_chunk.volume_min.X 
          * (float) (1 << pos_scale), p_chunk.volume_min.Y
          * (float) (1 << pos_scale), p_chunk.volume_min.Z * (float) (1 << pos_scale));
      Vector3 vector3_3 = Vector3.Subtract(vector3_1, vector3_2);

      Vector3 vector3_4 = Vector3.Add(vector3_2,
          new Vector3(vector3_3.X / 2f, vector3_3.Y / 2f, vector3_3.Z / 2f));

      entity.m_dir_angle = (ushort) p_chunk.rotate.Y;
      entity.m_pos = vector3_4;
      entity.m_cur_scale = vector3_3;
      entity.m_dormant = BitConverter.ToUInt32(tpl_data, 0) != 0U;
      tpl_size -= 4;
      tpl_data.CopyTo((Array) tpl_data, 4);
      entity.Init(tpl_data, tpl_size, new Vector3?(p_chunk.scale));
      entity.m_id = p_chunk.name_hash;
      return true;
    }

    public void Update(float dt) => this.Update(dt, (ColAABB) null, (ColAABB) null);

    public void Update(float dt, ColAABB draw_rect, ColAABB process_rect)
    {
      if (this.m_heap != 0U && this.m_entities != null)
      {
        for (int index = 0; index < this.m_max_entities; ++index)
        {
          foreach (Entity entity in this.m_entities[index])
          {
            if (!entity.m_dormant && !entity.m_destroy)
            {
              entity.m_onProcess = true;
              entity.m_onScreen = true;
              entity.Update(dt);
              entity.DrawUpdate(dt);
            }
            if (entity.m_destroy)
              this.m_kill_list[(int) this.m_killListSize++] = entity;
          }
        }
      }
      for (int index = 0; (long) index < (long) this.m_killListSize; ++index)
        this.Deactivate(this.m_kill_list[index]);
      this.m_killListSize = 0U;
    }

    public void DeactivateAllEntities(int type)
    {
      foreach (Entity entity in this.m_entities[type])
        entity.m_destroy = true;
    }

    public void RegisterFactory(ActorManager.ActorFactory factory) => this.m_factory = factory;

    public void UnregisterFactory() => this.m_factory = (ActorManager.ActorFactory) null;

    public void RegisterHashConverter(ActorManager.ActorTypeHashConvert convert)
    {
      this.m_typeConverter = convert;
    }

    public void UnregisterHashConverter()
    {
      this.m_typeConverter = (ActorManager.ActorTypeHashConvert) null;
    }

    public void AddMessageListener(MessageListener listen) => this.m_listeners.AddLast(listen);

    public void RemoveMessageListener(MessageListener listen) => this.m_listeners.Remove(listen);

    public void ClearAllListeners()
    {
      for (LinkedListNode<MessageListener> node = this.m_listeners.First; node != null; node = node.Next)
      {
        MessageListener dl = node.Value;
        this.m_listeners.Remove(node);
        Delete.SAFE_DELETE<MessageListener>(ref dl);
      }
      this.m_listeners.Clear();
    }

    public void Draw()
    {
      if (this.m_heap == 0U || this.m_entities == null)
        return;
      for (int index = 0; index < this.m_max_entities; ++index)
      {
        foreach (Entity entity in this.m_entities[index])
        {
          if (!entity.m_dormant && !entity.m_destroy)
            entity.Draw();
        }
      }
    }

    public Entity Add(int type) => this.Add(type, true);

    public Entity Add(int type, bool update_entity) => this.Add((EntityTypes) type, true);

    public Entity Add(Entity p_entity, int type)
    {
      if (p_entity != null)
      {
        this.m_entities[type].AddLast(p_entity);
        p_entity.m_type = (byte) type;
      }
      return p_entity;
    }

    public Entity Add(EntityTypes type) => this.Add(type, true);

    public Entity Add(EntityTypes type, bool update_entity)
    {
      for (int index1 = (int) this.m_inactiveCount - 1; index1 >= 0; --index1)
      {
        Entity entity = this.m_inactivelist[index1];
        if ((EntityTypes) entity.m_type == type)
        {
          this.m_entities[(int) type].AddLast(entity);
          --this.m_inactiveCount;
          for (int index2 = index1; (long) index2 < (long) this.m_inactiveCount; ++index2)
            this.m_inactivelist[index2] = this.m_inactivelist[index2 + 1];
          entity.Activate();
          return entity;
        }
      }
      Entity entity1 = this.m_factory((int) type);
      if (entity1 != null)
      {
        this.m_entities[(int) type].AddLast(entity1);
        entity1.m_type = (byte) type;
      }
      return entity1;
    }

    public Entity Add(Entity p_entity, EntityTypes type) => this.Add(p_entity, (int) type);

    public void Remove(Entity p_ent)
    {
      foreach (Entity entity in this.m_entities[(int) p_ent.m_type])
      {
        if (entity == p_ent)
        {
          if (!entity.m_selfCleanUp)
            entity.Release();
          this.m_entities[(int) p_ent.m_type].Remove(entity);
          break;
        }
      }
    }

    public void Clear()
    {
      this.m_killListSize = 0U;
      if (this.m_heap == 0U || this.m_entities == null)
        return;
      for (int index = 0; index < this.m_max_entities; ++index)
      {
        foreach (Entity entity in this.m_entities[index])
        {
          if (!entity.m_selfCleanUp)
            entity.Release();
        }
        this.m_entities[index].Clear();
      }
      for (int index = 0; (long) index < (long) this.m_inactiveCount; ++index)
        this.m_inactivelist[index].Release();
      this.m_inactiveCount = 0U;
    }

    public uint GetNumEntities()
    {
      int numEntities = 0;
      for (int index = 0; index < this.m_max_entities; ++index)
        numEntities += this.m_entities[index].Count;
      return (uint) numEntities;
    }

    public uint GetNumEntities(int type) => (uint) this.m_entities[type].Count;

    public uint GetNumEntities(int type_start, int type_end)
    {
      if (type_start > type_end)
        Mortar.Math.Swap(ref type_start, ref type_end);
      int numEntities = 0;
      for (int index = type_start; index < type_end; ++index)
        numEntities += this.m_entities[index].Count;
      return (uint) numEntities;
    }

    public uint GetNumEntities(int[] types)
    {
      int numEntities = 0;
      for (int index = 0; types[index] != -1; ++index)
      {
        int type = types[index];
        if (type != -1)
          numEntities += this.m_entities[type].Count;
      }
      return (uint) numEntities;
    }

    public uint GetNumTypes()
    {
      int numTypes = 0;
      for (int index = 0; index < this.m_max_entities; ++index)
      {
        if (this.m_entities[index].Count > 0)
          ++numTypes;
      }
      return (uint) numTypes;
    }

    public Entity Find(uint id)
    {
      for (int index = 0; index < this.m_max_entities; ++index)
      {
        foreach (Entity entity in this.m_entities[index])
        {
          if ((int) entity.m_id == (int) id)
            return entity;
        }
      }
      return (Entity) null;
    }

    public Entity Find(EntityTypes type, uint id)
    {
      foreach (Entity entity in this.m_entities[(int) type])
      {
        if ((int) entity.m_id == (int) id)
          return entity;
      }
      return (Entity) null;
    }

    public Entity GetEntity(EntityTypes type, uint idx)
    {
      uint num = 0;
      foreach (Entity entity in this.m_entities[(int) type])
      {
        if ((int) num == (int) idx)
          return entity;
        ++num;
      }
      return (Entity) null;
    }

    public Entity GetEntityFirst(EntityTypes type, ref LinkedListNode<Entity> iterator)
    {
      iterator = this.m_entities[(int) type].First;
      return iterator != null ? iterator.Value : (Entity) null;
    }

    public Entity GetEntityNext(EntityTypes type, ref LinkedListNode<Entity> iterator)
    {
      iterator = iterator.Next;
      return iterator != null ? iterator.Value : (Entity) null;
    }

    public int GetEntityIdx(Entity p_ent)
    {
      int type = (int) p_ent.m_type;
      if (Mortar.Math.BETWEEN(type, 0, this.m_max_entities - 1))
      {
        int entityIdx = 0;
        foreach (Entity entity in this.m_entities[type])
        {
          if (p_ent == entity)
            return entityIdx;
          ++entityIdx;
        }
      }
      return -1;
    }

    public bool SendMessage(uint id, Entity src, Message msg)
    {
      Entity p_receiver = this.Find(id);
      foreach (MessageListener listener in this.m_listeners)
      {
        if ((MessageType) listener.message == msg.message && (listener.to_id == 0U || (int) listener.to_id == (int) id) && (listener.from_id == 0U || src != null && (int) listener.from_id == (int) src.m_id))
          listener.p_target.ListenerCallback(src, p_receiver, msg);
      }
      this.m_listeners.Clear();
      if (p_receiver == null)
        return false;
      p_receiver.ReceiveMessage(src, msg);
      return true;
    }

    public uint GetNumInAABB(ColAABB aabb) => 0;

    public uint GetNumInProcess() => 0;

    public uint GetNumInDraw() => 0;

    public void DisplayUsage() => this.DisplayUsage(false);

    public void DisplayUsage(bool full) => throw new MissingMethodException();

    public void DrawDebug() => throw new MissingMethodException();

    public void SetCollisionVisible(byte state) => throw new MissingMethodException();

    public byte GetCollisionVisible() => this.m_collision_visible;

    public delegate Entity ActorFactory(int n);

    public delegate int ActorTypeHashConvert(uint hash, ref bool b);
  }
}
