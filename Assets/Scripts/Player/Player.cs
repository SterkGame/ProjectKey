using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath; 

    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.1f;

    public Slider healsSl;
 
    Vector2 inputVector;

    private Rigidbody2D rb;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _curentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _curentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        //GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    //private void GameInput_OnPlayerAttack(object sender, System.EventArgs e) {
    //    ActiveWeapon.Instance.GetActiveWeapon().Attack();
    //}

    private void Update() {
        healsSl.value = _curentHealth;
        inputVector = GameInput.Instance.GetMovementVector();
    }


    private void FixedUpdate() 
    {
        HandleMovement();
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _curentHealth = Mathf.Max(0, _curentHealth -= damage);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_curentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void HandleMovement() {
        rb.MovePosition(rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed) {
            _isRunning = true;
            
        } else {
            _isRunning = false;
        }
    }

    public bool IsRunning() {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

}
