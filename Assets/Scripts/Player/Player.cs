using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHits;
    public GameObject bloodPrefab;
    public GameObject guns;

    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.1f;

    public UnityEngine.UI.Slider healsSl;
 
    Vector2 inputVector;

    private Rigidbody2D rb;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _curentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;
    public AudioClip audioRun;

    AudioSource audioSrv;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _curentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        healsSl.maxValue = _maxHealth;
        audioSrv = GetComponent<AudioSource>();
    }


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
            OnTakeHits?.Invoke(this, EventArgs.Empty);

            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
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
            guns.SetActive(false);
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            FindObjectOfType<PauseMenu>().GameOverDeath();
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
            if (audioRun && !audioSrv.isPlaying)
            {
                audioSrv.pitch = UnityEngine.Random.Range(0.85f, 1.2f);
                audioSrv.volume = UnityEngine.Random.Range(0.75f, 1.1f);
                audioSrv.PlayOneShot(audioRun);
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MedicalClip>())
        {
            
            _curentHealth += 4;
            if (_curentHealth > _maxHealth)
            {
                _curentHealth = _maxHealth;
            }
            Destroy(collision.gameObject);
        }
    }
}


