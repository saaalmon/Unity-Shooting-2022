using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
  [SerializeField]
  private CinemachineImpulseSource _hitImpulse;
  [SerializeField]
  private CinemachineImpulseSource _deadImpulse;
  [SerializeField]
  private ParticleSystem _deadParticle;

  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _hpMax;

  [SerializeField]
  private int _itemDropCount;

  public int _score;

  private float _hp;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.TryGetComponent(out Shot shot))
    {
      _hp--;
      if (_hp > 0) Hit();
      else Dead();
    }
  }

  public void Init()
  {
    _hp = _hpMax;
  }

  private void Hit()
  {
    _hitImpulse.GenerateImpulse();

    Instantiate(_deadParticle, transform.position, Quaternion.identity);

  }

  private void Dead()
  {
    Destroy(gameObject);

    _deadImpulse.GenerateImpulse();

    Instantiate(_deadParticle, transform.position, Quaternion.identity);

    EventBus.Instance.NotifyDefeatEnemy(this);

    //GameManager._instance.AddScore(_score);

    //ItemDrop();
  }

  // private void ItemDrop()
  // {
  //   for (var i = 0; i < _itemDropCount; i++)
  //   {
  //     var item = Instantiate(_item, transform.position, Quaternion.identity);

  //     var angle = Random.Range(0, 360);
  //     var f = angle * Mathf.Deg2Rad;

  //     var itemDirection = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0);

  //     item.Init(itemDirection);
  //   }
  // }
}
