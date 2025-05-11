
// Type: Mortar.SystemManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class SystemManager
  {
    private static SystemManager instance;

    public static SystemManager GetInstance()
    {
      if (SystemManager.instance == null)
        SystemManager.instance = new SystemManager();
      return SystemManager.instance;
    }

    public void Init()
    {
    }

    public bool Update(ref float dt) => true;
  }
}
