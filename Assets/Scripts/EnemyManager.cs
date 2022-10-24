using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
  [SerializeField]
  private Enemy _prefab;
  [SerializeField]
  private float _interval;
  [SerializeField]
  private Vector2 _limit;

  public Enemy _enemy { get; private set; }

  private ObjectPool<Enemy> _pool;
  private GameObject _parent;

  // Start is called before the first frame update
  void Start()
  {
    _parent = new GameObject("Enemies");

    _pool = new ObjectPool<Enemy>(
        () => Instantiate(_enemy, _parent.transform),
        target => target.gameObject.SetActive(true),
        target => target.gameObject.SetActive(false),
        target => Destroy(target),
        true, 10, 20);
  }

  public IEnumerator Generate()
  {
    while (true)
    {
      _enemy = _prefab;

      var enemy = _pool.Get();
      var pos = new Vector2(Random.Range(-_limit.x, _limit.x), Random.Range(-_limit.y, _limit.y));
      enemy.transform.position = pos;
      enemy.Init(this);

      yield return new WaitForSeconds(_interval);
    }
  }

  public void ReleaseEnemy(Enemy enemy)
  {
    _pool.Release(enemy);
  }

  public void Defeat()
  {
    var enemies = _parent.GetComponentsInChildren<Enemy>();

    for (var i = 0; i < enemies.Length; i++)
    {
      ReleaseEnemy(enemies[i]);
    }
  }
}
