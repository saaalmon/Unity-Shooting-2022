using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryManager : MonoBehaviour
{
  [SerializeField]
  private CustomTextButton _button1;

  [SerializeField]
  private CustomTextButton _button2;

  [SerializeField]
  private CustomTextButton _button3;

  // Start is called before the first frame update
  void Start()
  {
    _button1.Selected();

    _button1.onClickCallback =
    () =>
    {
      Debug.Log(_button1.name);
    };

    _button2.onClickCallback =
    () =>
    {
      Debug.Log(_button2.name);
    };

    _button3.onClickCallback =
    () =>
    {
      Debug.Log(_button3.name);
    };
  }

  // Update is called once per frame
  void Update()
  {

  }
}
