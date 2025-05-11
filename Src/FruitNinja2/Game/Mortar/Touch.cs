
// Type: Mortar.Touch
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public class Touch
  {
    private const int MAX_TOUCH_POINTS = 4;
    public const int PLATFORM_IPHONEOS_RES_X = 480;
    private Touch.State[] currentState = ArrayInit.CreateFilledArray<Touch.State>(4);
    private Touch.State[] nextState = ArrayInit.CreateFilledArray<Touch.State>(4);
    private static int loop = 0;
    public RingBufferT<Touch.TEvnt> eventBuffer = new RingBufferT<Touch.TEvnt>();
    public uint externalIDGenerator;
    public static Touch instance = new Touch();

    private Touch()
    {
      this.externalIDGenerator = 1U;
      this.eventBuffer.Init(10);
      for (int index = 0; index < 4; ++index)
      {
        this.currentState[index].touchState = 1;
        this.nextState[index].touchState = 1;
      }
    }

    public void ___UpdateInternal(uint internalId, bool dn, float x, float y)
    {
      int index1 = -1;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        int index3 = (index2 + Touch.loop) % 4;
        if ((int) this.nextState[index3].internalId == (int) internalId)
        {
          if (dn)
          {
            this.nextState[index3].x = (int) x;
            this.nextState[index3].y = (int) y;
            return;
          }
          this.nextState[index3].touchState = 1;
          return;
        }
        if (this.nextState[index3].internalId == 0U)
          index1 = index3;
      }
      if (!dn || index1 == -1)
        return;
      ++Touch.loop;
      if (Touch.loop >= 4)
        Touch.loop = 0;
      this.nextState[index1].externalId = this.externalIDGenerator++;
      if (this.externalIDGenerator == 0U)
        this.externalIDGenerator = 1U;
      this.nextState[index1].internalId = internalId;
      this.nextState[index1].touchState = -1;
      this.nextState[index1].x = (int) x;
      this.nextState[index1].y = (int) y;
      this.nextState[index1].lx = (int) x;
      this.nextState[index1].ly = (int) y;
    }

    public void __UpdateInternal(
      uint internalId,
      bool dn,
      float x,
      float y,
      float timeStampSeconds)
    {
      Touch.TEvnt v;
      v.internalId = internalId;
      v.dn = dn;
      v.x = x;
      v.y = y;
      v.timeStampSeconds = timeStampSeconds;
      if (this.eventBuffer.Push(ref v))
        return;
      this.Update();
      this.eventBuffer.Push(ref v);
    }

    public int FindTouch(uint exId)
    {
      for (int touch = 0; touch < 4; ++touch)
      {
        if ((int) this.currentState[touch].externalId == (int) exId)
          return touch;
      }
      return -1;
    }

    public void _Update()
    {
      for (int index = 0; index < 4; ++index)
      {
        this.currentState[index] = this.nextState[index];
        this.nextState[index].Update();
      }
    }

    public int GetTouchCount()
    {
      int touchCount = 0;
      for (int index = 0; index < 4; ++index)
      {
        if (this.nextState[index].touchState != 1)
          ++touchCount;
      }
      return touchCount;
    }

    public Vector2 GetPosition()
    {
      for (int index = 0; index < 4; ++index)
      {
        if (this.nextState[index].touchState != 1)
          return new Vector2((float) this.nextState[index].x, (float) this.nextState[index].y);
      }
      return new Vector2();
    }

    public void Clear()
    {
      for (int index = 0; index < 4; ++index)
      {
        this.nextState[index].externalId = 0U;
        this.nextState[index].internalId = 0U;
        this.nextState[index].touchState = 1;
        this.nextState[index].x = 0;
        this.nextState[index].y = 0;
        this.nextState[index].lx = 0;
        this.nextState[index].ly = 0;
      }
    }

    public static Touch GetInstance() => Touch.instance;

    public void Update() => this.Update(0.0f);

    public void Update(float gameTime)
    {
      while (true)
      {
        Touch.TEvnt ans1 = new Touch.TEvnt();
        if (this.eventBuffer.Peek(ref ans1) && ((double) ans1.timeStampSeconds <= (double) gameTime || (double) gameTime == 0.0))
        {
          Touch.TEvnt ans2 = new Touch.TEvnt();
          this.eventBuffer.Pop(ref ans2);
          this.___UpdateInternal(ans2.internalId, ans2.dn, ans2.x, ans2.y);
        }
        else
          break;
      }
      this._Update();
    }

    public bool GetTouchPos(uint uid, out int x, out int y)
    {
      x = 0;
      y = 0;
      int touch = this.FindTouch(uid);
      if (touch == -1)
        return false;
      x = this.currentState[touch].x;
      y = this.currentState[touch].y;
      return this.currentState[touch].touchState != 1;
    }

    public bool GetTouchDelta(uint uid, out int x, out int y)
    {
      x = 0;
      y = 0;
      int touch = this.FindTouch(uid);
      if (touch == -1)
        return false;
      if (this.currentState[touch].touchState >= 0)
      {
        x = this.currentState[touch].x - this.currentState[touch].lx;
        y = this.currentState[touch].y - this.currentState[touch].ly;
      }
      return this.currentState[touch].touchState != 1;
    }

    public uint GetTouchInReigion(int x, int y, int w, int h)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (this.currentState[index].touchState < 1)
        {
          int x1 = this.currentState[index].x;
          int y1 = this.currentState[index].y;
          if (x1 >= x && x1 <= x + w && y1 >= y && y1 <= y + h)
            return this.currentState[index].externalId;
        }
      }
      return 0;
    }

    public uint GetMostRecentTouch()
    {
      return this.FindTouch(this.externalIDGenerator - 1U) != -1 ? this.externalIDGenerator - 1U : 0U;
    }

    public uint GetAnyTouch()
    {
      for (int index = 0; index < 4; ++index)
      {
        if (this.currentState[index].touchState < 1)
          return this.currentState[index].externalId;
      }
      return 0;
    }

    public void SendIndividualTouchCallbacks(InputDevice device)
    {
      for (int index = 0; index < 4; ++index)
      {
        int num = index;
        if (this.currentState[index].touchState < 1)
        {
          int x = this.currentState[index].x;
          int y = this.currentState[index].y;
          int lx = this.currentState[index].lx;
          int ly = this.currentState[index].ly;
          device.AxisEvent(153 + num, 32U, (float) x, (float) (x - lx), 0U);
          device.AxisEvent(169 + num, 32U, (float) y, (float) (y - ly), 0U);
          if (this.currentState[index].touchState == -1)
            device.ButtonPressed((uint) (137 + num), 1U, 1f, 0U);
          device.ButtonPressed((uint) (137 + num), 2U, 1f, 0U);
        }
        else
        {
          if (this.currentState[index].internalId != 0U && this.currentState[index].externalId != 0U)
            device.ButtonPressed((uint) (137 + num), 4U, 1f, 0U);
          device.ButtonPressed((uint) (137 + num), 8U, 1f, 0U);
        }
      }
    }

    private struct State
    {
      public int lx;
      public int ly;
      public int x;
      public int y;
      public uint internalId;
      public uint externalId;
      public int touchState;

      public void Update()
      {
        if (this.touchState == 1)
        {
          this.internalId = 0U;
          this.externalId = 0U;
        }
        else
        {
          this.lx = this.x;
          this.ly = this.y;
          if (this.touchState != -1)
            return;
          this.touchState = 0;
        }
      }
    }

    public struct TEvnt
    {
      public uint internalId;
      public bool dn;
      public float x;
      public float y;
      public float timeStampSeconds;
    }
  }
}
