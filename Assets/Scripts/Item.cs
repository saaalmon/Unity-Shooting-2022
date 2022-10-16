using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Item : MonoBehaviour
{
  private Rigidbody2D rb;

  [SerializeField]
  private float _speed;
  [SerializeField]
  private float _speedDec;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    rb.velocity *= _speedDec;
  }

  public void Init(Vector3 itemDirection)
  {
    rb = GetComponent<Rigidbody2D>();

    rb.velocity = itemDirection * _speed;
  }
}
