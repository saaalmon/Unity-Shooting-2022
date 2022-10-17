using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Item : MonoBehaviour
{
  private Rigidbody2D rb;
  private Transform _playerTrans;

  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _speedDec;
  [SerializeField]
  private float _followAccel;
  [SerializeField]
  private float _magnetDistance;

  private bool _isFollow = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Init(Vector3 itemDirection)
  {
    rb = GetComponent<Rigidbody2D>();

    _playerTrans = Player._instance?.transform;

    rb.velocity = itemDirection * _speed;
  }

  void FixedUpdate()
  {
    if (_playerTrans == null) return;

    var playerPos = _playerTrans.position;
    var distance = Vector3.Distance(playerPos, transform.position);
    if (distance < _magnetDistance)
    {
      _isFollow = true;
    }

    if (_isFollow)
    {
      var followDirection = playerPos - transform.position;
      followDirection.Normalize();

      rb.velocity = followDirection * _speed;
      rb.velocity *= _followAccel;
    }
    else
    {
      rb.velocity *= _speedDec;
    }
  }
}
