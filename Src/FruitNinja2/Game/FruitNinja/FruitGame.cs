
// Type: GameManager.FruitGame
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace GameManager
{
  public class FruitGame
  {
    public void Init(uint instance, string startUpCommandLine) => Game2.GameInitialise(instance);

    public void End()
    {
      Game2.GameTaskExit();
      Game2.GameDestroy();
    }

    public void Update(float timeSinceLastUpdate) => Game2.GameTaskUpdate(timeSinceLastUpdate);

    public void Draw(float timeSinceLastUpdate) => Game2.GameTaskDraw(timeSinceLastUpdate);

    public void Paused() => GameTask.SkipToPause(false);

    public void UnPaused()
    {
      if ((double) Game2.game_work.gameOverTransition == 0.0)
        return;
      GameTask.UnpauseGame();
    }

    public void LoadContent()
    {
    }

    public string SelfVersion() => "1.21";
  }
}
