using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Rendering;


[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnTakeHits;
    public GameObject bloodPrefab;
    public GameObject guns;
    public int medicalClipCount;
    public TextMeshProUGUI medicalClipText;


    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.1f;

    public UnityEngine.UI.Slider healsSl;
 
    Vector2 inputVector;

    private Rigidbody2D rb;
    private SortingGroup sortingGroup;
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
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start() {
        PlayerData data = SaveSystem.LoadPlayer();
        _maxHealth = data.maxHealth;
        _curentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        healsSl.maxValue = _maxHealth;
        audioSrv = GetComponent<AudioSource>();
        medicalClipText.text = medicalClipCount + "/3";
    }


    private void Update() {
        healsSl.value = _curentHealth;
        inputVector = GameInput.Instance.GetMovementVector();
        if (Input.GetKeyDown(KeyCode.F))
        {
            MedicalHeal();
            medicalClipText.text = medicalClipCount + "/3";
        }
            
    }
    Vector3 lastPosition;
    void LateUpdate()
    {

        if (transform.position != lastPosition)
        {
            sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 2);
            lastPosition = transform.position;
        }
    }


    private void FixedUpdate() 
    {
        HandleMovement();
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public void TakeDamage(int baseDamage)
    {
        int damage = baseDamage;
        var data = SaveSystem.LoadPlayer();

        switch (data.difficulty)
        {
            case GameDifficulty.Легкий:
                damage = Mathf.RoundToInt(baseDamage * 0.5f);
                break;
            case GameDifficulty.Середній:
                damage = baseDamage;
                break;
            case GameDifficulty.Важкий:
                damage = Mathf.RoundToInt(baseDamage * 2f);
                break;
        }

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

    public void MedicalHeal()
    {
        if (medicalClipCount > 0)
        {
            medicalClipCount -= 1;
            _curentHealth += _maxHealth / 2;
            if (_curentHealth > _maxHealth)
            {
                _curentHealth = _maxHealth;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MedicalClip>())
        {
            if (medicalClipCount < 3)
            {
                medicalClipCount += 1;
                medicalClipText.text = medicalClipCount + "/3";
            }
            else
            {
                medicalClipCount = 3;
            }
            Destroy(collision.gameObject);
        }
    }
}


