using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public partial class GameManager : MonoBehaviour
{
  private static readonly StateTitle stateTitle = new StateTitle();
  private static readonly StateGame stateGame = new StateGame();
  private static readonly StateResult stateResult = new StateResult();

  private GameStateBase currentState = stateTitle;

  [SerializeField]
  private EnemyManager _enemyManager;
  [SerializeField]
  private ShotManager _shotManager;
  [SerializeField]
  private UIManager _UIManager;
  [SerializeField]
  private CinemachineVirtualCamera _cinemaCamea;
  [SerializeField]
  private PlayableDirector _director;

  [SerializeField]
  private CanvasGroup _titleCanvas;
  [SerializeField]
  private CanvasGroup _gameCanvas;
  [SerializeField]
  private CanvasGroup _resultCanvas;

  [SerializeField]
  private TimelineAsset _titleIn;
  [SerializeField]
  private TimelineAsset _gameIn;
  [SerializeField]
  private TimelineAsset _gameOut;
  [SerializeField]
  private TimelineAsset _resultIn;

  [SerializeField]
  private Player _player;

  public float _timerMax;

  public Player _playerInstance;

  public IReadOnlyReactiveProperty<float> Timer => _timer;
  private readonly FloatReactiveProperty _timer = new FloatReactiveProperty(0);

  public IReadOnlyReactiveProperty<int> Score => _score;
  private readonly IntReactiveProperty _score = new IntReactiveProperty(0);

  public static GameManager _instance;

  private Coroutine _generateCor;
  private Coroutine _timerCor;

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

  public void ResultButton()
  {
    ChangeCurrentState(stateResult);
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

  public bool CompletedTimeLine(PlayableDirector director)
  {
    return director.time >= director.duration;
  }

  private IEnumerator CountTimer()
  {
    while (true)
    {
      _timer.Value -= Time.deltaTime;

      if (_timer.Value < 0)
      {
        _timer.Value = 0;
        GameFinish();
      }
      yield return 0;
    }
  }

  public void TitleInit()
  {

  }

  public void GameInit()
  {
    Debug.Log("play");

    var player = Instantiate(_player, transform.position, Quaternion.identity);
    _playerInstance = player;
    player.Init(_shotManager);

    _UIManager.SetHpGauge(player);

    _cinemaCamea.Follow = _playerInstance.transform;

    /*　2022/10/16　変更範囲　*/
    _timer.Value = _timerMax;
    _score.Value = 0;

    _generateCor = StartCoroutine(_enemyManager.Generate());
    _timerCor = StartCoroutine(CountTimer());
  }

  public void GameFinish()
  {
    Destroy(_playerInstance.gameObject);
    _playerInstance = null;

    StopCoroutine(_generateCor);
    StopCoroutine(_timerCor);

    _cinemaCamea.Follow = null;

    _enemyManager.Defeat();

    _director.playableAsset = _gameOut;
    _director.Play();

    Debug.Log("finish");
  }

  public void ResultInit()
  {

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

      owner._director.playableAsset = owner._titleIn;
      owner._director.Play();

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

      owner._director.playableAsset = owner._gameIn;
      owner._director.Play();

      // var player = Instantiate(owner._player, owner.transform.position, Quaternion.identity);
      // owner._playerInstance = player;
      // owner._UIManager.SetHpGauge(player);

      // owner._cinemaCamea.Follow = owner._playerInstance.transform;

      // /*　2022/10/16　変更範囲　*/
      // owner._timer.Value = owner._timerMax;
      // owner._score.Value = 0;
    }

    public override void OnUpdate(GameManager owner)
    {
      /*　2022/10/16　変更範囲　*/
      // owner._enemyManager.Generate();

      // owner._timer.Value -= Time.deltaTime;

      // Debug.Log(owner._timer);

      // if (owner._timer.Value < 0) owner.ChangeCurrentState(stateResult);
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

      owner._director.playableAsset = owner._resultIn;
      owner._director.Play();

      // Destroy(owner._playerInstance.gameObject);
      // owner._playerInstance = null;

      // owner._cinemaCamea.Follow = null;

      // owner._enemyManager.Defeat();
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
