
// Type: Mortar.InputDeviceIphoneTouch
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  internal class InputDeviceIphoneTouch : InputDevice
  {
    private int currTouchId;
    private int lx;
    private int ly;
    private uint randomTimestampThing;

    public override void Init()
    {
    }

    public override void Update(float dt)
    {
      ++this.randomTimestampThing;
      if (this.currTouchId == 0)
      {
        this.currTouchId = (int) Touch.GetInstance().GetMostRecentTouch();
        if (this.currTouchId == 0)
          this.currTouchId = (int) Touch.GetInstance().GetAnyTouch();
        if (this.currTouchId != 0)
        {
          int x;
          int y;
          Touch.GetInstance().GetTouchPos((uint) this.currTouchId, out x, out y);
          int absolute1 = x;
          int absolute2 = y;
          this.AxisEvent(116, 32U, (float) absolute1, (float) (absolute1 - this.lx), this.randomTimestampThing);
          this.AxisEvent(117, 32U, (float) absolute2, (float) (absolute2 - this.ly), this.randomTimestampThing);
          this.ButtonPressed(108U, 1U, 1f, this.randomTimestampThing);
          this.lx = absolute1;
          this.ly = absolute2;
        }
        else
          this.ButtonPressed(108U, 8U, 1f, this.randomTimestampThing);
      }
      else
      {
        uint num = Touch.GetInstance().GetMostRecentTouch();
        if (num == 0U)
          num = Touch.GetInstance().GetAnyTouch();
        bool flag = (long) num != (long) this.currTouchId;
        if (!Touch.GetInstance().GetTouchDelta((uint) this.currTouchId, out int _, out int _) || flag)
        {
          this.ButtonPressed(108U, 4U, 1f, this.randomTimestampThing);
          this.ButtonPressed(108U, 8U, 1f, this.randomTimestampThing);
          this.currTouchId = 0;
        }
        else
        {
          int x;
          int y;
          Touch.GetInstance().GetTouchPos((uint) this.currTouchId, out x, out y);
          int absolute3 = x;
          int absolute4 = y;
          this.AxisEvent(116, 32U, (float) absolute3, (float) (absolute3 - this.lx), this.randomTimestampThing);
          this.AxisEvent(117, 32U, (float) absolute4, (float) (absolute4 - this.ly), this.randomTimestampThing);
          this.ButtonPressed(108U, 2U, 1f, this.randomTimestampThing);
          this.lx = absolute3;
          this.ly = absolute4;
        }
      }
      Touch.GetInstance().SendIndividualTouchCallbacks((InputDevice) this);
    }

    public override void Destroy() => base.Destroy();

    public override void Reset()
    {
      this.currTouchId = 0;
      this.lx = 0;
      this.ly = 0;
      Touch.GetInstance().Clear();
    }

    public bool TestAxisDelta(int axis, float amount) => false;
  }
}
