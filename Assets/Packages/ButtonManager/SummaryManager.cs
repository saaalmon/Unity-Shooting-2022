using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryManager : MonoBehaviour
{
  [SerializeField]
  private CustomTextButton _start;

  [SerializeField]
  private CustomTextButton _config;

  [SerializeField]
  private CustomTextButton _credit;

  [SerializeField]
  private CustomTextButton _ranking;

  [SerializeField]
  private CustomTextButton _return;

  // Start is called before the first frame update
  void Start()
  {
    _start.Selected();

    _start.onClickCallback =
    () =>
    {
      Debug.Log(_start.name);
    };

    _config.onClickCallback =
    () =>
    {
      Debug.Log(_config.name);
    };

    _credit.onClickCallback =
    () =>
    {
      Debug.Log(_credit.name);
    };
  }

  // Update is called once per frame
  void Update()
  {

  }
}
