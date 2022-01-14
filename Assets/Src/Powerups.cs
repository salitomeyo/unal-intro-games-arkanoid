using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Powerups : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    public void Start()
    {
        Init();
    }

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        _collider.enabled = true;
    }

    public void OnHitCollision()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }
}
