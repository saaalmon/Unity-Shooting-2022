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

  /*2022/10/20　追加分*/
  [SerializeField]
  private float _speedDec;
  [SerializeField]
  private float _speedAdd;
  [SerializeField]
  private float _velocityLimit;

  private Transform _playerTrans;

  private Vector3 _shotDirection;
  public bool _isFollow = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Init(Vector3 shotDirection)
  {
    rb = GetComponent<Rigidbody2D>();

    /*2022/10/20　追加分*/
    _shotDirection = shotDirection;
    _playerTrans = Player._instance.transform;


    rb.velocity = _shotDirection * _speed;

    Destroy(gameObject, 4.0f);
  }

  void FixedUpdate()
  {
    /*　2022/10/20　追加分*/

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
