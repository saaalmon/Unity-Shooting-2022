using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Target : MonoBehaviour
{
private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        _mainCamera = Camera.main;

        this.UpdateAsObservable()
        .Subscribe(_ => 
        {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        })
        .AddTo(this);
    }
}
