using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Shot : MonoBehaviour
{
  private Rigidbody2D rb;

  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _speedDec;
  [SerializeField]
  private float _speedAdd;
  [SerializeField]
  private float _velocityLimit;
  [SerializeField]
  private float _timer;

  public Player _manager { get; set; }

  private Transform _playerTrans;

  private Vector3 _shotDirection;
  public bool _isFollow = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  public void Init(Player manager, Vector3 shotDirection)
  {
    if (rb == null) rb = GetComponent<Rigidbody2D>();

    _isFollow = false;

    _manager = manager;
    _shotDirection = shotDirection;
    _playerTrans = Player._instance.transform;

    rb.velocity = _shotDirection * _speed;
  }

  void FixedUpdate()
  {
    if (_playerTrans == null) return;

    var playerPos = _playerTrans.position;

    if (_isFollow)
    {
      var followDirection = playerPos - transform.position;
      followDirection.Normalize();

      rb.velocity = followDirection * _speed;
      rb.velocity *= _speedAdd;
    }
    else
    {
      rb.velocity *= _speedDec;

      if (rb.velocity.magnitude <= _velocityLimit)
      {
        _isFollow = true;
      }
    }
  }
}
