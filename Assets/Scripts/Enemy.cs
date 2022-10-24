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
  private ParticleSystem _hitParticle;
  [SerializeField]
  private ParticleSystem _deadParticle;

  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _hpMax;

  public EnemyManager _manager { get; set; }

  public int _score;

  private float _hp;

  // Start is called before the first frame update
  void Start()
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

  public void Init(EnemyManager manager)
  {
    _manager = manager;
    _hp = _hpMax;
  }

  private void Hit()
  {
    _hitImpulse.GenerateImpulse();

    Instantiate(_hitParticle, transform.position, Quaternion.identity);
  }

  private void Dead()
  {
    _manager.ReleaseEnemy(this);

    _deadImpulse.GenerateImpulse();

    Instantiate(_deadParticle, transform.position, Quaternion.identity);

    EventBus.Instance.NotifyDefeatEnemy(this);
  }
}
