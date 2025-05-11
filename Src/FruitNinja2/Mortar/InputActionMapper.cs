
// Type: Mortar.InputActionMapper
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class InputActionMapper
  {
    protected bool m_active;
    protected uint m_hash;
    protected uint m_creationHash;
    protected InputEvent m_event;
    protected InputActionMapper.InputCallback m_callback;

    public InputActionMapper(InputEvent e, InputActionMapper.InputCallback call)
      : this(e, call, 0U)
    {
    }

    public InputActionMapper(InputEvent e, InputActionMapper.InputCallback call, uint hash)
      : this(e, call, hash, 0U)
    {
    }

    public InputActionMapper(
      InputEvent e,
      InputActionMapper.InputCallback call,
      uint hash,
      uint creationHash)
    {
      this.m_hash = hash;
      this.m_active = true;
      this.m_event = new InputEvent();
      this.m_event.axis = e.axis;
      this.m_event.button = e.button;
      this.m_event.chr = e.chr;
      this.m_event.eventType = e.eventType;
      this.m_event.timeStamp = e.timeStamp;
      this.m_callback = call;
      this.m_creationHash = creationHash;
    }

    public uint GetHash() => this.m_hash;

    public uint GetParentHash() => this.m_creationHash;

    public void SetCallback(InputActionMapper.InputCallback callback) => this.m_callback = callback;

    public bool ProcessEvent(InputEvent e)
    {
      uint num1 = e.eventType & 4294901760U;
      uint num2 = e.eventType & (uint) ushort.MaxValue;
      if (((int) num1 & (int) this.m_event.eventType) != 0 && ((int) num2 & (int) this.m_event.eventType) != 0)
      {
        bool flag1 = false;
        bool flag2;
        switch (num1)
        {
          case 65536:
            if ((int) e.button.key == (int) this.m_event.button.key)
            {
              flag2 = flag1 | this.m_callback(e);
              break;
            }
            break;
          case 131072:
            if (this.m_event.axis.axis >= 137)
            {
              if (e.axis.axis == this.m_event.axis.axis)
                return flag2 = flag1 | this.m_callback(e);
              break;
            }
            if ((e.axis.axis & this.m_event.axis.axis) != 0)
              return flag2 = flag1 | this.m_callback(e);
            break;
        }
      }
      return false;
    }

    public InputEvent GetEvent() => this.m_event;

    public delegate bool InputCallback(InputEvent bob);
  }
}
