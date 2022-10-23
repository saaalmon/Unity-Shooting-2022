using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private SpriteRenderer sp;
  private Animator anim;

  public static Player _instance;

  [SerializeField]
  private CinemachineImpulseSource _hitImpulse;
  [SerializeField]
  private CinemachineImpulseSource _deadImpulse;
  [SerializeField]
  private CinemachineImpulseSource _shotImpulse;

  [SerializeField]
  private Shot _shot;
  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _invTimer;

  public int _hpMax;

  public IReadOnlyReactiveProperty<int> Hp => _hp;
  private readonly IntReactiveProperty _hp = new IntReactiveProperty(0);

  private bool _isInvincible = false;

  void Awake()
  {
    _instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<BoxCollider2D>();
    sp = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    _hp.Value = _hpMax;
  }

  // Update is called once per frame
  void Update()
  {
    Move();

    if (Input.GetMouseButtonDown(0))
    {
      Shot();
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.TryGetComponent(out Enemy enemy) && !_isInvincible)
    {
      _hp.Value--;
      if (_hp.Value > 0) Hit();
      else Dead();
    }

    if (other.gameObject.TryGetComponent(out Shot shot))
    {
      if (!shot._isFollow) return;

      Destroy(other.gameObject);
    }
  }

  private void Hit()
  {
    StartCoroutine(InvincibleTime());

    anim.SetTrigger("isHit");

    _hitImpulse.GenerateImpulse();
  }

  private void Dead()
  {
    anim.SetTrigger("isDead");

    _deadImpulse.GenerateImpulse();

    GameManager._instance.GameFinish();
  }

  private void Move()
  {
    var h = Input.GetAxisRaw("Horizontal");
    var v = Input.GetAxisRaw("Vertical");

    rb.velocity = new Vector2(h * _speed, v * _speed);

    anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));

    if (h < 0) sp.flipX = true;
    if (h > 0) sp.flipX = false;
  }

  private void Shot()
  {
    var shot = Instantiate(_shot, transform.position, Quaternion.identity);

    shot.Init(MouseDirection());

    _shotImpulse.GenerateImpulse();

    transform.DOScaleY(0.5f, 0.05f).SetLoops(2, LoopType.Yoyo)
    .OnComplete(() => transform.localScale = Vector3.one);
  }

  private Vector3 MouseDirection()
  {
    var screenPos = Camera.main.WorldToScreenPoint(transform.position);
    var direction = Input.mousePosition - screenPos;
    var angle = Utils.GetAngle(Vector3.zero, direction);

    return Utils.GetDirection(angle);
  }

  private IEnumerator InvincibleTime()
  {
    _isInvincible = true;

    yield return new WaitForSeconds(_invTimer);

    _isInvincible = false;
  }
}
