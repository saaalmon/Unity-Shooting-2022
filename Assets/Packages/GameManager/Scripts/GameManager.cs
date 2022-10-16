using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
  private static readonly StateTitle stateTitle = new StateTitle();
  private static readonly StateGame stateGame = new StateGame();
  private static readonly StateResult stateResult = new StateResult();

  private GameStateBase currentState = stateTitle;

  [SerializeField]
  private CanvasGroup _titleCanvas;
  [SerializeField]
  private CanvasGroup _gameCanvas;
  [SerializeField]
  private CanvasGroup _resultCanvas;

  public static GameManager _instance;

  void Awake()
  {
    _instance = this;

    _titleCanvas.gameObject.SetActive(false);
    _gameCanvas.gameObject.SetActive(false);
    _resultCanvas.gameObject.SetActive(false);
    currentState.OnEnter(this, null);
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    currentState.OnUpdate(this);
  }

  public void ChangeCurrentState(GameStateBase nextState)
  {
    currentState.OnExit(this, nextState);
    nextState.OnEnter(this, currentState);
    currentState = nextState;
  }

  public GameStateBase GetCurrentState()
  {
    return currentState;
  }
}

public partial class GameManager
{
  public class StateTitle : GameStateBase
  {
    public override void OnEnter(GameManager owner, GameStateBase prevState)
    {
      Debug.Log("Title");

      owner._titleCanvas.gameObject.SetActive(true);
    }

    public override void OnUpdate(GameManager owner)
    {

    }

    public override void OnExit(GameManager owner, GameStateBase nextState)
    {
      owner._titleCanvas.gameObject.SetActive(false);
    }
  }

  public class StateGame : GameStateBase
  {
    public override void OnEnter(GameManager owner, GameStateBase prevState)
    {
      Debug.Log("Game");

      owner._gameCanvas.gameObject.SetActive(true);
    }

    public override void OnUpdate(GameManager owner)
    {

    }

    public override void OnExit(GameManager owner, GameStateBase nextState)
    {
      owner._gameCanvas.gameObject.SetActive(false);
    }
  }

  public class StateResult : GameStateBase
  {
    public override void OnEnter(GameManager owner, GameStateBase prevState)
    {
      Debug.Log("Result");

      owner._resultCanvas.gameObject.SetActive(true);
    }

    public override void OnUpdate(GameManager owner)
    {

    }

    public override void OnExit(GameManager owner, GameStateBase nextState)
    {
      owner._resultCanvas.gameObject.SetActive(false);
    }
  }
}