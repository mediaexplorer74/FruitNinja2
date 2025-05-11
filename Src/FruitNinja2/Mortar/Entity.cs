
// Type: Mortar.Entity
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace Mortar
{
  public abstract class Entity
  {
    public const int ENTITY_INVALID = -1;
    public bool partOfPopup;
    public uint m_id;
    public Entity.EntityFlagShifts m_entity_flags;
    public Vector3 m_pos;
    public Vector3 m_vel;
    public Vector3 m_cur_scale;
    public byte m_type;
    public ushort m_dir_angle;
    public Col m_col_box;
    private static uint heapsize;

    public bool m_dormant
    {
      get => (this.m_entity_flags & Entity.EntityFlagShifts.DORMANT) != (Entity.EntityFlagShifts) 0;
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.DORMANT | (value ? Entity.EntityFlagShifts.DORMANT : (Entity.EntityFlagShifts) 0);
      }
    }

    public bool m_updateAlways
    {
      get
      {
        return (this.m_entity_flags & Entity.EntityFlagShifts.UPDATE_ALWAYS) != (Entity.EntityFlagShifts) 0;
      }
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.UPDATE_ALWAYS | (value ? Entity.EntityFlagShifts.UPDATE_ALWAYS : (Entity.EntityFlagShifts) 0);
      }
    }

    public bool m_onProcess
    {
      get
      {
        return (this.m_entity_flags & Entity.EntityFlagShifts.ON_PROCESS) != (Entity.EntityFlagShifts) 0;
      }
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.ON_PROCESS | (value ? Entity.EntityFlagShifts.ON_PROCESS : (Entity.EntityFlagShifts) 0);
      }
    }

    public bool m_onScreen
    {
      get
      {
        return (this.m_entity_flags & Entity.EntityFlagShifts.ON_SCREEN) != (Entity.EntityFlagShifts) 0;
      }
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.ON_SCREEN | (value ? Entity.EntityFlagShifts.ON_SCREEN : (Entity.EntityFlagShifts) 0);
      }
    }

    public bool m_destroy
    {
      get
      {
        return (this.m_entity_flags & Entity.EntityFlagShifts.SELF_CLEAN_UP) != (Entity.EntityFlagShifts) 0;
      }
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.SELF_CLEAN_UP | (value ? Entity.EntityFlagShifts.SELF_CLEAN_UP : (Entity.EntityFlagShifts) 0);
      }
    }

    public bool m_selfCleanUp
    {
      get => (this.m_entity_flags & Entity.EntityFlagShifts.DESTROY) != (Entity.EntityFlagShifts) 0;
      set
      {
        this.m_entity_flags = this.m_entity_flags & ~Entity.EntityFlagShifts.DESTROY | (value ? Entity.EntityFlagShifts.DESTROY : (Entity.EntityFlagShifts) 0);
      }
    }

    public Entity()
    {
      this.m_pos = Vector3.Zero;
      this.m_vel = Vector3.Zero;
    }

    public virtual void Init(byte[] tpl_data, int tpl_size, Vector3? size)
    {
    }

    public virtual void Release() => this.m_col_box = (Col) null;

    public abstract void Update(float dt);

    public abstract void Draw();

    public abstract void DrawUpdate(float dt);

    public void Update() => this.Update(0.0f);

    public void DrawUpdate() => this.DrawUpdate(0.0f);

    public virtual void PostLoad()
    {
    }

    public virtual bool InRect(ColAABB rect)
    {
      if (this.m_col_box != null)
      {
        this.m_col_box.centre.X = this.m_pos.X;
        this.m_col_box.centre.Y = this.m_pos.Y;
        this.m_col_box.centre.Z = this.m_pos.Z;
        Vector3 proj = Vector3.Zero;
        if (this.m_col_box.Collide((Col) rect, out proj))
          return true;
      }
      return false;
    }

    public virtual bool CollisionResponse(Entity p_ent2, uint col_1, uint col_2, ref Vector3 proj)
    {
      return false;
    }

    public virtual bool Collide(Entity p_ent2, Col col, out uint collide_flags, out Vector3 proj)
    {
      proj = Vector3.Zero;
      collide_flags = 0U;
      return this.m_col_box != null && this.m_col_box.Collide(col, out proj);
    }

    public virtual void ReceiveMessage(Entity p_sender, Message message)
    {
      switch (message.message)
      {
        case MessageType.MESSAGE_ACTIVE:
          this.m_dormant = false;
          break;
        case MessageType.MESSAGE_DORMANT:
          this.m_dormant = true;
          break;
      }
    }

    public virtual void ListenerCallback(Entity p_sender, Entity p_receiver, Message message)
    {
    }

    public void Activate() => this.m_dormant = false;

    public void Deactivate() => this.m_destroy = true;

    public static void HeapCreate(uint size) => Entity.heapsize = size;

    public static void HeapCreate() => Entity.heapsize = 268435455U;

    public static void HeapDestroy() => Entity.heapsize = 0U;

    public static void HeapClear()
    {
    }

    public static void HeapDisplay()
    {
    }

    public static void HeapDisplay(bool full)
    {
    }

    public static uint HeapGetSize() => Entity.heapsize;

    public static uint HeapGetFree() => Entity.heapsize;

    public static bool HeapExist() => Entity.heapsize != 0U;

    [Flags]
    public enum EntityFlagShifts
    {
      DORMANT = 1,
      UPDATE_ALWAYS = 2,
      ON_PROCESS = 4,
      ON_SCREEN = 8,
      DESTROY = 16, // 0x00000010
      SELF_CLEAN_UP = 32, // 0x00000020
    }
  }
}
