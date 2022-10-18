using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

  // Start is called before the first frame update
  void Start()
  {

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
