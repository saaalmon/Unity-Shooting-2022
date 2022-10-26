using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KanKikuchi.AudioManager;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
  private CircleCollider2D coll;
  private Animator anim;

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
  [SerializeField]
  private float _timer;

  public EnemyManager _manager { get; set; }

  public int _score;

  private float _hp;
  private bool _isWarning = true;
  private Coroutine _cor;

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

    if (coll == null) coll = GetComponent<CircleCollider2D>();
    if (anim == null) anim = GetComponent<Animator>();

    _cor = StartCoroutine(WarningTimer());
  }

  private void Hit()
  {
    _hitImpulse.GenerateImpulse();

    SoundManager.PlaySE(SEPath.ENEMY_HIT);

    Instantiate(_hitParticle, transform.position, Quaternion.identity);
  }

  private void Dead()
  {
    _manager.ReleaseEnemy(this);

    _deadImpulse.GenerateImpulse();

    SoundManager.PlaySE(SEPath.ENEMY_DEAD);

    Instantiate(_deadParticle, transform.position, Quaternion.identity);

    EventBus.Instance.NotifyDefeatEnemy(this);
  }

  private IEnumerator WarningTimer()
  {
    _isWarning = true;
    anim.SetBool("isWarning", _isWarning);
    coll.enabled = false;

    yield return new WaitForSeconds(_timer);

    _isWarning = false;
    anim.SetBool("isWarning", _isWarning);
    coll.enabled = true;
  }
}
