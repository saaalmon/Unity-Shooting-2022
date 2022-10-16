public enum GemeState
{
  None,
  Title,
  Game,
  Result,
}

public abstract class GameStateBase
{
  public virtual void OnEnter(GameManager owner, GameStateBase prevState) { }

  public virtual void OnUpdate(GameManager onwer) { }

  public virtual void OnExit(GameManager owner, GameStateBase nextState) { }
}