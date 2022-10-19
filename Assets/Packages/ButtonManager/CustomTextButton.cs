using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CustomTextButton : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDeselectHandler,
    ISelectHandler,
    ISubmitHandler
{
  public System.Action onClickCallback;

  [SerializeField]
  private Button _button;

  [SerializeField]
  private CanvasGroup _canvasGroup;

  public void Selected()
  {
    _button.Select();
  }

  private void OnEnable()
  {
    transform.DOScale(1.0f, 0.0f);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    Debug.Log("Pointer");

    onClickCallback?.Invoke();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    Selected();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    //transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
    //_canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    //transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
    //_canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
  }

  public void OnSelect(BaseEventData eventData)
  {
    transform.DOScale(1.2f, 0.24f).SetEase(Ease.OutCubic);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    transform.DOScale(1.0f, 0.24f).SetEase(Ease.OutCubic);
  }

  public void OnSubmit(BaseEventData eventData)
  {
    Debug.Log("Submit");

    onClickCallback?.Invoke();
  }
}
