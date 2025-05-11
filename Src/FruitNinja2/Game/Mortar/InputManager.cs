
// Type: Mortar.InputManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mortar
{
  public class InputManager
  {
    protected volatile bool m_loadingFile;
    protected volatile bool m_updating;
    protected LinkedList<InputDevice> m_deviceList = new LinkedList<InputDevice>();
    private static InputManager.KeyStringMap[] ParseKeykeymaps = new InputManager.KeyStringMap[61]
    {
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton1"), 108U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton2"), 109U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton3"), 110U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton4"), 111U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton5"), 112U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton6"), 113U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton7"), 114U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseButton8"), 115U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseAxisX"), 116U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("MouseAxisY"), 117U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch1"), 137U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch2"), 138U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch3"), 139U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch4"), 140U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch5"), 141U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch6"), 142U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch7"), 143U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch8"), 144U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch9"), 145U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch10"), 146U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch11"), 147U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch12"), 148U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch13"), 149U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch14"), 150U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch15"), 151U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("Touch16"), 152U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX1"), 153U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX2"), 154U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX3"), 155U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX4"), 156U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX5"), 157U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX6"), 158U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX7"), 159U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX8"), 160U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX9"), 161U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX10"), 162U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX11"), 163U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX12"), 164U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX13"), 165U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX14"), 166U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX15"), 167U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisX16"), 168U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY1"), 169U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY2"), 170U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY3"), 171U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY4"), 172U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY5"), 173U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY6"), 174U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY7"), 175U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY8"), 176U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY9"), 177U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY10"), 178U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY11"), 179U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY12"), 180U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY13"), 181U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY14"), 182U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY15"), 183U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("TouchAxisY16"), 184U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("AccelAxisX"), 185U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("AccelAxisY"), 186U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("AccelAxisZ"), 187U)
    };
    private static InputManager.KeyStringMap[] ParseActionkeymaps = new InputManager.KeyStringMap[7]
    {
      new InputManager.KeyStringMap(StringFunctions.StringHash("pressed"), 1U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("released"), 4U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("down"), 2U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("up"), 8U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("active"), 16U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("move"), 32U),
      new InputManager.KeyStringMap(StringFunctions.StringHash("dead"), 64U)
    };
    private static InputManager instance;

    private InputManager()
    {
    }

    protected uint ParseKey(uint hash)
    {
      for (int index = 0; index < InputManager.ParseKeykeymaps.Length; ++index)
      {
        if ((int) hash == (int) InputManager.ParseKeykeymaps[index].hash)
          return InputManager.ParseKeykeymaps[index].key;
      }
      return 0;
    }

    protected uint ParseAction(uint hash)
    {
      for (int index = 0; index < InputManager.ParseActionkeymaps.Length; ++index)
      {
        if ((int) hash == (int) InputManager.ParseActionkeymaps[index].hash)
          return InputManager.ParseActionkeymaps[index].key;
      }
      return 0;
    }

    protected bool ValidCharacter(byte c) => Math.BETWEEN((int) c, 32, 175);

    public static InputManager GetInstance()
    {
      if (InputManager.instance == null)
        InputManager.instance = new InputManager();
      return InputManager.instance;
    }

    public void Init()
    {
      InputDevice inputDevice = (InputDevice) new InputDeviceIphoneTouch();
      inputDevice.Init();
      this.m_deviceList.AddLast(inputDevice);
    }

    public void Update(float dt)
    {
      if (this.m_loadingFile)
        return;
      this.m_updating = true;
      foreach (InputDevice device in this.m_deviceList)
        device.Update(dt);
      this.m_updating = false;
    }

    public void Destroy()
    {
      this.m_loadingFile = false;
      this.m_updating = false;
      int num = 0;
      foreach (InputDevice device in this.m_deviceList)
      {
        InputDevice idevice = default;
        if (num++ == 0)
          device.ClearActions(0U, true);
        device.Destroy();
        Delete.SAFE_DELETE<InputDevice>(ref idevice);
      }
      this.m_deviceList.Clear();
    }

    public bool LoadConfigFile(string fileName)
    {
      this.m_loadingFile = true;
      do
        ;
      while (this.m_updating);
      uint creationHash = MortarFile.Exists(fileName) ? StringFunctions.StringHash(fileName) : throw new Exception("Input file is missing");
      string str = MortarFile.LoadText(fileName);
      uint hash = 0;
      uint num1 = 0;
      uint num2 = 0;
      char[] chArray = new char[(int) byte.MaxValue];
      int length = 0;
      if (str != null)
      {
        int num3 = 0;
        InputEvent e = new InputEvent();
        e.eventType = 0U;
        for (int index = 0; index < str.Length; ++index)
        {
          switch (str[index])
          {
            case '\t':
            case ' ':
              continue;
            case '\n':
              chArray[length] = char.MinValue;
              uint action1 = this.ParseAction(StringFunctions.StringHash(new string(chArray, 0, length)));
              num2 |= action1;
              e.eventType = num2;
              if (e.eventType < 16U)
              {
                e.eventType |= 65536U;
                e.button.key = num1;
                e.button.pressure = 0.0f;
              }
              else
              {
                e.eventType |= 131072U;
                e.axis.axis = (int) num1;
              }
              if (hash != 0U && num1 != 0U && num2 != 0U)
              {
                InputActionMapper action2 = new InputActionMapper(e, (InputActionMapper.InputCallback) null, hash, creationHash);
                InputManager.GetInstance().AddActionMapper(action2);
              }
              length = 0;
              num3 = 0;
              continue;
            case ',':
              chArray[length] = char.MinValue;
              if (num3 == 1)
              {
                uint key = this.ParseKey(StringFunctions.StringHash(new string(chArray, 0, length)));
                num1 |= key;
              }
              else
              {
                uint action3 = this.ParseAction(StringFunctions.StringHash(new string(chArray, 0, length)));
                num2 |= action3;
              }
              length = 0;
              continue;
            case ':':
              chArray[length] = char.MinValue;
              hash = StringFunctions.StringHash(new string(chArray, 0, length));
              length = 0;
              ++num3;
              continue;
            case ';':
              chArray[length] = char.MinValue;
              num1 = this.ParseKey(StringFunctions.StringHash(new string(chArray, 0, length)));
              length = 0;
              num2 = 0U;
              ++num3;
              continue;
            default:
              if (this.ValidCharacter((byte) str[index]))
              {
                chArray[length++] = str[index];
                continue;
              }
              continue;
          }
        }
        if (num3 == 2)
        {
          chArray[length] = char.MinValue;
          uint num4 = num2 | this.ParseAction(StringFunctions.StringHash(new string(chArray)));
          e.eventType = num4;
          if (e.eventType < 16U)
          {
            e.eventType |= 65536U;
            e.button.key = num1;
            e.button.pressure = 0.0f;
          }
          else
          {
            e.eventType |= 131072U;
            e.axis.axis = (int) num1;
          }
          if (hash != 0U && num1 != 0U && num4 != 0U)
          {
            InputActionMapper action = new InputActionMapper(e, (InputActionMapper.InputCallback) null, hash, creationHash);
            InputManager.GetInstance().AddActionMapper(action);
          }
        }
        this.m_loadingFile = false;
        return true;
      }
      this.m_loadingFile = false;
      return false;
    }

    public void ResetDevices()
    {
      foreach (InputDevice device in this.m_deviceList)
        device.Reset();
    }

    public void ClearActions(uint ParentHash)
    {
      int num = 0;
      foreach (InputDevice device in this.m_deviceList)
      {
        device.ClearActions(ParentHash, num == this.m_deviceList.Count<InputDevice>() - 1);
        ++num;
      }
    }

    public void AddActionMapper(InputActionMapper action)
    {
      foreach (InputDevice device in this.m_deviceList)
        device.AddAction(action);
    }

    public void RegisterInputCallback(string hash, InputActionMapper.InputCallback callback)
    {
      foreach (InputDevice device in this.m_deviceList)
        device.RegisterInputCallback(StringFunctions.StringHash(hash), hash, callback);
    }

    protected struct KeyStringMap(uint h, uint k)
    {
      public uint hash = h;
      public uint key = k;
    }
  }
}
