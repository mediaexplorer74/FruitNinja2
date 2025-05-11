
// Type: Mortar.AnimationManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class AnimationManager
  {
    private static AnimationManager instance;

    public static AnimationManager GetInstance()
    {
      if (AnimationManager.instance == null)
        AnimationManager.instance = new AnimationManager();
      return AnimationManager.instance;
    }

    public void Initialise()
    {
    }

    public void Initialise(int heapsize)
    {
    }
  }
}
