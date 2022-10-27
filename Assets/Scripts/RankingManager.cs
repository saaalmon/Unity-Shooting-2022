using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NCMB;

public class RankingManager : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _rankText;
  [SerializeField]
  private TextMeshProUGUI _nameText;
  [SerializeField]
  private TextMeshProUGUI _scoreText;

  public string _name { get; set; }
  public int _score { get; set; }

  private NCMBObject _data;

  // Start is called before the first frame update
  void Start()
  {

  }

  public void SetNCMBObject()
  {
    _data = new NCMBObject("data");

    _name = "NoName";
    _score = 0;
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

  public void SetRanking()
  {
    _data["Name"] = _name;
    _data["Score"] = _score;

    _data.SaveAsync();

    Debug.Log("登録完了");
  }

  public void DisplayScoreBoard()
  {
    var count = 0;

    var ranktemp = "";
    var nametemp = "";
    var scoretemp = "";

    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("data");

    query.OrderByDescending("Score");

    query.Limit = 5;
    query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
    {
      if (e != null)
      {
        Debug.Log("ランキング取得失敗");
      }
      else
      {
        Debug.Log("ランキング取得成功");

        foreach (NCMBObject obj in objList)
        {
          count++;
          ranktemp += count.ToString() + "\r\n";
          nametemp += obj["Name"].ToString() + "\r\n";
          scoretemp += obj["Score"].ToString() + "\r\n";
        }

        _rankText.text = ranktemp;
        _nameText.text = nametemp;
        _scoreText.text = scoretemp;
      }
    });
  }
}
