
// Type: Mortar.InputDevice
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;

#nullable disable
namespace Mortar
{
  public abstract class InputDevice
  {
    protected LinkedList<InputActionMapper> m_actions = new LinkedList<InputActionMapper>();

    public abstract void Init();

    public abstract void Update(float dt);

    public virtual void Destroy() => this.m_actions.Clear();

    public virtual void Reset()
    {
    }

    public void ButtonPressed(uint button, uint flags, float pressure, uint timestamp)
    {
      this.CheckActions(new InputEvent()
      {
        eventType = 65536U | flags,
        button = {
          key = button,
          pressure = pressure
        },
        timeStamp = timestamp
      });
    }

    public void AxisEvent(int axis, uint flags, float absolute, float relative, uint timestamp)
    {
      this.CheckActions(new InputEvent()
      {
        eventType = 131072U | flags,
        axis = {
          axis = axis,
          absolutePos = absolute,
          relativePos = relative
        },
        timeStamp = timestamp
      });
    }

    public virtual void AddAction(InputActionMapper action) => this.m_actions.AddLast(action);

    public void CheckActions(InputEvent e)
    {
      foreach (InputActionMapper action in this.m_actions)
        action.ProcessEvent(e);
    }

    public void ClearActions(uint ParentHash, bool del)
    {
      for (LinkedListNode<InputActionMapper> node = this.m_actions.First; node != null; node = node.Next)
      {
        if ((int) node.Value.GetParentHash() == (int) ParentHash || ParentHash == 0U)
        {
          InputActionMapper dl = node.Value;
          this.m_actions.Remove(node);
          if (del)
            Delete.SAFE_DELETE<InputActionMapper>(ref dl);
        }
      }
    }

    public void RegisterInputCallback(
      uint hash,
      string str,
      InputActionMapper.InputCallback callback)
    {
      bool flag = false;
      foreach (InputActionMapper action in this.m_actions)
      {
        if ((int) action.GetHash() == (int) hash)
        {
          flag = true;
          action.SetCallback(callback);
        }
      }
      int num = flag ? 1 : 0;
    }
  }
}
