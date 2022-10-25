using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShotManager : MonoBehaviour
{
  [SerializeField]
  private Shot _prefab;

  public Shot _shot { get; private set; }

  private ObjectPool<Shot> _pool;
  private GameObject _parent;

  // Start is called before the first frame update
  void Start()
  {
    _parent = new GameObject("Shots");

    _pool = new ObjectPool<Shot>(
        () => Instantiate(_shot, _parent.transform),
        target => target.gameObject.SetActive(true),
        target => target.gameObject.SetActive(false),
        target => Destroy(target),
        true, 10, 20);
  }

  public void Generate(Vector3 pos, Quaternion rot, Vector3 shotDirection)
  {
    _shot = _prefab;

    var shot = _pool.Get();
    shot.transform.position = pos;
    shot.transform.rotation = rot;
    shot.Init(this, shotDirection);
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
}
