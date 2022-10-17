using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Player : MonoBehaviour
{
  private Rigidbody2D rb;
  private CircleCollider2D coll;
  private SpriteRenderer sp;
  private Animator anim;

  public static Player _instance;

  [SerializeField]
  private CinemachineImpulseSource _hitImpulse;
  [SerializeField]
  private CinemachineImpulseSource _deadImpulse;

  [SerializeField]
  private Shot _shot;
  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _hpMax;
  [SerializeField]
  private float _invTimer;

  private float _hp;

  void Awake()
  {
    _instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<CircleCollider2D>();
    sp = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    _hp = _hpMax;
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
    if (other.gameObject.TryGetComponent(out Item item))
    {
      Destroy(other.gameObject);
    }

    if (other.gameObject.TryGetComponent(out Enemy enemy))
    {
      _hp--;
      if (_hp > 0) Hit();
      else Dead();
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
    Destroy(gameObject);

    anim.SetTrigger("isDead");

    _deadImpulse.GenerateImpulse();
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
    coll.enabled = false;

    yield return new WaitForSeconds(_invTimer);

    coll.enabled = true;
  }
}
