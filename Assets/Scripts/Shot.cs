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

    rb.velocity = shotDirection * _speed;

    Destroy(gameObject, 4.0f);
  }
}
