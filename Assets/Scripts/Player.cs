using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Player : MonoBehaviour
{
  private Rigidbody2D rb;
  private CircleCollider2D coll;

  [SerializeField]
  private Shot _shot;
  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _hpMax;
  [SerializeField]
  private float _invTimer;

  private float _hp;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<CircleCollider2D>();

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
      //Destroy(gameObject);

      _hp--;
      if (_hp > 0) Hit();
      else Dead();
    }
  }

  private void Hit()
  {
    StartCoroutine(InvincibleTime());
  }

  private void Dead()
  {
    Destroy(gameObject);
  }

  private void Move()
  {
    var h = Input.GetAxisRaw("Horizontal");
    var v = Input.GetAxisRaw("Vertical");

    rb.velocity = new Vector2(h * _speed, v * _speed);

    //anim.SetFloat("isSpeed", Mathf.Abs(h) /* + Mathf.Abs(v)*/);

    // if (h < 0) sp.flipX = true;
    // if (h > 0) sp.flipX = false;

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
