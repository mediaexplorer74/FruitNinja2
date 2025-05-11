
// Type: GameManager.HUD
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class HUD
  {
    public float[] m_tint = new float[3];
    public float[] m_backTint = new float[3];
    protected LinkedList<HUDControl> m_controls = new LinkedList<HUDControl>();
    protected List<HUDControl> m_notifications = new List<HUDControl>();
    private static float[] tint = new float[3]{ 1f, 1f, 1f };

    public HUD()
    {
      for (int index = 0; index < 3; ++index)
      {
        this.m_backTint[index] = 1f;
        this.m_tint[index] = 1f;
      }
    }

    public void Init() => this.m_controls.Clear();

    public void Release()
    {
      foreach (HUDControl control in this.m_controls)
        control.m_deleteCall(control);
      this.m_controls.Clear();
    }

    public void AddNotification(HUDControl control) => this.m_notifications.Add(control);

    public void AddControl(HUDControl control) => this.AddControl(control, false);

    public void AddControl(HUDControl control, bool toFront)
    {
      if (toFront)
        this.m_controls.AddFirst(control);
      else
        this.m_controls.AddLast(control);
    }

    public void RemoveControl(HUDControl control)
    {
      if (control == null)
        return;
      control.m_deleteCall(control);
      this.m_controls.Remove(control);
    }

    public void ResetControls()
    {
      foreach (HUDControl control in this.m_controls)
        control.Reset();
    }

    public void Update(float dt)
    {
      MissControl.PreUpdate(dt);
      LinkedListNode<HUDControl> node = this.m_controls.First;
      while (node != null)
      {
        if (node.Value.GetActive())
          node.Value.Update(dt);
        if (node.Value.Terminate())
        {
          node.Value.m_deleteCall(node.Value);
          LinkedListNode<HUDControl> next = node.Next;
          this.m_controls.Remove(node);
          node = next;
        }
        else
          node = node.Next;
      }
      if (this.m_notifications.Count <= 0)
        return;
      HUDControl notification = this.m_notifications[0];
      notification.Update(dt);
      if (!notification.Terminate())
        return;
      this.m_notifications.Remove(notification);
    }

    public void Save()
    {
      foreach (HUDControl control in this.m_controls)
        control.Save();
    }

    public void Draw() => this.Draw(HUD.HUD_ORDER.HUD_ORDER_NORMAL);

    public void Draw(HUD.HUD_ORDER order)
    {
      foreach (HUDControl control in this.m_controls)
      {
        if (control.m_drawOrder == order && control.GetActive() && (!PopOverControl.IsInPopup || control.partOfPopup))
        {
          if (control.m_canBeTinted)
          {
            control.PreDraw(this.m_tint);
            control.Draw(this.m_tint);
          }
          else
          {
            control.PreDraw(HUD.tint);
            control.Draw(HUD.tint);
          }
        }
      }
      if (this.m_notifications.Count <= 0)
        return;
      HUDControl notification = this.m_notifications[0];
      if (notification.m_drawOrder != order || !notification.GetActive() || PopOverControl.IsInPopup && !notification.partOfPopup)
        return;
      if (notification.m_canBeTinted)
      {
        notification.PreDraw(this.m_tint);
        notification.Draw(this.m_tint);
      }
      else
      {
        notification.PreDraw(HUD.tint);
        notification.Draw(HUD.tint);
      }
    }

    public void Skip()
    {
      foreach (HUDControl control in this.m_controls)
        control.Skip();
    }

    public void OnPause()
    {
      foreach (HUDControl control in this.m_controls)
      {
        if (control.GetType() == HUD_TYPE.HUD_TYPE_SCROLLING_LIST)
          ((ScrollingMenu) control).ClearTouch();
      }
    }

    public enum HUD_ORDER
    {
      HUD_ORDER_NORMAL,
      HUD_ORDER_POST,
      HUD_ORDER_BEFORE_SPLAT,
      HUD_ORDER_AFTER_SPLAT,
      HUD_ORDER_BEFORE_BOMB,
      HUD_ORDER_AFTER_BOMB,
    }
  }
}
