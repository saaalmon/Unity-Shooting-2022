using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameManager : MonoBehaviour
{
  private static readonly StateTitle stateTitle = new StateTitle();
  private static readonly StateGame stateGame = new StateGame();
  private static readonly StateResult stateResult = new StateResult();

  private GameStateBase currentState = stateTitle;

  [SerializeField]
  private EnemyManager _enemyManager;
  [SerializeField]
  private UIManager _UIManager;

  [SerializeField]
  private CanvasGroup _titleCanvas;
  [SerializeField]
  private CanvasGroup _gameCanvas;
  [SerializeField]
  private CanvasGroup _resultCanvas;

  public float _timerMax;

  public IReadOnlyReactiveProperty<float> Timer => _timer;
  private readonly FloatReactiveProperty _timer = new FloatReactiveProperty(0);

  public IReadOnlyReactiveProperty<int> Score => _score;
  private readonly IntReactiveProperty _score = new IntReactiveProperty(0);

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
    EventBus.Instance.Subscribe((EventBus.OnDefeatEnemy)OnDefeatEnemy);
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

  public void StartButton()
  {
    ChangeCurrentState(stateGame);
  }

  public void ReturnButton()
  {
    ChangeCurrentState(stateTitle);
  }


  public void AddScore(int score)
  {
    _score.Value += score;

    Debug.Log(_score.Value);
  }

  public void OnDefeatEnemy(Enemy enemy)
  {
    AddScore(enemy._score);
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

      owner._UIManager.StartInit();

      /*　2022/10/16　変更範囲　*/
      //owner.ChangeCurrentState(stateGame);
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

      owner._UIManager.GameInit();

      /*　2022/10/16　変更範囲　*/
      owner._timer.Value = owner._timerMax;
    }

    public override void OnUpdate(GameManager owner)
    {
      /*　2022/10/16　変更範囲　*/
      owner._enemyManager.Generate();

      owner._timer.Value -= Time.deltaTime;

      Debug.Log(owner._timer);

      if (owner._timer.Value < 0) owner.ChangeCurrentState(stateResult);
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

      owner._UIManager.ResultInit();
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