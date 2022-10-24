using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine.Pool;

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
  private Shot _prefab;
  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _invTimer;

  public int _hpMax;

  public Shot _shot { get; private set; }

  private ObjectPool<Shot> _pool;
  private GameObject _parent;

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

    var parent = GameObject.Find("Shots");

    if (parent == null) _parent = new GameObject("Shots");
    else _parent = parent;

    _pool = new ObjectPool<Shot>(
        () => Instantiate(_shot, _parent.transform),
        target => target.gameObject.SetActive(true),
        target => target.gameObject.SetActive(false),
        target => Destroy(target),
        true, 10, 20);
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
      //enemy._manager.ReleaseEnemy(enemy);

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

  private void Hit()
  {
    anim.SetTrigger("isHit");

    _hitImpulse.GenerateImpulse();

    StartCoroutine(InvincibleTime());
  }

  private void Dead()
  {
    anim.SetTrigger("isDead");

    _deadImpulse.GenerateImpulse();

    GameManager._instance.GameFinish();

    ClearShot();
  }

  private void Move()
  {
    var h = Input.GetAxis("Horizontal");
    var v = Input.GetAxis("Vertical");

    rb.velocity = new Vector2(h * _speed, v * _speed);

    anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));

    if (h < 0) sp.flipX = true;
    if (h > 0) sp.flipX = false;
  }

  private void Shot()
  {
    _shot = _prefab;

    //プールオブジェクトの取得
    var prefab = _pool.Get();
    prefab.transform.position = transform.position;
    prefab.Init(this, MouseDirection());

    _shotImpulse.GenerateImpulse();

    // transform.DOScaleY(0.5f, 0.05f).SetLoops(2, LoopType.Yoyo)
    // .OnComplete(() => transform.localScale = Vector3.one);
  }

  public void ReleaseShot(Shot shot)
  {
    _pool.Release(shot);
  }

  public void ClearShot()
  {
    var shots = _parent.GetComponentsInChildren<Shot>();

    for (var i = 0; i < shots.Length; i++)
    {
      _pool.Release(shots[i]);
    }
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
