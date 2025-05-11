
// Type: Mortar.InputEvent
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class InputEvent
  {
    public uint eventType;
    public ushort chr;
    public ButtonEvent button;
    public axisEvent axis;
    public uint timeStamp;

    public enum Propagation
    {
      Continue = 0,
      Stop = 255, // 0x000000FF
    }
  }
}
