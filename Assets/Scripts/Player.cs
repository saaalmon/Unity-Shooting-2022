using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine.Pool;
using KanKikuchi.AudioManager;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private SpriteRenderer sp;
  private Animator anim;
  private ShotManager _shotManager;
  private Camera _mainCamera;

  public static Player _instance;

  [SerializeField]
  private CinemachineImpulseSource _hitImpulse;
  [SerializeField]
  private CinemachineImpulseSource _deadImpulse;
  [SerializeField]
  private CinemachineImpulseSource _shotImpulse;

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

      shot._manager.ReleaseShot(shot);
    }
  }

  public void Init(ShotManager manager)
  {
    _shotManager = manager;
    _mainCamera = Camera.main;
    _hp.Value = _hpMax;

    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<BoxCollider2D>();
    sp = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();
  }

  private void Hit()
  {
    anim.SetTrigger("isHit");

    _hitImpulse.GenerateImpulse();

    SoundManager.PlaySE(SEPath.PLAYER_HIT);

    StartCoroutine(InvincibleTime());
  }

  private void Dead()
  {
    anim.SetTrigger("isDead");

    _deadImpulse.GenerateImpulse();

    SoundManager.PlaySE(SEPath.PLAYER_DEAD);

    GameManager._instance.GameFinish();

    _shotManager.ClearShot();
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
    var shotDirection = MouseDirection();
    _shotManager.Generate(transform.position, Quaternion.identity, shotDirection);

    _shotImpulse.GenerateImpulse();

    SoundManager.PlaySE(SEPath.SHOT);
  }

  private Vector3 MouseDirection()
  {
    var screenPos = _mainCamera.WorldToScreenPoint(transform.position);
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
