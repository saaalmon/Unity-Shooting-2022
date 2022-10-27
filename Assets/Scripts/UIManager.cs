using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UniRx;
using KanKikuchi.AudioManager;

public class UIManager : MonoBehaviour
{
  [SerializeField]
  private GameManager _gameManager;
  [SerializeField]
  private RankingManager _rankingManager;

  [SerializeField]
  private TMP_InputField _nameInput;
  [SerializeField]
  private CustomTextButton _startButton;
  [SerializeField]
  private CustomTextButton _configButton;
  [SerializeField]
  private CustomTextButton _creditButton;
  [SerializeField]
  private CustomTextButton _configCloseButton;
  [SerializeField]
  private CustomTextButton _readmeCloseButton;

  [SerializeField]
  private CanvasGroup _configPanel;
  [SerializeField]
  private CanvasGroup _readmePanel;

  [SerializeField]
  private TextMeshProUGUI _readyText;
  [SerializeField]
  private TextMeshProUGUI _timerText;
  [SerializeField]
  private Image _hpGauge;

  [SerializeField]
  private TextMeshProUGUI _resultScoreText;
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
    _nameInput.text = null;

    _nameInput.Select();

    _startButton.onClickCallback =
    () =>
    {
      _gameManager.StartButton();

      SoundManager.PlaySE(SEPath.BUTTON1);

      Debug.Log(_startButton.name);
    };

    _configButton.onClickCallback =
    () =>
    {
      OpenPanel(_configPanel, _configCloseButton);

      Debug.Log(_configButton.name);
    };

    _creditButton.onClickCallback =
    () =>
    {
      OpenPanel(_readmePanel, _readmeCloseButton);

      Debug.Log(_creditButton.name);
    };

    _configCloseButton.onClickCallback =
    () =>
    {
      ClosePanel(_configPanel, _startButton);

      Debug.Log(_configCloseButton.name);
    };

    _readmeCloseButton.onClickCallback =
    () =>
    {
      ClosePanel(_readmePanel, _startButton);

      Debug.Log(_readmeCloseButton.name);
    };
  }

  public void GameInit()
  {
    _rankingManager.SetName(_nameInput.text.ToString());
  }

  public void ResultInit()
  {
    _returnButton.Selected();

    _returnButton.onClickCallback =
    () =>
    {
      Debug.Log(_returnButton.name);

      SoundManager.PlaySE(SEPath.BUTTON1);

      _gameManager.ReturnButton();
    };
  }

  public void OpenPanel(CanvasGroup panel, CustomTextButton button)
  {
    var seq = DOTween.Sequence()
   .AppendCallback(() => panel.gameObject.SetActive(true))
   .AppendCallback(() => panel.transform.localScale = Vector3.zero)
   .Append(panel.transform.DOScale(Vector3.one, 0.1f))
   .Play();

    SoundManager.PlaySE(SEPath.BUTTON1);

    button.Selected();
  }

  public void ClosePanel(CanvasGroup panel, CustomTextButton button)
  {
    var seq = DOTween.Sequence()
       .Append(panel.transform.DOScale(Vector3.zero, 0.1f))
       .AppendCallback(() => panel.gameObject.SetActive(false))
       .Play();

    SoundManager.PlaySE(SEPath.BUTTON2);

    button.Selected();
  }

  public void SetResultScore()
  {
    var seq = DOTween.Sequence()
       .Append(_resultScoreText.DOCounter(0, _resultScore, 0.5f))
       .AppendCallback(() => SoundManager.PlaySE(SEPath.SCORE))
       .Play();
  }

  public void SetHpGauge(Player player)
  {
    _gameManager._playerInstance.Hp.Subscribe(h =>
    {
      _hpGauge.fillAmount = ((float)h / _gameManager._playerInstance._hpMax);
    }).AddTo(player);
  }

  public void SetReadyText(string text)
  {
    _readyText.text = text;
  }

  public void PlayGoSE()
  {
    SoundManager.PlaySE(SEPath.GO);
  }
}
