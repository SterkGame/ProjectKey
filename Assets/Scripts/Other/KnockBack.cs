using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Camera))]

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _knockBackMovingTimerMax;

    private float _knockBackMovingTimer;

    private Rigidbody2D rb;

    public bool IsGettingKnockedBack { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockBack(Transform damageSource)
    {
        IsGettingKnockedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        rb.velocity = Vector3.zero;
        IsGettingKnockedBack =  false;
    }
}
