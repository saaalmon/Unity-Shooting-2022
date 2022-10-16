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

  private float _timer;

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

      var pos = new Vector2(Random.Range(-_limit.x, _limit.x), Random.Range(-_limit.y, _limit.y));
      var enemy = Instantiate(_enemy, pos, Quaternion.identity);
      enemy.Init();
    }
  }
}
