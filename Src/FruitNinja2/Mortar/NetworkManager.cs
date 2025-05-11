
// Type: Mortar.NetworkManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class NetworkManager
  {
    private static NetworkManager instance;

    public static NetworkManager GetInstance()
    {
      if (NetworkManager.instance == null)
        NetworkManager.instance = new NetworkManager();
      return NetworkManager.instance;
    }

    public bool UserHasEnabledNetwork() => true;

    public bool UnlockAchievement(string id) => true;

    public bool IsOnline() => false;
  }
}
