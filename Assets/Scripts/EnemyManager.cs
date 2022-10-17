using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  [SerializeField]
  private Enemy _enemy;
  [SerializeField]
  private float _interval;
  [SerializeField]
  private Vector2 _limit;
  [SerializeField]
  private int _length;

  private float _timer;

  private int[,] _mapList;
  private List<Vector2> _pointList = new List<Vector2>();

  void Awake()
  {
    // //初期化
    // for (var y = 0; y < _length; y++)
    // {
    //   for (var x = 0; x < _length; x++)
    //   {
    //     //_mapList[x, y] = 0;

    //     _pointList.Add(new Vector2(x, y));

    //     //Debug.Log(_mapList[x, y]);
    //   }
    // }

    // for (var i = 0; i < _pointList.Count; i++)
    // {
    //   Debug.Log(_pointList[i]);
    // }
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    //Generate();
  }

  public void Generate()
  {
    _timer += Time.deltaTime;

    if (_timer > _interval)
    {
      _timer = 0;

      // //修正箇所
      // var rand = Random.Range(0, _pointList.Count);
      // var randPos = _pointList[rand];
      // Debug.Log(randPos);
      // _pointList.RemoveAt(rand);


      var pos = new Vector2(Random.Range(-_limit.x, _limit.x), Random.Range(-_limit.y, _limit.y));
      var enemy = Instantiate(_enemy, pos, Quaternion.identity);
      enemy.Init();
    }
  }
}
