using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _rankText;
  [SerializeField]
  private TextMeshProUGUI _nameText;
  [SerializeField]
  private TextMeshProUGUI _scoreText;

  public string _name { private get; set; }
  public int _score { private get; set; }

  // Start is called before the first frame update
  void Start()
  {
    _name = "NoName";
    _score = 0;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SetName(string name)
  {
    _name = name;

    Debug.Log(_name);
  }

  public void SetScore(int score)
  {
    _score = score;

    Debug.Log(_score);
  }

  public void DisplayScoreBoard()
  {

  }
}
