using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UniRx;

public class UIManager : MonoBehaviour
{
  [SerializeField]
  private GameManager _gameManager;

  [SerializeField]
  private CustomTextButton _startButton;
  [SerializeField]
  private CustomTextButton _configButton;
  [SerializeField]
  private CustomTextButton _creditButton;

  [SerializeField]
  private TextMeshProUGUI _timerText;
  [SerializeField]
  private Image _hpGauge;

  [SerializeField]
  private TextMeshProUGUI _resultScoreText;
  [SerializeField]
  private CustomTextButton _rankingButton;
  [SerializeField]
  private CustomTextButton _returnButton;

  private int _resultScore;

  // Start is called before the first frame update
  void Start()
  {
    _gameManager.Timer.Subscribe(t =>
    {
      _timerText.text = t.ToString("F2");
    })
    .AddTo(this);

    _gameManager.Score.Subscribe(s =>
    {
      _resultScore = s;

      //Debug.Log("Score:" + s);
    })
    .AddTo(this);
  }

  public void StartInit()
  {
    _startButton.Selected();

    _startButton.onClickCallback =
    () =>
    {
      _gameManager.StartButton();

      Debug.Log(_startButton.name);
    };

    _configButton.onClickCallback =
    () =>
    {
      Debug.Log(_configButton.name);
    };

    _creditButton.onClickCallback =
    () =>
    {
      Debug.Log(_creditButton.name);
    };
  }

  public void GameInit()
  {

  }

  public void ResultInit()
  {
    _rankingButton.Selected();

    _resultScoreText.DOCounter(0, _resultScore, 0.5f);

    _rankingButton.onClickCallback =
    () =>
    {
      Debug.Log(_rankingButton.name);
    };

    _returnButton.onClickCallback =
    () =>
    {
      Debug.Log(_returnButton.name);

      _gameManager.ReturnButton();
    };
  }
}
